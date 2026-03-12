using SAB.Unit;
using System.Collections.Generic;

namespace SAB.DataManger
{
    public class CharacterRepository
    {
        private const string _characterBaseJson = "CharacterBaseData";

        public readonly Dictionary<int, BaseStats> CharacterBaseData;

        public CharacterRepository()
        {
            CharacterBaseData = DataBase.DeserializeObjectByKey(
                dtos: AssetManager.DeserializeJsonSync<Character_Base_Stats_DTO[]>(_characterBaseJson),
                keySelector: a => a.ID,
                converter: a => new BaseStats(a.ID, a.Name, a.Desc, new[] { a.HP, a.ATK, a.PDEF, a.MDEF, a.SPD })
                );
        }
    }
}
