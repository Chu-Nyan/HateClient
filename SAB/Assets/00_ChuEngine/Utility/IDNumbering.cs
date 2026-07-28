using System.Collections.Generic;

namespace Chu.Utility
{
    public class IDNumbering
    {
        private readonly Queue<int> _deactivate;
        private int _nextNumber;

        public IDNumbering(int startNumber = 0)
        {
            _deactivate = new(32);
            _nextNumber = startNumber;
        }

        public IDNumbering(int startNumber, int bufferCapacity)
        {
            _deactivate = new(bufferCapacity);
            _nextNumber = startNumber;
        }

        public int GetID()
        {
            if (_deactivate.Count == 0)
            {
                _deactivate.Enqueue(_nextNumber);
                _nextNumber++;
            }

            return _deactivate.Dequeue();
        }

        public void ReleaseID(int number)
        {
            _deactivate.Enqueue(number);
        }
    }
}
