using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class Status
    {
        int _current;
        int _max;
        int _min;


        public int Current { get { return _current; } }
        public int Max { get { return _max; } }
        public int Min { get { return _min; } }


        public Status(int min, int max)
        {
            _min = min;
            _max = max;
            _current = max;
        }


        public void FullRestore()
        {
            _current = _max;
        }

        public void Clear()
        {
            _current = _min;
        }

        public void Restore(int value)
        {
            _current = ((_current + value) > _max) ? _max : _current + value;
        }

        public void Remove(int value)
        {
            _current = ((_current - value) < _min) ? _min : _current - value;
        }
    }
}
