using UnityEngine;

namespace SAB.Cutscene
{
    public interface ICutsceneObject
    {
        public Transform transform { get; }
        public CutsceneObjectType CutsceneType { get; }
        public void SetActive(bool value);
        public void SetCutsceneData(IObjectConfig data);
        public IObjectConfig GetCutsceneConfig();
    }
}
