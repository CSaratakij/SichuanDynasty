using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SichuanDynasty
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        GameObject[] firstSelectedCards;

        [SerializeField]
        EventSystem[] eventSystem;


        public const int MAX_PLAYER_SUPPORT = 2;
        public const int MAX_PLAYER_HEALTH_PER_GAME = 30;
        public const int MAX_PHASE_PER_PLAYER = 2;
        public const int MAX_FIELD_CARD_PER_GAME = 4;
        public const float MAX_TIME_PER_PHASE = 60.0f;

        public bool IsGameInit { get { return _isGameInit; } }
        public bool IsGameStart { get { return _isGameStart; } }
        public bool IsGameOver { get { return _isGameOver; } }
        public bool IsGamePause { get { return _isGamePause; } }

        public int TotalTurn { get { return _totalTurn; } }
        public Phase CurrentPhase { get { return _currentPhase; } }

        public Player[] Players { get { return _players; } }


        public enum Phase
        {
            Shuffle,
            Battle,
            None
        }


        int _firstPlayerIndex;
        int _currentPlayerIndex;
        int _totalTurn;

        bool _isGameInit;
        bool _isGameStart;
        bool _isGameOver;
        bool _isGamePause;

        bool _isNextTurn;
        bool _isHasWinner;

        bool _isInitNextTurn;

        bool _isAttacked;
        bool _isHealed;

        Phase _currentPhase;

        Player[] _players;
        Timer _timer;


        List<int> _currentSelectedCardCache;


        public GameController()
        {
            _firstPlayerIndex = 0;
            _currentPlayerIndex = 0;
            _totalTurn = 0;
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
            _isGamePause = false;
            _isNextTurn = false;
            _isHasWinner = false;
            _isInitNextTurn = false;
            _isAttacked = false;
            _isHealed = false;
            _currentPhase = Phase.None;
            _players = new Player[MAX_PLAYER_SUPPORT];
            _currentSelectedCardCache = new List<int>();
        }

        public void ExitGame()
        {
            Application.Quit();
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

        public void GameStart(int firstPlayerIndex)
        {
            _isNextTurn = true;
            _firstPlayerIndex = firstPlayerIndex;
            _currentPlayerIndex = firstPlayerIndex;
            _players[firstPlayerIndex].SetTurn(true);
            _currentPhase = Phase.Shuffle;

            foreach (Player player in _players) {
                player.FirstDraw(MAX_FIELD_CARD_PER_GAME);
            }

            eventSystem[0].gameObject.SetActive(false);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(true);
            eventSystem[_currentPlayerIndex].SetSelectedGameObject(firstSelectedCards[_currentPlayerIndex]);

            _isGameStart = true;
        }

        public void GameOver()
        {
            _isGameOver = true;
        }

        public void ToggleGamePause()
        {
            _isGamePause = !_isGamePause;
            Time.timeScale = (_isGamePause) ? 0.0f : 1.0f;
        }

        public void SetFirstStarter(int index)
        {
            _firstPlayerIndex = index;
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
            HandleGame();
        }

        void HandleGame()
        {
            if (_isGameInit && _isGameStart && !_isGameOver) {

                if (!_isHasWinner) {

                    if (_isNextTurn) {
                        _timer.Stop();
                        _timer.StartCountDown();
                        _isInitNextTurn = false;
                        _isNextTurn = false;
                    }

                    if (!_timer.IsStarted) {
                        if (!_isInitNextTurn) {
                            _NextTurn();
                        }

                    } else {
                        if (!_timer.IsFinished) {
                            _PhaseHandle();

                        }
                    }

                } else {
                    GameOver();

                }
            }
        }

        void _PhaseHandle()
        {
            switch (_currentPhase) {
                case Phase.Shuffle:
                    _ShufflePhaseHandle();
                break;

                case Phase.Battle:
                    _BattlePhaseHandle();
                break;

                default:
                break;
            }
        }

        void _ShufflePhaseHandle()
        {
            if (_currentPlayerIndex == 0) {
                if (Input.GetButtonDown("Player1_Y")) {
                    _NextPhase();
                } 

            } else if (_currentPlayerIndex == 1) {
                if (Input.GetButtonDown("Player2_Y")) {
                    _NextPhase();
                }

            }
        }

        void _BattlePhaseHandle()
        {
            if (_currentPlayerIndex == 0) {
                if (Input.GetButtonDown("Player1_Y")) {
                    if (!_isInitNextTurn) {
                        _NextTurn();
                        _isInitNextTurn = true;
                    }

                } else if (Input.GetButtonDown("Player1_X")) {
                    if (!_isAttacked) {
                        _Attack(_currentPlayerIndex);
                        _isAttacked = true;
                    }
                } else if (Input.GetButtonDown("Player1_B")) {
                    if (!_isHealed) {
                        _Heal(_currentPlayerIndex);
                        _isHealed = true;
                    }

                }

            } else if (_currentPlayerIndex == 1) {
                if (Input.GetButtonDown("Player2_Y")) {
                    if (!_isInitNextTurn) {
                        _NextTurn();
                        _isInitNextTurn = true;
                    }

                } else if (Input.GetButtonDown("Player2_X")) {
                    if (!_isAttacked) {
                        _Attack(_currentPlayerIndex);
                        _isAttacked = true;
                    }

                } else if (Input.GetButtonDown("Player2_B")) {
                    if (!_isHealed) {
                        _Heal(_currentPlayerIndex);
                        _isHealed = true;
                    }

                }

            }
        }

        void _Attack(int playerIndex)
        {
        }

        void _Heal(int playerIndex)
        {
        }

        void _NextPhase()
        {
            if (_currentPhase == Phase.Shuffle) {
                _currentPhase = Phase.Battle;

            }
        }

        void _NextTurn()
        {
            _isInitNextTurn = true;
            _isAttacked = false;
            _isHealed = false;
            _timer.Stop();
            StartCoroutine("_NextTurnCallBack");
        }

        void _ChangePlayer()
        {
            _players[_currentPlayerIndex].SetTurn(false);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(false);

            _currentPlayerIndex = (_currentPlayerIndex == (_players.Length - 1)) ? 0 : _currentPlayerIndex + 1;

            _players[_currentPlayerIndex].SetTurn(true);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(true);
            eventSystem[_currentPlayerIndex].SetSelectedGameObject(firstSelectedCards[_currentPlayerIndex]);
        }

        IEnumerator _NextTurnCallBack()
        {
            yield return new WaitForSeconds(0.8f);
            _totalTurn++;
            _ChangePlayer();
            _currentPhase = Phase.Shuffle;
            _isNextTurn = true;
        }
    }
}
