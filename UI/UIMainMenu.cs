//-----------------------------------------------------------------------------
// UIMainMenu is the main menu UI.
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
	public class UIMainMenu : UIScreen
	{
		SpriteFont m_TitleFont;
		SpriteFont m_ButtonFont;
		string m_Title;

        Texture2D[] menuBG;
        float menuBGIndex = 0;

        Texture2D title;
        Texture2D newGameIcon;
        Texture2D exitIcon;
        Texture2D newGameIcon_2;
        Texture2D exitIcon_2;
        Texture2D levelEditorIcon;
        Texture2D levelEditorIcon2;

		public UIMainMenu(ContentManager Content) :
			base(Content)
		{
			m_TitleFont = m_Content.Load<SpriteFont>("Fonts/QuartzTitle");
			m_ButtonFont = m_Content.Load<SpriteFont>("Fonts/QuartzButton");


            //Create background
            menuBG = new Texture2D[8];
            menuBG[0] = Content.Load<Texture2D>("UI/menu/menu-1");
            menuBG[1] = Content.Load<Texture2D>("UI/menu/menu-2");
            menuBG[2] = Content.Load<Texture2D>("UI/menu/menu-3");
            menuBG[3] = Content.Load<Texture2D>("UI/menu/menu-4");
            menuBG[4] = Content.Load<Texture2D>("UI/menu/menu-5");
            menuBG[5] = Content.Load<Texture2D>("UI/menu/menu-6");
            menuBG[6] = Content.Load<Texture2D>("UI/menu/menu-7");
            menuBG[7] = Content.Load<Texture2D>("UI/menu/menu-8");

            //Title
            title = Content.Load<Texture2D>("UI/MenuButtons/tactics");
            newGameIcon = Content.Load<Texture2D>("UI/MenuButtons/newgame");
            exitIcon = Content.Load<Texture2D>("UI/MenuButtons/exit");
            newGameIcon_2 = Content.Load<Texture2D>("UI/MenuButtons/newgame_2");
            exitIcon_2 = Content.Load<Texture2D>("UI/MenuButtons/exit_2");
            levelEditorIcon = Content.Load<Texture2D>("UI/MenuButtons/leveleditor");
            levelEditorIcon2 = Content.Load<Texture2D>("UI/MenuButtons/leveleditor_2");


			// Create buttons
			Point vPos = new Point();
			vPos.X = (int) (GraphicsManager.Get().Width / 2.0f);
			vPos.Y = (int)(GraphicsManager.Get().Height / 2.0f);

			m_Title = "Tactics";


            vPos.Y += 125;
            vPos.X += 220;
			m_Buttons.AddLast(new Button(vPos, newGameIcon, newGameIcon_2, NewGame));

			vPos.Y += 50;
			m_Buttons.AddLast(new Button(vPos, exitIcon, exitIcon_2, Exit));

            vPos.Y += 50;
            m_Buttons.AddLast(new Button(vPos, levelEditorIcon, levelEditorIcon2, LaunchEditor));

		}

		public void NewGame()
		{
			GameState.Get().SetState(eGameState.GameplaySetup);
		}

        public void loadLevel(String f)
        {
            GameState.Get().LoadLevel(f);
        }

		public void Options()
		{
		}

		public void Exit()
		{
			GameState.Get().Exit();
		}

        public void LaunchEditor()
        {
            LevelEditor editor = new LevelEditor(this);
            editor.Show();
        }

		public override void Update(float fDeltaTime)
		{
			base.Update(fDeltaTime);
		}

		public override void Draw(float fDeltaTime, SpriteBatch DrawBatch)
		{

            DrawBatch.Draw(menuBG[(int)(menuBGIndex%8)], Vector2.Zero, Color.White);
            menuBGIndex+= 9*fDeltaTime;

            

			Vector2 vOffset = Vector2.Zero;
            vOffset.Y = 375;
            vOffset.X = 620;
            DrawBatch.Draw(title, vOffset, Color.White);
			//DrawCenteredString(DrawBatch, m_Title, m_TitleFont, Color.DarkBlue, vOffset);

			base.Draw(fDeltaTime, DrawBatch);
		}
	}
}
