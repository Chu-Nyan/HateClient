using Chu.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.UI
{
    public class SpeechBubbleUIView : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _canvas;
        [SerializeField]
        private BubbleElement _bluePrint;
        private ObjectPooling<BubbleElement> _pool;

        private Dictionary<int, BubbleElement> _elements;

        public void Awake()
        {
            _elements = new();
            _pool = new(() => Instantiate(_bluePrint, transform));
        }

        public void ShowBubble(int id, string text)
        {
            if (_elements.TryGetValue(id, out var element) == true)
            {
                element.SetText(text);
            }
            else
            {
                var bubble = _pool.Dequeue();
                _elements[id] = bubble;
                bubble.SetActive(true);
                bubble.SetText(text);
            }
        }

        public void HideBubble(int id)
        {
            var element = _elements[id];
            element.SetActive(false);
            _pool.Enqueue(element);
            _elements.Remove(id);
        }

        public void SetBubblePosition(int id, Vector3 screenPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, screenPos, null, out var localPosition);
            _elements[id].SetPosition(localPosition);
        }
    }
}
