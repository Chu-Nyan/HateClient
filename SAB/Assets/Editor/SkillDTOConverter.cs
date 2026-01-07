using SAB.Unit.Combat;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "SkillDTOConverter", menuName = "Scriptable Objects/SkillDTOConverter", order = 3)]
public class SkillDTOConverter : ScriptableObject
{
    [SerializeField]
    private TextAsset _skillBase;
    [SerializeField]
    private TextAsset _skillLogic;
    [SerializeField]
    private TextAsset _instance;
    [SerializeField]
    private TextAsset _dot;
    [SerializeField]
    private TextAsset _aoe;
    [SerializeField]
    private TextAsset _time;

    [SerializeField]
    private string _jsonGeneratePath;

    [ContextMenu("GOGO")]
    public void ConvertCharacterDTO()
    {
        var baseData = JsonToDictionary<SkillID, SkillData>(_skillBase.text, x => x.ID);
        var flowDTO = ConvertFromJson<Skill_Logic_DTO[]>(_skillLogic.text);
        var instanceDTO = JsonToDictionary<int, InstanceStepData>(_instance.text, x => x.ID);
        var dotDTO = JsonToDictionary<int, DotStepData>(_dot.text, x => x.ID);
        var aoeDTO = JsonToDictionary<int, AoEStepData>(_aoe.text, x => x.ID);
        var timeDTO = JsonToDictionary<int, TimerStepData>(_time.text, x => x.ID);


        var flowByID = new Dictionary<SkillID, List<int>>();
        for (int i = 0; i < flowDTO.Length; i++)
        {
            if (flowByID.ContainsKey(flowDTO[i].ID) == false)
            {
                flowByID.Add(flowDTO[i].ID, new List<int>());
            }

            flowByID[flowDTO[i].ID].Add(flowDTO[i].LogicID);
        }

        foreach (var item in flowByID)
        {
            baseData[item.Key].FlowIDs = item.Value.ToArray();
        }


        WriteAllText(baseData, "SkillData.json");
        WriteAllText(instanceDTO, "SkillFlowInstance.json");
        WriteAllText(dotDTO, "SkillFlowDoT.json");
        WriteAllText(aoeDTO, "SkillFlowAoE.json");
        WriteAllText(timeDTO, "SkillFlowTimer.json");
    }

    private Dictionary<T,K> JsonToDictionary<T, K>(string json, Func<K, T> keySelctor)
    {
        List<K> list = JsonConvert.DeserializeObject<List<K>>(json);
        return list.ToDictionary(keySelctor);
    }

    private void WriteAllText(object obj, string fileName)
    {
        string text = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText(Path.Combine(_jsonGeneratePath, fileName), text);
    }

    private T ConvertFromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}
