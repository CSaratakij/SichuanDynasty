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

                    if (gameController.Players[playerIndex].SelectedDeck.Cards.Count > 0) {
                        gameObject.SetActive(true);
                        txtCardNum.text = gameController.Players[playerIndex].SelectedDeck.Cards[0].ToString();

                    } else {
                        gameObject.SetActive(false);

                    }
                }
            }
        }
    }
}
