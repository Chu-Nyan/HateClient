using SAB.Unit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Utility
{
    /// <summary>
    /// 애니메이터 해쉬 접근을 도와줌
    /// </summary>
    /// <typeparam name="T">파라미터 대응</typeparam>
    /// <typeparam name="K">스테이트 대응</typeparam>
    public class AnimatorHelper<T,K> where T : Enum where K : Enum
    {
        private readonly Animator _animator;
        private readonly Dictionary<T, int> _idByEnum;
        private readonly Dictionary<int, K> _hashByState;
        private readonly Dictionary<K, AniClipEvent> _eventByState;

        public AnimatorHelper(Animator animator, Dictionary<T, string> clipStrings, Dictionary<K, string> stateStrings)
        {
            _animator = animator;
            _idByEnum = new(clipStrings.Count);
            _hashByState = new(stateStrings.Count);
            _eventByState = new(stateStrings.Count);


            foreach (var item in clipStrings)
            {
                int hash = Animator.StringToHash(item.Value);
                _idByEnum[item.Key] = hash;
            }

            foreach (var item in stateStrings)
            {
                int hash = Animator.StringToHash(item.Value);
                _hashByState[hash] = item.Key;
            }
        }

        public AnimatorHelper(Animator animator, Dictionary<T, int> clipStrings)
        {
            _animator = animator;
            _idByEnum = clipStrings;
        }

        public void TickForEvent()
        {
            AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);
            K currentState = _hashByState[info.shortNameHash];

            if (_eventByState.TryGetValue(currentState, out AniClipEvent clipevent) == true)
            {
                float time = info.normalizedTime;
                clipevent.Tick(time);
            }
        }

        public AnimatorStateInfo GetCurrentStateInfo()
        {
            return _animator.GetCurrentAnimatorStateInfo(0);
        }

        public K GetStateHash(int key)
        {
            return _hashByState[key];
        }

        public void SetBool(T key, bool value)
        {
#if UNITY_EDITOR == false
            if (_idByEnum.ContainsKey(key) == false)
                return;
#endif

            _animator.SetBool(_idByEnum[key], value);
        }

        public void SetInt(T key, int value)
        {
#if UNITY_EDITOR == false
            if (_idByEnum.ContainsKey(key) == false)
                return;
#endif

            _animator.SetInteger(_idByEnum[key], value);
        }

        public void SetFloat(T key, float value)
        {
#if UNITY_EDITOR == false
            if (_idByEnum.ContainsKey(key) == false)
                return;
#endif  

            _animator.SetFloat(_idByEnum[key], value);
        }

        public void SetTrigger(T key)
        {
            _animator.SetTrigger(_idByEnum[key]);
        }
    }
}
