using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;

        [SerializeField]
        Image[] imgResults;

        [SerializeField]
        Sprite[] spriteAllResults;

        [SerializeField]
        Text txtTotalTurn;


        void Update()
        {
            if (gameController) {

                if (gameController.IsGameOver) {
                    txtTotalTurn.text = gameController.TotalTurn.ToString();

                    for (int i = 0; i < gameController.Players.Length; i++) {
                        imgResults[i].sprite = (gameController.Players[i].IsWin) ? spriteAllResults[0] : spriteAllResults[1];

                    }
                }
            }
        }
    }
}
