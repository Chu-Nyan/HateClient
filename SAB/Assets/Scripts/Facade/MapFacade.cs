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
        private readonly TopViewCamera _camera;

        public MapFacade(CutsceneMapTrigger triggers, TopViewCamera camera)
        {
            _triggers = triggers;
            _camera = camera;
        }

        public async void Change(MapType type)
        {
            try
            {
                await LoadMapAsync(type);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Map Change Failed, Map type : {type} \n{ex.Message}\n{ex.StackTrace}");
            }
        }

        private async Task LoadMapAsync(MapType type)
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
                if (mapDef.Favorites.TryGetValue(item.Key, out var id) == true)
                {
                    CharacterFactory.Instance.Create(request.BrainType, request.UnitID, request.Position, request.Rotation, id);
                }
                else
                {
                    CharacterFactory.Instance.Create(request.BrainType, request.UnitID, request.Position, request.Rotation);
                }
            }

            var player = CharacterFactory.Instance.Create(BrainType.Player, 1, new Vector3(100, 0, 100), Quaternion.identity, UniqueEntityType.Player);
            _camera.StickCameraArm(player.transform);
        }
    }
}
