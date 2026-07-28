using TMPro;
using UnityEngine;

namespace SAB.UI
{
    public class BubbleElement : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private RectTransform _rectTransform;

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetPosition(Vector2 canvasLocalPosition)
        {
            _rectTransform.anchoredPosition = canvasLocalPosition;
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
