using Chu.Collision;
using Chu.Core;
using Chu.Data;
using SAB.DataManger;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    public class CutsceneController : MonoBehaviour
    {
        private readonly static NyanLayer _layer = NyanLayer.TriggerZone;
        private readonly static NyanLayerMask _mask = new(NyanLayer.PlayerUnit);

        [SerializeField]
        private PlayableDirector _director;
        [SerializeField]
        private CinemachineBrain _cameraBrain;
        [SerializeField]
        private MarkerReceiver _markerReceiver;

        private readonly CutscenePool _pool = new();
        // Map
        private readonly List<CollisionTrigger> _mapTriggers = new(16);
        private readonly Dictionary<int, CutsceneData> _dataByTriggerID = new();
        private readonly Dictionary<int, ICutsceneObject> _sceneObject = new();
        private UniqueEntityContainer _uniqueEntity;

        // Cutscene in progress
        private int _playID;
        private readonly Dictionary<string, TrackAsset> _trackByName = new();
        private readonly Dictionary<int, ICutsceneObject> _objectByID = new();

        public event Action<int> CutsceneStarted;
        public event Action<int> CutsceneStopped;

        public int PlayID
        {
            get => _playID;
        }

        private void Awake()
        {
            if (_cameraBrain == null && Camera.main.TryGetComponent<CinemachineBrain>(out var brain) == true)
                _cameraBrain = brain;

            _markerReceiver.Init(_objectByID);
            _director.stopped += OnTimelineStopped;
        }

        public void Init(CinemachineBrain brain, UniqueEntityContainer uniqueEntity)
        {
            _cameraBrain = brain;
            _uniqueEntity = uniqueEntity;
        }

        public void ChangeMap(MapType type, MapReferenceHub hub)
        {
            if (DataBase.Instance.CutSceneRepo.DataByMapType.TryGetValue(type, out CutsceneData[] datas) == false)
            {
                Debug.LogError($"{type} cutscene data is null");
            }
            else
            {
                SetupMap(datas);
                foreach (var item in hub.Characters)
                {
                    _sceneObject.Add(item.Key, item.Value);
                }
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
            trigger.RegisterOnEntered(OnStarted);

            _mapTriggers.Add(trigger);
            _dataByTriggerID.Add(trigger.ID, data);
        }

        public void Play(int cutsceneID)
        {
            _playID = cutsceneID;
            ClearPlayingCutscene();

            CutsceneData data = _dataByTriggerID[cutsceneID];
            PlayableAsset playableAsset = AssetManager.LoadAssetSync<PlayableAsset>(data.AssetPath);

            CacheTracks(playableAsset);
            PrepareCutsceneObject(data);
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

        private void PrepareCutsceneObject(CutsceneData data)
        {
            // Generate
            var container = data.SpawnContainer;
            foreach (var config in container)
            {
                var obj = _pool.DequeueObject(config.Value);
                obj.SetCutscenePreset(config.Value);
                _objectByID[config.Key] = obj;
            }

            // Init
            foreach (var item in container.GetTable<VCamFollowData>())
            {
                VCamFollow cam = (VCamFollow)_objectByID[item.Key];
                cam.SetFollow(GetObject(data, item.Value.TargetID).transform);
            }
        }

        private void BindingTrack(CutsceneData cutsceneData)
        {
            foreach (var item in _trackByName)
            {
                var track = item.Value;

                if (track is MarkerTrack)
                {
                    _director.SetGenericBinding(track, this);
                }
                else if (track is CinemachineTrack)
                {
                    _director.SetGenericBinding(track, _cameraBrain);

                    foreach (var clip in track.GetClips())
                    {
                        var id = cutsceneData.BindingIDByTrack[clip.displayName];

                        var shot = clip.asset as CinemachineShot;
                        IVCam vcam = (IVCam)GetObject(cutsceneData, id);
                        _director.SetReferenceValue(shot.VirtualCamera.exposedName, vcam.CinemachineCamera);

                    }
                }
                else if (track is AnimationTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, ((SingleMesh)GetObject(cutsceneData, id)).Animator);
                }
                else if (track is ActivationTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, GetObject(cutsceneData, id).transform.gameObject);
                }
                else if (track is InGameTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, (MonoBehaviour)GetObject(cutsceneData, id));
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

        private void OnStarted(int id)
        {
            CutsceneStarted?.Invoke(id);
            Play(id);
        }

        private void OnTimelineStopped(PlayableDirector director)
        {
            ClearPlayingCutscene();
            CutsceneStopped?.Invoke(_playID);
        }

        private ICutsceneObject GetObject(CutsceneData data, int id)
        {
            return data.BindingSourceByID[id] switch
            {
                BindingSource.Spawn => _objectByID[id],
                BindingSource.SceneObject => _sceneObject[data.SceneObjectBindingIDs[id]],
                BindingSource.Slot => (ICutsceneObject)_uniqueEntity.GetEntity(data.BindingSlots[id]),
                _ => throw new System.Exception(),
            };
        }

        public CutsceneData GetCutsceneData(int id)
        {
            return _dataByTriggerID[id];
        }

#if (UNITY_EDITOR)
        [ContextMenu("Play")]
        private void PlayCutsceneOnEditMode()
        {
            if (_objectByID.Count == 0)
            {
                var arr = transform.parent.GetComponentsInChildren<CutsceneObjectPreset>();
                foreach (var item in arr)
                {
                    _objectByID.Add(item.ObjectID, item.GetComponent<ICutsceneObject>());
                }
            }

            _director.RebuildGraph();
            _director.time = 0;
            _director.Evaluate();
            _director.Play();
        }
#endif
    }
}
