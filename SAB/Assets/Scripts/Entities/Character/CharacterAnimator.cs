using Chu.Utility;
using SAB.Unit;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator
{
    private readonly AnimatorHelper<AniParamator> _animator;

    public CharacterAnimator(Animator animator)
    {
        var clipData = new Dictionary<AniParamator, string>()
        {
            { AniParamator.MoveSpeed, "MoveSpeed" },
            { AniParamator.CombatMode, "IsCombatMode" }
        };
        _animator = new(animator, clipData);
    }

    public void Tick(StateContext data, float moveSpd)
    {
        if (data.PlayAnimationClip == AniParamator.MoveSpeed)
        {
            float value = data.IsMoveing == true ? 5f * moveSpd * 0.1f : 0;
            PlayMoveAnimation(value);
        }
    }

    public void SetCombatMode(bool value)
    {
        _animator.SetBool(AniParamator.CombatMode, value);
    }

    private void PlayMoveAnimation(float value)
    {
        _animator.SetFloat(AniParamator.MoveSpeed, value);
    }

}