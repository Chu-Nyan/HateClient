using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.GameSystem;
using SAB.Unit;
using UnityEngine;

namespace SAB.Facade
{
    public class CutsceneFacade
    {
        private CutsceneController _cutscene;
        private AgentController _agent;
        private EntityContainer _entityContainer;

        public CutsceneFacade(CutsceneController cutscene, AgentController agent, EntityContainer entityContainer)
        {
            _cutscene = cutscene;
            _agent = agent;
            _entityContainer = entityContainer;

            cutscene.CutsceneStopped += Stop;
        }

        public void Play(string name)
        {
            if (_cutscene.IsPlaying == true)
            {
                Debug.LogWarning($"Multiple playback detected \nRequest : {name}");
                return;
            }

            var data = DataBase.Instance.CutSceneRepo.DataByName[name];
            _agent.Player.SetActive(!data.LockPlayer);
            PrepareCharacter(data);
            _cutscene.Play(data);
        }

        private void PrepareCharacter(CutsceneData cutsceneData)
        {
            if (cutsceneData.SpawnContainer.TryGetTable<CharacterSpawnRequest>(out var datas) == true)
            {
                foreach (var item in datas)
                {
                    var acterData = item.Value;
                    var acter = CharacterFactory.Instance.Create(item.Value.BrainType, acterData.UnitID, acterData.Position, acterData.Rotation);
                    _cutscene.RegisterObject(item.Key, acter);
                }
            }

            foreach (var item in cutsceneData.SceneObjectBindingIDs)
            {
                var obj = _entityContainer.ObjectByFavorite[item.Value.ToString()];
                if (obj is ICutsceneObject cutsceneobj)
                {
                    _cutscene.RegisterObject(item.Key, cutsceneobj);
                }
            }

            foreach (var item in cutsceneData.BindingSlots)
            {
                if (_entityContainer.ObjectByUniqueType.TryGetValue(item.Value, out var obj) == false)
                {
                    Debug.LogError($"{item.Value}, unique type is null");
                    obj = CharacterFactory.Instance.Create(BrainType.None, 1, Vector3.zero, Quaternion.identity);
                }

                if (obj is ICutsceneObject cutsceneobj)
                {
                    _cutscene.RegisterObject(item.Key, cutsceneobj);
                }
            }
        }

        public void Stop(CutsceneData data)
        {
            if (data.LockPlayer == true)
            {
                _agent.Player.SetActive(true);
            }
        }
    }
}
