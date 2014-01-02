//-----------------------------------------------------------------------------
// The main GameState Singleton. All actions that change the game state,
// as well as any global updates that happen during gameplay occur in here.
// Because of this, the file is relatively lengthy.
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

namespace itp380
{
	public enum eGameState
	{
		None = 0,
		MainMenu,
        GameplaySetup,
        SetupLoadedLevel,
        PickAndPlaceUnits,
		Gameplay
	}

	public class GameState : itp380.Patterns.Singleton<GameState>
	{
		Game m_Game;
		eGameState m_State;
		public eGameState State
		{
			get { return m_State; }
		}

        String xmlFile;
		eGameState m_NextState;
		Stack<UI.UIScreen> m_UIStack;
		bool m_bPaused = false;
		public bool IsPaused
		{
			get { return m_bPaused; }
			set	{ m_bPaused = value; }
		}

		// Keeps track of all active game objects
		LinkedList<GameObject> m_GameObjects = new LinkedList<GameObject>();

		// Camera Information
		Camera m_Camera;
		public Camera Camera
		{
			get { return m_Camera; }
		}

		public Matrix CameraMatrix
		{
			get { return m_Camera.CameraMatrix; }
		}

		// Timer class for the global GameState
		Utils.Timer m_Timer = new Utils.Timer();

		UI.UIGameplay m_UIGameplay;

        // Skybox
        Objects.Misc.Skybox skybox = null;
        // City
        Objects.ArtAsset city = null;

        // Gameplay stuff
        const int MAXPLAYERS = 2;
        const int MAXUNITS = 5;
        const bool RANDOMSEED = false;
        const float UNITSPAWNHEIGHT = 0.15f;
        private Objects.Player m_activePlayer;
        public Objects.Player activePlayer
        {
            get { return m_activePlayer; }
        }
        private List<Objects.Player> m_players = new List<Objects.Player>(MAXPLAYERS);
        public List<Objects.Player> players
        {
            get { return m_players; }
        }
        private List<Objects.Unit> m_availableUnits = new List<Objects.Unit>();
        public List<Objects.Unit> availableUnits
        {
            get { return m_availableUnits; }
        }
        private Objects.Board m_Board;
        public Objects.Board board
        {
            get { return m_Board; }
        }
        private bool isUnitMoving = false;
        public bool unitMoving
        {
            get { return isUnitMoving; }
        }
        private List<Objects.Chest> m_chests = new List<Objects.Chest>();
        public List<Objects.Chest> chests
        {
            get { return m_chests; }
        }
        public Random random = RANDOMSEED ? new Random() : new Random(1);
		
		public void Start(Game game)
		{
			m_Game = game;
			m_State = eGameState.None;
			m_UIStack = new Stack<UI.UIScreen>();

            //Camera Change
			m_Camera = new Camera(m_Game);
		}

        public void LoadLevel(String f)
        {
            xmlFile = f;
            SetState(eGameState.SetupLoadedLevel);
        }

		public void SetState(eGameState NewState)
		{
			m_NextState = NewState;
		}

		private void HandleStateChange()
		{
			if (m_NextState == m_State)
				return;

			switch (m_NextState)
			{
				case eGameState.MainMenu:
                    SoundManager.Get().PlayBackgroundMusic("Menu");
					m_UIStack.Clear();
					m_UIGameplay = null;
					m_Timer.RemoveAll();
					m_UIStack.Push(new UI.UIMainMenu(m_Game.Content));
					ClearGameObjects();
                    System.Diagnostics.Debug.Print("GameState::HandleStateChange(): Transition to MainMenu");
					break;
                case eGameState.GameplaySetup:
                    InitNewGame();
                    System.Diagnostics.Debug.Print("GameState::HandleStateChange(): Transition to GameplaySetup");
                    break;
                case eGameState.SetupLoadedLevel:
                    InitLoadedLevel();
                    System.Diagnostics.Debug.Print("GameState::HandleStateChange(): Transition to LoadLevel");
                    break;
                case eGameState.PickAndPlaceUnits:
                    SoundManager.Get().PlayBackgroundMusic("Gameplay");
                    m_UIStack.Push(new UI.UIPickAndPlaceUnits(m_Game.Content));
                    System.Diagnostics.Debug.Print("GameState::HandleStateChange(): Transition to PickAndPlaceUnits");
                    break;
				case eGameState.Gameplay:
                    PopUI();
                    m_Board.resetALLTiles(false);
                    m_UIGameplay = new UI.UIGameplay(m_Game.Content);
			        m_UIStack.Push(m_UIGameplay);
                    // Any additional Gameplay Initialization here
                    InitGameplay();
                    TargetCamera();
                    System.Diagnostics.Debug.Print("GameState::HandleStateChange(): Transition to Gameplay");
					break;
			}

			m_State = m_NextState;
		}

