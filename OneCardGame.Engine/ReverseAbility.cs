using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Engine
{
    public class ReverseAbility : AbilityBehavior
    {
        public void SpecialAbility(Dealer dealer)
        {
            Dealer.Direction direction = dealer.GetCurrentDirection();
            if (direction == Dealer.Direction.Left)
            {
                dealer.SetCurrentDirection(Dealer.Direction.Right);
            }
            else
            {
                dealer.SetCurrentDirection(Dealer.Direction.Left);
            }
        }
    }
}
