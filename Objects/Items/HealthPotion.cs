using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace itp380.Objects.Items
{
    public class HealthPotion : Item
    {
        private int m_hpRestored = 15;

        public HealthPotion()
            : base()
        {
            m_name = "Health Potion";

            m_remainingUses = 1;
            m_type = ItemType.potion;
        }

        public int hpRestored
        {
            get { return m_hpRestored; }
        }
    }
}