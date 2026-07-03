using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FactionTable
{
    [SerializeField]
    public Dictionary<FactionType, Faction> Factions;

    public FactionTable()
    {
        Factions = new();
        UpdateFaction();
    }

    public void UpdateFaction()
    {
        var factionList = (FactionType[])Enum.GetValues(typeof(FactionType));

        foreach (var type in factionList)
        {
            if (Factions.TryGetValue(type, out var faction) == false)
            {
                faction = new();
                Factions.Add(type, faction);
            }

            var old = faction.FactionRelations;
            faction.FactionRelations = new FactionRelation[factionList.Length];

            if (old != null)
                Array.Copy(old, faction.FactionRelations, Math.Min(old.Length, faction.FactionRelations.Length));
        }
    }

    public void SetRelation(FactionType a, FactionType b, FactionRelation state)
    {
        Factions[a].FactionRelations[(int)b] = state;
        Factions[b].FactionRelations[(int)a] = state;
    }
}
