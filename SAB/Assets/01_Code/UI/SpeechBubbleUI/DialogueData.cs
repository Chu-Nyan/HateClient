namespace SAB.UI
{
    public readonly struct DialogueData
    {
        public readonly int SequenceID;
        public readonly string TextID;
        public readonly float Time;

        public DialogueData(int sequenceID, string textID, float time = 0)
        {
            SequenceID = sequenceID;
            TextID = textID;
            if (time == 0)
                time = 2;

            Time = time;
        }
    }
}
