using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class GameController : MonoBehaviour
    {
        public const int MAX_PLAYER_SUPPORT = 2;
        public const int MAX_PHASE_PER_PLAYER = 2;
        public const float MAX_TIME_PER_PHASE = 60.0f;

        public bool IsGameInit { get { return _isGameInit; } }
        public bool IsGameStart { get { return _isGameStart; } }
        public bool IsGameOver { get { return _isGameOver; } }


        public enum Phase
        {
            None,
            Shuffle,
            Battle
        }


        int _currentPlayerIndex;

        bool _isGameInit;
        bool _isGameStart;
        bool _isGameOver;

        bool _isNextTurn;
        bool _isHasWinner;

        bool _isInitNextTurn;

        Phase _currentPhase;
        Player[] _players;
        Timer _timer;


        public GameController()
        {
            _currentPlayerIndex = 0;
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
            _isNextTurn = false;
            _isHasWinner = false;
            _isInitNextTurn = false;
            _currentPhase = Phase.None;
            _players = new Player[MAX_PLAYER_SUPPORT];
        }

        public void GameReset()
        {
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
        }

        public void GameInit()
        {
            _isGameOver = false;
            _isGameInit = true;
        }

        public void GameStart()
        {
            _isGameStart = true;
            //Test only..
            _isNextTurn = true;
        }

        public void GameOver()
        {
            _isGameOver = true;
        }


        void Awake()
        {
            for (int i = 0; i < _players.Length; i++) {
                _players[i] = gameObject.AddComponent(typeof(Player)) as Player;
            }
            _timer = gameObject.AddComponent(typeof(Timer)) as Timer;
            _timer.SetMaxTime(MAX_TIME_PER_PHASE);
        }

        void Update()
        {
            if (_isGameInit && _isGameStart && !_isGameOver) {
                Debug.Log("Game is started..");

                if (_isNextTurn) {
                    _timer.StartCountDown();
                    _isInitNextTurn = false;
                    _isNextTurn = false;
                }

                if (!_isInitNextTurn && !_timer.IsStarted && !_isHasWinner) {
                    Debug.Log("Begin next turn..");
                    _isInitNextTurn = true;
                    StartCoroutine("_NextTurn");
                }
                Debug.Log("Timer Left : " + _timer.TimeLeft);
            }
        }

        IEnumerator _NextTurn()
        {
            yield return new WaitForSeconds(2.0f);
            _isNextTurn = true;
        }
    }
}
