using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects.Units
{
    public class Archer : Unit
    {
        public Archer(Game game, Player owner, float z)
            : base(game, owner, z)
        {
            m_name = "Archer";

            m_maxHP = 40;
            m_baseAttackDamage = 20;
            m_dexterity = 20;
            m_defense = 15;

            m_maxMovementDistance = 4;
            m_range = 2;
            m_inventorySize = 4;

            m_weaponType = WeaponType.bow;

            m_attackSound = "Arrow";

            InitializeUnit();
        }
        public override void CreateDefaultTexture()
        {
            base.CreateDefaultTexture();
            m_defaultTexture = m_Game.Content.Load<Texture2D>("Models/ArcherIcon");
            m_grayTexture = m_Game.Content.Load<Texture2D>("Models/Greys/Archer_Gray");
            m_blockTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Block/archer_block");
            m_hitTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Hit/archer_hit");
        }

        public override void CreateWalkAnim() {
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Archer/archer_walk_01"));
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Archer/archer_walk_02"));
        }
        public override void CreateIdleAnim() { }
        public override void CreateAttackAnim()
        {
           
          
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Archer/archer_atk_14"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Archer/archer_atk_15"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Archer/archer_atk_16"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Archer/archer_atk_17"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Archer/archer_atk_18"));
        }

    }
}
