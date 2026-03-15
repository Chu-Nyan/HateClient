using Chu.Collision;

namespace SAB.Unit.Combat
{
    public class CollisionLogicData
    {
        public readonly int ID;
        public readonly int Order;
        public readonly float ActiveTime;
        public readonly float Speed;
        public readonly ShapeParam[] Hitboxes;
        public readonly IStepData[] OnHitSteps;

        public CollisionLogicData(int id, int order, float active, float spd, ShapeParam[] hitboxes, IStepData[] onHitSteps)
        {
            ID = id;
            Hitboxes = hitboxes;
            Order = order;
            ActiveTime = active;
            Speed = spd;
            OnHitSteps = onHitSteps;
        }
    }
}
