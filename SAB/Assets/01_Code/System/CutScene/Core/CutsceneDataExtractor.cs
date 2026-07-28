using Chu.Utility.Unity;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    public class CutsceneDataExtractor : MonoBehaviour
    {
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private int _searchDepth = 1;

        private int _idCount;
        private Dictionary<GameObject, int> _idBySceneObject;

        public CutsceneTriggerData[] GetTriggerData()
        {
            List<CutscenePreset> directors = Utility.GetComponentsWithDepth<CutscenePreset>(_root, _searchDepth);
            var datas = new CutsceneTriggerData[directors.Count];

            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = new()
                {
                    Name = directors[i].PlayableDirector.playableAsset.name,
                    CenterX = directors[i].Center.x,
                    CenterY = directors[i].Center.y,
                    TriggerZones = directors[i].TriggerZones
                };
            }

            return datas;
        }

        public CutsceneDataDto[] GetCutsceneData()
        {
            _idCount = 0;
            _idBySceneObject = new();

            List<CutsceneDataDto> cutSceneDatas = new();
            List<CutscenePreset> directors = Utility.GetComponentsWithDepth<CutscenePreset>(_root, _searchDepth);

            foreach (var director in directors)
            {
                Dictionary<string, int> objIDByTrackName = new();

                CutsceneDataDto cutsceneData = new(director.PlayableDirector.playableAsset.name, director.LockPlayer, objIDByTrackName);
                TimelineAsset timelineAssets = director.PlayableDirector.playableAsset as TimelineAsset;
                var tracks = timelineAssets.GetOutputTracks();

                foreach (var track in tracks)
                {
                    if (track is MarkerTrack)
                        continue;

                    if (track is CinemachineTrack camTrack)
                        ExtractCameraTrackData(cutsceneData, director.PlayableDirector, camTrack);
                    else
                    {
                        var bindingTrack = director.PlayableDirector.GetGenericBinding(track);
                        if (bindingTrack == null)
                            continue;

                        AddSceneObject(cutsceneData, bindingTrack.GameObject(), track.name);
                    }
                }

                cutSceneDatas.Add(cutsceneData);
            }

            return cutSceneDatas.ToArray();
        }

        private void ExtractCameraTrackData(CutsceneDataDto dto, PlayableDirector director, CinemachineTrack track)
        {
            foreach (var clip in track.GetClips())
            {
                var shot = clip.asset as CinemachineShot;
                if (shot == null)
                    continue;
                if (shot.VirtualCamera.Resolve(director).gameObject == null)
                    continue;

                AddSceneObject(dto, shot.VirtualCamera.Resolve(director).gameObject, clip.displayName);
            }
        }

        private void AddSceneObject(CutsceneDataDto dto, GameObject obj, string clipName)
        {
            int id = GetOrRegisterID(obj);
            dto.AddObject(id, obj, this);
            dto.AddTrackData(clipName, id);
        }

        public int GetOrRegisterID(GameObject obj)
        {
            if (_idBySceneObject.TryGetValue(obj, out int id) == false)
            {
                id = ++_idCount;
                _idBySceneObject[obj] = id;
            }

            return id;
        }
    }
}
