using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects.Units
{
    public class Berserker : Unit
    {
        public Berserker(Game game, Player owner, float z)
            : base(game, owner, z)
        {
            m_name = "Berserker";
            m_maxHP = 75;
            m_baseAttackDamage = 30;
            m_dexterity = 10;
            m_defense = 18;

            m_maxMovementDistance = 4;
            m_range = 1;
            m_inventorySize = 4;

            m_weaponType = WeaponType.axe;

            InitializeUnit();
        }
        public override void CreateDefaultTexture()
        {
            base.CreateDefaultTexture();
            m_defaultTexture = m_Game.Content.Load<Texture2D>("Models/BerserkerIcon");
            m_grayTexture = m_Game.Content.Load<Texture2D>("Models/Greys/Berserker_Gray");
            m_blockTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Block/berserker_block");

        }
        public override void CreateWalkAnim() {
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Berserker/berserker"));
            m_walkAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Berserker/berserker_walk_2"));
            m_hitTexture = m_Game.Content.Load<Texture2D>("UnitAnims/Hit/berserker_hit");
        }
        public override void CreateIdleAnim() { }
        public override void CreateAttackAnim()
        {
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Berserker/berserker_atk_2"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Berserker/berserker_atk_3"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Berserker/berserker_atk_4"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Berserker/berserker_atk_5"));
            m_attackAnim.Add(m_Game.Content.Load<Texture2D>("UnitAnims/Berserker/berserker_atk_6"));
        }
    }
}
