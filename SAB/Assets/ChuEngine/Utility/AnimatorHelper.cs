using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Utility
{
    public class AnimatorHelper<T> where T : Enum
    {
        private readonly Animator _animator;
        private readonly Dictionary<T, int> _idByEnum;

        public AnimatorHelper(Animator animator, Dictionary<T, string> clipStrings)
        {
            _animator = animator;
            _idByEnum = new(clipStrings.Count);
            foreach (var item in clipStrings)
            {
                int hash = Animator.StringToHash(item.Value);
                _idByEnum[item.Key] = hash;
            }
        }

        public AnimatorHelper(Animator animator, Dictionary<T, int> clipStrings)
        {
            _animator = animator;
            _idByEnum = clipStrings;
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
    }
}
