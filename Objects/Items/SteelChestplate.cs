using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace itp380.Objects.Items
{
    public class SteelChestplate : Armor
    {
        public SteelChestplate()
            : base()
        {
            m_name = "Steel Chestplate";

            m_bonusArmor = 5;
            m_remainingUses = 5;
            m_type = ItemType.armor;
        }
    }
}