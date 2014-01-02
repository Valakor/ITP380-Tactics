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
	public class UIPickAndPlaceUnits : UIScreen
	{
        private const float UNITSPAWNHEIGHT = 0.15f;
        
        SpriteFont m_FixedFont;
		SpriteFont m_FixedSmall;
		SpriteFont m_StatusFont;

        Texture2D PickAndPlaceUI;
        Vector2 screenCenter = new Vector2();
        Point botMenuLeft = new Point();


        Texture2D swordsmanIcon;
            Texture2D swordsmanIcon_2;
        Texture2D archerIcon;
            Texture2D archerIcon_2;
        Texture2D pikemanIcon;
            Texture2D pikemanIcon_2;
        Texture2D berserkerIcon;
            Texture2D berserkerIcon_2;
        Texture2D clericIcon;
            Texture2D clericIcon_2;
        Texture2D kingIcon;
            Texture2D kingIcon_2;
        

        Objects.Tile curTile = null;
        Objects.Tile prevTile = null;

        // Menu Gui
        Texture2D menuGui;
        Texture2D closedGui;

        // This data is used to show the stats of a unit on screen.
        Objects.Unit showUnit = null;

        //Buttons
        Button nextButton;

        //Animation Lists
        int nextIconIndex = 0;
        List<Texture2D> nextIcon = new List<Texture2D>();

        //Animation for Next Player
        int nextPlayerIndex = -1;
        int showID = 0;
        List<Texture2D> nextPlayer = new List<Texture2D>();
        List<Texture2D> nextPlayer2 = new List<Texture2D>();

        String instructions = "\nInstructions: \n" +
            "1. Click a purple tile and then \n click a unit's icon to place\n the unit. \n\n" +
            "2. Click on a unit's icon in the \n menu below to see its stats. \n\n" +
            "3. You can replace already\n placed units. \n\n" +
            "4. The top right shows your \n maximum number of placeable units. \n\n" +
            "5. You must have a King to \n continue!";

        public UIPickAndPlaceUnits(ContentManager Content) :
			base(Content)
		{
            //Center of the screen
            screenCenter.X = (int)(GraphicsManager.Get().Width / 2.0f);
            screenCenter.Y = (int)(GraphicsManager.Get().Height / 2.0f);

            //Bottom menu. The very left of that menu.
            botMenuLeft.X = (int)(GraphicsManager.Get().Width * 75 / 1024.0f);
            botMenuLeft.Y = (int)(GraphicsManager.Get().Height * 670 / 768.0f);

			m_FixedFont = Content.Load<SpriteFont>("Fonts/FixedText");
			m_FixedSmall = Content.Load<SpriteFont>("Fonts/FixedSmall");
			m_StatusFont = Content.Load<SpriteFont>("Fonts/FixedTitle");

            //Entire UI for game
            PickAndPlaceUI = Content.Load<Texture2D>("UI/PickAndPlaceUI");

            // Info UI
            menuGui = Content.Load<Texture2D>("UI/menu/Gui-test");
            closedGui = Content.Load<Texture2D>("UI/menu/Gui-closed");

            //Load Icons
            swordsmanIcon = Content.Load<Texture2D>("Models/SwordsmanIcon");
            berserkerIcon = Content.Load<Texture2D>("Models/BerserkerIcon");
            archerIcon = Content.Load<Texture2D>("Models/ArcherIcon");
            clericIcon = Content.Load<Texture2D>("Models/ClericIcon");
            pikemanIcon = Content.Load<Texture2D>("Models/PikemanIcon");
            kingIcon = Content.Load<Texture2D>("Models/King_Icon");

            swordsmanIcon_2 = Content.Load<Texture2D>("Models/SwordsmanIcon_2");
            berserkerIcon_2 = Content.Load<Texture2D>("Models/BerserkerIcon_2");
            archerIcon_2 = Content.Load<Texture2D>("Models/ArcherIcon_2");
            clericIcon_2 = Content.Load<Texture2D>("Models/ClericIcon_2");
            pikemanIcon_2 = Content.Load<Texture2D>("Models/PikemanIcon_2");


            kingIcon_2 = Content.Load<Texture2D>("Models/King_Icon-2");


                

            //Load Next Button
            nextIcon.Add(Content.Load<Texture2D>("UI/nextButton/nextButton"));
            nextIcon.Add(Content.Load<Texture2D>("UI/nextButton/nextButton-2"));
            nextIcon.Add(Content.Load<Texture2D>("UI/nextButton/nextButton-3"));
            nextIcon.Add(nextIcon[1]);

            loadNextPlayerStuff(Content);

            //Buttons -- SUPER HACKY
            m_Buttons.AddLast(new Button(botMenuLeft, swordsmanIcon, swordsmanIcon_2, SpawnSwordsMan));
            botMenuLeft.X += 100;
            m_Buttons.AddLast(new Button(botMenuLeft, archerIcon, archerIcon_2, SpawnArcher));
            botMenuLeft.X += 100;
            m_Buttons.AddLast(new Button(botMenuLeft, pikemanIcon, pikemanIcon_2, SpawnPikeMan));
            botMenuLeft.X += 100;
            m_Buttons.AddLast(new Button(botMenuLeft, berserkerIcon, berserkerIcon_2, SpawnBerserker));
            botMenuLeft.X += 100;
            m_Buttons.AddLast(new Button(botMenuLeft, clericIcon, clericIcon_2, SpawnCleric));
            botMenuLeft.X += 100;
            m_Buttons.AddLast(new Button(botMenuLeft, kingIcon, kingIcon_2, SpawnKing));

            //Initialize buttons
            botMenuLeft.X = 900;
            nextButton = new Button(botMenuLeft, nextIcon[0], nextIcon[2], NextPlayer);
            
            //Next Icon  
            m_Buttons.AddLast(nextButton);

            //Show player 1's turn
            showNextPlayerSign(1) ;
            ShowPlaceableTiles();

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

        #region Spawns

        public void SpawnSwordsMan()
        {
            if (curTile != null)
            {
                //If you are trying to replace a unit that is not your own. Then return;

                if (curTile.Owner == GameState.Get().activePlayer)
                {
                    if (curTile.Occupant != null)
                    {
                        if (curTile.Occupant.getOwner() != GameState.Get().activePlayer) return;
                        else
                        {
                            GameState.Get().RemoveUnitFromPlayer(curTile.Occupant);
                        }
                    }

                    GameState.Get().AddUnitToPlayer(typeof(Objects.Units.Swordsman), UNITSPAWNHEIGHT, curTile);
                }
            }
            else showInfo("Swordsman");

            LastUnit();
        }

        public void SpawnArcher()
        {
            if (curTile != null)
            {
                //If you are trying to replace a unit that is not your own. Then return;

                if (curTile.Owner == GameState.Get().activePlayer)
                {
                    if (curTile.Occupant != null)
                    {
                        if (curTile.Occupant.getOwner() != GameState.Get().activePlayer) return;
                        else
                        {
                            GameState.Get().RemoveUnitFromPlayer(curTile.Occupant);
                        }
                    }

                    GameState.Get().AddUnitToPlayer(typeof(Objects.Units.Archer), UNITSPAWNHEIGHT, curTile);
                }
            }
            else showInfo("Archer");

            LastUnit();
        }

        public void SpawnKing()
        {
            if (curTile != null)
            {
                //If you are trying to replace a unit that is not your own. Then return;

                if (curTile.Owner == GameState.Get().activePlayer)
                {
                    if (curTile.Occupant != null)
                    {
                        if (curTile.Occupant.getOwner() != GameState.Get().activePlayer) return;
                        else
                        {
                            GameState.Get().RemoveUnitFromPlayer(curTile.Occupant);
                        }
                    }

                    if (!GameState.Get().activePlayer.hasKing())
                    {
                        GameState.Get().AddUnitToPlayer(typeof(Objects.Units.King), UNITSPAWNHEIGHT, curTile);
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("UIPickAndPlaceUnits::SpawnKing(): Only one King allowed.");
                    }
                }
            }
            else showInfo("King");

            LastUnit();
        }

        public void SpawnPikeMan()
        {
            if (curTile != null)
            {
                //If you are trying to replace a unit that is not your own. Then return;

                if (curTile.Owner == GameState.Get().activePlayer)
                {
                    if (curTile.Occupant != null)
                    {
                        if (curTile.Occupant.getOwner() != GameState.Get().activePlayer) return;
                        else
                        {
                            GameState.Get().RemoveUnitFromPlayer(curTile.Occupant);
                        }
                    }

                    GameState.Get().AddUnitToPlayer(typeof(Objects.Units.Pikeman), UNITSPAWNHEIGHT, curTile);
                }
            }
            else showInfo("Pikeman");

            LastUnit();
        }

        public void SpawnBerserker()
        {
            if (curTile != null)
            {
                //If you are trying to replace a unit that is not your own. Then return;

                if (curTile.Owner == GameState.Get().activePlayer)
                {
                    if (curTile.Occupant != null)
                    {
                        if (curTile.Occupant.getOwner() != GameState.Get().activePlayer) return;
                        else
                        {
                            GameState.Get().RemoveUnitFromPlayer(curTile.Occupant);
                        }
                    }

                    GameState.Get().AddUnitToPlayer(typeof(Objects.Units.Berserker), UNITSPAWNHEIGHT, curTile);
                }
            }
            else showInfo("Berserker");

            LastUnit();
        }

        public void SpawnCleric()
        {
            if (curTile != null)
            {
                //If you are trying to replace a unit that is not your own. Then return;

                if (curTile.Owner == GameState.Get().activePlayer)
                {
                    if (curTile.Occupant != null)
                    {
                        if (curTile.Occupant.getOwner() != GameState.Get().activePlayer) return;
                        else
                        {
                            GameState.Get().RemoveUnitFromPlayer(curTile.Occupant);
                        }
                    }

                    GameState.Get().AddUnitToPlayer(typeof(Objects.Units.Cleric), UNITSPAWNHEIGHT, curTile);
                }
            }
            else showInfo("Cleric");

            LastUnit();
        }

        public void LastUnit()
        {
            if (GameState.Get().activePlayer.maxUnits - GameState.Get().activePlayer.units.Count == 0 && GameState.Get().activePlayer.hasKing())
            {
                m_Timer.AddTimer("nextButton", 0.2f, loopNextButton, true);
            }
        }

        public void NextPlayer()
        {
            if (GameState.Get().activePlayer.hasKing())
            {
                m_Timer.RemoveTimer("nextButton");
                if (GameState.Get().activePlayer.maxUnits == GameState.Get().activePlayer.units.Count)
                {
                    showNextPlayerSign(GameState.Get().activePlayer.playerID + 1);
                    GameState.Get().UpdateActivePlayer();
                    ShowPlaceableTiles();
                }
                nextIconIndex = 0;
                nextButton.m_TexDefault = nextIcon[0];
            }
        }

        public void ShowPlaceableTiles()
        {
            foreach (Objects.Tile t in GameState.Get().board.tiles)
            {
                if (t == null) continue;
                
                if (t.Owner != null && t.Owner == GameState.Get().activePlayer)
                {
                    t.changeState(Objects.Tile.TileState.placeable);
                }
                else
                {
                    t.changeState(Objects.Tile.TileState.none);
                }
            }
        }

        #endregion

        #region Animation_Timers

        private void loopNextButton()
        {
            nextIconIndex ++;
            if (nextIconIndex > 3) nextIconIndex = 0;
            nextButton.m_TexDefault = nextIcon[nextIconIndex];
        }

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

        #endregion



        public void showInfo(String s)
        {
            
            
            //show the info stats of the selected unit.
            switch (s)
            {
                case "Swordsman": showUnit = new Objects.Units.Swordsman(null, null, 0); break;
                case "Archer": showUnit = new Objects.Units.Archer(null, null, 0); break;
                case "Berserker": showUnit = new Objects.Units.Berserker(null, null, 0); break;
                case "Pikeman": showUnit = new Objects.Units.Pikeman(null, null, 0); break;
                case "Cleric": showUnit = new Objects.Units.Cleric(null, null, 0); break;
                case "King": showUnit = new Objects.Units.King(null, null, 0); break;
                default: break;
            }
        }

        public void showNextPlayerSign(int id)
        {
            nextPlayerIndex = 0;
            m_Timer.AddTimer("next player sign", 0.1f, nextPlayerSign, true);
            showID = id;
        }

        public override bool MouseClick(Point Position)
        {
            bool bReturn = base.MouseClick(Position);

            //Stop displaying unit text if player clicks out of the screen.
            if (!bReturn) showUnit = null;
            
            HighlightSelectedTile(Position);

            return bReturn || (curTile != null);
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

            DrawBatch.Draw(PickAndPlaceUI, Vector2.Zero, Color.White);

            DrawBatch.DrawString(m_FixedSmall, "Player: " + GameState.Get().activePlayer.playerID.ToString(), new Vector2(700, 10), Color.White);
            DrawBatch.DrawString(m_FixedSmall, "Available Units: " + (GameState.Get().activePlayer.maxUnits - GameState.Get().activePlayer.units.Count).ToString(), new Vector2(700, 30), Color.White);

            if (showUnit != null)
            {
                DrawBatch.Draw(menuGui, new Vector2(660, 30), Color.White);

                //Draw the stats of the selected unit.
                DrawBatch.DrawString(m_FixedSmall, "Type: ", new Vector2(700, 70), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "HP: ", new Vector2(700, 90), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Attack: ", new Vector2(700, 110), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Dexterity: ", new Vector2(700, 130), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Defense: ", new Vector2(700, 150), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Movement: ", new Vector2(700, 170), Color.White);
                DrawBatch.DrawString(m_FixedSmall, "Range: ", new Vector2(700, 190), Color.White);


                DrawBatch.DrawString(m_FixedSmall, showUnit.getName(), new Vector2(860, 70), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.HP.ToString(), new Vector2(860, 90), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.baseAttackDamage.ToString(), new Vector2(860, 110), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.dexterity.ToString(), new Vector2(860, 130), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.defense.ToString(), new Vector2(860, 150), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.moveDistance.ToString(), new Vector2(860, 170), Color.White);
                DrawBatch.DrawString(m_FixedSmall, showUnit.range.ToString(), new Vector2(860, 190), Color.White);
            }
            else
            {
                DrawBatch.Draw(closedGui, new Vector2(660, 30), Color.White);
            }

            DrawBatch.DrawString(m_FixedSmall, instructions, new Vector2(620, 210), Color.White);

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
