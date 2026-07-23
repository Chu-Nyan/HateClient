using UnityEngine;

namespace SAB.GameSystem
{
    [CreateAssetMenu(fileName = "WorldProfile", menuName = "SO")]
    public class WorldProfile : ScriptableObject
    {
        public FactionTable FactionTable;
    }
}
