namespace SAB.Unit.Combat
{
    public class TimerStepData : IStepData
    {
        public readonly int ID;
        public readonly float Duration;

        public int GetID
        {
            get => ID;
        }

        public TimerStepData(int id, float duration)
        {
            ID = id;
            Duration = duration;
        }
    }
}
