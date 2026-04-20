using UnityEditor.AddressableAssets;
using UnityEngine;

namespace SAB.Cutscene
{
    public class GenericAniTrackObject : MonoBehaviour, ICutsceneSerializable
    {
        public int ID
        {
            get => gameObject.GetInstanceID();
        }

        public GenericAniTrackData GetData()
        {
            int id = gameObject.GetInstanceID();
            string name = gameObject.name;

            var filter = gameObject.GetComponent<MeshFilter>();
            return new GenericAniTrackData(id, name, GetAddressablePath(filter.sharedMesh));
        }

        private string GetAddressablePath(Mesh mesh)
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(mesh);
            string guid = UnityEditor.AssetDatabase.AssetPathToGUID(path);

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings.FindAssetEntry(guid);

            if (entry != null)
                return entry.address;
            else
                throw new System.Exception($"{mesh.name}, No Addressable Asset");
        }

        [ContextMenu("Print Data Log")]
        public void PrintDebugLog()
        {
            GenericAniTrackData data = GetData();
            Debug.Log(data.ToString());
        }
    }
}
