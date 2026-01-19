using Chu.AI;

namespace SAB.Unit.State
{
    public class IMovementState : IMachineState<CharacterState, CharacterStats>
    {
        public CharacterState Type
        {
            get => CharacterState.Move;
        }

        public void Enter(CharacterStats context)
        {
            context.CurrentStats.PlayAnimationClip = AnimationClipType.IdleAndMove;
        }

        public bool Tick(CharacterStats context)
        {
            return context.CurrentStats.IsMoveing == false;
        }

        public void Exit(CharacterStats context)
        {
        }
    }
}
