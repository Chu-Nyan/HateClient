using Chu.Core;
using UnityEngine;

namespace Chu.Utility.Unity
{
    [DefaultExecutionOrder(-1000)]
    public class HideOnPlay : MonoBehaviour
    {
        [SerializeField]
        private bool _isActive;

        private void Awake()
        {
            gameObject.SetActive(_isActive);
            if (_isActive == true)
            {
                ChuEngine.Run("en");
            }
        }
    }
}
