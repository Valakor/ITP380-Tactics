using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace itp380.Objects.Items
{
    public class SteelSword : Weapon
    {  
        public SteelSword() 
            : base()
        {
            m_name = "Steel Sword";

            m_bonusDamage = 5;
            m_remainingUses = 5;
            m_type = ItemType.weapon;
        }
    }
}