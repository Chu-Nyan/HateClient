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
    private TextAsset _characterDropItem;
    [SerializeField]
    private TextAsset _characterLevel;
    [SerializeField]
    private string _characterDBGeneratePath;

    [ContextMenu("GOGO")]
    public void ConvertCharacterDTO()
    {
        var baseDTO = JsonConvert.DeserializeObject<Unit_CommonDTO[]>(_characterCommon.text);
        var dropDTO = JsonConvert.DeserializeObject<Unit_DropItemDTO[]>(_characterDropItem.text);
        var lvDTO = JsonConvert.DeserializeObject<Unit_LevelDTO[]>(_characterLevel.text);

        var dropTableDic = new Dictionary<UnitType, List<DropItem>>();
        for (int i = 0; i < dropDTO.Length; i++)
        {
            if (dropTableDic.ContainsKey(dropDTO[i].Unit) == false)
                dropTableDic.Add(dropDTO[i].Unit, new());

            var item = new DropItem
            {
                Item = dropDTO[i].Item,
                DropRate = dropDTO[i].DropRate,
                MinAmount = dropDTO[i].MinAmount,
                MaxAmount = dropDTO[i].MaxAmount
            };
            dropTableDic[dropDTO[i].Unit].Add(item);
        }

        var lvDic = new Dictionary<UnitType, Dictionary<int, LevelStats>>();
        for (int i = 0; i < lvDTO.Length; i++)
        {
            if (lvDic.ContainsKey(lvDTO[i].Type) == false)
                lvDic.Add(lvDTO[i].Type, new());

            var item = new LevelStats
            {
                HP = lvDTO[i].HP,
                MP = lvDTO[i].MP,
                EXP = lvDTO[i].EXP,
                ATK = lvDTO[i].ATK,
                PDEF = lvDTO[i].PDEF,
                MDEF = lvDTO[i].MDEF
            };

            lvDic[lvDTO[i].Type][lvDTO[i].Level] = item;
        }


        var baseData = new Dictionary<UnitType, CharacterBaseStats>();
        for (int i = 0; i < baseDTO.Length; i++)
        {
            var data = new CharacterBaseStats
            {
                Type = baseDTO[i].Type,
                Speed = baseDTO[i].SPD,
                LevelData = lvDic[baseDTO[i].Type],
            };
            if (dropTableDic.TryGetValue(baseDTO[i].Type, out var table) == false)
                table = new();

            data.DropTable = new(table);

            baseData.Add(data.Type, data);
        }

        var json = JsonConvert.SerializeObject(baseData, Formatting.Indented);
        File.WriteAllText(Path.Combine(Application.dataPath, "CharacterData.json"), json);

    }
}

//[CustomEditor(typeof(SABDTOConverterEditor))]
//public class SABDTOConverterEditor : Editor
//{

//    [MenuItem("Tools/Unit DB Generator")]
//    public static void ShowWindow()
//    {
//        GetWindow<UnitDBEditorWindow>("Unit DB Generator");
//    }

//    private void OnGUI()
//    {
//        var helper = (SABDTOConverter)target;

//        GUILayout.Label("유닛 DB 생성 도구", EditorStyles.boldLabel);

//        EditorGUILayout.Space();

//        _unitCommon = (TextAsset)EditorGUILayout.ObjectField("UnitCommon", _unitCommon, typeof(TextAsset), false);
//        _unitDropItem = (TextAsset)EditorGUILayout.ObjectField("UnitDropItem", _unitDropItem, typeof(TextAsset), false);
//        _unitLevel = (TextAsset)EditorGUILayout.ObjectField("UnitLevel", _unitLevel, typeof(TextAsset), false);

//        EditorGUILayout.Space();

//        GUILayout.Label("출력 경로 (상대 경로 또는 Assets 하위 경로)", EditorStyles.label);
//        _unitDBGeneratePath = EditorGUILayout.TextField(_unitDBGeneratePath);

//        EditorGUILayout.Space(10);

//        if (GUILayout.Button("유닛 DB 생성"))
//        {
//            GenerateUnitDB();
//        }
//    }
//}