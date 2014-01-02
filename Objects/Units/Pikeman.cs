using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects.Units
{
    public class Pikeman : Unit
    {

        public Pikeman(Game game, Player owner, float z)
            : base(game, owner, z)
        {
            m_name = "Pikeman";
            
            m_maxHP = 60;
            m_baseAttackDamage = 25;
            m_dexterity = 18;
            m_defense = 18;

            m_maxMovementDistance = 5;
            m_range = 1;
            m_inventorySize = 4;

            m_weaponType = WeaponType.spear;

            InitializeUnit();
        }
        public override void CreateDefaultTexture()
        {
            base.CreateDefaultTexture();
            m_defaultTexture = m_Game.Content.Load<Texture2D>("Models/PikemanIcon");
            m_grayTexture = m_Game.Content.Load<Texture2D>("Models/Greys/Pikeman_Gray");
            m_blockTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Block/pike_block");
            m_hitTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Hit/pike_hit");

        }
        public override void CreateWalkAnim()
        {
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_walk_01"));
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_walk_02"));
        }
        public override void CreateIdleAnim() { }
        public override void CreateAttackAnim()
        {
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_01"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_02"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_03"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_04"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_04"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_04"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_04"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_03"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_02"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Pikeman/pike_atk_01"));
        }
    }
}
