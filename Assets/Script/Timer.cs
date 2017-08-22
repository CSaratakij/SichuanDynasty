using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        float maxTime;


        public float TimeLeft { get { return _timeLeft; } }
        public bool IsStarted { get { return _isStarted; } }
        public bool IsFinished { get { return _isFinished; } }
        public bool IsPaused { get { return _isPaused; } }

        float _timeLeft;

        bool _isStarted;
        bool _isFinished;
        bool _isPaused;


        public Timer()
        {
            _timeLeft = 0.0f;
            _isStarted = false;
            _isFinished = true;
            _isPaused = false;
        }

        public void StartCountDown()
        {
            _timeLeft = maxTime;
            _isFinished = false;
            _isStarted = true;
            StartCoroutine("_TimerCallBack");
        }

        public void SetMaxTime(float value)
        {
            maxTime = value;
        }

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }


        IEnumerator _TimerCallBack()
        {
            while (_timeLeft > 0.0f) {
                if (_isPaused) {
                    yield return null;

                } else {
                    yield return new WaitForSeconds(1.0f);

                    _timeLeft -= 1.0f;
                    if (_timeLeft <= 0.0f) {
                        break;
                    }
                }
            }

            _isFinished = true;
            _isStarted = false;
        }
    }
}
