using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using SichuanDynasty.UI;

namespace SichuanDynasty
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        GameObject[] firstSelectedCards;

        [SerializeField]
        EventSystem[] eventSystem;

        [SerializeField]
        GameObject[] parentCards;

        [SerializeField]
        UIManager uiManager;


        public const int MAX_PLAYER_SUPPORT = 2;
        public const int MAX_PLAYER_HEALTH_PER_GAME = 31;
        public const int MAX_PHASE_PER_PLAYER = 2;
        public const int MAX_FIELD_CARD_PER_GAME = 4;
        public const int MAX_HEAL_CARD = 2;
        public const float MAX_TIME_PER_PHASE = 60.0f;


        public bool IsGameInit { get { return _isGameInit; } }
        public bool IsGameStart { get { return _isGameStart; } }
        public bool IsGameOver { get { return _isGameOver; } }
        public bool IsGamePause { get { return _isGamePause; } }
        public bool IsInteractable { get { return _isInteractable; } }
        public bool IsExceedHealCard { get { return _isExceedHealCard; } }

        public int TotalTurn { get { return _totalTurn; } }
        public int CurrentPlayerIndex { get { return _currentPlayerIndex; } }
        public Phase CurrentPhase { get { return _currentPhase; } }

        public Player[] Players { get { return _players; } }

        public int[] FieldCardCache_1 { get { return _fieldCache_1; } }
        public int[] FieldCardCache_2 { get { return _fieldCache_2; } }


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
        bool _isInteractable;

        bool _isExceedHealCard;

        Phase _currentPhase;

        Player[] _players;
        Timer _timer;


        List<int> _currentSelectedCardCache;

        int[] _fieldCache_1;
        int[] _fieldCache_2;

        int _healCardStack;


        public GameController()
        {
            _currentPlayerIndex = 0;
            _totalTurn = 0;
            _isGameInit = false;
            _isGameStart = false;
            _isGameOver = true;
            _isGamePause = false;
            _isNextTurn = false;
            _isHasWinner = false;
            _isInitNextTurn = false;
            _isInteractable = true;
            _isExceedHealCard = false;
            _currentPhase = Phase.None;
            _players = new Player[MAX_PLAYER_SUPPORT];
            _currentSelectedCardCache = new List<int>();
            _fieldCache_1 = new int[MAX_FIELD_CARD_PER_GAME];
            _fieldCache_2 = new int[MAX_FIELD_CARD_PER_GAME];
            _healCardStack = 0;
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            _currentPlayerIndex = firstPlayerIndex;
            _players[firstPlayerIndex].SetTurn(true);
            _currentPhase = Phase.Shuffle;

            foreach (Player player in _players) {
                player.FirstDraw(MAX_FIELD_CARD_PER_GAME);
            }

            eventSystem[0].gameObject.SetActive(false);

            eventSystem[_currentPlayerIndex].gameObject.SetActive(true);
            eventSystem[_currentPlayerIndex].SetSelectedGameObject(firstSelectedCards[_currentPlayerIndex]);

            _fieldCache_1 = _players[0].FieldDeck.Cards.ToArray();
            _fieldCache_2 = _players[1].FieldDeck.Cards.ToArray();

            _isGameStart = true;
            _isNextTurn = true;
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

        public void NextPhase()
        {
            if (_currentPhase == Phase.Shuffle) {
                _currentPhase = Phase.Battle;
            }

            SetInteractable(true);
            _currentSelectedCardCache.Clear();
            _players[_currentPlayerIndex].SelectedDeck.Cards.Clear();
        }

        public void ToggleSelect(int index)
        {
            var targetPlayer = _players[_currentPlayerIndex];
            var targetCard = 0;

            if (_currentPlayerIndex == 0) {
                targetCard = _fieldCache_1[index];

            } else if (_currentPlayerIndex == 1) {
                targetCard = _fieldCache_2[index];

            }

            switch (_currentPhase) {
                case Phase.Shuffle:
                    targetPlayer.NormalDeck.Cards.Add(targetCard);
                    targetPlayer.FieldDeck.Cards.Remove(targetCard);

                    var newCard = targetPlayer.NormalDeck.Cards[0];

                    targetPlayer.FieldDeck.Cards.Add(newCard);
                    targetPlayer.NormalDeck.Cards.Remove(newCard);

                    if (_currentPlayerIndex == 0) {
                        _fieldCache_1 = targetPlayer.FieldDeck.Cards.ToArray();

                    } else if (_currentPlayerIndex == 1) {
                        _fieldCache_2 = targetPlayer.FieldDeck.Cards.ToArray();

                    }

                break;

                case Phase.Battle:
                    if (_currentSelectedCardCache.Contains(targetCard)) {
                        _currentSelectedCardCache.Remove(targetCard);

                    } else {
                        _currentSelectedCardCache.Add(targetCard);

                    }

                break;

                default:
                break;
            }
        }

        public void SetInteractable(bool value)
        {
            _isInteractable = value;
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

                    if (_timer.IsStarted) {
                        if (!_timer.IsFinished) {
                            _PhaseHandle();

                        }

                    } else {
                        if (_timer.IsFinished) {
                            if (!_isInitNextTurn) {
                                _NextTurn();
                                _isInitNextTurn = true;
                            }
                        }
                    }

                } else {
                    GameOver();
                    _timer.Stop();

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
                    NextPhase();
                } 

            } else if (_currentPlayerIndex == 1) {
                if (Input.GetButtonDown("Player2_Y")) {
                    NextPhase();
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
                        _Attack(1);

                } else if (Input.GetButtonDown("Player1_B")) {
                        _Heal(0);

                }

            } else if (_currentPlayerIndex == 1) {
                if (Input.GetButtonDown("Player2_Y")) {
                    if (!_isInitNextTurn) {
                        _NextTurn();
                        _isInitNextTurn = true;
                    }

                } else if (Input.GetButtonDown("Player2_X")) {
                        _Attack(0);

                } else if (Input.GetButtonDown("Player2_B")) {
                        _Heal(1);

                }
            }
        }

        void _MoveUsedCard()
        {
            if (_currentSelectedCardCache.Count > 0) {
                for (int i = 0; i < _currentSelectedCardCache.Count; i++) {
                    _players[_currentPlayerIndex].DisableDeck.Cards.Add(_currentSelectedCardCache[i]);
                    _players[_currentPlayerIndex].FieldDeck.Cards.Remove(_currentSelectedCardCache[i]);
                }

                uiManager.InitHandleSelectingCards(_currentPlayerIndex);
                uiManager.UpdateAvailableButton(_currentPlayerIndex);
            }
        }

        void _Attack(int targetIndex)
        {
            var isAttakAble = _IsAttackAble(targetIndex);

            if (isAttakAble) {

                _SetActivateCard(_currentPlayerIndex, false);

                var totalPoint = 0;
                for (int i = 0; i < _currentSelectedCardCache.Count; i++) {
                    totalPoint += _currentSelectedCardCache[i];

                }

                _MoveUsedCard();

                _currentSelectedCardCache.Clear();
                _players[targetIndex].Health.Remove(totalPoint);

                _CheckWinner();
                _ReHightlightCard();
            }
        }

        bool _IsAttackAble(int targetIndex)
        {
            var totalPoint = 0;
            foreach (int point in _currentSelectedCardCache) {
                totalPoint += point;
            }

            return totalPoint <= _players[targetIndex].Health.Current;
        }

        void _Heal(int targetIndex)
        {
            var isHealable = _IsHealable();

            if (isHealable) {
                _SetActivateCard(_currentPlayerIndex, false);
                var totalPoint = 0;

                foreach (int point in _currentSelectedCardCache) {
                    totalPoint += point;
                }

                _MoveUsedCard();
                _currentSelectedCardCache.Clear();

                _players[targetIndex].Health.Restore(totalPoint);
                _ReHightlightCard();
            }
        }

        bool _IsHealable()
        {
            var totalPoint = 0;

            if (_currentSelectedCardCache.Count > MAX_HEAL_CARD) {
                _isExceedHealCard = true;
                return false;

            } else {
                if (_healCardStack < MAX_HEAL_CARD) {
                    if (_currentSelectedCardCache.Count <= (MAX_HEAL_CARD - _healCardStack)) {

                        foreach (int point in _currentSelectedCardCache) {
                            totalPoint += point;
                        }

                        if ((_players[_currentPlayerIndex].Health.Current + totalPoint) <= MAX_PLAYER_HEALTH_PER_GAME) {
                            _healCardStack += _currentSelectedCardCache.Count;
                            _isExceedHealCard = _healCardStack >= MAX_HEAL_CARD;
                            return true;

                        } else {
                            return false;

                        }
                    } else {
                        _isExceedHealCard = true;
                        return false;

                    }

                } else {
                    _isExceedHealCard = true;
                    return false;

                }
            }
        }

        void _CheckWinner()
        {
            for (int i = 0; i < _players.Length; i++) {
                if (_players[i].Health.Current <= 0) {

                    if (i == 0) {
                        _players[0].SetWin(false);
                        _players[1].SetWin(true);

                    } else if (i == 1) {
                        _players[0].SetWin(true);
                        _players[1].SetWin(false);

                    }

                    _isHasWinner = true;
                    break;
                }
            }
        }

        void _SetActivateCard(int playerIndex, bool isActivate)
        {
            foreach (Transform cardObj in parentCards[playerIndex].gameObject.transform) {
                var view = cardObj.gameObject.GetComponent<FieldCardView>();
                if (view.IsSelected) {
                    cardObj.gameObject.SetActive(isActivate);
                }
            }
        }

        void _DrawNewCard(int playerIndex)
        {
            var totalDraw = MAX_FIELD_CARD_PER_GAME - _players[playerIndex].FieldDeck.Cards.Count;

            for (int i = 0; i < totalDraw; i++) {
                var newCardIndex = (int)Random.Range(0, _players[playerIndex].NormalDeck.Cards.Count - 1);
                var newCard = _players[playerIndex].NormalDeck.Cards[newCardIndex];

                _players[playerIndex].FieldDeck.Cards.Add(newCard);
                _players[playerIndex].NormalDeck.Cards.Remove(newCard);
            }

            if (playerIndex == 0) {
                _fieldCache_1 = _players[playerIndex].FieldDeck.Cards.ToArray();

            } else if (playerIndex == 1) {
                _fieldCache_2 = _players[playerIndex].FieldDeck.Cards.ToArray();

            }

            for (int i = 0; i < _players[playerIndex].DisableDeck.Cards.Count; i++) {
                _players[playerIndex].NormalDeck.Cards.Add(_players[playerIndex].DisableDeck.Cards[i]);
            }

            _players[playerIndex].DisableDeck.Cards.Clear();
        }

        void _NextTurn()
        {
            _isInitNextTurn = true;
            _timer.Stop();
            uiManager.DisableHandlingSelectingCards();
            StartCoroutine("_NextTurnCallBack");
        }

        void _ChangePlayer()
        {
            _currentSelectedCardCache.Clear();
            _players[_currentPlayerIndex].SelectedDeck.Cards.Clear();

            _players[_currentPlayerIndex].SetTurn(false);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(false);

            _currentPlayerIndex = (_currentPlayerIndex == (_players.Length - 1)) ? 0 : _currentPlayerIndex + 1;

            _players[_currentPlayerIndex].SetTurn(true);
            eventSystem[_currentPlayerIndex].gameObject.SetActive(true);

            eventSystem[_currentPlayerIndex].SetSelectedGameObject(firstSelectedCards[_currentPlayerIndex]);
        }

        void _ReHightlightCard()
        {
            foreach (Transform cardObj in parentCards[_currentPlayerIndex].gameObject.transform) {
                if (cardObj.gameObject.activeSelf) {
                    eventSystem[_currentPlayerIndex].SetSelectedGameObject(cardObj.gameObject);
                    break;
                }
            }
        }

        IEnumerator _NextTurnCallBack()
        {
            yield return new WaitForSeconds(0.8f);
            _totalTurn++;
            _healCardStack = 0;
            _isExceedHealCard = false;
            _DrawNewCard(_currentPlayerIndex);
            _SetActivateCard(_currentPlayerIndex, true);
            _ChangePlayer();
            _currentPhase = Phase.Shuffle;
            _isNextTurn = true;
        }
    }
}
