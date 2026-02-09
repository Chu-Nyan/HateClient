using Chu.Utility;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator
{
    private readonly AnimatorHelper<AniParamator, AniState> _animator;
    private readonly Dictionary<WeaponStance, RuntimeAnimatorController> _animatorByWeaponType;

    public CharacterAnimator(Animator animator)
    {
        var clipData = new Dictionary<AniParamator, string>()
        {
            { AniParamator.MoveSpeed, "MoveSpeed" },
            { AniParamator.CombatMode, "IsCombatMode" },
            { AniParamator.Attack, "Attack" }
        };

        var stateData = new Dictionary<AniState, string>()
        {
            { AniState.Movement, "IdleAndMove" },
            { AniState.CombatMoveMent, "Combat_Movement" },
            { AniState.Attack, "Attack" }
        };
        _animator = new(animator, clipData, stateData);

        _animatorByWeaponType = new Dictionary<WeaponStance, RuntimeAnimatorController>()
        {
            { WeaponStance.Unarmed, _animator.RuntimeAnaimator},
            { WeaponStance.Sword, AssetManager.LoadAssetSync<AnimatorOverrideController>("SwordAnimator") },
            { WeaponStance.SwordAndShield, AssetManager.LoadAssetSync<AnimatorOverrideController>("SwordAndShieldAnimator") }
        };
    }

    public void Tick(StateContext data, float moveSpd)
    {
        AnimatorStateInfo info = _animator.GetCurrentStateInfo();
        AniState currentState = _animator.GetStateHash(info.shortNameHash);

        if (currentState == AniState.Movement || currentState == AniState.CombatMoveMent)
        {
            float value = data.IsMoveing == true ? 5f * moveSpd * 0.1f : 0;
            PlayMoveAnimation(value);
        }

        _animator.Tick();
    }

    public void RegisterAnimationEvent(AniState state, AniEventData data, float timeing, Action<AniEventData> action)
    {
        AniClipEvent clipEvent = new(data, timeing, action);
        _animator.RegisterStateEvent(state, clipEvent);
    }

    public void ChangeStance(WeaponStance stance)
    {
        _animator.SetRuntimeAnimator(_animatorByWeaponType[stance]);
    }

    private void PlayMoveAnimation(float value)
    {
        _animator.SetFloat(AniParamator.MoveSpeed, value);
    }

    public void SetCombatMode(bool value)
    {
        _animator.SetBool(AniParamator.CombatMode, value);
    }

    public void SetAttack()
    {
        _animator.SetTrigger(AniParamator.Attack);
    }
}
