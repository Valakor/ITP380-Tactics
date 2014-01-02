using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace itp380.Objects
{
    public abstract class Item
    {
        protected string m_name = "";
        protected int m_remainingUses = 1;
        
        public enum ItemType { none, weapon, armor, potion };
        protected ItemType m_type = ItemType.none;
        
        public Item()
        {
            
        }

        public string name
        {
            get { return m_name; }
        }

        public ItemType type
        {
            get { return m_type; }
        }
        
        public int remainingUses
        {
            get { return m_remainingUses; }
        }

        public virtual void use()
        {
            m_remainingUses--;
        }
    }
}