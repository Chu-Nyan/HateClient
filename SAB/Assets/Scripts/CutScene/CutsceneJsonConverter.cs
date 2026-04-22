using Chu.Utility.UnityHelper;
using Newtonsoft.Json;
using SAB.DataManger;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<CutsceneData> cutSceneDatas = new();

            var authorings = Utility.GetComponentsWithDepth<CutsceneAuthoring>(_root, _searchDepth);
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            foreach (var authoring in authorings)
            {
                HashSet<string> nameChecker = new();
                Dictionary<string, int> binding = new();
                Dictionary<int, SingleMeshData> GenericAniData = new();
                List<CutsceneVCamData> cameraTracks = new();

                TimelineAsset timelineAssets = authoring.PlayableDirector.playableAsset as TimelineAsset;
                var tracks = timelineAssets.GetOutputTracks();

                foreach (var track in tracks)
                {
                    if (nameChecker.Add(track.name) == false)
                    {
                        Debug.LogError($"{track.name}, 중복된 트랙 이름");
                        continue;
                    }

                    if (track is CinemachineTrack cinemachineTrack)
                    {
                        cameraTracks.Add(CameraTrackToData(authoring.PlayableDirector, cinemachineTrack));
                        continue;
                    }

                    var bindingTrack = authoring.PlayableDirector.GetGenericBinding(track);
                    if (bindingTrack == null)
                        continue;

                    var serializableObj = bindingTrack.GetComponent<ICutsceneSerializable>();
                    if (serializableObj == null)
                        continue;

                    binding.Add(track.name, serializableObj.ID);
                    if (serializableObj is SingleMesh generic == true)
                        GenericAniData.TryAdd(serializableObj.ID, generic.GetData(track.name));
                }

                string path = AssetDatabase.GetAssetPath(authoring.PlayableDirector.playableAsset);
                string guid = AssetDatabase.AssetPathToGUID(path);
                var entry = settings.FindAssetEntry(guid);

                CutsceneData data = new(
                    name: authoring.name,
                    assetPath: entry.address,
                    center: authoring.Center,
                    triggerZones: authoring.TriggerZone,
                    idByTrack: binding,
                    singleMesh: GenericAniData,
                    vcamDatas: cameraTracks
                    );

                cutSceneDatas.Add(data);
            }

            string json = JsonConvert.SerializeObject(cutSceneDatas, Formatting.Indented);
            AssetDatabase.GetAssetPath(_path);
            string fileName = $"{string.Format(CutSceneRepository.FileNameFormat, _mapType)}";
            Utility.GenerateFile(AssetDatabase.GetAssetPath(_path), $"{fileName}.json", json);
        }

        private CutsceneVCamData CameraTrackToData(PlayableDirector director, CinemachineTrack track)
        {
            var vcamData = CameraClipToData(director, track);
            var staticDic = vcamData[typeof(VCamStaticData)].OfType<VCamStaticData>().ToDictionary((k) => k.ID);
            var followDic = vcamData[typeof(VCamFollowData)].OfType<VCamFollowData>().ToDictionary((k) => k.ID);
            var trackData = new CutsceneVCamData
            {
                Name = track.name,
                VCamIDByClipName = GetVCamClipBinding(director, track),
                StaticData = staticDic,
                FollowData = followDic
            };

            return trackData;
        }

        private Dictionary<string, int> GetVCamClipBinding(PlayableDirector director, CinemachineTrack track)
        {
            Dictionary<string, int> dic = new();

            foreach (var clip in track.GetClips())
            {
                var shot = clip.asset as CinemachineShot;
                if (shot == null)
                    continue;
                var vcam = shot.VirtualCamera.Resolve(director);
                if (vcam == null)
                    continue;

                dic.Add(clip.displayName, vcam.gameObject.GetInstanceID());
            }

            return dic;
        }

        private Dictionary<Type, List<IVCamData>> CameraClipToData(PlayableDirector director, CinemachineTrack track)
        {
            Dictionary<Type, List<IVCamData>> dic = new();

            foreach (var clip in track.GetClips())
            {
                var shot = clip.asset as CinemachineShot;
                if (shot == null)
                    continue;
                var vcam = shot.VirtualCamera.Resolve(director).GetComponent<IVCam>();
                if (vcam == null)
                    continue;

                IVCamData data = vcam.GetSerializedData();
                Type type = data.GetType();
                dic.TryAdd(type, new());
                dic[type].Add(data);
            }

            return dic;
        }
    }
}
