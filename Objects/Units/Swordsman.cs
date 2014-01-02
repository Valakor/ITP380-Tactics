using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects.Units
{
    public class Swordsman : Unit
    {    
        public Swordsman(Game game, Player owner, float z) 
            : base(game, owner, z)
        {
            m_name = "Swordsman";

            if (owner != null)
            {
                m_defaultTexture = m_Game.Content.Load<Texture2D>("Models/SwordsmanIcon");
                m_selectedTexture = m_Game.Content.Load<Texture2D>("Models/SwordsmanIcon_2");
                m_blockTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Block/s_block");
                m_hitTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Hit/s_hit");
            }

            m_maxHP = 65;
            m_baseAttackDamage = 20;
            m_dexterity = 15;
            m_defense = 20;

            m_maxMovementDistance = 4;
            m_range = 1;
            m_inventorySize = 4;

            m_weaponType = WeaponType.sword;

            InitializeUnit();
            
        }
        public override void CreateDefaultTexture()
        {
            
                m_defaultTexture = m_Game.Content.Load<Texture2D>("Models/SwordsmanIcon");
                m_grayTexture = m_Game.Content.Load<Texture2D>("Models/Greys/Swordsman_Gray");

           
        }
        public override void CreateWalkAnim()
        {

                m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/swordsman_walk_01"));
                m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/swordsman_walk_03"));

           
        }

        public override void CreateAttackAnim()
        {
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_01"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_02"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_03"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_04"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_05"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_06"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_07"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_08"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_09"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Swordsman/s_atk_10"));
        }

        public override void CreateIdleAnim()
        {
            
        }


    }
}
