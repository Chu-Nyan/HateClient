using Chu.Collision;
using Chu.Collision.Layer;
using Chu.Data;
using Chu.Utility;
using SAB.DataManger;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    public class MainCutsceneDirector
    {
        private readonly static NyanLayer _layer = NyanLayer.TriggerZone;
        private readonly static NyanLayerMask _mask = new(NyanLayer.PlayerUnit);

        private readonly PlayableDirector _director;
        private readonly CinemachineBrain _cameraBrain;

        private readonly ObjectPooling<CollisionTrigger> _triggerPool;
        private readonly Dictionary<Type, ObjectPooling<IVCam>> _vcamPool;
        private readonly ObjectPooling<SingleMesh> _singleMeshPool;

        // Map
        private readonly List<CollisionTrigger> _mapTriggers;
        private readonly Dictionary<int, CutsceneData> _dataByTriggerID;

        // Cutscene in progress
        private readonly Dictionary<string, TrackAsset> _trackByName;
        private readonly Dictionary<int, IVCam> _vcamsByID;
        private readonly Dictionary<int, SingleMesh> _singleMeshByID;

        public MainCutsceneDirector(PlayableDirector director, CinemachineBrain brain)
        {
            _triggerPool = new(() => new CollisionTrigger());
            _vcamPool = new()
            {
                { typeof(VCamStatic), new(() => AssetManager.GenerateLoadAssetSync<VCamStatic>(Const.Asset_VCamStatic), a => a.SetActive(true))},
                { typeof(VCamFollow), new(() => AssetManager.GenerateLoadAssetSync<VCamFollow>(Const.Asset_VCamFollow), a => a.SetActive(true))},
            };
            _singleMeshPool = new(() => AssetManager.GenerateLoadAssetSync<SingleMesh>(Const.Asset_SingleMesh), a => a.SetActive(true));

            _mapTriggers = new();
            _dataByTriggerID = new();
            _trackByName = new();
            _vcamsByID = new();
            _singleMeshByID = new();

            _director = director;
            _cameraBrain = brain;
        }

        public void ChangeMap(MapType type)
        {
            if (DataBase.Instance.CutSceneRepo.DataByMapType.TryGetValue(type, out CutsceneData[] datas) == false)
            {
                Debug.LogError($"{type} cutscene data is null");
            }
            else
            {
                SetupMap(datas);
            }
        }

        private void SetupMap(CutsceneData[] datas)
        {
            MapClear();
            for (int i = 0; i < datas.Length; i++)
            {
                var data = datas[i];
                GenerateCutsceneTrigger(data);
            }
        }

        private void GenerateCutsceneTrigger(CutsceneData data)
        {
            CollisionTrigger trigger = _triggerPool.Dequeue();
            IShape shape = ShapeFactory.GenerateShape(data.TriggerZones);
            trigger.Setup(shape, new Pose2D(data.Center, 0), _layer, _mask, true); // TODO : 컷씬 활성화 여부
            trigger.RegisterOnEntered(OnCutsceneTriggerEnter);

            _mapTriggers.Add(trigger);
            _dataByTriggerID.Add(trigger.ID, data);
        }

        private void OnCutsceneTriggerEnter(int id)
        {
            CutSceneClear();

            CutsceneData data = _dataByTriggerID[id];
            PlayableAsset playableAsset = AssetManager.LoadAssetSync<PlayableAsset>(data.AssetPath);

            CacheTracks(playableAsset);
            PrepareCutsceneObject(data);
            BindingObject(data);
            _director.playableAsset = playableAsset;
            _director.RebuildGraph();
            _director.time = 0;
            _director.Evaluate();
            _director.Play();
            Debug.Log("Play");
        }

        private void CacheTracks(PlayableAsset playable)
        {
            foreach (var output in playable.outputs)
            {
                var track = output.sourceObject as TrackAsset;
                if (track == null)
                    continue;

                if (_trackByName.TryAdd(track.name, track) == false)
                    Debug.LogError($"Cutscene : {playable.name} track : {track.name}, Duplicate name detected");
            }
        }

        // 직렬화된 키값으로 오브젝트 생성 후 바인딩
        private void PrepareCutsceneObject(CutsceneData data)
        {
            PrepareVCam(data.VCamData);
            PrepareSingleMesh(data.SingleMeshDataByID);
        }

        private void PrepareVCam(List<CutsceneVCamData> vcamData)
        {
            for (int i = 0; i < vcamData.Count; i++)
            {
                foreach (var item in vcamData[i].StaticData)
                {
                    var obj = GetVCam<VCamStatic>();
                    obj.ApplySerializedData(item.Value);
                    _vcamsByID[item.Key] = obj;
                }

                foreach (var item in vcamData[i].FollowData)
                {
                    var obj = GetVCam<VCamFollow>();
                    obj.ApplySerializedData(item.Value);
                    _vcamsByID[item.Key] = obj;
                }
            }
        }

        private void PrepareSingleMesh(Dictionary<int, SingleMeshData> datas)
        {
            foreach (var item in datas)
            {
                SingleMeshData data = item.Value;
                var singleMesh = _singleMeshPool.Dequeue();
                singleMesh.ApplySerializedData(data);

                _singleMeshByID.Add(data.ID, singleMesh);
            }
        }

        private void BindingObject(CutsceneData data)
        {
            BindingSingleMeshTrack(data.BindingIDByTrack, data.SingleMeshDataByID);
            BindingVCamTrack(data.VCamData);
        }

        private void BindingVCamTrack(List<CutsceneVCamData> data)
        {
            foreach (var trackData in data)
            {
                if (_trackByName.TryGetValue(trackData.Name, out var track))
                {
                    if (track is not CinemachineTrack)
                    {
                        Debug.LogWarning($"TrackName : {trackData}, Not CinemachineTrack");
                    }
                    else
                    {
                        _director.SetGenericBinding(track, _cameraBrain);

                        foreach (var clip in track.GetClips())
                        {
                            var id = trackData.VCamIDByClipName[clip.displayName];
                            var shot = clip.asset as CinemachineShot;
                            _director.SetReferenceValue(shot.VirtualCamera.exposedName, _vcamsByID[id].CinemachineCamera);

                        }
                        BindingVcamFollowTrack(trackData.FollowData);
                    }
                }
                else
                    Debug.LogWarning($"TrackName : {trackData}, Not Found");
            }
        }

        private void BindingVcamFollowTrack(Dictionary<int, VCamFollowData> datas)
        {
            foreach (var item in datas)
            {
                var data = item.Value;
                if (_vcamsByID[data.ID] is VCamFollow follow)
                {
                    follow.SetFollow(_singleMeshByID[data.TargetID].transform);
                }
            }
        }

        private void BindingSingleMeshTrack(Dictionary<string, int> trackBinding, Dictionary<int, SingleMeshData> datas)
        {
            foreach (var item in datas)
            {
                var data = item.Value;

                if (_trackByName.TryGetValue(data.TrackName, out var track))
                {
                    if (track is not AnimationTrack)
                    {
                        Debug.LogWarning($"TrackName : {data.TrackName}, Not AnimationTrack");
                    }
                    else
                    {
                        var id = trackBinding[track.name];
                        _director.SetGenericBinding(track, _singleMeshByID[id].Animator);
                    }
                }
                else
                    Debug.LogWarning($"TrackName : {data.TrackName}, Not Found");
            }
        }

        private void CutSceneClear()
        {
            foreach (var item in _vcamsByID)
            {
                item.Value.SetActive(false);
                _vcamPool[item.Value.GetType()].Enqueue(item.Value);
            }

            foreach (var item in _singleMeshByID)
            {
                item.Value.SetActive(false);
                _singleMeshPool.Enqueue(item.Value);
            }

            _trackByName.Clear();
            _vcamsByID.Clear();
            _singleMeshByID.Clear();
        }

        private void MapClear()
        {
            _dataByTriggerID.Clear();

            for (int i = 0; i < _mapTriggers.Count; i++)
            {
                _mapTriggers[i].SetActive(false);
                _triggerPool.Enqueue(_mapTriggers[i]);
            }
            _mapTriggers.Clear();
        }

        private T GetVCam<T>() where T : IVCam
        {
            var vcam = (T)_vcamPool[typeof(T)].Dequeue();
            return vcam;
        }
    }
}
