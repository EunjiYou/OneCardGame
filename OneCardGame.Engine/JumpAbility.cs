using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Engine
{
    public class JumpAbility : AbilityBehavior
    {
        public void SpecialAbility(Dealer dealer)
        {
            dealer.SetNextTurn();
        }
    }
}