		protected void ClearGameObjects()
		{
			// Clear out any and all game objects
			foreach (GameObject o in m_GameObjects)
			{
				RemoveGameObject(o, false);
			}
			m_GameObjects.Clear();
		}

        public void InitLoadedLevel()
        {
            ClearGameObjects();
            m_UIStack.Clear();

            skybox = new Objects.Misc.Skybox(m_Game);
            skybox.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)Math.PI / 2);
            skybox.Scale *= 2f;
            SpawnGameObject(skybox);

            m_bPaused = false;
            GraphicsManager.Get().ResetProjection();

            m_Timer.RemoveAll();

            // Init the two players
            m_players.Clear();
            for (int i = 1; i <= MAXPLAYERS; i++)
            {
                m_players.Add(new Objects.Player(i, MAXUNITS));
            }
            m_activePlayer = m_players[0];

            // Init the available in-game units
            m_availableUnits.Clear();
            m_availableUnits.Add(new Objects.Units.Archer(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Berserker(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Cleric(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Pikeman(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Swordsman(m_Game, null, 1));

            //Begin Parsing and creating board
            System.Xml.XmlTextReader reader = new System.Xml.XmlTextReader(xmlFile);
            reader.WhitespaceHandling = System.Xml.WhitespaceHandling.None;

                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case System.Xml.XmlNodeType.Element:
                            Console.Write(reader.Name + ": ");
                            while (reader.MoveToNextAttribute())
                            {
                                Console.Write(reader.Name + ": " + reader.Value + " ");
                            }
                            break;
                        case System.Xml.XmlNodeType.EndElement:
                            Console.WriteLine("");
                            break;
                    }
                }




        }

        public void InitNewGame()
		{
			ClearGameObjects();
			m_UIStack.Clear();

            skybox = new Objects.Misc.Skybox(m_Game);
            skybox.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)Math.PI / 2);
            skybox.Scale *= 2f;
            SpawnGameObject(skybox);

            // -- Make the city.

