using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class FieldCardView : MonoBehaviour
    {
        [SerializeField]
        int playerIndex;

        [SerializeField]
        int fieldCardIndex;

        [SerializeField]
        GameController gameController;

        [SerializeField]
        Text txtCard;

        void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {
                    txtCard.text = gameController.Players[playerIndex].FieldDeck.Cards[fieldCardIndex].ToString();
                }
            }
        }
    }
}
