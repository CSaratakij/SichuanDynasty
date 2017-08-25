using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


        bool _isInitShowGameOver;


        public UIManager()
        {
            _isInitShowGameOver = false;
        }


        void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart) {

                    if (!gameController.IsGameOver) {

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

        IEnumerator _ShowGameOverUI()
        {
            yield return new WaitForSeconds(1.0f);
            gameplayUI.SetActive(false);
            gameOverUI.SetActive(true);
        }
    }
}
