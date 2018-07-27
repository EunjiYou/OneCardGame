using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Engine
{
    public class Player
    {
        public List<Card> cards = new List<Card>();
        public int no;

        public Player(int no)
        {
            this.no = no;
        }

        public void TakeCard(Card card)
        {
            cards.Add(card);
        }

        public Card PlayCard(int number)
        {
            Card card = cards[number - 1];
            cards.Remove(card);
            return card;
        }
        
        public int GetCardAmount()
        {
            return cards.Count;
        }

        public Card GetCard(int number)
        {
            return cards[number - 1];
        }
    }
}
