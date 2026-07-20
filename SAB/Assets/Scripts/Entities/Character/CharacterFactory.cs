using Chu.Core;
using Chu.Utility;
using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.Unit;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFactory : Singleton<CharacterFactory>
{
    private readonly Dictionary<int, BaseStats> _unitDatas;
    private readonly Dictionary<int, CustomizingData> _customizingData;

    private readonly ObjectPooling<Character> _pool;
    private AgentController _agentController;
    private UniqueEntityContainer _entityContainer;

    public CharacterFactory(DataBase db)
    {
        _unitDatas = db.CharacterRepo.CharacterBaseData;
        _customizingData = db.CharacterRepo.CustomizingData;
        _pool = new(() =>
        {
            return AssetManager.GenerateLoadAssetSync<Character>(Const.Asset_Character);
        });
    }

    public void Init(AgentController agent, UniqueEntityContainer container)
    {
        _agentController = agent;
        _entityContainer = container;
    }

    public Character Create(BrainType type, int unitID, Vector3 respawn)
    {
        var acter = _pool.Dequeue();
        acter.SetPositionWithNavMash(respawn);
        acter.SetupStats(_unitDatas[unitID]);
        SetCustomizing(acter, unitID);

        _agentController.BindReceiver(acter, type);
        return acter;
    }

    public Character Create(BrainType type, int unitID, Vector3 respawn, UniqueEntityType uniqueType)
    {
        var acter = Create(type, unitID, respawn);
        _entityContainer.SetUniqueEntity(uniqueType, acter);
        return acter;
    }

    private void SetCustomizing(Character acter, CustomizingData data)
    {
        acter.SetCustomizing(CustomizingPart.Hair, data.Hair);
        acter.SetCustomizing(CustomizingPart.Mouth, data.Mouth);
        acter.SetCustomizing(CustomizingPart.Eyebrow, data.Eyebrow);
        acter.SetCustomizing(CustomizingPart.Eye, data.Eye);
    }

    private void SetCustomizing(Character acter, int id)
    {
        if (_customizingData.TryGetValue(id, out var data) == false)
            return;

        SetCustomizing(acter, data);
    }

    public void Enqueue(Character character)
    {
        _pool.Enqueue(character);
    }
}
