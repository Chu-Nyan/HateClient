using Chu.Utility;
using NUnit.Framework;
using SAB.MeshSlot;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarSO", menuName = "Scriptable Objects/AvatarSO", order = 1)]
public class AvatarSO : ScriptableObject
{
    public SerializablePair<SlotType, string>[] MeshAddress;
    private Dictionary<SlotType, string> _pathByType;

    private void OnEnable()
    {
        _pathByType = new Dictionary<SlotType, string>();

        for (int i = 0; i < MeshAddress.Length; i++)
        {
            if (_pathByType.ContainsKey(MeshAddress[i].Key) == true)
            {
                Debug.Log("중복 키");
            }

            _pathByType[MeshAddress[i].Key] = MeshAddress[i].Value;
        }
    }

    public string GetAssress(SlotType type, int index)
    {
        return $"{_pathByType[type]}{index:D2}";
    }
}
