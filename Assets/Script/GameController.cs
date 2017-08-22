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
        public const int MAX_PHASE_PER_PLAYER = 2;
        public const int MAX_FIELD_CARD_PER_GAME = 4;
        public const float MAX_TIME_PER_PHASE = 60.0f;
        public const float MAX_PLAYER_HEALTH_PER_GAME = 30.0f;

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

        Phase _currentPhase;

        Player[] _players;
        Timer _timer;

        Deck _currentPlayerSelectedCardCache;


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
            _currentPhase = Phase.None;
            _players = new Player[MAX_PLAYER_SUPPORT];
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

                if (_timer.IsStarted && !_timer.IsFinished && !_isHasWinner) {
                    _PhaseHandle();
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
                //Get Input if player want to be on next phase -> Battle

            } else if (_currentPlayerIndex == 1) {
                //Get Input if player want to be on next phase -> Battle

            }
        }

        void _BattlePhaseHandle()
        {
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

        void _ChangePlayer()
        {
            _players[_currentPlayerIndex].SetTurn(false);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(false);

            _currentPlayerIndex = (_currentPlayerIndex == (_players.Length - 1)) ? 0 : _currentPlayerIndex + 1;

            _players[_currentPlayerIndex].SetTurn(true);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(true);
        }

        IEnumerator _NextTurn()
        {
            yield return new WaitForSeconds(0.8f);
            _totalTurn++;
            _currentPhase = Phase.Shuffle;
            _ChangePlayer();
            _isNextTurn = true;
        }
    }
}
