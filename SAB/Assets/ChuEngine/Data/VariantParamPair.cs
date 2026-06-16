using System;
using UnityEngine;

namespace Chu.Data
{
    [Serializable]
    public class VariantParamPair
    {
        public string Key;
        [SerializeReference]
        public IVariantParam Param;

        public VariantParamPair(string key, IVariantParam value)
        {
            Key = key;
            Param = value;
        }

        public override string ToString()
        {
            return $"{Key} / {Param}";
        }
    }
}