            city = new Objects.ArtAsset(m_Game, new Vector3(-4, 0, 0), "City");
            city.Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, (float)Math.PI / 2);
            city.Scale *= 0.8f;
            SpawnGameObject(city);
			
			m_bPaused = false;
			GraphicsManager.Get().ResetProjection();
			
			m_Timer.RemoveAll();
					
			// TODO: Add any gameplay setup here ------------------------
            
            // Init the two players
            m_players.Clear();
            for (int i = 1; i <= MAXPLAYERS; i++)
            {
                m_players.Add(new Objects.Player(i, MAXUNITS));
            }
            m_activePlayer = m_players[0];

            // Init the available in-game units
            m_availableUnits.Clear();
            m_availableUnits.Add(new Objects.Units.Archer(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Berserker(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Cleric(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Pikeman(m_Game, null, 1));
            m_availableUnits.Add(new Objects.Units.Swordsman(m_Game, null, 1));

            // Init background art for the Board
            InitBackground();

            // Init the Game Board
            InitGameBoard();
		}

        private void InitGameplay()
        {
            foreach (Objects.Player p in m_players)
            {
                p.activeUnit = p.units[0];
            }
        }

        private void CreateArtAsset(string asset, Vector2 pos, Quaternion rot)
        {
            Objects.ArtAsset thing = new Objects.ArtAsset(m_Game, new Vector3(pos.X - 8, pos.Y - 4, 0), asset);
            thing.Rotation = rot;
            SpawnGameObject(thing);
        }
        
        private void InitBackground(string levelName = "")
        {
            // Hardcoded board for now
            for (int x = 0; x < 11; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    CreateArtAsset("Tile", new Vector2(x, y), Quaternion.Identity);
                }
            }
            CreateArtAsset("Corner Wall", new Vector2(0, 0), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI));
            CreateArtAsset("Corner Wall", new Vector2(10, 0), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2));
            CreateArtAsset("Corner Wall", new Vector2(0, 10), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Corner Wall", new Vector2(10, 10), Quaternion.Identity);
            for (int x = 1; x < 10; x++)
            {
                CreateArtAsset("Horizontal Wall", new Vector2(x, 0), Quaternion.Identity);
                CreateArtAsset("Horizontal Wall", new Vector2(x, 10), Quaternion.Identity);
            }
            for (int y = 1; y < 10; y++)
            {
                CreateArtAsset("Horizontal Wall", new Vector2(0, y), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
                CreateArtAsset("Horizontal Wall", new Vector2(10, y), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            }
            CreateArtAsset("Horizontal Wall", new Vector2(2, 2), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Horizontal Wall", new Vector2(2, 8), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Horizontal Wall", new Vector2(8, 2), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Horizontal Wall", new Vector2(8, 8), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Horizontal Wall", new Vector2(3, 5), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Horizontal Wall", new Vector2(7, 5), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Horizontal Wall", new Vector2(3, 6), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Horizontal Wall", new Vector2(7, 4), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));

            CreateArtAsset("Corner Wall", new Vector2(2, 3), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2));
            CreateArtAsset("Corner Wall", new Vector2(2, 7), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI));
            CreateArtAsset("Corner Wall", new Vector2(8, 3), Quaternion.Identity);
            CreateArtAsset("Corner Wall", new Vector2(8, 7), Quaternion.CreateFromAxisAngle(Vector3.UnitZ, -(float)Math.PI / 2));

            for (int x = 3; x < 8; x++)
            {
                CreateArtAsset("Horizontal Wall", new Vector2(x, 3), Quaternion.Identity);
                CreateArtAsset("Horizontal Wall", new Vector2(x, 7), Quaternion.Identity);
            }

            // Make a chest
            Objects.Chest chest = new Objects.Chest(m_Game);
            chest.Position = new Vector3(5 - 8, 5 - 4, 0.05f);
            chest.addItem(new Objects.Items.SteelSword());
            m_chests.Add(chest);
            SpawnGameObject(chest);
        }

        private void InitGameBoard(string levelName = "")
        {
            // Hardcoded board for now
            Objects.Tile[,] tiles = Objects.Board.CreateDefaultBoard(m_Game, new Point(11, 11));
            
            for (int x = 0; x < 11; x++)
            {
                tiles[x, 0] = null;
                tiles[x, 10] = null;
            }
            for (int y = 0; y < 11; y++)
            {
                tiles[0, y] = null;
                tiles[10, y] = null;
            }
            tiles[2, 2] = null;
            tiles[8, 2] = null;
            for (int x = 2; x < 9; x++)
            {
                tiles[x, 3] = null;
            }
            tiles[7, 4] = null;
            tiles[7, 5] = null;
            tiles[3, 5] = null;
            tiles[3, 6] = null;
            for (int x = 2; x < 9; x++)
            {
                tiles[x, 7] = null;
            }
            tiles[2, 8] = null;
            tiles[8, 8] = null;

            // Init spawn areas for units
            for (int x = 3; x <= 7; x++)
            {
                tiles[x, 2].Owner = m_players[0];
                tiles[x, 8].Owner = m_players[1];
            }

            // Make a place for a treasure chest
            tiles[5, 5] = null;

            // Init board
            m_Board = new Objects.Board(m_Game, tiles);
            foreach(Objects.Tile t in m_Board.tiles)
            {
                if (t != null)
                    SpawnGameObject(t);
            }
        }

		public void Update(float fDeltaTime)
		{
			HandleStateChange();

			switch (m_State)
			{
				case eGameState.MainMenu:
					UpdateMainMenu(fDeltaTime);
					break;
                case eGameState.GameplaySetup:
                    UpdateGameplaySetup(fDeltaTime);
                    break;
                case eGameState.PickAndPlaceUnits:
                    UpdatePickAndPlaceUnits(fDeltaTime);
                    break;
				case eGameState.Gameplay:
					UpdateGameplay(fDeltaTime);
					break;
			}

			foreach (UI.UIScreen u in m_UIStack)
			{
				u.Update(fDeltaTime);
			}


		}

		void UpdateMainMenu(float fDeltaTime)
		{

		}

        void UpdateGameplaySetup(float fDeltaTime)
        {
            m_NextState = eGameState.PickAndPlaceUnits;
        }

        void UpdatePickAndPlaceUnits(float fDeltaTime)
        {
            if (!IsPaused)
            {
                m_Camera.Update(fDeltaTime);

                LinkedList<GameObject> temp = new LinkedList<GameObject>(m_GameObjects);
                foreach (GameObject o in temp)
                {
                    if (o.Enabled)
                    {
                        o.Update(fDeltaTime);
                    }
                }
                m_Timer.Update(fDeltaTime);
            }
        }

		void UpdateGameplay(float fDeltaTime)
		{
			if (!IsPaused)
			{
				m_Camera.Update(fDeltaTime);

				// Update objects in the world
				// We have to make a temp copy in case the objects list changes
				LinkedList<GameObject> temp = new LinkedList<GameObject>(m_GameObjects);
				foreach (GameObject o in temp)
				{
					if (o.Enabled)
					{
						o.Update(fDeltaTime);
					}
				}
				m_Timer.Update(fDeltaTime);
			}
		}

		public void SpawnGameObject(GameObject o)
		{
			o.Load();
			m_GameObjects.AddLast(o);
			GraphicsManager.Get().AddGameObject(o);
		}

		public void RemoveGameObject(GameObject o, bool bRemoveFromList = true)
		{
			o.Enabled = false;
			o.Unload();
			GraphicsManager.Get().RemoveGameObject(o);
			if (bRemoveFromList)
			{
				m_GameObjects.Remove(o);
			}
		}

        public Vector3 toWorldSpace(Point p, float zOffset = 0)
        {
            Vector2 screenSpace = new Vector2();
            screenSpace.X = 50 + 55 * p.X;
            screenSpace.Y = 50 + 55 * p.Y;

            GraphicsDevice device = GraphicsManager.Get().GraphicsDevice;
            Vector3 worldPos = device.Viewport.Unproject(
                new Vector3(screenSpace.X, screenSpace.Y, 0.5f),
                GraphicsManager.Get().Projection, CameraMatrix, Matrix.Identity);

            return worldPos + new Vector3(0, 0, zOffset);
        }

        public void UpdateActivePlayer()
        {
            switch (m_State)
            {
                case eGameState.PickAndPlaceUnits:
                    {
                        if (m_activePlayer.playerID == MAXPLAYERS)
                        {
                            m_NextState = eGameState.Gameplay;
                        }
                        break;
                    }

                case eGameState.Gameplay:
                    {
                        m_Board.resetALLTiles(false);
                        UI.UIGameplay gameplayUI = (UI.UIGameplay)GetCurrentUI();
                        gameplayUI.removeMenu();
                       
                        break;
                    }
            }
            
            while (true)
            {
                m_activePlayer = m_players[m_activePlayer.playerID % MAXPLAYERS]; // Note: Index starts at 0, but player IDs start at 1, so this increments m_activePlayer
                if (!m_activePlayer.dead)
                {
                    break;
                }
            }

            if (m_State == eGameState.Gameplay)
            {
                foreach (Objects.Unit unit in m_activePlayer.units)
                {
                    unit.animate(Objects.Unit.AnimState.none);
                }
                UI.UIGameplay gameplayUI = (UI.UIGameplay)GetCurrentUI();
                gameplayUI.NextPlayer(m_activePlayer.playerID);
            }
            
            
            try
            {
                m_activePlayer.activeUnit = m_activePlayer.units[0];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            System.Diagnostics.Debug.Print("GameState::UpdateActivePlayer(): Update to Player " + m_activePlayer.playerID);
        }

        private void CheckIfPlayerLost(Objects.Unit deadUnit)
        {
            if (deadUnit.isKing)
            {
                deadUnit.getOwner().dead = true;
                GameOver(true, deadUnit.getOwner());
                return;
            }
            foreach (Objects.Player p in m_players)
            {
                if (p.activeUnits != 1)
                {
                    return;
                }
            }
            GameOver(false, null);
        }

        private void UpdateActiveUnit(Objects.Player player)
        {   
            int currentUnit = player.units.IndexOf(player.activeUnit);
            int nextUnit = (currentUnit + 1) % player.activeUnits;

            player.activeUnit = player.units[nextUnit];
            player.activeUnit.movedThisTurn = false;
            player.activeUnit.performedActionThisTurn = false;

            if (player == m_activePlayer && nextUnit == 0) 
            {
                UpdateActivePlayer();
            }

            TargetCamera();
            System.Diagnostics.Debug.Print("GameState::UpdateActiveUnit(Objects.Player): Player " + player.playerID + "'s active unit updated to Unit " + nextUnit + " (" + player.activeUnit.GetType().Name + ")");
        }

        public void CheckToUpdateActiveUnit()
        {
            if (m_activePlayer.activeUnit.movedThisTurn && m_activePlayer.activeUnit.performedActionThisTurn)
            {
                
                if (m_State == eGameState.Gameplay)
                {
                    m_activePlayer.activeUnit.animate(Objects.Unit.AnimState.done);

                    UI.UIGameplay gameplayUI = (UI.UIGameplay) GetCurrentUI();

                    gameplayUI.removeMenu();
                }
                
                m_Board.resetALLTiles(false);

                UpdateActiveUnit(m_activePlayer);
            }
        }

        private void TargetCamera()
        {
            Vector3 newTarget = m_activePlayer.activeUnit.Position;
            newTarget.Z = m_Camera.Target.Z;
            m_Camera.Target = newTarget;
        }
        
        public void AddUnitToPlayer(System.Type unit, float z = 0, Objects.Tile t = null)
        {
            if (m_activePlayer.units.Count == m_activePlayer.maxUnits)
            {
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Tried to add too many units to Player " + m_activePlayer.playerID);
                return;
            }

            Objects.Unit newUnit = null;
            if (unit == typeof(Objects.Units.Archer))
            {
                newUnit = new Objects.Units.Archer(m_Game, m_activePlayer, z);
                m_activePlayer.units.Add(newUnit);
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Successfully added Archer to Player " + m_activePlayer.playerID);
            }
            else if (unit == typeof(Objects.Units.Berserker))
            {
                newUnit = new Objects.Units.Berserker(m_Game, m_activePlayer, z);
                m_activePlayer.units.Add(newUnit);
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Successfully added Berserker to Player " + m_activePlayer.playerID);
            }
            else if (unit == typeof(Objects.Units.Cleric))
            {
                newUnit = new Objects.Units.Cleric(m_Game, m_activePlayer, z);
                m_activePlayer.units.Add(newUnit);
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Successfully added Cleric to Player " + m_activePlayer.playerID);
            }
            else if (unit == typeof(Objects.Units.Pikeman))
            {
                newUnit = new Objects.Units.Pikeman(m_Game, m_activePlayer, z);
                m_activePlayer.units.Add(newUnit);
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Successfully added Pikeman to Player " + m_activePlayer.playerID);
            }
            else if (unit == typeof(Objects.Units.Swordsman))
            {
                newUnit = new Objects.Units.Swordsman(m_Game, m_activePlayer, z);
                m_activePlayer.units.Add(newUnit);
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Successfully added Swordsman to Player " + m_activePlayer.playerID);
            }
            else if (unit == typeof(Objects.Units.King))
            {
                newUnit = new Objects.Units.King(m_Game, m_activePlayer, z);
                m_activePlayer.units.Add(newUnit);
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Successfully added King to Player " + m_activePlayer.playerID);
            }
            else
            {
                System.Diagnostics.Debug.Print("GameState::AddUnitToPlayer(Objects.Unit): Invalid Unit type provided");
                return;
            }

            if (t != null)
            {
                //newUnit.Position = toWorldSpace(t.GridLocation, newUnit.z);
                newUnit.Position = new Vector3(t.GridLocation.X - 8, t.GridLocation.Y - 4, UNITSPAWNHEIGHT);
                newUnit.InitializeUnit();
                newUnit.gridLocation = t.GridLocation;
                t.Occupant = newUnit;
                newUnit.getInventory().Add(new Objects.Items.HealthPotion());
                newUnit.DrawOrder = eDrawOrder.Transparent;
                SpawnGameObject(newUnit);
            }
        }

        public void RemoveUnitFromPlayer(Objects.Unit toRemove)
        {
            System.Diagnostics.Debug.Print("GameState::RemoveUnitFromPlayer(Objects.Unit): Removing " + toRemove.GetType().Name + " from Player " + m_activePlayer.playerID);
            m_Board.tiles[toRemove.gridLocation.X, toRemove.gridLocation.Y].Occupant = null;
            m_activePlayer.units.Remove(toRemove);
            RemoveGameObject(toRemove);
        }

		public void MouseClick(Point Position)
		{
            if (m_State == eGameState.Gameplay && !IsPaused)
			{
				// TODO: Respond to mouse clicks here
			}
		}

        public Objects.Tile TileClicked(Point Position)
        {
            // Make raycast
            Ray ray = RayCastFromMouseToWorld(Position);

            foreach (Objects.Tile t in m_Board.tiles)
            {
                if (t == null) continue;

                if (ray.Intersects(t.WorldBounds) != null)
                {
                    // You found the tile that the mouse is clicking on
                    return t;
                }
            }

            return null;
        }

        public Objects.Tile TileClicked(Ray ray)
        {
            foreach (Objects.Tile t in m_Board.tiles)
            {
                if (t == null) continue;

                if (ray.Intersects(t.WorldBounds) != null)
                {
                    // You found the tile that the mouse is clicking on
                    return t;
                }
            }

            return null;
        }

        public Objects.Chest ChestClicked(Ray ray)
        {
            foreach (Objects.Chest c in m_chests)
            {
                if (ray.Intersects(c.WorldBounds) != null)
                {
                    // You found the chest that the mouse is clicking on
                    return c;
                }
            }

            return null;
        }

        private Ray RayCastFromMouseToWorld(Point Position)
        {
            // Raycast from mouse into world
            GraphicsDevice device = GraphicsManager.Get().GraphicsDevice;
            Vector3 start = device.Viewport.Unproject(
                new Vector3(Position.X, Position.Y, 0),
                GraphicsManager.Get().Projection, CameraMatrix, Matrix.Identity);
            Vector3 end = device.Viewport.Unproject(
                new Vector3(Position.X, Position.Y, 1),
                GraphicsManager.Get().Projection, CameraMatrix, Matrix.Identity);

            Vector3 dir = Vector3.Normalize(end - start);

            // Make raycast
            return new Ray(start, dir);
        }

        private int GetManhattanDistance(Vector3 one, Vector3 two)
        {
            return (int) (Math.Abs(one.X - two.X) + Math.Abs(one.Y - two.Y));
        }

        public Objects.Unit HandleGameplayMouseClick(Point Position)
        {
            Ray raycast = RayCastFromMouseToWorld(Position);
            Objects.Tile selectedTile = TileClicked(raycast);
            Objects.Chest selectedChest = null;
            if (selectedTile == null)
            {
                selectedChest = ChestClicked(raycast);
                if (selectedChest != null)
                {   
                    if (GetManhattanDistance(selectedChest.Position, m_activePlayer.activeUnit.Position) == 1 && !m_activePlayer.activeUnit.performedActionThisTurn)
                    {
                        if (m_activePlayer.activeUnit.getInventory().Count < m_activePlayer.activeUnit.inventorySize)
                        {
                            m_activePlayer.activeUnit.performedActionThisTurn = true;
                            m_activePlayer.activeUnit.getInventory().Add(selectedChest.getContents());
                            System.Diagnostics.Debug.Print("GameState::HandleGameplayMouseClick(Point): Player " + m_activePlayer.playerID + "'s " + m_activePlayer.activeUnit.GetType().Name + " received a " + selectedChest.getContents().name + ".");
                            m_chests.Remove(selectedChest);
                            RemoveGameObject(selectedChest);
                            return null;
                        }
                    }
                }
            }

            if (selectedTile == null || (
                !m_activePlayer.currentPossibleMoves.Contains(selectedTile.GridLocation) &&
                !m_activePlayer.currentPossibleAttacks.Contains(selectedTile.GridLocation)))
            {
                m_Board.resetALLTiles(false);
                m_activePlayer.currentPossibleMoves.Clear();
                m_activePlayer.currentPossibleAttacks.Clear();
            }
            else if (!isUnitMoving)
            {
                if (m_activePlayer.currentPossibleMoves.Contains(selectedTile.GridLocation)) {
                    MoveUnit(m_activePlayer.activeUnit, selectedTile);
                }
                else if (m_activePlayer.currentPossibleAttacks.Contains(selectedTile.GridLocation))
                {
                    if (selectedTile.Occupant != null)
                    {
                        if (m_activePlayer.activeUnit is Objects.Units.Cleric)
                        {
                            if (selectedTile.Occupant.getOwner() == m_activePlayer)
                            {
                                AttackUnit(m_activePlayer.activeUnit, selectedTile.Occupant, selectedTile, true);
                            }
                        }
                        else
                        {
                            if (selectedTile.Occupant.getOwner() != m_activePlayer)
                            {
                                AttackUnit(m_activePlayer.activeUnit, selectedTile.Occupant, selectedTile, false);
                            }
                        }
                    }
                }
            }

            if (selectedTile != null)
            {
                return selectedTile.Occupant;
            }
            return null;
        }

        public void MoveUnitShow(Objects.Unit unit)
        {
            if (unit.movedThisTurn) return;

            m_activePlayer.currentPossibleAttacks.Clear();
            m_activePlayer.currentPossibleMoves = m_Board.PossiblePath(unit.gridLocation);
        }

        public void MoveUnit(Objects.Unit unit, Objects.Tile to)
        {
            System.Diagnostics.Debug.Print("GameState::MoveUnit(Objects.Unit, Objects.Tile): Player " + m_activePlayer.playerID + "'s " + unit.GetType().Name + " moving to " + to.GridLocation);
            
            List<Point> path = m_Board.ShortestPath(unit.gridLocation, to.GridLocation);
            m_Board.tiles[unit.gridLocation.X, unit.gridLocation.Y].Occupant = null;
            to.Occupant = m_activePlayer.activeUnit;
            unit.gridLocation = to.GridLocation;
            unit.moveTo(path);
            unit.movedThisTurn = true;
            isUnitMoving = true;
            m_activePlayer.currentPossibleMoves.Clear();
        }

        public void AttackUnitShow(Objects.Unit unit)
        {
            if (unit.performedActionThisTurn) return;

            m_activePlayer.currentPossibleMoves.Clear();
            m_activePlayer.currentPossibleAttacks = m_Board.AttackRange(unit.gridLocation);
        }

        public void HealUnitShow(Objects.Unit unit)
        {
            if (unit.performedActionThisTurn) return;

            m_activePlayer.currentPossibleMoves.Clear();
            m_activePlayer.currentPossibleAttacks = m_Board.HealRange(unit.gridLocation);
        }

        public void AttackUnit(Objects.Unit attacker, Objects.Unit defender, Objects.Tile defenderTile, bool friendly)
        { 
            m_Board.resetALLTiles(false);

            attacker.animate(Objects.Unit.AnimState.attack, true);

            if (!friendly)
            {
                System.Diagnostics.Debug.Print("GameState::AttackUnit(Objects.Unit, Objects.Unit, Objects.Tile): Player " + m_activePlayer.playerID + "'s " + attacker.GetType().Name + " attacking Player " + defender.getOwner().playerID + "'s " + defender.GetType().Name);

                int dodgeChance = defender.dexterity - attacker.dexterity / 5;
                bool dodged = random.Next(100) <= dodgeChance;

                if (!dodged)
                {
                    int damage = attacker.baseAttackDamage;
                    int armor = defender.defense;
                    //animation
                    defender.hitOrMiss(true);
                    //end animation
                    if (attacker.equippedWeapon != null)
                    {
                        damage += attacker.equippedWeapon.bonusDamage;
                        attacker.equippedWeapon.use();
                        if (attacker.equippedWeapon.remainingUses <= 0)
                        {
                            attacker.getInventory().Remove(attacker.equippedWeapon);
                        }
                    }
                    if (defender.equippedArmor != null)
                    {
                        armor += defender.equippedArmor.bonusArmor;
                        defender.equippedArmor.use();
                        if (defender.equippedArmor.remainingUses <= 0)
                        {
                            defender.getInventory().Remove(attacker.equippedArmor);
                        }
                    }

                    int trueDamage = (int)Math.Round(damage * (1 - armor / 41.6667f));
                    if (attacker.weaponType == Objects.Unit.WeaponType.sword && defender.weaponType == Objects.Unit.WeaponType.axe)
                    {
                        trueDamage = (int) (trueDamage * 1.2);
                    }
                    else if (attacker.weaponType == Objects.Unit.WeaponType.axe && defender.weaponType == Objects.Unit.WeaponType.spear)
                    {
                        trueDamage = (int)(trueDamage * 1.2);
                    }
                    else if (attacker.weaponType == Objects.Unit.WeaponType.spear && defender.weaponType == Objects.Unit.WeaponType.sword)
                    {
                        trueDamage = (int)(trueDamage * 1.2);
                    }
                    defender.HP -= trueDamage;
                    System.Diagnostics.Debug.Print("\t" + trueDamage + " damage was done.");
                }
                else
                {
                    System.Diagnostics.Debug.Print("\tAttack was dodged!");
                    //aniamtion
                    defender.hitOrMiss(false);
                    //end animation
                    if (attacker.equippedWeapon != null)
                    {
                        attacker.equippedWeapon.use();
                        if (attacker.equippedWeapon.remainingUses <= 0)
                        {
                            attacker.getInventory().Remove(attacker.equippedWeapon);
                        }
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.Print("GameState::AttackUnit(Objects.Unit, Objects.Unit, Objects.Tile): Player " + m_activePlayer.playerID + "'s " + attacker.GetType().Name + " healing Player " + defender.getOwner().playerID + "'s " + defender.GetType().Name);
                defender.HP = Math.Min(defender.HP + attacker.baseAttackDamage, defender.maxHP);
            }

            if (defender.HP <= 0)
            {
                defenderTile.Occupant = null;
                defender.getOwner().units.Remove(defender);
                if (defender == defender.getOwner().activeUnit)
                {
                    UpdateActiveUnit(defender.getOwner());
                }
                RemoveGameObject(defender);
                CheckIfPlayerLost(defender);
            }

            attacker.performedActionThisTurn = true;
            m_activePlayer.currentPossibleAttacks.Clear();
        }

        public void UseItemUnit(Objects.Unit unit, Objects.Item itemToUse)
        {
            switch (itemToUse.type)
            {
            case Objects.Item.ItemType.potion:
                {
                    Objects.Items.HealthPotion pot = (Objects.Items.HealthPotion) itemToUse;
                    unit.HP = (int) MathHelper.Min(unit.maxHP, unit.HP + pot.hpRestored);
                    itemToUse.use();
                    SoundManager.Get().PlaySoundCue("Potion");
                    unit.performedActionThisTurn = true;
                    break;
                }
            case Objects.Item.ItemType.armor:
                {
                    if (unit.equippedArmor == itemToUse)
                    {
                        unit.equippedArmor = null;
                    }
                    else
                    {
                        unit.equippedArmor = (Objects.Items.Armor) itemToUse;
                    }
                    break;
                }
            case Objects.Item.ItemType.none:
                {
                    System.Diagnostics.Debug.Print("GameState::UseItemUnit(Objects.Unit, Objects.Item): Item type is none.");
                    return;
                }
            case Objects.Item.ItemType.weapon:
                {
                    if (unit.equippedWeapon == itemToUse)
                    {
                        unit.equippedWeapon = null;
                    }
                    else
                    {
                        unit.equippedWeapon = (Objects.Items.Weapon) itemToUse;
                    }
                    break;
                }
            default:
                {
                    System.Diagnostics.Debug.Print("GameState::UseItemUnit(Objects.Unit, Objects.Item): Item type not set.");
                    return;
                }
            }

            if (itemToUse.remainingUses <= 0)
            {
                unit.getInventory().Remove(itemToUse);
            }
        }

        public void WaitUnit(Objects.Unit unit)
        {
            m_activePlayer.currentPossibleMoves.Clear();
            m_activePlayer.currentPossibleAttacks.Clear();
            m_Board.resetALLTiles(false);
            //gray unit here
            

            if (unit.getAnimState() != Objects.Unit.AnimState.none)
            {
                unit.performedActionThisTurn = true;
                unit.movedThisTurn = true;
            }
            else {
                m_activePlayer.activeUnit.animate(Objects.Unit.AnimState.done);
                UpdateActiveUnit(m_activePlayer);
            }
        }

        public void UnitDoneMoving()
        {
            isUnitMoving = false;
        }

		// I'm the last person to get keyboard input, so don't need to remove
		public void KeyboardInput(SortedList<eBindings, BindInfo> binds, float fDeltaTime)
		{
			if ((m_State == eGameState.Gameplay || m_State == eGameState.PickAndPlaceUnits) && !IsPaused)
			{
                Camera.KeyboardInput(binds, fDeltaTime);
			}
		}

		public UI.UIScreen GetCurrentUI()
		{
			return m_UIStack.Peek();
		}

		public int UICount
		{
			get { return m_UIStack.Count; }
		}

		// Has to be here because only this can access stack!
		public void DrawUI(float fDeltaTime, SpriteBatch batch)
		{
			// We draw in reverse so the items at the TOP of the stack are drawn after those on the bottom
			foreach (UI.UIScreen u in m_UIStack.Reverse())
			{
				u.Draw(fDeltaTime, batch);
			}
		}

		// Pops the current UI
		public void PopUI()
		{
			m_UIStack.Peek().OnExit();
			m_UIStack.Pop();
		}

		public void ShowPauseMenu()
		{
			IsPaused = true;
			m_UIStack.Push(new UI.UIPauseMenu(m_Game.Content));
		}

		public void Exit()
		{
			m_Game.Exit();
		}

		void GameOver(bool victorious, Objects.Player loser)
		{
            if (victorious)
            {
                System.Diagnostics.Debug.Print("PLAYER " + loser.playerID + " LOSES!!!");
                IsPaused = true;
                m_UIStack.Push(new UI.UIGameOver(m_Game.Content, loser));
            }
            else
            {
                System.Diagnostics.Debug.Print("IT'S A TIE!!!");
                IsPaused = true;
                m_UIStack.Push(new UI.UIGameOver(m_Game.Content, null));
            }
		}
	}
}
