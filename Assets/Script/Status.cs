using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class Status
    {
        float _current;
        float _max;
        float _min;


        public float Current { get { return _current; } }
        public float Max { get { return _max; } }
        public float Min { get { return _min; } }


        public Status(float min, float max)
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

        public void Restore(float value)
        {
            _current = ((_current + value) > _max) ? _max : _current + value;
        }

        public void Remove(float value)
        {
            _current = ((_current - value) < _min) ? _min : _current - value;
        }
    }
}
