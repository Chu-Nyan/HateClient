using Chu.Core;
using SAB.Unit;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class CharacterRepository
    {
        private const string _characterBaseJson = "Dto_Character_Base_Stats";
        private const string _customizingJson = "Dto_Human_Customizing";
        private const string _defaultEquipmentJson = "Dto_Human_DefaultEquipment";

        public readonly Dictionary<int, BaseStats> CharacterBaseData;
        public readonly Dictionary<int, CustomizingData> CustomizingData;
        public readonly Dictionary<int, ItemType[]> DefaultEquipment;

        public CharacterRepository()
        {
            CharacterBaseData = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<CharacterBaseStatsDto>(AssetManager.LoadJson(_characterBaseJson)),
                keySelector: a => a.ID,
                converter: a => new BaseStats(a.ID, a.Faction, a.Name, a.Desc, new[] { a.HP, a.ATK, a.PDEF, a.MDEF, a.SPD })
            );

            CustomizingData = DataBase.DeserializeObjectByKey(
                dtos: DataBase.ConvertJsonToArray<HumanCustomizingDto>(AssetManager.LoadJson(_customizingJson)),
                keySelector: a => a.ID,
                converter: a => new CustomizingData(a)
            );

            DefaultEquipment = DataBase.DeserializeArrayByKey(
                dtos: DataBase.ConvertJsonToArray<HumanDefaultEquipmentDto>(AssetManager.LoadJson(_defaultEquipmentJson)),
                keySelector: a => a.ID,
                converter: a => (ItemType)a.EquipmentID
            );
        }
    }
}
