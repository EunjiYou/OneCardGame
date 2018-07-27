using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneCardGame.Engine
{
    public class JokerAttackAbility : AbilityBehavior
    {
        public void SpecialAbility(Dealer dealer)
        {
            dealer.PlusDamage(5);
        }
    }
}
