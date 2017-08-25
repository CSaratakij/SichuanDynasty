using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class DiscardCardView : MonoBehaviour
    {
        [SerializeField]
        int playerIndex;

        [SerializeField]
        GameController gameController;

        [SerializeField]
        Text txtCardNum;


        void Update()
        {
            if (gameController) {

                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {

                }
            }
        }
    }
}
