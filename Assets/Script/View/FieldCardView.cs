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

        [SerializeField]
        Image imgCard;

        [SerializeField]
        Sprite[] imgAllCardState;


        public bool IsSelected { get { return _isSelected; } }


        bool _isSelected;


        public FieldCardView()
        {
            _isSelected = false;
        }

        public void ToggleSelect()
        {
            if (gameController.IsInteractable) {
                _isSelected = !_isSelected;
                gameController.ToggleSelect(fieldCardIndex);

                if (_isSelected && (gameController.CurrentPhase == GameController.Phase.Shuffle)) {
                    gameController.SetInteractable(false);
                    StartCoroutine("_ChangeToBattlePhase");
                }
            }
        }


        void Update()
        {
            if (gameController) {

                if (gameController.IsGameInit && gameController.IsGameStart && !gameController.IsGameOver) {

                    if (playerIndex == 0) {
                        txtCard.text = gameController.FieldCardCache_1[fieldCardIndex].ToString();

                    } else if (playerIndex == 1) {
                        txtCard.text = gameController.FieldCardCache_2[fieldCardIndex].ToString();
                    }

                    imgCard.sprite = (_isSelected) ? imgAllCardState[1] : imgAllCardState[0];

                    if (_isSelected) { 
                        if (gameController.CurrentPhase == GameController.Phase.Shuffle) {
                            ToggleSelect();
                            _isSelected = false;
                        }
                    }
                }
            }
        }

        IEnumerator _ChangeToBattlePhase()
        {
            yield return new WaitForSeconds(0.5f);
            gameController.NextPhase();
        }
    }
}
