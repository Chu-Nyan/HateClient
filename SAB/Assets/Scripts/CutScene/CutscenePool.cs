using Chu.Core;
using Chu.Utility;
using System;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutscenePool
    {
        private readonly Dictionary<Type, ObjectPooling<ICutsceneObject>> _objPool;
        private readonly Dictionary<Type, Type> _dataTypeByObjectType;

        public CutscenePool()
        {
            _objPool = new()
            {
                { typeof(VCamStatic), new(() => AssetManager.GenerateLoadAssetSync<VCamStatic>(Const.Asset_VCamStatic), a => a.SetActive(true))},
                { typeof(VCamFollow), new(() => AssetManager.GenerateLoadAssetSync<VCamFollow>(Const.Asset_VCamFollow), a => a.SetActive(true))},
                { typeof(SingleMesh), new(() => AssetManager.GenerateLoadAssetSync<SingleMesh>(Const.Asset_SingleMesh), a => a.SetActive(true))},
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

            if (_objPool.TryGetValue(type, out var pool) == true)
                return pool.Dequeue();

            throw new Exception();
        }

        public void EnqueueObject(ICutsceneObject config)
        {
            Type type = config.GetType();

            if (_objPool.TryGetValue(type, out var pool) == true)
                pool.Enqueue(config);
        }
    }
}
