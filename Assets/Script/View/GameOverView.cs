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
                        anims[0].SetBool("IsWin", true);
                        anims[1].SetBool("IsLose", true);

                    } else {
                        anims[0].SetBool("IsLose", true);
                        anims[1].SetBool("IsWin", true);

                    }

                    for (int i = 0; i < gameController.Players.Length; i++) {
                        imgResults[i].sprite = (gameController.Players[i].IsWin) ? spriteAllResults[0] : spriteAllResults[1];

                    }
                }
            }
        }
    }
}
