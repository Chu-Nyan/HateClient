using Chu.Utility;
using System.Collections.Generic;
using UnityEngine;

public class MapReferenceHub : MonoBehaviour
{
    [SerializeField]
    private GameObject _linkedObjectRoot;
    [SerializeField]
    private Character[] _characters;
    [SerializeField]
    private SerializablePair<MonoBehaviour, string>[] _favoriteObjects;

    public IReadOnlyList<Character> Characters
    {
        get => _characters;
    }

    public IReadOnlyList<SerializablePair<MonoBehaviour, string>> FavoriteObjects
    {
        get => _favoriteObjects;
    }

#if UNITY_EDITOR
    [ContextMenu("Auto Binding From Root")]
    public void AutoBinding()
    {
        _characters = _linkedObjectRoot.GetComponentsInChildren<Character>();
    }
#endif
}
