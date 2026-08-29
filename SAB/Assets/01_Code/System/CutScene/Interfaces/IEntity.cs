using UnityEngine;

namespace SAB.Cutscene
{
    public interface IEntity
    {
        public Transform transform { get; }
        public void SetActive(bool value);
    }
}
