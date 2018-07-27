using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Engine
{
    //게임에 있을 카드들, 데미지의 관리
    public class Board
    {
        private List<Card> cards = new List<Card>();
        private List<Card> fieldCard = new List<Card>();
        private Card currentCard;


        //카드 셔플
        public void SetCard()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j <= 13; j++)
                {
                    Card card = new Card((Card.Pattern)i, j);
                    cards.Add(card);
                }
            }
            cards.Add(new Card(Card.Pattern.Joker, -1));
            cards.Add(new Card(Card.Pattern.Joker, -1));

            cards = cards.OrderBy(x => Guid.NewGuid()).ToList();

            //for (int i = 0; i < cards.Count; i++)
            //{
            //    System.Console.WriteLine(cards[i].ToText());
            //}
        }

        //보드에 있는 카드를 가져옴
        public Card GetBoardCard()
        {
            Card card = cards[cards.Count-1];
            cards.Remove(card);

            //보드에 남은 카드가 부족하면 필드에 쌓여있던 카드들을 전부 가져와 셔플
            if (cards.Count == 0)
            {
                while (fieldCard.Count > 1)
                {
                    Card tempCard = fieldCard[0];
                    cards.Add(tempCard);
                    fieldCard.Remove(tempCard);
                }

                cards = cards.OrderBy(x => Guid.NewGuid()).ToList();
            }

            return card;
        }

        //보드에 있는 카드로부터 한 장을 가져와 필드카드에 사용한다.
        public void FieldCardSetting(Card card)
        {
            fieldCard.Add(card);
        }

        public Card GetFieldCard()
        {
            return fieldCard[fieldCard.Count-1];
        }

        public int GetBoardCardsAmount()
        {
            return cards.Count;
        }
    }
}
