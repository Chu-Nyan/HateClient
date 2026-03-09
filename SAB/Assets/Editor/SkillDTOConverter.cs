using SAB.Unit.Combat;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;
using Chu.Collision;

[CreateAssetMenu(fileName = "SkillDTOConverter", menuName = "Scriptable Objects/SkillDTOConverter", order = 3)]
public class SkillDTOConverter : ScriptableObject
{
    [SerializeField]
    private TextAsset _skillBase;
    [SerializeField]
    private TextAsset _skillstep;
    [SerializeField]
    private TextAsset _instance;
    [SerializeField]
    private TextAsset _dot;
    [SerializeField]
    private TextAsset _aoe;
    [SerializeField]
    private TextAsset _time;
    [SerializeField]
    private TextAsset _hitBox;

    [SerializeField]
    private string _jsonGeneratePath;

    [ContextMenu("GOGO")]
    public void ConvertCharacterDTO()
    {
        var baseData = JsonToDictionary<SkillID, SkillData>(_skillBase.text, x => x.ID);
        var baseDataDTO = ConvertFromJson<Skill_Base_DTO[]>(_skillBase.text);
        var hitboxData = ConvertHitboxDataFromDTO(ConvertFromJson<Skill_HitBox_DTO[]>(_hitBox.text));

        var flowstepDTO = ConvertFromJson<Skill_FlowStep_DTO[]>(_skillstep.text);
        var instanceDTO = JsonToDictionary<int, InstanceStepData>(_instance.text, x => x.ID);
        var dotDTO = JsonToDictionary<int, DotStepData>(_dot.text, x => x.ID);
        var aoeDTO = JsonToDictionary<int, AoEStepData>(_aoe.text, x => x.ID);
        var timeDTO = JsonToDictionary<int, TimerStepData>(_time.text, x => x.ID);

        var flowByID = new Dictionary<SkillID, List<int>>();
        for (int i = 0; i < flowstepDTO.Length; i++)
        {
            if (flowByID.ContainsKey(flowstepDTO[i].ID) == false)
            {
                flowByID.Add(flowstepDTO[i].ID, new List<int>());
            }

            flowByID[flowstepDTO[i].ID].Add(flowstepDTO[i].LogicID);
        }

        foreach (var item in flowByID)
        {
            baseData[item.Key].HitFlowStepIDs = item.Value.ToArray();
        }

        foreach (var item in baseDataDTO)
        {
            baseData[item.ID].HitBoxes = hitboxData[item.HitBoxID];
        }

        WriteAllText(baseData, "SkillData.json");
        WriteAllText(instanceDTO, "SkillStepInstance.json");
        WriteAllText(dotDTO, "SkillStepDoT.json");
        WriteAllText(aoeDTO, "SkillStepAoE.json");
        WriteAllText(timeDTO, "SkillStepTimer.json");
    }

    private Dictionary<int, ShapeParam[]> ConvertHitboxDataFromDTO(Skill_HitBox_DTO[] baseDTO)
    {
        var dic = new Dictionary<int, List<Skill_HitBox_DTO>>();
        for (int i = 0; i < baseDTO.Length; i++)
        {
            if (dic.TryGetValue(baseDTO[i].ID, out var list) ==  false)
            {
                list = new List<Skill_HitBox_DTO>();
                dic[baseDTO[i].ID] = list;
            }

            list.Add(baseDTO[i]);
        }

        var hitBoxByID = new Dictionary<int, ShapeParam[]>();

        foreach (var item in dic)
        {
            List<Skill_HitBox_DTO> list = item.Value;
            var hitboxArr = new ShapeParam[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                hitboxArr[i] = new ShapeParam(list[i].ShapeType, list[i].OffsetX, list[i].OffsetY, list[i].Param1, list[i].Param2); ;
            }

            hitBoxByID[item.Key] = hitboxArr;
        }

        return hitBoxByID;
    }

    private Dictionary<T, K> JsonToDictionary<T, K>(string json, Func<K, T> keySelctor)
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
