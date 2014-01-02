using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects.Misc
{
    public class Skybox : GameObject
    {
        public Skybox(Game game)
            : base(game)
        {
            Enabled = true;
            
            m_ModelName = "Models/Skybox/skybox_model";
        }

        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);
            
            Position = GameState.Get().Camera.Position;
        }
    }
}
