using Chu.Core;
using Chu.Utility;
using SAB.Unit;
using System;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutscenePool
    {
        private readonly ObjectPooling<CollisionTrigger> _triggerPool;
        private readonly Dictionary<Type, ObjectPooling<ICutsceneObject>> _objPool;
        private readonly Dictionary<Type, Func<IObjectConfig, ICutsceneObject>> _dequeue;
        private readonly Dictionary<Type, Action<ICutsceneObject>> _enqueue;
        private readonly Dictionary<Type, Type> _dataTypeByObjectType;

        public CutscenePool()
        {
            _triggerPool = new(() => new CollisionTrigger());
            _objPool = new()
            {
                { typeof(VCamStaticData), new(() => AssetManager.GenerateLoadAssetSync<VCamStatic>(Const.Asset_VCamStatic), a => a.SetActive(true))},
                { typeof(VCamFollowData), new(() => AssetManager.GenerateLoadAssetSync<VCamFollow>(Const.Asset_VCamFollow), a => a.SetActive(true))},
                { typeof(SingleMeshData), new(() => AssetManager.GenerateLoadAssetSync<SingleMesh>(Const.Asset_SingleMesh), a => a.SetActive(true))},
            };

            _dequeue = new()
            {
                { typeof(SpawnRequest), GetCharacter }
            };

            _enqueue = new()
            {
                { typeof(SpawnRequest), (a) => CharacterGenerator.Instance.Enqueue((Character)a) }
            };

            _dataTypeByObjectType = new()
            {
                { typeof(VCamStaticData), typeof(VCamStatic) },
                { typeof(VCamFollowData), typeof(VCamFollow) },
                { typeof(SingleMeshData), typeof(SingleMesh) },
                { typeof(SpawnRequest), typeof(Character) },
            };
        }

        public CollisionTrigger DequeueTrigger()
        {
            return _triggerPool.Dequeue();
        }

        public void EnqueueTrigger(CollisionTrigger trigger)
        {
            _triggerPool.Enqueue(trigger);
        }

        public ICutsceneObject DequeueObject(IObjectConfig config)
        {
            Type type = config.GetType();

            if (_objPool.TryGetValue(type, out var pool) == true)
                return pool.Dequeue();

            if (_dequeue.TryGetValue(type, out var func) == true)
                return func(config);

            throw new Exception();
        }

        private ICutsceneObject GetCharacter(IObjectConfig data)
        {
            SpawnRequest request = (SpawnRequest)data;
            return CharacterGenerator.Instance.Ready(request.Position)
                .SetData(request.ID)
                .SetCustomizing(request.ID)
                .Release();
        }

        public void EnqueueObject(ICutsceneObject config)
        {
            Type type = _dataTypeByObjectType[config.GetType()];

            if (_objPool.TryGetValue(type, out var pool) == true)
                pool.Enqueue(config);

            if (_enqueue.TryGetValue(type, out var action) == true)
                action(config);

            throw new Exception();
        }
    }
}
