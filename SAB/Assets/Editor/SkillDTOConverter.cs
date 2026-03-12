using System.IO;
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
        WriteAllText(_skillBase.text, "SkillData.json");
        WriteAllText(_collisionLogic.text, "CollisionLogic.json");
        WriteAllText(_hitBox.text, "HitBoxData.json");
        WriteAllText(_skillstep.text, "SkillFlowStep.json");
        WriteAllText(_instance.text, "SkillStepInstance.json");
        WriteAllText(_dot.text, "SkillStepDoT.json");
        WriteAllText(_aoe.text, "SkillStepAoE.json");
        WriteAllText(_time.text, "SkillStepTimer.json");
    }

    private void WriteAllText(string json, string fileName)
    {
        File.WriteAllText(Path.Combine(_jsonGeneratePath, fileName), json);
    }
}

