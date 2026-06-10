using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class ObjectDataContainer
    {
        public Dictionary<int, VCamStaticData> StaticDatas;
        public Dictionary<int, VCamFollowData> FollowDatas;
        public Dictionary<int, SingleMeshData> SingleMeshDatas;

        public ObjectDataContainer()
        {
            StaticDatas = new();
            FollowDatas = new();
            SingleMeshDatas = new();
        }

        public void Add(int id, IObjectConfig data)
        {
            if (data is VCamStaticData tatic)
                Add(id, tatic);
            else if (data is VCamFollowData follow)
                Add(id, follow);
            else if (data is SingleMeshData singleMesh)
                Add(id, singleMesh);
        }

        public void Add(int id, VCamStaticData data)
        {
            StaticDatas.Add(id, data);
        }

        public void Add(int id, VCamFollowData data)
        {
            FollowDatas.Add(id, data);
        }

        public void Add(int id, SingleMeshData data)
        {
            SingleMeshDatas.Add(id, data);
        }

        public IEnumerator<CutsceneObjectConfig> GetEnumerator()
        {
            foreach (var item in StaticDatas)
                yield return new(item.Key, CutsceneObjectType.VCamStatic, item.Value);
            foreach (var item in FollowDatas)
                yield return new(item.Key, CutsceneObjectType.VCamFollow, item.Value);
            foreach (var item in SingleMeshDatas)
                yield return new(item.Key, CutsceneObjectType.SingleMesh, item.Value);
        }
    }
}
