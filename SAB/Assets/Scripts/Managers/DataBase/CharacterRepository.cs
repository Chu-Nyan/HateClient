using SAB.Unit;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class CharacterRepository
    {
        private const string _characterBaseJson = "Dto_Character_Base_Stats";
        private const string _customizingJson = "Dto_Character_Customizing";
        private const string _defaultEquipmentJson = "Dto_Character_DefaultEquipment";

        public readonly Dictionary<int, BaseStats> CharacterBaseData;
        public readonly Dictionary<int, CustomizingData> CustomizingData;
        public readonly Dictionary<int, ItemType[]> DefaultEquipment;

        public CharacterRepository()
        {
            CharacterBaseData = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<CharacterBaseStatsDto[]>(_characterBaseJson),
                keySelector: a => a.ID,
                converter: a => new BaseStats(a.ID, a.Name, a.Desc, new[] { a.HP, a.ATK, a.PDEF, a.MDEF, a.SPD })
                );

            CustomizingData = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<CharacterCustomizingDto[]>(_customizingJson),
                keySelector: a => a.ID,
                converter: a => new CustomizingData(a)
                );

            DefaultEquipment = DataBase.DeserializeArrayByKey(
                dtos: AssetManager.DeserializeJsonSync<CharacterDefaultEquipmentDto[]>(_defaultEquipmentJson),
                keySelector: a => a.ID,
                converter: a => (ItemType)a.EquipmentID
                );
        }
    }
}
