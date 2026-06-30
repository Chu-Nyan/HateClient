using SAB.Cutscene;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEntityContainer
{
    private readonly Dictionary<UniqueEntityType, MonoBehaviour> _objectByUniqueType;

    public UniqueEntityContainer()
    {
        _objectByUniqueType = new();
    }

    public void SetUniqueEntity(UniqueEntityType type, MonoBehaviour obj, bool isOverwrite = true)
    {
        if (isOverwrite == true)
            _objectByUniqueType[type] = obj;
        else
            _objectByUniqueType.TryAdd(type, obj);
    }

    public MonoBehaviour GetEntity(UniqueEntityType type)
    {
        return _objectByUniqueType[type];
    }
}
