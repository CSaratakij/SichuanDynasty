using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;

        [SerializeField]
        Text txtTimer;


        void Update()
        {
            if (gameController && txtTimer) {
                if (gameController.IsGameInit && gameController.IsGameStart) {
                    txtTimer.text = gameController.GetComponent<Timer>().TimeLeft.ToString();
                }
            }
        }
    }
}
