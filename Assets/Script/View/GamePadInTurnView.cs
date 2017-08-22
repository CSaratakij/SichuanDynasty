using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class GamePadInTurnView : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;

        [SerializeField]
        GameObject[] imgAllHideOptions;


        void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {
                    var isShow = (gameController.CurrentPhase == GameController.Phase.Battle);
                    foreach (GameObject obj in imgAllHideOptions) {
                        obj.SetActive(isShow);
                    }
                }
            }
        }
    }
}
