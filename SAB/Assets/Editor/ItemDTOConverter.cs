using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System;
using SAB.Item;

[CreateAssetMenu(fileName = "ItemDTOConverter", menuName = "Scriptable Objects/ItemDTOConverter", order = 3)]
public class ItemDTOConverter : ScriptableObject
{
    [SerializeField]
    private TextAsset _base;
    [SerializeField]
    private TextAsset _equipment;
    [SerializeField]
    private TextAsset _weapon;
    [SerializeField]
    private string _jsonGeneratePath;

    [ContextMenu("GOGO")]
    public void Convert()
    {
        File.WriteAllText(Path.Combine(_jsonGeneratePath, "ItemData.json"), _base.text);
        File.WriteAllText(Path.Combine(_jsonGeneratePath, "EquipmentData.json"), _equipment.text);
        File.WriteAllText(Path.Combine(_jsonGeneratePath, "WeaponData.json"), _weapon.text);
    }
}