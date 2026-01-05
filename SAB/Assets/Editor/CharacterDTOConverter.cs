using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDTOConverter", menuName = "Scriptable Objects/CharacterDTOConverter", order = 2)]
public class CharacterDTOConverter : ScriptableObject
{
    [SerializeField]
    private TextAsset _characterCommon;
    [SerializeField]
    private string _characterDBGeneratePath;

    [ContextMenu("GOGO")]
    public void ConvertCharacterDTO()
    {
        var baseDTO = JsonConvert.DeserializeObject<Unit_CommonDTO[]>(_characterCommon.text);

        for (int i = 0; i < baseDTO.Length; i++)
        {
        }

    }

}
