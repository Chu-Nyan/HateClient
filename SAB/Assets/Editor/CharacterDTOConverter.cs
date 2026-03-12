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
        File.WriteAllText(Path.Combine(_characterDBGeneratePath, _fileName), _characterCommon.text);
    }
}
