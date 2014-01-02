using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects.Units
{
    public class Cleric : Unit
    {
        public Cleric(Game game, Player owner, float z)
            : base(game, owner, z)
        {
            m_name = "Cleric";
            
            m_maxHP = 45;
            m_baseAttackDamage = 20;
            m_dexterity = 12;
            m_defense = 15;

            m_maxMovementDistance = 3;
            m_range = 3;
            m_inventorySize = 4;

            m_weaponType = WeaponType.staff;

            m_attackSound = "Heal";

            InitializeUnit();
        }
        public override void CreateDefaultTexture()
        {
            base.CreateDefaultTexture();
            m_defaultTexture = m_Game.Content.Load<Texture2D>("Models/ClericIcon");
            m_grayTexture = m_Game.Content.Load<Texture2D>("Models/Greys/Cleric_Gray");
            m_blockTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Block/cleric_block");
            m_hitTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Hit/cleric_dmg");

        }
        public override void CreateWalkAnim()
        {
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Cleric/cleric_walk_1"));
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Cleric/cleric_walk_2"));
        }
        public override void CreateIdleAnim() { }
        public override void CreateAttackAnim()
        {
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Cleric/cleric_atk_1"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Cleric/cleric_atk_2"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Cleric/cleric_atk_3"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Cleric/cleric_atk_4"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Cleric/cleric_atk_5"));
        }
    }
}
