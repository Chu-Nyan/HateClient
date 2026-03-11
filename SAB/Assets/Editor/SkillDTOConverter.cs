using Chu.Collision;
using Newtonsoft.Json;
using SAB.Unit.Combat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
    private TextAsset _collisionLogic;
    [SerializeField]
    private TextAsset _hitBox;

    [SerializeField]
    private string _jsonGeneratePath;

    [ContextMenu("GOGO")]
    public void ConvertCharacterDTO()
    {
        var baseById = JsonToDictionary<SkillID, Skill_Base_DTO>(_skillBase.text, x => x.ID);
        var collsionLogicByID = ConvertJsonToListByID<Skill_CollisionLogic_DTO>(_collisionLogic.text, "ID");
        var hitboxesById = ConvertJsonToListByID<Skill_HitBox_DTO>(_hitBox.text, "ID");
        var flowstepDTO = ConvertJsonToListByID<Skill_FlowStep_DTO>(_skillstep.text, "ID");
        var instanceDTO = JsonToDictionary<int, Skill_Step_Instance_DTO>(_instance.text, x => x.ID);
        var dotDTO = JsonToDictionary<int, Skill_Step_DoT_DTO>(_dot.text, x => x.ID);
        var aoeDTO = JsonToDictionary<int, Skill_Step_AoE_DTO>(_aoe.text, x => x.ID);
        var timeDTO = JsonToDictionary<int, Skill_Step_Timer_DTO>(_time.text, x => x.ID);

        WriteAllText(baseById, "SkillData.json");
        WriteAllText(collsionLogicByID, "CollisionLogic.json");
        WriteAllText(hitboxesById, "HitBoxData.json");
        WriteAllText(flowstepDTO, "SkillFlowStep.json");
        WriteAllText(instanceDTO, "SkillStepInstance.json");
        WriteAllText(dotDTO, "SkillStepDoT.json");
        WriteAllText(aoeDTO, "SkillStepAoE.json");
        WriteAllText(timeDTO, "SkillStepTimer.json");
    }

    private Dictionary<int, List<T>> ConvertJsonToListByID<T>(string json, string idFieldName)
    {
        var dtoArray = JsonConvert.DeserializeObject<T[]>(json);
        var dtoDictionary = new Dictionary<int, List<T>>();
        FieldInfo fieldInfo = typeof(T).GetField(idFieldName);

        if (fieldInfo == null)
            throw new ArgumentException($"필드 {idFieldName}을 찾을 수 없습니다.");

        foreach (var item in dtoArray)
        {
            // 2. 리플렉션으로 객체의 값을 가져옵니다.
            int id = (int)fieldInfo.GetValue(item);

            if (!dtoDictionary.TryGetValue(id, out var list))
            {
                list = new List<T>();
                dtoDictionary.Add(id, list);
            }
            list.Add(item);
        }

        return dtoDictionary;
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
}

