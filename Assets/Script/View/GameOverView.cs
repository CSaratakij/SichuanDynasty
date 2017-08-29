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

        [SerializeField]
        Animator[] anims;


        void Update()
        {
            if (gameController) {

                if (gameController.IsGameOver) {
                    txtTotalTurn.text = gameController.TotalTurn.ToString();

                    var isPlayer1Win = gameController.Players[0].IsWin;

                    if (isPlayer1Win) {
                        anims[0].Play("Win");
                        anims[1].Play("Lose");

                    } else {
                        anims[0].Play("Lose");
                        anims[1].Play("Win");

                    }

                    for (int i = 0; i < gameController.Players.Length; i++) {
                        imgResults[i].sprite = (gameController.Players[i].IsWin) ? spriteAllResults[0] : spriteAllResults[1];

                    }
                }
            }
        }
    }
}
