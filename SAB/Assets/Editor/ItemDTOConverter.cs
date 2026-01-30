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
        var itemByID = JsonToDictionary<int, ItemTemplateData>(_base.text, x => x.ID);
        var equipByID = JsonToDictionary<int, EquipmentTemplateData>(_equipment.text, x => x.ID);
        var weaponDTO = JsonToDictionary<int, WeaponTemplateData>(_weapon.text, x => x.ID);

        WriteAllText(itemByID, "ItemData.json");
        WriteAllText(equipByID, "EquipmentData.json");
        WriteAllText(weaponDTO, "WeaponData.json");
    }

    private Dictionary<T, K> JsonToDictionary<T, K>(string json, Func<K, T> keySelctor)

    {
        List<K> list = JsonConvert.DeserializeObject<List<K>>(json);
        return list.ToDictionary(keySelctor);
    }

    private void WriteAllText(object obj, string fileName)
    {
        string text = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText(Path.Combine(_jsonGeneratePath, fileName), text);
    }

    private T ConvertFromJson<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }
}