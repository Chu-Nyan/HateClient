using Chu.Core;
using SAB.UI;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    public partial class CutsceneController : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector _director;
        [SerializeField]
        private CinemachineBrain _cameraBrain;
        [SerializeField]
        private MarkerReceiver _markerReceiver;

        private readonly CutscenePool _pool = new();

        // Cutscene in progress
        private CutsceneData _cutsceneData;
        private readonly Dictionary<string, TrackAsset> _trackByName = new();
        private readonly Dictionary<int, ICutsceneObject> _objectByBindingID = new();
        private bool _isPlaying;

        public event Action<CutsceneData> CutsceneStopped;

        public bool IsPlaying
        {
            get => _isPlaying;
        }

        private void Awake()
        {
            if (_cameraBrain == null && Camera.main.TryGetComponent<CinemachineBrain>(out var brain) == true)
                _cameraBrain = brain;

            _director.stopped += OnTimelineStopped;
            _markerReceiver.Init(ShowDialog);
        }

        public void Init(CinemachineBrain brain)
        {
            _cameraBrain = brain;
        }

        public void Play(CutsceneData data)
        {
            _isPlaying = true;
            ClearPlayingCutscene();

            _cutsceneData = data;
            PlayableAsset playableAsset = AssetManager.LoadAssetSync<PlayableAsset>(data.Name);
            _director.playableAsset = playableAsset;
            CacheTracks(playableAsset);
            PrepareCutsceneObject(data);
            BindingTrack(data);

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

        public void RegisterObject(int bindingID, ICutsceneObject obj)
        {
            if (_objectByBindingID.TryAdd(bindingID, obj) == false)
            {
                Debug.LogError($"{bindingID}, {obj}, Duplicate ID detected");
            }
        }

        private void PrepareCutsceneObject(CutsceneData data)
        {
            // Generate
            var container = data.SpawnContainer;

            foreach (var item in container)
            {
                var obj = _pool.DequeueObject(item.Value);
                obj.SetCutscenePreset(item.Value);
                RegisterObject(item.Key, obj);
            }

            // Init
            if (container.TryGetTable<VCamFollowData>(out var followDatas) == true)
            {
                foreach (var item in followDatas)
                {
                    VCamFollow cam = (VCamFollow)_objectByBindingID[item.Key];
                    cam.SetFollow(_objectByBindingID[item.Value.TargetID].transform);
                }
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
                        IVCam vcam = (IVCam)_objectByBindingID[id];
                        _director.SetReferenceValue(shot.VirtualCamera.exposedName, vcam.CinemachineCamera);

                    }
                }
                else if (track is AnimationTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, (_objectByBindingID[id].transform.GetComponent<Animator>()));
                }
                else if (track is ActivationTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, _objectByBindingID[id].transform.gameObject);
                }
                else if (track is InGameTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, (MonoBehaviour)_objectByBindingID[id]);
                }
            }
        }

        public void ClearPlayingCutscene()
        {
            if (_trackByName.Count == 0)
                return;

            foreach (var item in _objectByBindingID)
            {
                if (_cutsceneData.BindingSourceByID[item.Key] == BindingSource.SceneObject)
                    continue;
                if (_cutsceneData.BindingSourceByID[item.Key] == BindingSource.Slot)
                    continue;
                if (_cutsceneData.PersistentObjects.Contains(item.Key) == true)
                    continue;

                item.Value.SetActive(false);
                if (item.Value is IOnlyCutscene only)
                {
                    _pool.EnqueueObject(only);
                }
            }

            _objectByBindingID.Clear();
            _trackByName.Clear();
        }

        private void OnTimelineStopped(PlayableDirector director)
        {
            _isPlaying = false;
            ClearPlayingCutscene();
            CutsceneStopped?.Invoke(_cutsceneData);
        }

        private void ShowDialog(DialogMarker marker)
        {
            var id = _cutsceneData.BindingIDByTrack[marker.SpeakerTrack];

            if (_objectByBindingID[id] is ISpeachable able)
            {
                able.Speech(new(0, marker.TextID, marker.Time), true);
            }
            else
            {
                var ui = UIManager.Instance.GetUI<SpeechBubbleUI>();
                ui.ShowDialogue(_objectByBindingID[id].transform, new(0, marker.TextID, marker.Time), true);
            }
        }
    }
}
