using UnityEngine;

namespace Chu.Utility
{
    [ExecuteAlways]
    [DefaultExecutionOrder(-1000)]
    public class HideOnPlay : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
