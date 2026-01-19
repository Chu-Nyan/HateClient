using Chu.AI;

namespace SAB.Unit.State
{
    public class IAttackState : IMachineState<CharacterState, CharacterStats>
    {
        public CharacterState Type
        {
            get => CharacterState.Attack;
        }

        public void Enter(CharacterStats context)
        {
            context.CurrentStats.PlayAnimationClip = AnimationClipType.Attack;
        }

        public bool Tick(CharacterStats context)
        {
            return context.CurrentStats.IsAttacking == false;
        }

        public void Exit(CharacterStats context)
        {
        }
    }

}
