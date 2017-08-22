using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField]
        int playerIndex;

        [SerializeField]
        GameController gameController;

        [SerializeField]
        Text txtHealths;


        void Update()
        {
            if (gameController) {
                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {
                    txtHealths.text = gameController.Players[playerIndex].Health.Current.ToString();
                }
            }
        }
    }
}
