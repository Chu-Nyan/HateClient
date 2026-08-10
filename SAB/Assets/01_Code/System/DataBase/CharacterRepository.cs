using Chu.Core;
using SAB.Unit;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class CharacterRepository
    {
        private const string BasePath = "CharacterBaseStatsData";
        private const string CustomizingPath = "HumanCustomizingData";
        private const string DefaultEquipmentPath = "HumanDefaultEquipmentData";

        public readonly Dictionary<int, BaseStats> CharacterBaseData;
        public readonly Dictionary<int, int> SkillSetIDByActerID;
        public readonly Dictionary<int, CustomizingData> CustomizingData;
        public readonly Dictionary<int, ItemType[]> DefaultEquipment;

        public CharacterRepository()
        {
            var baseDTO = DataBase.ConvertJsonToArray<CharacterBaseStatsDto>(AssetManager.LoadJson(BasePath));
            CharacterBaseData = DataBase.DeserializeObjectByKey(
                dtos: baseDTO,
                keySelector: a => a.ID,
                converter: a => new BaseStats(a.ID, a.Faction, a.NameKey, a.DescKey, new[] { a.HP, a.ATK, a.PDEF, a.MDEF, a.SPD })
            );

            SkillSetIDByActerID = DataBase.DeserializeObjectByKey(
                dtos: baseDTO,
                keySelector: a => a.ID,
                converter: a => a.SkillSetID
            );

            CustomizingData = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<HumanCustomizingDto>(AssetManager.LoadJson(CustomizingPath)),
                keySelector: a => a.ID,
                converter: a => new CustomizingData(a)
            );

            DefaultEquipment = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<HumanDefaultEquipmentDto>(AssetManager.LoadJson(DefaultEquipmentPath)),
                keySelector: a => a.ID,
                converter: a => (ItemType)a.EquipmentID
            );
        }
    }
}
