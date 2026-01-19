using Chu.Utility;
using SAB.Unit;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator
{
    private readonly AnimatorHelper<AnimationClipType> _animator;

    public CharacterAnimator(Animator animator)
    {
        var clipData = new Dictionary<AnimationClipType, string>()
        {
            { AnimationClipType.IdleAndMove, "MoveSpeed" }
        };
        _animator = new(animator, clipData);
    }

    public void Tick(CharacterCurrentStats stats)
    {
        if (stats.PlayAnimationClip == AnimationClipType.IdleAndMove)
        {
            float value = stats.IsMoveing == true ? 5f * stats.MoveSpeed * 0.1f : 0;
            PlayMoveAnimation(value);
        }
    }

    private void PlayMoveAnimation(float value)
    {
        _animator.SetFloat(AnimationClipType.IdleAndMove, value);
    }
}