using Chu.Core;
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
        [SerializeField]
        private PlayableDirector _director;
        [SerializeField]
        private CinemachineBrain _cameraBrain;
        [SerializeField]
        private MarkerReceiver _markerReceiver;

        private readonly CutscenePool _pool = new();
        // Map
        private readonly Dictionary<int, ICutsceneObject> _sceneObjectByID = new();
        private UniqueEntityContainer _uniqueEntity;

        // Cutscene in progress
        private CutsceneData _cutsceneData;
        private readonly Dictionary<string, TrackAsset> _trackByName = new();
        private readonly Dictionary<int, ICutsceneObject> _objectByID = new();

        public event Action<CutsceneData> CutsceneStopped;

        private void Awake()
        {
            if (_cameraBrain == null && Camera.main.TryGetComponent<CinemachineBrain>(out var brain) == true)
                _cameraBrain = brain;

            _director.stopped += OnTimelineStopped;
            _markerReceiver.Init(ShowDialog);
        }

        public void Init(CinemachineBrain brain, UniqueEntityContainer uniqueEntity)
        {
            _cameraBrain = brain;
            _uniqueEntity = uniqueEntity;
        }

        public void SetupMap(MapReferenceHub hub)
        {
            _sceneObjectByID.Clear();
            foreach (var item in hub.Characters)
            {
                _sceneObjectByID.Add(item.Key, item.Value);
            }
        }

        public void Play(CutsceneData data)
        {
            _cutsceneData = data;
            ClearPlayingCutscene();
            PlayableAsset playableAsset = AssetManager.LoadAssetSync<PlayableAsset>(data.AssetPath);
            _director.playableAsset = playableAsset;
            CacheTracks(playableAsset);
            PrepareCutsceneObject(data);
            BindingTrack(data);

            _director.RebuildGraph();
            _director.time = 0;
            _director.Evaluate();
            _director.Play();
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
                    _director.SetGenericBinding(track, (GetObject(cutsceneData, id).transform.GetComponent<Animator>()));
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

        public void ClearPlayingCutscene()
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

        private void OnTimelineStopped(PlayableDirector director)
        {
            ClearPlayingCutscene();
            CutsceneStopped?.Invoke(_cutsceneData);
        }

        private ICutsceneObject GetObject(CutsceneData data, int id)
        {
            return data.BindingSourceByID[id] switch
            {
                BindingSource.Spawn => _objectByID[id],
                BindingSource.SceneObject => _sceneObjectByID[data.SceneObjectBindingIDs[id]],
                BindingSource.Slot => (ICutsceneObject)_uniqueEntity.GetEntity(data.BindingSlots[id]),
                _ => throw new System.Exception(),
            };
        }

        private bool TryGetPlayObject(CutsceneData data, int id, out ICutsceneObject obj)
        {
            obj = null;
            if (data.BindingSourceByID.ContainsKey(id) == false)
                return false;

            obj = GetObject(data, id);
            return true;
        }

        private void ShowDialog(DialogMarker marker)
        {
            var id = _cutsceneData.BindingIDByTrack[marker.SpeakerTrack];
            if (TryGetPlayObject(_cutsceneData, id, out var obj) == true)
            {
                if (obj is ISpeachable able)
                {
                    able.Speech(new(0, marker.TextID, marker.Time), true);
                }
            }
        }
    }
}
