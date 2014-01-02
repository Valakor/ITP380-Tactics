using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Chest : GameObject
    {
        private Item contents = null;
        
        public Chest(Game game)
            : base(game)
        {
            m_ModelName = "Models/Chest/chest_model";
            Scale = 0.1f;
        }

        public void addItem(Item item)
        {
            this.contents = item;
        }

        public Item getContents()
        {
            return contents;
        }
    }
}
