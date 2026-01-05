using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDTOConverter", menuName = "Scriptable Objects/CharacterDTOConverter", order = 2)]
public class CharacterDTOConverter : ScriptableObject
{
    [SerializeField]
    private TextAsset _characterCommon;
    [SerializeField]
    private string _characterDBGeneratePath;
    private string _fileName = "CharacterData.json";

    [ContextMenu("GOGO")]
    public void ConvertCharacterDTO()
    {
        var baseDTO = JsonConvert.DeserializeObject<Character_Base_Stats_DTO[]>(_characterCommon.text);
        var data = new Dictionary<UnitType, CharacterBaseStats>();

        for (int i = 0; i < baseDTO.Length; i++)
        {
            CharacterBaseStats baseStats = ToBaseStats(baseDTO[i]);
            data.Add(baseStats.Type, baseStats);
        }

        var text = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(Path.Combine(_characterDBGeneratePath, _fileName), text);
    }

    public static CharacterBaseStats ToBaseStats(Character_Base_Stats_DTO dto)
    {
        return new CharacterBaseStats
        {
            NameID = dto.Name,
            DescID = dto.Desc,
            Type = dto.Type,
            HP = dto.HP,
            ATK = dto.ATK,
            PDEF = dto.PDEF,
            MDEF = dto.MDEF,
            SPD = dto.SPD
        };
    }

}
