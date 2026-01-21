using Chu.AI;

namespace SAB.Unit.State
{
    public class IAttackState : IMachineState<CharacterState, StateContext>
    {
        public CharacterState Type
        {
            get => CharacterState.Attack;
        }

        public void Enter(StateContext context)
        {
            context.PlayAnimationClip = AnimationClipType.Attack;
        }

        public bool Tick(StateContext context)
        {
            return context.IsAttacking == false;
        }

        public void Exit(StateContext context)
        {
        }
    }

}
