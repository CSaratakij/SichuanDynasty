using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;

        [SerializeField]
        GameObject pausePanel;


        void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {
                    if (Input.GetButtonDown("Player_Pause")) {
                        gameController.ToggleGamePause();
                        pausePanel.SetActive(gameController.IsGamePause);
                    }
                }
            }
        }
    }
}
