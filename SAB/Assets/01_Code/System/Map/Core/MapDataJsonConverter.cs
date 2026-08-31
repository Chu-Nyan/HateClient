using UnityEngine;

namespace SAB.GameSystem
{
    public class MapDataJsonConverter : MonoBehaviour
    {
        [SerializeField]
        private DataExporter[] _converters;

        public void Convert()
        {
            foreach (var item in _converters)
            {
                item.Export();
            }
            Debug.Log("Data Exported");
        }
    }
}
