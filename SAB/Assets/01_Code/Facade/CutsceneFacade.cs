using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.GameSystem;
using SAB.Unit;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Facade
{
    public class CutsceneFacade
    {
        private CutsceneController _cutscene;
        private AgentController _agent;
        private EntityContainer _entityContainer;

        private Dictionary<int, ICutsceneObject> _objectByActorID;

        public CutsceneFacade(CutsceneController cutscene, AgentController agent, EntityContainer entityContainer)
        {
            _objectByActorID = new();

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

            _objectByActorID.Clear();

            var data = DataBase.Instance.CutSceneRepo.DataByName[name];
            _agent.Player.SetActive(!data.LockPlayer);
            PrepareObject(_cutscene, data);
            _cutscene.Play(data, _objectByActorID);
        }

        private void PrepareObject(CutsceneController controller, CutsceneData cutsceneData)
        {
            foreach (var item in cutsceneData.CharacterData)
            {
                var obj = CharacterFactory.Instance.Create(item.Value);
                _objectByActorID.Add(item.Key, obj);
            }

            foreach (var item in cutsceneData.StaticData)
            {
                var obj = CameraFactory.Instance.CreateStatic(item.Value);
                _objectByActorID.Add(item.Key, obj);
            }

            foreach (var item in cutsceneData.FollowData)
            {
                var obj = CameraFactory.Instance.CreateFollow(item.Value);
                _objectByActorID.Add(item.Key, obj);
            }

            foreach (var item in cutsceneData.FavoritesByID)
            {
                if (_entityContainer.ObjectByFavorite[item.Value] is not ICutsceneObject cutsceneobj)
                    throw new System.Exception("");

                _objectByActorID.Add(item.Key, cutsceneobj);
            }

            foreach (var item in cutsceneData.UniqueSlotByID)
            {
                if (_entityContainer.ObjectByUniqueType.TryGetValue(item.Value, out var obj) == false)
                    throw new System.Exception($"unique type is null\n{item.Value}");

                if (obj is not ICutsceneObject cutsceneobj)
                    throw new System.Exception("");

                _objectByActorID.Add(item.Key, cutsceneobj);
            }

            foreach (var item in cutsceneData.FollowData)
            {
                var obj = (VCamFollow)_objectByActorID[item.Key];
                obj.SetThirdPersonTarget(_objectByActorID[item.Value.TargetID].transform);
            }
        }

        public void Stop(CutsceneData data)
        {
            foreach (var item in _objectByActorID)
            {
                if (data.BindingSourceByID[item.Key] == BindingSource.SceneObject)
                    continue;
                if (data.BindingSourceByID[item.Key] == BindingSource.Slot)
                    continue;
                if (data.PersistentObjects.Contains(item.Key) == true)
                    continue;

                item.Value.SetActive(false);
            }

            if (data.LockPlayer == true)
            {
                _agent.Player.SetActive(true);
            }
        }
    }
}
