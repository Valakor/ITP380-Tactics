using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace itp380.Objects.Items
{
    public class Weapon : Item
    {
        protected int m_bonusDamage = 0;

        public Weapon()
            : base()
        {
            m_name = "";

            m_remainingUses = 1;
            m_type = ItemType.weapon;
        }

        public int bonusDamage
        {
            get { return m_bonusDamage; }
        }
    }
}