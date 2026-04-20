using Chu.Utility.UnityHelper;
using Newtonsoft.Json;
using SAB.DataManger;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;
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
            List<CutsceneData> cutSceneDatas = new();

            var authorings = Utility.GetComponentsWithDepth<CutsceneAuthoring>(_root, _searchDepth);
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            foreach (var authoring in authorings)
            {
                HashSet<string> nameChecker = new();
                Dictionary<string, int> binding = new();
                Dictionary<int, GenericAniTrackData> GenericAniData = new();

                TimelineAsset timelineAssets = authoring.PlayableDirector.playableAsset as TimelineAsset;
                var tracks = timelineAssets.GetOutputTracks();

                foreach (var track in tracks)
                {
                    if (nameChecker.Add(track.name) == false)
                    {
                        Debug.LogError($"{track.name}, 중복된 트랙 이름");
                        continue;
                    }

                    var bindingTrack = authoring.PlayableDirector.GetGenericBinding(track);
                    if (bindingTrack == null)
                        continue;

                    var serializableObj = bindingTrack.GetComponent<ICutsceneSerializable>();
                    if (serializableObj == null)
                        continue;

                    binding.Add(track.name, serializableObj.ID);
                    if (serializableObj is GenericAniTrackObject generic == true)
                        GenericAniData.TryAdd(serializableObj.ID, generic.GetData());
                }

                string path = AssetDatabase.GetAssetPath(authoring.PlayableDirector.playableAsset);
                string guid = AssetDatabase.AssetPathToGUID(path);
                var entry = settings.FindAssetEntry(guid);
                cutSceneDatas.Add(new(authoring.name, entry.address, authoring.Center, authoring.TriggerZone, binding, GenericAniData));
            }

            string json = JsonConvert.SerializeObject(cutSceneDatas, Formatting.Indented);
            AssetDatabase.GetAssetPath(_path);
            string fileName = $"{string.Format(CutSceneRepository.FileNameFormat, _mapType)}";
            Utility.GenerateFile(AssetDatabase.GetAssetPath(_path), $"{fileName}.json", json);
        }
    }
}
