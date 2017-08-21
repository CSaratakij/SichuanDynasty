using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SichuanDynasty.UI
{
    public class ReadyUIView : MonoBehaviour
    {
        [SerializeField]
        GameController gameController;

        [SerializeField]
        Image[] imgReadyList;

        [SerializeField]
        Sprite[] preReadySprites;

        [SerializeField]
        Sprite[] postReadySprites;

        [SerializeField]
        GameObject nextUI;


        bool _isProcess;
        bool[] _readyList;


        public ReadyUIView()
        {
            imgReadyList = new Image[GameController.MAX_PLAYER_SUPPORT];
            preReadySprites = new Sprite[GameController.MAX_PLAYER_SUPPORT];
            postReadySprites = new Sprite[GameController.MAX_PLAYER_SUPPORT];
            _readyList = new bool[GameController.MAX_PLAYER_SUPPORT];
            _isProcess = true;
        }


        void Update()
        {
            if (gameController && _isProcess) {
                if (gameController.IsGameInit) {

                    if (Input.GetButtonDown("StartPlayer1")) {
                        _ToggleReady(0);

                    } else if (Input.GetButtonDown("StartPlayer2")) {
                        _ToggleReady(1);

                    }

                    _CheckIsAllReady();
                }
            }
        }

        void _ToggleReady(int playerIndex)
        {
            _readyList[playerIndex] = !_readyList[playerIndex];
            var isReady = _readyList[playerIndex];

            if (isReady) {
                imgReadyList[playerIndex].sprite = postReadySprites[playerIndex];

            } else {
                imgReadyList[playerIndex].sprite = preReadySprites[playerIndex];

            }
        }

        void _CheckIsAllReady()
        {
            var isReady = true;
            foreach (bool result in _readyList) {
                if (result == false) {
                    isReady = false;
                    break;
                }
            }
            if (isReady) {
                _ChangeToNextUI();
            }
        }

        void _ChangeToNextUI()
        {
            gameObject.SetActive(false);
            nextUI.SetActive(true);
            gameController.GameStart();
            _isProcess = false;
        }
    }
}
