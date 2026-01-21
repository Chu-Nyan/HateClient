using Newtonsoft.Json;
using SAB.Unit;
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
        var data = new Dictionary<UnitType, BaseStats>();

        for (int i = 0; i < baseDTO.Length; i++)
        {
            BaseStats baseStats = ToBaseStats(baseDTO[i]);
            data.Add(baseStats.Type, baseStats);
        }

        var text = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(Path.Combine(_characterDBGeneratePath, _fileName), text);
    }

    public BaseStats ToBaseStats(Character_Base_Stats_DTO dto)
    {
        var stats = new float[] 
        {
            dto.HP,
            dto.ATK,
            dto.PDEF,
            dto.MDEF,
            dto.SPD
        };

        return new BaseStats
        {
            NameID = dto.Name,
            DescID = dto.Desc,
            Type = dto.Type,
            Stats = stats
        };
    }
}
