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
            _deck = new Deck(1, 9);
            _fieldDeck = new Deck(0, 0);
            _selectedDeck = new Deck(0, 0);
        }

        public void SetTurn(bool isTurn)
        {
            _isTurn = isTurn;
        }
    }
}
