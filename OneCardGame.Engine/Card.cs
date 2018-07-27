using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Engine
{
    public class Card
    {
        public enum Pattern { None = -1, Spade, Clover, Diamond, Hart, Joker};

        public int number;
        public Pattern pattern = Pattern.None;
        private AbilityBehavior abilityBehavior;


        public Card(Pattern pattern, int cardNum)
        {
            this.pattern = pattern;
            number = cardNum;
            SetSpecialAbility();
        }

        public void SetSpecialAbility()
        {
            switch (number)
            {
                case 1:
                    abilityBehavior = new OneAttackAbility();
                    break;

                case 2:
                    abilityBehavior = new TwoAttackAbility();
                    break;

                case 7:
                    abilityBehavior = new ChangePatternAbility();
                    break;

                case 11:
                    abilityBehavior = new OneMoreTimeAbility();
                    break;

                case 12:
                    abilityBehavior = new ReverseAbility();
                    break;

                case 13:
                    abilityBehavior = new JumpAbility();
                    break;

                default:
                    abilityBehavior = new NoAbility();
                    break;
            }

            if (pattern == Pattern.Joker)
            {
                abilityBehavior = new JokerAttackAbility();
            }
        }

        public void SpecialAbility(Dealer dealer)
        {
            abilityBehavior.SpecialAbility(dealer);
        }

        public String GetAbilityName()
        {
            return abilityBehavior.ToString();
        }

        public String ToText()
        {
            String text = "";

            switch (pattern)
            {
                case Pattern.Spade:
                    text = "♠";
                    break;
                case Pattern.Clover:
                    text = "♣";
                    break;
                case Pattern.Diamond:
                    text = "◆";
                    break;
                case Pattern.Hart:
                    text = "♥";
                    break;
                case Pattern.Joker:
                    text = "★Joker★";
                    break;
            }
            if(pattern != Pattern.Joker)
            {
                switch (number)
                {
                    case 1:
                        text += "A";
                        break;

                    case 11:
                        text += "K";
                        break;

                    case 12:
                        text += "Q";
                        break;

                    case 13:
                        text += "J";
                        break;

                    default:
                        text += number.ToString();
                        break;
                }
            }
            
            return text;
        }
    }
}
