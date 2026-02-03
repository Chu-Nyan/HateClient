using Chu.Utility;
using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator
{
    private readonly AnimatorHelper<AniParamator, AniState> _animator;
    private readonly Dictionary<AniState, AniClipEvent> _eventByState;
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
        _eventByState = new();
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

        if (data.PlayAnimationClip == AniParamator.MoveSpeed)
        {
            float value = data.IsMoveing == true ? 5f * moveSpd * 0.1f : 0;
            PlayMoveAnimation(value);
        }

        if (_eventByState.TryGetValue(currentState, out AniClipEvent clipevent) == true)
        {
            float time = info.normalizedTime;
            clipevent.Tick(time);
        }
    }

    public void SetAnimationEvent(AniState state, AniEventData data, float timeing, Action<AniEventData> action)
    {
        if (_eventByState.ContainsKey(state) == false)
            _eventByState[state] = new AniClipEvent();

        _eventByState[state].Setup(data, timeing, action);
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
