using Chu.Utility.Json;
using Chu.Utility.Unity;
using Newtonsoft.Json;
using SAB.DataManger;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SAB.Cutscene
{
    public class CutsceneJsonConverter : MonoBehaviour
    {
        [SerializeField]
        private MapType _mapType;
        [SerializeField]
        private Transform _root;
        [SerializeField]
        private int _searchDepth = 1;
        [SerializeField]
        private DefaultAsset _path;

        private int _idCount;
        private Dictionary<GameObject, int> _idBySceneObject;

        public void ExportJson()
        {
            List<CutsceneDataDto> cutSceneDatas = new();
            _idCount = 0;
            _idBySceneObject = new();

            List<CutsceneDirector> directors = Utility.GetComponentsWithDepth<CutsceneDirector>(_root, _searchDepth);
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            foreach (var director in directors)
            {
                Dictionary<string, int> objIDByTrackName = new();

                string path = AssetDatabase.GetAssetPath(director.PlayableDirector.playableAsset);
                string guid = AssetDatabase.AssetPathToGUID(path);
                var entry = settings.FindAssetEntry(guid);

                CutsceneDataDto cutsceneData = new(
                    name: director.PlayableDirector.playableAsset.name,
                    assetPath: entry.address,
                    center: director.Center,
                    triggerZones: director.TriggerZone,
                    idByTrack: objIDByTrackName
                    );

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

            string json = JsonConvert.SerializeObject(cutSceneDatas, Formatting.Indented, new JsonSerializerSettings().WithUnity());
            AssetDatabase.GetAssetPath(_path);
            string fileName = $"{string.Format(CutSceneRepository.FileNameFormat, _mapType)}";
            Utility.GenerateFile(AssetDatabase.GetAssetPath(_path), $"{fileName}.json", json);
            Debug.Log("Cutscene Data Exported");
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
