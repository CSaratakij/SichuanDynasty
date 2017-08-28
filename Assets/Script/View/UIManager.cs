using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;

        [SerializeField]
        GameObject pausePanel;

        [SerializeField]
        GameObject[] disableDecksView;

        [SerializeField]
        Text[] txtCardNum;

        [SerializeField]
        GameObject gameOverUI;

        [SerializeField]
        GameObject gameplayUI;

        [SerializeField]
        EventSystem[] allEventSystem;

        [SerializeField]
        GameObject btnRestart;

        [SerializeField]
        GameObject[] player1Cards;

        [SerializeField]
        GameObject[] player2Cards;

        [SerializeField]
        Image imgCurrentPlayerTurn;

        [SerializeField]
        Sprite[] spritePlayerTurn;

        [SerializeField]
        GameObject[] selectDialogs;

        [SerializeField]
        GameObject[] imgPhases;

        [SerializeField]
        Image imgPlayerTurn;

        [SerializeField]
        Sprite[] spriteAllPlayerTurn;

        [SerializeField]
        GameObject[] imgWarning;


        bool _isInitShowGameOver;

        bool _isInitHandle;
        bool _isHandlingSelectingCards;
        bool _isAxisInUse;

        List<GameObject> _currentAvailableButton;
        int _currentSelectIndex;


        public UIManager()
        {
            _isInitShowGameOver = false;
            _isInitHandle = false;
            _isHandlingSelectingCards = false;
            _isAxisInUse = false;
            _currentAvailableButton = new List<GameObject>();
            _currentSelectIndex = 0;
        }

        public void InitHandleSelectingCards(int playerIndex)
        {
            _isInitHandle = true;
            UpdateAvailableButton(playerIndex);
        }

        public void DisableHandlingSelectingCards()
        {
            _isInitHandle = false;
            _isHandlingSelectingCards = false;
            _currentAvailableButton.Clear();
            _currentSelectIndex = 0;
            _isAxisInUse = false;
        }

        public void UpdateAvailableButton(int playerIndex)
        {
            _currentAvailableButton.Clear();
            _currentSelectIndex = 0;

            if (playerIndex == 0) {
                foreach (GameObject obj in player1Cards) {
                    if (obj.activeSelf) {
                        _currentAvailableButton.Add(obj);
                    }
                }

            } else if (playerIndex == 1) {
                foreach (GameObject obj in player2Cards) {
                    if (obj.activeSelf) {
                        _currentAvailableButton.Add(obj);
                    }
                }
            }
        }

        public void HideAllSelectDialog()
        {
            foreach (GameObject obj in selectDialogs) {
                obj.SetActive(false);
            }
        }

        public void AlertCurrentPhase(GameController.Phase phase)
        {
            switch (phase) {
                case GameController.Phase.Shuffle:
                    imgPlayerTurn.sprite = gameController.Players[0].IsTurn ? spriteAllPlayerTurn[0] : spriteAllPlayerTurn[1];
                    StartCoroutine("_ShowShufflePhaseAlert");
                break;

                case GameController.Phase.Battle:
                    StartCoroutine("_ShowBattlePhaseAlert");
                break;
            }
        }

        public void AlertWarning(int cause)
        {
            ClearWarning();
            if (cause < imgWarning.Length) {
                imgWarning[cause].SetActive(true);
            }
        }

        public void ClearWarning()
        {
            foreach (GameObject obj in imgWarning) {
                obj.SetActive(false);
            }
        }


        void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart) {

                    if (!gameController.IsGameOver) {

                        imgCurrentPlayerTurn.sprite = gameController.Players[0].IsTurn ? spritePlayerTurn[0] : spritePlayerTurn[1];

                        if (Input.GetButtonDown("Player_Pause")) {
                            gameController.ToggleGamePause();
                            pausePanel.SetActive(gameController.IsGamePause);
                        }

                        for (int i = 0; i < disableDecksView.Length; i++) {

                            if (gameController.Players[i].DisableDeck.Cards.Count > 0) {
                                disableDecksView[i].SetActive(true);
                                txtCardNum[i].text = gameController.Players[i].DisableDeck.Cards[0].ToString();

                            } else {
                                disableDecksView[i].SetActive(false);

                            }
                        }

                        if (_isInitHandle) {

                            for (int i = 0; i < gameController.Players.Length; i++) {
                                if (gameController.Players[i].IsTurn) {
                                    _HandleSelectCards(i);
                                    break;
                                }
                            }

                            _isInitHandle = false;
                        }

                        if (_isHandlingSelectingCards) {
                            _HandleSelectCardsFromPlayer(gameController.CurrentPlayerIndex);
                            
                        }

                    } else {
                        if (gameOverUI && gameplayUI) {
                            if (!_isInitShowGameOver) {
                                StartCoroutine("_ShowGameOverUI");
                                _isInitShowGameOver = true;

                            }
                        }
                    }
                }
            }
        }

        void _HandleSelectCards(int playerIndex)
        {
            var isNeedHandle = _IsNeedHandleSelecting(playerIndex);
            if (isNeedHandle) {

                if (playerIndex == 0) {
                    foreach (GameObject obj in player1Cards) {
                        if (obj.activeSelf) {
                            allEventSystem[1].SetSelectedGameObject(obj);
                        }
                        break;
                    }

                } else if (playerIndex == 1) {
                    foreach (GameObject obj in player2Cards) {
                        if (obj.activeSelf) {
                            allEventSystem[2].SetSelectedGameObject(obj);
                        }
                        break;
                    }

                }

                _isHandlingSelectingCards = true;
            }
        }

        bool _IsNeedHandleSelecting(int playerIndex)
        {
            var isNeed = false;

            if (playerIndex == 0) {
                foreach (GameObject obj in player1Cards) {
                    if (!obj.activeSelf) {
                        isNeed = true;
                        break;
                    }
                }

            } else if (playerIndex == 1) {
                foreach (GameObject obj in player2Cards) {
                    if (!obj.activeSelf) {
                        isNeed = true;
                        break;
                    }
                }
            }

            return isNeed;
        }

        void _HandleSelectCardsFromPlayer(int playerIndex)
        {
            if (_currentAvailableButton.Count > 0) {
                if (playerIndex == 0) {

                    var axis = Input.GetAxisRaw("Player1_Vertical");
                    if (!_isAxisInUse) {
                        if (axis == 1) {
                            _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? 0 : _currentSelectIndex - 1;
                            allEventSystem[1].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;

                        } else if (axis == -1) {
                            _currentSelectIndex = (_currentSelectIndex + 1) > (_currentAvailableButton.Count - 1) ? (_currentAvailableButton.Count - 1) : _currentSelectIndex + 1;
                            allEventSystem[1].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;
                        }
                    } else {
                        if (axis == 0) {
                            _isAxisInUse = false;

                        }
                    }

                } else if (playerIndex == 1) {
                    var axis = Input.GetAxisRaw("Player2_Vertical");
                    if (!_isAxisInUse) {
                        if (axis == 1) {
                            _currentSelectIndex = (_currentSelectIndex - 1) < 0 ? 0 : _currentSelectIndex - 1;
                            allEventSystem[2].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;

                        } else if (axis == -1) {
                            _currentSelectIndex = (_currentSelectIndex + 1) > (_currentAvailableButton.Count - 1) ? (_currentAvailableButton.Count - 1) : _currentSelectIndex + 1;
                            allEventSystem[2].SetSelectedGameObject(_currentAvailableButton[_currentSelectIndex]);
                            _isAxisInUse = true;
                        }
                    } else {
                        if (axis == 0) {
                            _isAxisInUse = false;

                        }
                    }


                }
            }
        }


        IEnumerator _ShowGameOverUI()
        {
            yield return new WaitForSeconds(1.0f);
            gameplayUI.SetActive(false);
            gameOverUI.SetActive(true);

            for (int i = 0; i < allEventSystem.Length; i++) {
                allEventSystem[i].gameObject.SetActive(false);
            }

            allEventSystem[0].gameObject.SetActive(true);
            allEventSystem[0].SetSelectedGameObject(btnRestart);
        }

        IEnumerator _ShowShufflePhaseAlert()
        {
            imgPlayerTurn.gameObject.SetActive(true);
            imgPhases[0].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            imgPhases[0].SetActive(false);
            imgPlayerTurn.gameObject.SetActive(false);
        }

        IEnumerator _ShowBattlePhaseAlert()
        {
            imgPhases[1].SetActive(true);
            yield return new WaitForSeconds(1.0f);
            imgPhases[1].SetActive(false);
        }
    }
}
