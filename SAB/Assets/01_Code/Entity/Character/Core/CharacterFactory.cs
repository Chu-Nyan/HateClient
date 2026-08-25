using Chu.Core;
using Chu.Utility;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.GameSystem;
using SAB.Item;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.Unit
{
    public class CharacterFactory : Singleton<CharacterFactory>
    {
        private const string Asset_Obj_Character = "Character";

        private readonly Dictionary<int, BaseStats> _unitDatas;
        private readonly Dictionary<int, CustomizingData> _customizingData;

        private readonly Transform _root;
        private readonly ObjectPooling<Character> _pool;
        private AgentController _agentController;
        private EntityContainer _entityContainer;

        public CharacterFactory(DataBase db)
        {
            _root = new GameObject("Characters").transform;
            _unitDatas = db.CharacterRepo.CharacterBaseData;
            _customizingData = db.CharacterRepo.CustomizingData;
            _pool = new(() =>
            {
                return AssetManager.GenerateLoadAssetSync<Character>(Asset_Obj_Character, _root);
            });
        }

        public void Init(AgentController agent, EntityContainer container)
        {
            _agentController = agent;
            _entityContainer = container;
        }

        public Character Create(BrainType type, int unitID, Vector3 pos, Quaternion rot)
        {
            var acter = _pool.Dequeue();
            InitCharacter(acter, type, unitID, pos, rot);
            return acter;
        }

        public Character Create(BrainType type, int unitID, Vector3 pos, Quaternion rot, string favoriteID)
        {
            var acter = Create(type, unitID, pos, rot);
            _entityContainer.AddFavoriteObject(favoriteID, acter);
            return acter;
        }

        public Character Create(BrainType type, int unitID, Vector3 pos, Quaternion rot, UniqueEntityType uniqueType)
        {
            var acter = Create(type, unitID, pos, rot);
            _entityContainer.AddUniqueObject(uniqueType, acter);
            return acter;
        }

        private void InitCharacter(Character acter, BrainType type, int unitID, Vector3 pos, Quaternion rot)
        {
            var skill = DataBase.Instance.SkillRepo.SkillSet[_unitDatas[unitID].DefaultSkillSetID];
            acter.SetupStats(_unitDatas[unitID], skill);
            acter.SetupNavMesh(pos);
            acter.transform.rotation = rot;
            SetCustomizing(acter, unitID);
            SetEquipment(acter, unitID);
            acter.RegisterDeactivated(Release);

            _agentController.BindReceiver(acter, type);
            _entityContainer.AddCharacter(acter);
        }

        private void SetEquipment(Character acter, int unitID)
        {
            var equipments = DataBase.Instance.CharacterRepo.DefaultEquipment[unitID];
            foreach (var id in equipments)
            {
                var item = ItemFactory.Instance.GenerateItem((int)id);
                acter.Equip(item as IHasEquipmentData);
            }
        }

        public void LateInitialize(Character acter)
        {
            InitCharacter(acter, BrainType.None, acter.Stats.CharacterID, acter.transform.position, acter.transform.rotation);
        }

        public void LateInitialize(Character acter, int favoriteID)
        {
            InitCharacter(acter, BrainType.None, acter.Stats.CharacterID, acter.transform.position, acter.transform.rotation);
            _entityContainer.AddFavoriteObject(favoriteID.ToString(), acter);
        }

        private void SetCustomizing(Character acter, CustomizingData data)
        {
            acter.SetCustomizing(CustomizingPart.Hair, data.Hair);
            acter.SetCustomizing(CustomizingPart.Mouth, data.Mouth);
            acter.SetCustomizing(CustomizingPart.Eyebrow, data.Eyebrow);
            acter.SetCustomizing(CustomizingPart.Eye, data.Eye);
        }

        private void SetCustomizing(Character acter, int id)
        {
            if (_customizingData.TryGetValue(id, out var data) == false)
                return;

            SetCustomizing(acter, data);
        }

        public void Release(Character character)
        {
            _pool.Enqueue(character);
            _entityContainer.RemoveCharacter(character);
        }
    }
}
