using UnityEngine;

namespace SAB.Cutscene
{
    public interface ICutsceneObject
    {
        public Transform transform { get; }
        public abstract void SetActive(bool value);
        public abstract void SetCutscenePreset(ICutscenePreset data);


    }
}

