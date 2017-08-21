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


        bool _isGameInit;
        bool _isGameStart;
        bool _isGameOver;


        public GameController()
        {
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
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
    }
}
