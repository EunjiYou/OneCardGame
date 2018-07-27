using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Engine
{
    //턴 구분 및 카드 제시에 대한 검사
    public class Dealer
    {
        public enum Direction { Left = 0, Right };

        private int currentPlayerNumbers;
        private Card.Pattern currentPattern;
        private int currentNumber;
        private int playCount = 1;
        public Card.Pattern changePattern = Card.Pattern.None;
        private Direction direction = Direction.Right;
        private int damage = 0;


        //선제를 잡는 플레이어가 지정되는 게임일 시 이를 딜러에게 전해줌
        public void SetCurrentPlayer(int playerNumber)
        {
            currentPlayerNumbers = playerNumber;
        }

        public void SetNextTurn()
        {
            if (direction == Direction.Left)
            {
                if (--currentPlayerNumbers < 0)
                {
                    //마지막 플레이어의 위치로 이동
                    currentPlayerNumbers = 1;
                }
            }
            else
            {
                if (++currentPlayerNumbers > 1)
                {
                    //처음 플레이어 위치로 이동
                    currentPlayerNumbers = 0;
                }
            }
        }

        public int GetCurrentPlayer()
        {
            //다음 턴이 시작되었을 때 count를 초기화
            playCount = 1;
            return currentPlayerNumbers;
        }

        //필드의 문양을 바꿔줌
        public void SetCurrentPattern(Card.Pattern pattern)
        {
            currentPattern = pattern;
        }

        public void ChangePattern(Card.Pattern pattern)
        {
            changePattern = pattern;
        }

        public void SetCurrentFieldCard(Board board)
        {
            Card card = board.GetFieldCard();
            currentPattern = card.pattern;
            currentNumber = card.number;
        }
        
        public Direction GetCurrentDirection()
        {
            return direction;
        }

        public void SetCurrentDirection(Direction direction)
        {
            this.direction = direction;
        }

        public int GetPlayCount()
        {
            return playCount;
        }

        public void PlusPlayCount()
        {
            playCount++;
        }

        public void ResetPlayCount()
        {
            playCount = 0;
        }

        public void PlusDamage(int i)
        {
            damage += i;
        }

        public void ResetDamage()
        {
            damage = 0;
        }

        public int GetDamage()
        {
            return damage;
        }

        public bool CanPlayCard(Card card)
        {
            //공격카드 패널티가 쌓이고 있는 도중엔 패널티 카드 우위에 따라서 카드를 제시해야만 함
            if (damage > 1)
            {
                if (currentPattern == Card.Pattern.Joker)
                {
                    if (card.pattern == Card.Pattern.Joker)
                    {
                        return true;
                    }
                    return false;
                }
                if (currentNumber == 1)
                {
                    if (card.number == 1 || card.pattern == Card.Pattern.Joker)
                    {
                        return true;
                    }
                    return false;
                }
                if (currentNumber == 2)
                {
                    if (card.number == 1 || card.number == 2 || card.pattern == Card.Pattern.Joker)
                    {
                        return true;
                    }
                    return false;
                }
            }
            //패널티를 다 받아서 공격카드를 내지 않아도 될 때
            else
            {
                //조커가 맨 앞에 있으면 아무 카드나 낼 수 있음
                if (currentPattern == Card.Pattern.Joker)
                {
                    return true;
                }

                //문양 또는 숫자가 같을 경우 true
                if (card.pattern == currentPattern || card.number == currentNumber
                    || card.pattern == Card.Pattern.Joker)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
