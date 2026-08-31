using Chu.Utility;
using SAB.Cutscene;
using SAB.DataManger;
using SAB.EntityAgent;
using SAB.GameSystem;
using SAB.Unit;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SAB.Facade
{
    public class MapFacade
    {
        private readonly CutsceneMapTrigger _triggers;
        private readonly VCamFollow _mainCam;

        public MapFacade(CutsceneMapTrigger triggers, VCamFollow main)
        {
            _triggers = triggers;
            _mainCam = main;
        }

        public async void Change(MapType type, int playerSpawnPoint)
        {
            try
            {
                await LoadMapAsync(type, playerSpawnPoint);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Map Change Failed, Map type : {type} \n{ex.Message}\n{ex.StackTrace}");
            }
        }

        private async Task LoadMapAsync(MapType type, int playerSpawnPoint)
        {
            var op = SceneManager.LoadSceneAsync($"Scene_{type}", LoadSceneMode.Additive);
            while (op.isDone == false)
            {
                await Task.Yield();
            }

            await Task.Yield();

            var mapDef = DataBase.Instance.MapRepo.DataByType[type];

            _triggers.SetupMap(mapDef.CutsceneTriggers);
            foreach (var item in mapDef.Characters)
            {
                var request = item.Value;
                mapDef.Favorites.TryGetValue(item.Key, out var favoritesID);
                mapDef.UniqueObjs.TryGetValue(item.Key, out var uniqueType);
                CharacterFactory.Instance.Create(request.BrainType, request.UnitID, request.Position, request.Rotation, favoritesID, uniqueType);
                var spawnPos2D = mapDef.SpawnPoint[playerSpawnPoint];
                Quaternion rotation = Quaternion.Euler(new(0, spawnPos2D.EulerY, 0));
                var player = CharacterFactory.Instance.Create(BrainType.Player, 1, spawnPos2D.Position.ToVector3XZ(), rotation, UniqueObjType.Player);
                _mainCam.SetThirdPersonTarget(player.transform);
            }
        }
    }
}
