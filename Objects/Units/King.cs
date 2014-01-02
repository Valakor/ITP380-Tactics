﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects.Units
{
    public class King : Unit
    {
        public King(Game game, Player owner, float z)
            : base(game, owner, z)
        {
            m_name = "King";

            m_maxHP = 35;
            m_baseAttackDamage = 10;
            m_dexterity = 10;
            m_defense = 15;

            m_maxMovementDistance = 2;
            m_range = 1;
            m_inventorySize = 4;

            m_weaponType = WeaponType.none;

            m_isKing = true;

            InitializeUnit();
        }

        public override void CreateDefaultTexture()
        {
            base.CreateDefaultTexture();
            m_defaultTexture = m_Game.Content.Load<Texture2D>("Models/King_Icon");
            m_grayTexture = m_Game.Content.Load<Texture2D>("Models/Greys/King_Icon-grey");
        }

        public override void CreateWalkAnim()
        {
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("Models/King_Icon"));
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_walk_2"));
            m_blockTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Block/king_block");
            m_hitTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Hit/king_hit");
        }

        public override void CreateIdleAnim() { }
        
        public override void CreateAttackAnim()
        {
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_walk_2"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_atk_2"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_atk_3"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_atk_3"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_atk_3"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_atk_3"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_atk_2"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_walk_2"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_walk_2"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/King/King_walk_2"));
        }
    }
}
