using Chu.Utility;
using SAB.GameSystem;
using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    public class CutsceneDataExtractor : DataExporter
    {
        [SerializeField]
        private MapType _mapType;
        [SerializeField]
        private DefaultAsset _generatorFolder;
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private int _searchDepth = 1;

        private int _idCount;
        private Dictionary<GameObject, int> _idBySceneObject;

        protected override string DataPath
        {
            get => Path.Combine(AssetDatabase.GetAssetPath(_generatorFolder), string.Format(Const.Asset_DB_Cutscene, _mapType));
        }

        public CutsceneDataDto[] GetCutsceneData()
        {
            List<CutsceneDataDto> cutSceneDatas = new();
            List<CutscenePreset> directors = _root.GetComponentsWithDepth<CutscenePreset>(_searchDepth);

            foreach (var director in directors)
            {
                _idCount = 0;
                _idBySceneObject = new();
                var playable = director.PlayableDirector;
                CutsceneDataDto cutsceneData = new(playable.playableAsset.name, director.LockPlayer, new());

                foreach (var track in ((TimelineAsset)playable.playableAsset).GetOutputTracks())
                {
                    if (track is MarkerTrack)
                        continue;

                    if (track is CinemachineTrack camTrack)
                        ExtractCameraTrackData(cutsceneData, playable, camTrack);
                    else
                    {
                        var bindingTrack = playable.GetGenericBinding(track);
                        if (bindingTrack == null)
                            continue;

                        AddSceneObject(cutsceneData, bindingTrack.GameObject(), track.name);
                    }
                }

                cutSceneDatas.Add(cutsceneData);
            }

            return cutSceneDatas.ToArray();
        }

        protected override object GenerateData()
        {
            return GetCutsceneData();
        }

        private void ExtractCameraTrackData(CutsceneDataDto dto, PlayableDirector director, CinemachineTrack track)
        {
            foreach (var clip in track.GetClips())
            {
                if (clip.asset is not CinemachineShot shot)
                    continue;

                var virtualCamera = shot.VirtualCamera.Resolve(director);
                if (virtualCamera == null)
                    throw new System.Exception($"Camera binding null, Cutscene : {director.name}, clip : {clip.displayName}");

                AddSceneObject(dto, virtualCamera.gameObject, clip.displayName);
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
