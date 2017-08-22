using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SichuanDynasty
{
    public class Deck
    {
        List<int> _cardList;


        public List<int> Cards { get { return _cardList; } }


        public Deck()
        {
            _cardList = new List<int>();
        }

        public void AddCard(int initCardValue, int totalCard)
        {
            for (int i = 0; i < totalCard; i++) {
                _cardList.Add(initCardValue);
                initCardValue++;
            }
        }
    }
}
