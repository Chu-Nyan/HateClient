using SAB.Cutscene;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chu.Data
{
    [Serializable]
    public class VariableParam
    {
        public static readonly Dictionary<ParameterType, Type> EnumTypes = new()
        {
             { ParameterType.UniqueEntity,typeof(UniqueEntityType) }
        };

        public ParameterType Type;
        public int IntValue;
        public float FloatValue;
        public bool BoolValue;
        public string StringValue;
        public Vector2 Vector2Value;
        public Vector3 Vector3Value;
        public MonoBehaviour Script;

        public VariableParam(ParameterType type)
        {
            Type = type;
        }
    }
}

