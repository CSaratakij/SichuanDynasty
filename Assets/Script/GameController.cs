using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class GameController : MonoBehaviour
    {
        public const int MAX_PLAYER_SUPPORT = 2;
        public const int MAX_PHASE_PER_PLAYER = 2;

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


        Phase _currentPhase;
        Player[] _players;


        public GameController()
        {
            _currentPlayerIndex = 0;
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
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
        }

        void Update()
        {
            if (_isGameInit && _isGameStart && !_isGameOver) {
                Debug.Log("Game is started..");
            }
        }
    }
}
