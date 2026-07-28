using Chu.Core;
using Chu.Utility;
using System;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutscenePool
    {
        private const string VCamStaticPath = "VCamStatic";
        private const string VCamFollowPath = "VCamFollow";
        private const string SingleMeshPath = "SingleMesh";

        private readonly Dictionary<Type, ObjectPooling<IOnlyCutscene>> _objPool;
        private readonly Dictionary<Type, Type> _dataTypeByObjectType;

        public CutscenePool()
        {
            _objPool = new()
            {
                { typeof(VCamStatic), new(() => AssetManager.GenerateLoadAssetSync<VCamStatic>(VCamStaticPath), a => a.SetActive(true))},
                { typeof(VCamFollow), new(() => AssetManager.GenerateLoadAssetSync<VCamFollow>(VCamFollowPath), a => a.SetActive(true))},
                { typeof(SingleMesh), new(() => AssetManager.GenerateLoadAssetSync<SingleMesh>(SingleMeshPath), a => a.SetActive(true))},
            };

            _dataTypeByObjectType = new()
            {
                { typeof(VCamStaticData), typeof(VCamStatic) },
                { typeof(VCamFollowData), typeof(VCamFollow) },
                { typeof(SingleMeshData), typeof(SingleMesh) },
            };

        }

        public ICutsceneObject DequeueObject(ICutscenePreset config)
        {
            Type type = _dataTypeByObjectType[config.GetType()];

            if (_objPool.TryGetValue(type, out var pool) == false)
                throw new Exception();

            return pool.Dequeue();
        }

        public void EnqueueObject(IOnlyCutscene obj)
        {
            Type type = obj.GetType();

            if (_objPool.TryGetValue(type, out var pool) == false)
                throw new Exception();

            pool.Enqueue(obj);
        }
    }
}
