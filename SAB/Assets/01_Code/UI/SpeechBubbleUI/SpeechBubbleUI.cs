using Chu.Core;
using UnityEngine;

namespace SAB.UI
{
    public class SpeechBubbleUI : UIController
    {
        [SerializeField]
        private SpeechBubbleUIView _view;
        private SpeechBubbleUIModel _model;

        private void Awake()
        {
            _model = new();
        }

        private void Update()
        {
            _model.Tick(Time.deltaTime);

            foreach (var id in _model.StepChanged)
            {
                string text = ChuEngine.Instance.TextResource[_model.GetCurrentStepTextID(id)];
                _view.ShowBubble(id, text);
            }

            foreach (var id in _model.QueueFinished)
            {
                _model.Remove(id);
                _view.HideBubble(id);
            }

            foreach (var item in _model.AnchorByID)
            {
                var pos = RectTransformUtility.WorldToScreenPoint(Camera.main, item.Value.position);
                _view.SetBubblePosition(item.Key, pos);
            }

            _model.StepChanged.Clear();
            _model.QueueFinished.Clear();
        }

        public void ShowDialogue(Transform anchor, DialogueData text, bool isOverwrite)
        {
            _model.AddDialogue(anchor, text, isOverwrite);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
