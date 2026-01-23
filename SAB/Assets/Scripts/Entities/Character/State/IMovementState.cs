using Chu.AI;

namespace SAB.Unit.State
{
    public class IMovementState : IMachineState<CharacterState, StateContext>
    {
        public CharacterState Type
        {
            get => CharacterState.Move;
        }

        public void Enter(StateContext context)
        {
            context.PlayAnimationClip = AniParamator.MoveSpeed;
        }

        public bool Tick(StateContext context)
        {
            return context.IsMoveing == false;
        }

        public void Exit(StateContext context)
        {
        }
    }
}
