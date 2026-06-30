using Chu.Utility;
using UnityEngine;

public class MapReferenceHub : MonoBehaviour
{
    [SerializeField]
    private GameObject _linkedObjectRoot;

    public SerializablePair<int, Character>[] Characters;

#if UNITY_EDITOR
    [ContextMenu("Auto Binding From Root")]
    public void AutoBinding()
    {
        var actorArr = _linkedObjectRoot.GetComponentsInChildren<Character>();
        Characters = new SerializablePair<int, Character>[actorArr.Length];
        for (int i = 0; i < actorArr.Length; i++)
        {
            int id = actorArr[i].gameObject.name.GetHashCode();

            Characters[i] = new SerializablePair<int, Character>(id, actorArr[i]);
        }
    }
#endif
}
