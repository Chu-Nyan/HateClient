using Chu.Collision;
using Chu.Data;
using Chu.Utility;
using System;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutsceneMapTrigger
    {
        private readonly ObjectPooling<CollisionTrigger> _triggerPooling;
        private readonly List<CollisionTrigger> _triggers;
        private readonly Dictionary<int, CutsceneTriggerData> _dataByTriggerID;

        public Action<string> TriggerEnterAction;

        public CutsceneMapTrigger()
        {
            _triggerPooling = new(() =>
            {
                var trigger = new CollisionTrigger();
                trigger.RegisterOnEntered(PlayEnterAction);
                return trigger;
            });
            _triggers = new List<CollisionTrigger>(16);
            _dataByTriggerID = new();
        }

        public void Init(Action<string> triggerEnterAction)
        {
            TriggerEnterAction = triggerEnterAction;
        }

        public void SetupMap(CutsceneTriggerData[] datas)
        {
            Clear();

            foreach (var item in datas)
            {
                CollisionTrigger trigger = _triggerPooling.Dequeue();
                IShape shape = ShapeParam.ConvertShape(item.TriggerZones);
                trigger.Setup(shape, new Pose2D(item.Center, 0), true);
                _triggers.Add(trigger);
                _dataByTriggerID.Add(trigger.ID, item);
            }
        }

        private void Clear()
        {
            foreach (var trigger in _triggers)
            {
                _triggerPooling.Enqueue(trigger);
            }
            _triggers.Clear();
        }

        private void PlayEnterAction(int id)
        {
            TriggerEnterAction?.Invoke(_dataByTriggerID[id].Name);
        }
    }
}
