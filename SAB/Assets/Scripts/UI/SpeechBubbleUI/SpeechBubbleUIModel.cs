using System.Collections.Generic;
using UnityEngine;

namespace SAB.UI
{
    public partial class SpeechBubbleUIModel
    {
        public readonly Dictionary<int, Transform> AnchorByID;
        public readonly Dictionary<int, Queue<DialogueData>> QueueByID;
        public readonly Dictionary<int, float> PlayTimeByID;

        public readonly HashSet<int> StepChanged;
        public readonly HashSet<int> QueueFinished;

        public SpeechBubbleUIModel()
        {
            AnchorByID = new();
            QueueByID = new();
            PlayTimeByID = new();

            StepChanged = new();
            QueueFinished = new();
        }

        public void Tick(float time)
        {
            foreach (var item in AnchorByID)
            {
                PlayTimeByID[item.Key] += time;

                var queue = QueueByID[item.Key];

                if (PlayTimeByID[item.Key] >= queue.Peek().Time)
                {
                    queue.Dequeue();
                    if (queue.Count == 0)
                        QueueFinished.Add(item.Key);
                    else
                        StepChanged.Add(item.Key);
                }
            }
        }

        public void AddDialogue(Transform anchor, DialogueData text, bool isOverwrite)
        {
            int id = anchor.name.GetHashCode();
            if (AnchorByID.TryAdd(id, anchor) == true)
            {
                QueueByID[id] = new();
                PlayTimeByID[id] = 0;
            }
            else if (isOverwrite == true && QueueByID[id].Peek().SequenceID != text.SequenceID)
            {
                QueueByID[id].Clear();
                QueueByID[id].Enqueue(text);
                PlayTimeByID[id] = 0;
            }

            QueueByID[id].Enqueue(text);
            StepChanged.Add(id);
        }

        public void Remove(int id)
        {
            AnchorByID.Remove(id);
            QueueByID.Remove(id);
            PlayTimeByID.Remove(id);
        }

        public string GetCurrentStepTextID(int objID)
        {
            return QueueByID[objID].Peek().TextID;
        }
    }
}
