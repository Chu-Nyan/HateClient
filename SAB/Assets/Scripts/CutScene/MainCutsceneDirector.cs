using Chu.Collision;
using Chu.Core;
using Chu.Data;
using SAB.DataManger;
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

        private readonly CutscenePool _pool;
        // Map
        private readonly List<CollisionTrigger> _mapTriggers;
        private readonly Dictionary<int, CutsceneData> _dataByTriggerID;

        // Cutscene in progress
        private readonly Dictionary<string, TrackAsset> _trackByName;
        private readonly Dictionary<int, ICutsceneObject> _objectByID;

        public MainCutsceneDirector(PlayableDirector director, CinemachineBrain brain)
        {
            _mapTriggers = new();
            _dataByTriggerID = new();
            _objectByID = new();
            _trackByName = new();
            _pool = new();

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
            CollisionTrigger trigger = _pool.DequeueTrigger();
            IShape shape = ShapeFactory.GenerateShape(data.TriggerZones);
            trigger.Setup(shape, new Pose2D(data.Center, 0), _layer, _mask, true); // TODO : 컷씬 활성화 여부
            trigger.RegisterOnEntered(OnCutsceneTriggerEnter);

            _mapTriggers.Add(trigger);
            _dataByTriggerID.Add(trigger.ID, data);
        }

        private void OnCutsceneTriggerEnter(int id)
        {
            ClearPlayingCutscene();
            CutsceneData data = _dataByTriggerID[id];
            PlayableAsset playableAsset = AssetManager.LoadAssetSync<PlayableAsset>(data.AssetPath);

            CacheTracks(playableAsset);
            PrepareCutsceneObject(data.ObjectDataContainer);
            BindingTrack(data);
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
        private void PrepareCutsceneObject(ObjectDataContainer container)
        {
            // Generate
            foreach (var config in container)
            {
                var obj = _pool.DequeueObject(config.Value);
                obj.SetCutsceneData(config.Value);
                _objectByID[config.Key] = obj;
            }

            // Init
            foreach (var item in container.GetTable<VCamFollowData>())
            {
                VCamFollow cam = (VCamFollow)_objectByID[item.Key];
                cam.SetFollow(_objectByID[item.Value.TargetID].transform);
            }
        }

        private void BindingTrack(CutsceneData cutsceneData)
        {
            foreach (var item in _trackByName)
            {
                var track = item.Value;

                if (track is CinemachineTrack)
                {
                    _director.SetGenericBinding(track, _cameraBrain);

                    foreach (var clip in track.GetClips())
                    {
                        var id = cutsceneData.BindingIDByTrack[clip.displayName];

                        var shot = clip.asset as CinemachineShot;
                        IVCam vcam = (IVCam)_objectByID[id];
                        _director.SetReferenceValue(shot.VirtualCamera.exposedName, vcam.CinemachineCamera);

                    }
                }
                if (track is AnimationTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, ((SingleMesh)_objectByID[id]).Animator);
                }
            }
        }

        private void ClearPlayingCutscene()
        {
            if (_trackByName.Count == 0)
                return;

            foreach (var item in _objectByID)
            {
                item.Value.SetActive(false);
                _pool.EnqueueObject(item.Value);
            }

            _objectByID.Clear();
            _trackByName.Clear();
        }

        private void MapClear()
        {
            _dataByTriggerID.Clear();

            for (int i = 0; i < _mapTriggers.Count; i++)
            {
                _mapTriggers[i].SetActive(false);
                _pool.EnqueueTrigger(_mapTriggers[i]);
            }
            _mapTriggers.Clear();
        }
    }
}
