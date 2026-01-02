using Chu.Utility;
using System;

namespace Chu
{
    public class InstanceIDService
    {
        private static InstanceIDService _instance;
        private readonly IDNumbering _numbering;

        public InstanceIDService()
        {
            if (_instance != null)
                throw new Exception("InstanceIDService 중복 생성");

            _numbering = new();
            _instance = this;
            _numbering.GetID();
        }

        public static int AcquireID()
        {
            return _instance._numbering.GetID();
        }
    }
}
