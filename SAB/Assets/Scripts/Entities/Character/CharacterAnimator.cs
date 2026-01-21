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

    public void Tick(StateContext data, float moveSpd)
    {
        if (data.PlayAnimationClip == AnimationClipType.IdleAndMove)
        {
            float value = data.IsMoveing == true ? 5f * moveSpd * 0.1f : 0;
            PlayMoveAnimation(value);
        }
    }

    private void PlayMoveAnimation(float value)
    {
        _animator.SetFloat(AnimationClipType.IdleAndMove, value);
    }
}