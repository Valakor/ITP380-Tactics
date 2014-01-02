using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace itp380.Objects.Items
{
    public class Armor : Item
    {
        protected int m_bonusArmor = 0;

        public Armor()
            : base()
        {
            m_name = "";

            m_remainingUses = 1;
            m_type = ItemType.armor;
        }

        public int bonusArmor
        {
            get { return m_bonusArmor; }
        }
    }
}