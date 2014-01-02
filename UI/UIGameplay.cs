//-----------------------------------------------------------------------------
// UIGameplay is UI while in the main game state.
// Because there are so many aspects to the UI, this class is relatively large.
//
// __Defense Sample for Game Programming Algorithms and Techniques
// Copyright (C) Sanjay Madhav. All rights reserved.
//
// Released under the Microsoft Permissive License.
// See LICENSE.txt for full details.
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace itp380.UI
{
	public class UIGameplay : UIScreen
	{
		SpriteFont m_FixedFont;
		SpriteFont m_FixedSmall;
		SpriteFont m_StatusFont;

        Objects.Tile prevTile = null;
        Objects.Tile curTile = null;

        Objects.Unit showUnit = null;

        bool menuOn = false;
        bool itemMenuOn = false;

        Point moveMenuPos = new Point(700, 300);

        //Animation for Next Player
        int nextPlayerIndex = -1;
        int showID = 0;
        List<Texture2D> nextPlayer = new List<Texture2D>();
        List<Texture2D> nextPlayer2 = new List<Texture2D>();

        // Menu Gui stuff
        Texture2D menuGui;
        Texture2D closedGui;
        Texture2D _menu;
        Texture2D _move;
        Texture2D _move_sel;
        Texture2D _attack;
        Texture2D _attack_sel;
        Texture2D _item;
        Texture2D _item_sel;
        Texture2D _wait;
        Texture2D _wait_sel;
        Texture2D _menu_inventory;
        Texture2D _back;
        Texture2D _back_sel;

		public UIGameplay(ContentManager Content) :
			base(Content)
		{
			m_FixedFont = Content.Load<SpriteFont>("Fonts/FixedText");
			m_FixedSmall = Content.Load<SpriteFont>("Fonts/FixedSmall");
			m_StatusFont = Content.Load<SpriteFont>("Fonts/FixedTitle");

            loadNextPlayerStuff(Content);

            // Info UI
            menuGui = Content.Load<Texture2D>("UI/menu/Gui-test");
            closedGui = Content.Load<Texture2D>("UI/menu/Gui-closed");

            _menu = Content.Load<Texture2D>("UI/menu/menu-menu");
            _move = Content.Load<Texture2D>("UI/menu/menu-move");
            _move_sel = Content.Load<Texture2D>("UI/menu/menu-move-sel");
            _attack = Content.Load<Texture2D>("UI/menu/menu-attack");
            _attack_sel = Content.Load<Texture2D>("UI/menu/menu-attack-sel");
            _item = Content.Load<Texture2D>("UI/menu/menu-item");
            _item_sel = Content.Load<Texture2D>("UI/menu/menu-item-sel");
            _wait = Content.Load<Texture2D>("UI/menu/menu-wait");
            _wait_sel = Content.Load<Texture2D>("UI/menu/menu-wait-sel");
            _menu_inventory = Content.Load<Texture2D>("UI/menu/menu-inventory");
            _back = Content.Load<Texture2D>("UI/menu/menu-back");
            _back_sel = Content.Load<Texture2D>("UI/menu/menu-back-sel");

            showNextPlayerSign(GameState.Get().activePlayer.playerID);
		}
        private void loadNextPlayerStuff(ContentManager Content)
        {
            //Load Next Player 
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-1"));
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2"));
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-3"));
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-4"));
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-5"));
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-6"));
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-7"));
            nextPlayer.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-8"));
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[7]);
            nextPlayer.Add(nextPlayer[6]);
            nextPlayer.Add(nextPlayer[5]);
            nextPlayer.Add(nextPlayer[4]);
            nextPlayer.Add(nextPlayer[3]);
            nextPlayer.Add(nextPlayer[2]);
            nextPlayer.Add(nextPlayer[1]);
            nextPlayer.Add(nextPlayer[0]);

            //Next player 2
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-1"));
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2-2"));
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2-3"));
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2-4"));
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2-5"));
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2-6"));
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2-7"));
            nextPlayer2.Add(Content.Load<Texture2D>("UI/nextPlayer/pct-2-8"));
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[7]);
            nextPlayer2.Add(nextPlayer2[6]);
            nextPlayer2.Add(nextPlayer2[5]);
            nextPlayer2.Add(nextPlayer2[4]);
            nextPlayer2.Add(nextPlayer2[3]);
            nextPlayer2.Add(nextPlayer2[2]);
            nextPlayer2.Add(nextPlayer2[1]);
            nextPlayer2.Add(nextPlayer2[0]);
        }

        public void NextPlayer(int id)
        {
            showNextPlayerSign(id);
        }

        //Timers
        private void nextPlayerSign()
        {
            if (nextPlayerIndex < nextPlayer.Count)
                nextPlayerIndex++;
            if (nextPlayerIndex >= nextPlayer.Count)
            {
                nextPlayerIndex = -1;
                m_Timer.RemoveTimer("next player sign");
            }
        }

        public void showNextPlayerSign(int id)
        {
            nextPlayerIndex = 0;
            m_Timer.AddTimer("next player sign", 0.1f, nextPlayerSign, true);
            showID = id;
        }

        public void menuAppear()
        {
            menuOn = true;
            moveMenuPos = new Point(750, 250);
            //spawn buttons
            m_Buttons.AddLast(new Button(moveMenuPos, _menu, _menu, null));
            moveMenuPos.Y += 50;
            //m_Buttons.AddLast(new Button(moveMenuPos, "Move", m_FixedFont, Color.White, Color.Yellow, move, eButtonAlign.Left));
            m_Buttons.AddLast(new Button(moveMenuPos, _move, _move_sel, move));
            moveMenuPos.Y += 50;
            //m_Buttons.AddLast(new Button(moveMenuPos, "Attack", m_FixedFont, Color.White, Color.Yellow, attack, eButtonAlign.Left));
            m_Buttons.AddLast(new Button(moveMenuPos, _attack, _attack_sel, attack));
            moveMenuPos.Y += 50;
            //m_Buttons.AddLast(new Button(moveMenuPos, "Item", m_FixedFont, Color.White, Color.Yellow, item, eButtonAlign.Left));
            m_Buttons.AddLast(new Button(moveMenuPos, _item, _item_sel, item));
            moveMenuPos.Y += 50;
            //m_Buttons.AddLast(new Button(moveMenuPos, "Wait", m_FixedFont, Color.White, Color.Yellow, wait, eButtonAlign.Left));
            m_Buttons.AddLast(new Button(moveMenuPos, _wait, _wait_sel, wait));
        }

        public void itemMenuAppear()
        {
            if (itemMenuOn == true) return;
            removeMenu();
            itemMenuOn = true; 
            moveMenuPos = new Point(750, 250);
            //max number of items is 5

            m_Buttons.AddLast(new Button(moveMenuPos, _menu_inventory, _menu_inventory, null));
            moveMenuPos.Y += 50;

            //1
            if (showUnit.getInventory().Count > 0)
            {
                String i1 = showUnit.getInventory()[0].name + " ["+showUnit.getInventory()[0].remainingUses+"] ";
                if (showUnit.getInventory()[0] == showUnit.equippedWeapon || showUnit.getInventory()[0] == showUnit.equippedArmor)
                {
                    i1 = "[Eqp] " + i1;
                }

                m_Buttons.AddLast(new Button(moveMenuPos, i1, m_FixedFont, Color.White, Color.Yellow, item1, eButtonAlign.Left));
                moveMenuPos.Y += 50;
            }
            //2
            if (showUnit.getInventory().Count > 1)
            {
                String i2 = showUnit.getInventory()[1].name+" ["+showUnit.getInventory()[1].remainingUses+"] ";
                if (showUnit.getInventory()[1] == showUnit.equippedWeapon || showUnit.getInventory()[1] == showUnit.equippedArmor)
                {
                    i2 = "[Eqp] " + i2;
                }
                    m_Buttons.AddLast(new Button(moveMenuPos, i2, m_FixedFont, Color.White, Color.Yellow, item2, eButtonAlign.Left));
                
                moveMenuPos.Y += 50;
            }
            //3
            if (showUnit.getInventory().Count > 2)
            {

                String i3 = showUnit.getInventory()[2].name+" ["+showUnit.getInventory()[2].remainingUses+"] ";
                if (showUnit.getInventory()[2] == showUnit.equippedWeapon || showUnit.getInventory()[2] == showUnit.equippedArmor)
                {
                    i3 = "[Eqp] " + i3;
                }
                m_Buttons.AddLast(new Button(moveMenuPos, i3, m_FixedFont, Color.White, Color.Yellow, item3, eButtonAlign.Left));
                moveMenuPos.Y += 50;
            }
            //4
            if (showUnit.getInventory().Count > 3)
            {
                String i4 = showUnit.getInventory()[3].name +" ["+showUnit.getInventory()[3].remainingUses+"] ";
                if (showUnit.getInventory()[3] == showUnit.equippedWeapon || showUnit.getInventory()[3] == showUnit.equippedArmor)
                {
                    i4 = "[Eqp] " + i4;
                }
                m_Buttons.AddLast(new Button(moveMenuPos, i4, m_FixedFont, Color.White, Color.Yellow, item4, eButtonAlign.Left));
                moveMenuPos.Y += 50;
            }
            //5
            if (showUnit.getInventory().Count > 4)
            {
                String i5 = showUnit.getInventory()[4].name + " [" + showUnit.getInventory()[4].remainingUses + "] ";
                if (showUnit.getInventory()[4] == showUnit.equippedWeapon || showUnit.getInventory()[4] == showUnit.equippedArmor)
                {
                    i5 = "[Eqp] " + i5;
                }
                m_Buttons.AddLast(new Button(moveMenuPos, i5, m_FixedFont, Color.White, Color.Yellow, item5, eButtonAlign.Left));
                moveMenuPos.Y += 50;
            }

            m_Buttons.AddLast(new Button(moveMenuPos, _back, _back_sel, backItem));

        }

        #region Items
        private void item1()
        {
            GameState.Get().UseItemUnit(showUnit,showUnit.getInventory()[0]);
            useItemMenu();
        }
        private void item2()
        {
            GameState.Get().UseItemUnit(showUnit, showUnit.getInventory()[1]);
            useItemMenu();
        }
        private void item3()
        {
            GameState.Get().UseItemUnit(showUnit, showUnit.getInventory()[2]);
            useItemMenu();
        }
        private void item4()
        {
            GameState.Get().UseItemUnit(showUnit, showUnit.getInventory()[3]);
            useItemMenu();
        }
        private void item5()
        {
            GameState.Get().UseItemUnit(showUnit, showUnit.getInventory()[4]);
            useItemMenu();
        }
        private void backItem()
        {
            removeMenu();
            showMenu();
        }

        private void useItemMenu()
        {
            removeMenu();
            itemMenuAppear();
        }
        #endregion

        public void removeMenu()
        {
            menuOn = false;
            itemMenuOn = false;
            m_Buttons.Clear();
        }


        private void move()
        {
            GameState.Get().MoveUnitShow(showUnit);
        }

        private void item()
        {
            itemMenuAppear();
        }

        private void attack()
        {
            if (showUnit is Objects.Units.Cleric)
            {
                GameState.Get().HealUnitShow(showUnit);
            }
            else
            {
                GameState.Get().AttackUnitShow(showUnit);
            }
        }

        private void wait()
        {
            GameState.Get().WaitUnit(showUnit);
            removeMenu();
        }


        public override bool MouseClick(Point Position)
        {
            bool bReturn = base.MouseClick(Position);

            if (!bReturn)
            {
                showUnit = GameState.Get().HandleGameplayMouseClick(Position);
                showMenu();
                HighlightSelectedTile(Position);
            }

            return bReturn || (curTile != null);
        }

        public void showMenu()
        {
            if (showUnit != null && showUnit == GameState.Get().activePlayer.activeUnit)
            {
                if (!menuOn)
                {
                    menuAppear();
                }
            }

            else removeMenu();
        }

        public void HighlightSelectedTile(Point Position)
        {
            prevTile = curTile;
            curTile = GameState.Get().TileClicked(Position);
            
            if (prevTile != null)
            {
                prevTile.deSelect();
            }
            if (curTile != null)
            {
                curTile.select();
            }
        }

		public override void Update(float fDeltaTime)
		{
			base.Update(fDeltaTime);
		}

        Vector2 nextPlayerDraw = new Vector2(0, 250);
		public override void Draw(float fDeltaTime, SpriteBatch DrawBatch)
		{     
            //Draw selected unit.
            if (showUnit != null)
            {
                DrawBatch.Draw(menuGui, new Vector2(660, 30), Color.White);
                
                //Draw the stats of the selected unit.
                DrawBatch.DrawString(m_FixedSmall, "Owner: ", new Vector2(700, 60), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Type: ", new Vector2(700, 80), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "HP: ", new Vector2(700, 100), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Attack: ", new Vector2(700, 120), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Dexterity: ", new Vector2(700, 140), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Defense: ", new Vector2(700, 160), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Movement: ", new Vector2(700, 180), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Range: ", new Vector2(700, 200), Color.White);

                DrawBatch.DrawString(m_FixedSmall, "Player "+showUnit.getOwner().playerID.ToString(), new Vector2(860, 60), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.getName(), new Vector2(860, 80), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.HP.ToString(), new Vector2(860, 100), Color.White);
                String atkDmg = showUnit.baseAttackDamage.ToString() ;
                if (showUnit.equippedWeapon != null) { atkDmg += " + "+showUnit.equippedWeapon.bonusDamage; }
                DrawBatch.DrawString(m_FixedSmall, atkDmg, new Vector2(860, 120), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.dexterity.ToString(), new Vector2(860, 140), Color.White);
                String defense = showUnit.defense.ToString();
                if (showUnit.equippedArmor != null)
                {
                    defense += " + " + showUnit.equippedArmor.bonusArmor;
                }
                DrawBatch.DrawString(m_FixedSmall, defense, new Vector2(860, 160), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.moveDistance.ToString(), new Vector2(860, 180), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.range.ToString(), new Vector2(860, 200), Color.White);
            }
            else
            {
                DrawBatch.Draw(closedGui, new Vector2(660, 30), Color.White);
            }

            if (showID == 1 && nextPlayerIndex >= 0)
            {
                DrawBatch.Draw(nextPlayer[nextPlayerIndex], nextPlayerDraw, Color.White);
            }
            else if (showID == 2 && nextPlayerIndex >= 0)
            {
                DrawBatch.Draw(nextPlayer2[nextPlayerIndex], nextPlayerDraw, Color.White);
            }


			base.Draw(fDeltaTime, DrawBatch);
		}

		public override void KeyboardInput(SortedList<eBindings, BindInfo> binds)
		{
			GameState g = GameState.Get();
			if (binds.ContainsKey(eBindings.UI_Exit))
			{
				g.ShowPauseMenu();
				binds.Remove(eBindings.UI_Exit);
			}

			// Handle any input before the gameplay screen can look at it

			base.KeyboardInput(binds);
		}
	}
}
