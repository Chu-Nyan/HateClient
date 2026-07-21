using SAB.Cutscene;
using System.Collections.Generic;
using UnityEngine;

public class EntityContainer
{
    private readonly Dictionary<UniqueEntityType, MonoBehaviour> _objectByUniqueType;
    private readonly Dictionary<int, Character> _character;
    public readonly Dictionary<string, MonoBehaviour> ObjectByFavorite;

    public Dictionary<int, Character> Character
    {
        get => _character;
    }

    public EntityContainer()
    {
        _character = new();
        _objectByUniqueType = new();
        ObjectByFavorite = new(0);
    }

    public void SetUniqueEntity(UniqueEntityType type, MonoBehaviour obj, bool isOverwrite = true)
    {
        if (isOverwrite == true)
            _objectByUniqueType[type] = obj;
        else
            _objectByUniqueType.TryAdd(type, obj);
    }

    public MonoBehaviour GetUniqueEntity(UniqueEntityType type)
    {
        return _objectByUniqueType[type];
    }
}
