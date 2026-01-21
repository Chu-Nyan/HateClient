using Chu.AI;

namespace SAB.Unit.State
{
    public class IIdleState : IMachineState<CharacterState, StateContext>
    {
        public CharacterState Type
        {
            get => CharacterState.Idle;
        }

        public void Enter(StateContext context)
        {
            context.PlayAnimationClip = AnimationClipType.IdleAndMove;
        }

        public bool Tick(StateContext context)
        {
            return true;
        }

        public void Exit(StateContext context)
        {
        }
    }
}
