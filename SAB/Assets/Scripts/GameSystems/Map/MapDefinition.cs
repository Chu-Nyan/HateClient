using Chu.Data;
using SAB.Cutscene;
using SAB.Unit;
using System.Collections.Generic;

namespace SAB.GameSystem
{
    public class MapDefinition
    {
        public Pose2D[] SpawnPoint;
        public Dictionary<int, string> Favorites;
        public Dictionary<int, CharacterSpawnRequest> Characters;
        public CutsceneTriggerData[] CutsceneTriggers;
    }
}
