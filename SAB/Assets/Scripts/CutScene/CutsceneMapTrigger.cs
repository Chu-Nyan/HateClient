using Chu.Collision;
using Chu.Data;
using Chu.Utility;
using SAB.DataManger;
using System;
using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutsceneMapTrigger
    {
        private readonly ObjectPooling<CollisionTrigger> _triggerPooling;
        private readonly List<CollisionTrigger> _triggers;
        private readonly Dictionary<int, string> _cutsceneNameByTriggerID;

        public event Action<string> PlayRequested;

        public CutsceneMapTrigger()
        {
            _triggerPooling = new(() =>
            {
                var trigger = new CollisionTrigger();
                trigger.RegisterOnEntered(Play);
                return trigger;
            });
            _triggers = new List<CollisionTrigger>(16);
            _cutsceneNameByTriggerID = new();
        }

        public void Init(Action<string> playRequested)
        {
            PlayRequested = playRequested;
        }

        public void SetupMap(MapType type)
        {
            Clear();
            var cutsceneList = DataBase.Instance.CutSceneRepo.CutsceneNameByMapType[type];
            var allDatas = DataBase.Instance.CutSceneRepo.DataByName;

            foreach (var name in cutsceneList)
            {
                GenerateCutsceneTrigger(allDatas[name]);
            }
        }

        private void GenerateCutsceneTrigger(CutsceneData data)
        {
            CollisionTrigger trigger = _triggerPooling.Dequeue();
            IShape shape = ShapeParam.ConvertShape(data.TriggerZones);
            trigger.Setup(shape, new Pose2D(data.Center, 0), true); // TODO : 컷씬 활성화 여부
            _triggers.Add(trigger);
            _cutsceneNameByTriggerID.Add(trigger.ID, data.Name);
        }

        private void Clear()
        {
            foreach (var trigger in _triggers)
            {
                _triggerPooling.Enqueue(trigger);
            }
            _triggers.Clear();
        }

        private void Play(int id)
        {
            string name = _cutsceneNameByTriggerID[id];
            PlayRequested?.Invoke(name);
        }
    }
}
