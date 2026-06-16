using UnityEngine;

namespace SAB.Cutscene
{
    public interface ICutsceneObject
    {
        public Transform transform { get; }
        public void SetActive(bool value);
        public void SetCutsceneData(IObjectConfig data);
    }
}
