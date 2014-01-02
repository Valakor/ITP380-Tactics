using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects
{
    public class Tile : GameObject
    {
        private Objects.Unit m_Occupant;
        private int m_MoveCost;

        private List<Tile> adjacentTiles;
        private Point position;
        public Point GridLocation
        {
            get { return position; }
            set { position = value; }
        }

        //A*
        public Tile parent;
        public int f;
        public int g;
        public int h;

        public bool open;

        //Possile Moves
        public int distFromOrigin;
        public bool inPriorityQ;

        public enum TileState { none, atk, move, selected, placeable };
        private TileState state = TileState.none;
        private TileState prevState = TileState.none;

        //Unit placement
        Player owner = null;

        Texture2D m_NormTxt;
        Texture2D m_AtkTxt;
        Texture2D m_MoveTxt;
        Texture2D m_Placeable;
        Texture2D m_Selected;

        public void resetTile(bool keepSelected)
        {
            f = 0;
            g = 0;
            h = 0;
            open = true;
            parent = null;
            inPriorityQ = false;
            distFromOrigin = -1;
            if (keepSelected && state == TileState.selected)
            {
                prevState = TileState.none;
            }
            else
            {
                state = TileState.none;
                prevState = TileState.none;
            }
        }

        #region Properties
        public TileState tileState
        {
            get { return state; }
        }
        
        public void changeState(TileState newState)
        {
            prevState = state;
            state = newState;
        }

        public void select()
        {
            prevState = state;
            state = TileState.selected;
        }

        public void deSelect()
        {
            state = prevState;
            prevState = TileState.none;
        }

        public Player Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public List<Tile> getNeighbors(){
            return adjacentTiles;
        }

        public Objects.Unit Occupant
        {
            get { return m_Occupant; }
            set { m_Occupant = value; }
        }

        public int MoveCost
        {
            get { return m_MoveCost; }
            set { m_MoveCost = value; }
        }

        #endregion

        public Tile(Game game, Point pos) : base (game)
        {
            m_Occupant = null;
            m_MoveCost = 1;

            Scale *= 0.95f;

            adjacentTiles = new List<Tile>();
            position = pos;
            open = true;
            parent = null;

            //m_Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            m_ModelName = "Tile/Tile";
        }


        public override void Load()
        {
            base.Load();
            m_NormTxt = m_Game.Content.Load<Texture2D>("Tile/tile_norm");
            m_MoveTxt = m_Game.Content.Load<Texture2D>("Tile/tile_moveable");
            m_AtkTxt = m_Game.Content.Load<Texture2D>("Tile/tile_attack");
            m_Placeable = m_Game.Content.Load<Texture2D>("Tile/tile_place");
            m_Selected = m_Game.Content.Load<Texture2D>("Tile/tile_selected");

        }

        public override void Draw(float fDeltaTime)
        {
            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    switch (state)
                    {
                        case TileState.move: effect.Texture = m_MoveTxt; break;

                        case TileState.atk: effect.Texture = m_AtkTxt; break;

                        case TileState.none: effect.Texture = m_NormTxt; break;

                        case TileState.selected: effect.Texture = m_Selected; break;

                        case TileState.placeable: effect.Texture = m_Placeable; break;

                        default: break;

                    }

                }
            }
            base.Draw(fDeltaTime);
        }

    }
}
