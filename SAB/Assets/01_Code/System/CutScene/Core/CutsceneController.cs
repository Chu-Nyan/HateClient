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

        // Cutscene in progress
        private CutsceneData _cutsceneData;
        private Dictionary<string, TrackAsset> _trackByName;
        private Dictionary<int, IEntity> _objectByActorID;
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

            _trackByName = new();
            _director.stopped += OnTimelineStopped;
            _markerReceiver.Init(ShowDialog);
        }

        public void Init(CinemachineBrain brain)
        {
            _cameraBrain = brain;
        }

        public void Play(CutsceneData data, Dictionary<int, IEntity> objectByActorID)
        {
            _cutsceneData = data;
            _objectByActorID = objectByActorID;

            PlayableAsset playableAsset = AssetManager.LoadAssetSync<PlayableAsset>(data.Name);
            _director.playableAsset = playableAsset;
            CacheTracks(playableAsset);
            BindingTrack(data);

            _director.time = 0;
            _director.Evaluate();
            _director.Play();
            _isPlaying = true;
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
                        IVCam vcam = (IVCam)_objectByActorID[id];
                        _director.SetReferenceValue(shot.VirtualCamera.exposedName, vcam.CinemachineCamera);

                    }
                }
                else if (track is AnimationTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, (_objectByActorID[id].transform.GetComponent<Animator>()));
                }
                else if (track is ActivationTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, _objectByActorID[id].transform.gameObject);
                }
                else if (track is InGameTrack)
                {
                    var id = cutsceneData.BindingIDByTrack[track.name];
                    _director.SetGenericBinding(track, (MonoBehaviour)_objectByActorID[id]);
                }
            }
        }

        private void ShowDialog(DialogMarker marker)
        {
            var id = _cutsceneData.BindingIDByTrack[marker.SpeakerTrack];

            if (_objectByActorID[id] is ISpeachable able)
            {
                able.Speech(new(0, marker.TextID, marker.Time), true);
            }
            else
            {
                var ui = UIManager.Instance.GetUI<SpeechBubbleUI>();
                ui.ShowDialogue(_objectByActorID[id].transform, new(0, marker.TextID, marker.Time), true);
            }
        }

        private void OnTimelineStopped(PlayableDirector director)
        {
            _isPlaying = false;
            CutsceneStopped?.Invoke(_cutsceneData);
            _objectByActorID.Clear();
            _trackByName.Clear();
        }
    }
}
