namespace SAB.Unit.Combat
{
    public class TimerStepData : IStepData
    {
        public int ID;
        public float Duration;

        public int GetID
        {
            get => ID;
        }
    }
}
