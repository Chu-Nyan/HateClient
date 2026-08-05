using Chu.Collision;

namespace SAB.Skill
{
    public class CollisionLogicData
    {
        public readonly int ID;
        public readonly int Order;
        public readonly float ActiveTime;
        public readonly float Speed;
        public readonly ShapeParam[] Hitboxes;
        public readonly IStepData[] HitSequence;

        public CollisionLogicData(int id, int order, float active, float spd, ShapeParam[] hitboxes, IStepData[] hitSequence)
        {
            ID = id;
            Hitboxes = hitboxes;
            Order = order;
            ActiveTime = active;
            Speed = spd;
            HitSequence = hitSequence;
        }
    }
}
