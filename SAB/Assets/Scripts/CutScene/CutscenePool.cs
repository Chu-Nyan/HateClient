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
        private readonly Dictionary<Type, Func<ICutscenePreset, ICutsceneObject>> _dequeue;
        private readonly Dictionary<Type, Action<ICutsceneObject>> _enqueue;
        private readonly Dictionary<Type, Type> _dataTypeByObjectType;

        public CutscenePool()
        {
            _triggerPool = new(() => new CollisionTrigger());
            _objPool = new()
            {
                { typeof(VCamStatic), new(() => AssetManager.GenerateLoadAssetSync<VCamStatic>(Const.Asset_VCamStatic), a => a.SetActive(true))},
                { typeof(VCamFollow), new(() => AssetManager.GenerateLoadAssetSync<VCamFollow>(Const.Asset_VCamFollow), a => a.SetActive(true))},
                { typeof(SingleMesh), new(() => AssetManager.GenerateLoadAssetSync<SingleMesh>(Const.Asset_SingleMesh), a => a.SetActive(true))},
            };

            _dequeue = new()
            {
                { typeof(Character), GetCharacter }
            };

            _enqueue = new()
            {
                { typeof(Character), (a) => CharacterGenerator.Instance.Enqueue((Character)a) }
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

        public ICutsceneObject DequeueObject(ICutscenePreset config)
        {
            Type type = _dataTypeByObjectType[config.GetType()];

            if (_objPool.TryGetValue(type, out var pool) == true)
                return pool.Dequeue();

            if (_dequeue.TryGetValue(type, out var func) == true)
                return func(config);

            throw new Exception();
        }

        private ICutsceneObject GetCharacter(ICutscenePreset data)
        {
            SpawnRequest request = (SpawnRequest)data;
            return CharacterGenerator.Instance.Ready(request.Position)
                .SetData(request.ID)
                .SetCustomizing(request.ID)
                .Release();
        }

        public void EnqueueObject(ICutsceneObject config)
        {
            Type type = config.GetType();

            if (_objPool.TryGetValue(type, out var pool) == true)
                pool.Enqueue(config);
            else if (_enqueue.TryGetValue(type, out var action) == true)
                action(config);
            else
                throw new Exception(type.ToString());
        }
    }
}
