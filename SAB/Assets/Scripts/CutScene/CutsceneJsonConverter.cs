using Chu.Utility;
using Chu.Utility.Json;
using Chu.Utility.UnityHelper;
using Newtonsoft.Json;
using SAB.DataManger;
using System;
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

        public void ExportJson()
        {
            List<CutsceneDataDto> cutSceneDatas = new();
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
                    if (track is CinemachineTrack camTrack)
                    {
                        ExtractCameraTrackData(objIDByTrackName, cutsceneData, director.PlayableDirector, camTrack);
                        continue;
                    }

                    var bindingTrack = director.PlayableDirector.GetGenericBinding(track);
                    if (bindingTrack == null)
                        continue;

                    var cutsceneObj = bindingTrack.GetComponent<CutsceneObject>();
                    if (cutsceneObj == null)
                        continue;

                    cutsceneData.AddObjectData(cutsceneObj.ID, cutsceneObj.GetCutsceneObjectData());
                    TryAddDictionary(objIDByTrackName, track.name, cutsceneObj.ID);
                }

                cutSceneDatas.Add(cutsceneData);
            }

            string json = JsonConvert.SerializeObject(cutSceneDatas, Formatting.Indented, new JsonSerializerSettings().WithUnity());
            AssetDatabase.GetAssetPath(_path);
            string fileName = $"{string.Format(CutSceneRepository.FileNameFormat, _mapType)}";
            Utility.GenerateFile(AssetDatabase.GetAssetPath(_path), $"{fileName}.json", json);
            Debug.Log("Cutscene Data Exported");
        }

        private void ExtractCameraTrackData(Dictionary<string, int> objIdByTrackName, CutsceneDataDto dto, PlayableDirector director, CinemachineTrack track)
        {
            foreach (var clip in track.GetClips())
            {
                var shot = clip.asset as CinemachineShot;
                if (shot == null)
                    continue;
                if (shot.VirtualCamera.Resolve(director).TryGetComponent<CutsceneObject>(out var cutsceneObj) == false)
                    continue;

                dto.AddObjectData(cutsceneObj.ID, cutsceneObj.GetCutsceneObjectData());
                TryAddDictionary(objIdByTrackName, clip.displayName, cutsceneObj.ID);
            }
        }

        private void TryAddDictionary(Dictionary<string, int> dic, string key, int value)
        {
            if (dic.TryAdd(key, value) == false
             && dic[key] != value)
            {
                throw new Exception(string.Format(ErrorMessages.DuplicateKeyMismatched, key, dic[key], value));
            }
        }
    }
}
