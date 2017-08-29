using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class Player : MonoBehaviour
    {
        bool _isTurn;
        bool _isWin;

        Deck _deck;
        Deck _fieldDeck;
        Deck _selectedDeck;
        Deck _disableDeck;

        Status _health;
        Status _criticalStack;


        public bool IsTurn { get { return _isTurn; } }
        public bool IsWin { get { return _isWin; } }
        public bool IsCritical { get { return _health.Current <= GameController.MAX_CRITICAL_STACK; } }
        public Deck NormalDeck { get { return _deck; } }
        public Deck FieldDeck { get { return _fieldDeck; } }
        public Deck SelectedDeck { get { return _selectedDeck; } }
        public Deck DisableDeck { get { return _disableDeck; } }
        public Status Health { get { return _health; } }
        public Status CriticalStack { get { return _criticalStack; } }


        public Player()
        {
            _isTurn = false;
            _isWin = false;
            _fieldDeck = new Deck();
            _selectedDeck = new Deck();
            _deck = new Deck();
            _disableDeck = new Deck();
            _deck.AddCards(1, 9);
            _health = new Status(0, GameController.MAX_PLAYER_HEALTH_PER_GAME);
            _criticalStack = new Status(0, GameController.MAX_CRITICAL_STACK);
            _criticalStack.Clear();
        }

        public void SetTurn(bool isTurn)
        {
            _isTurn = isTurn;
        }

        public void SetWin(bool value)
        {
            _isWin = value;
        }

        public void FirstDraw(int totalCard)
        {
            for (int i = 0; i < totalCard; i++) {
                var index = (int)(Random.Range(0, _deck.Cards.Count - 1));
                _fieldDeck.Cards.Add(_deck.Cards[index]);
                _deck.Cards.RemoveAt(index);
            }
        }
    }
}
