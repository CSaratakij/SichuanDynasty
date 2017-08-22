using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class Player : MonoBehaviour
    {
        bool _isTurn;

        Deck _deck;
        Deck _fieldDeck;
        Deck _selectedDeck;


        public bool IsTurn { get { return _isTurn; } }
        public Deck NormalDeck { get { return _deck; } }
        public Deck FieldDeck { get { return _fieldDeck; } }
        public Deck SelectedDeck { get { return _selectedDeck; } }


        public Player()
        {
            _isTurn = false;
            _fieldDeck = new Deck();
            _selectedDeck = new Deck();
            _deck = new Deck();
            _deck.AddCard(1, 9);
        }

        public void SetTurn(bool isTurn)
        {
            _isTurn = isTurn;
        }
    }
}
