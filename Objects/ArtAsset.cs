using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects
{
    class ArtAsset: GameObject
    {

        enum Assets { none, tile, horizontalWall, cornerWall, city };
        Assets asset = Assets.none;

        //Choices
        /*
         * 
         * Tile
         * Horizontal Wall
         * Cornal Wall
         * City
         * 
         * */

        public ArtAsset(Game game, Vector3 position, String choice) : base (game)
        {

           // m_Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitX, 90);
            GenerateAsset(choice);

            Position = position;

            GenerateModel();
            //LoadTextures();
        }

        private void GenerateAsset(String choice)
        {
            switch (choice)
            {
                case "Tile":
                    asset = Assets.tile;
                    break;

                case "Horizontal Wall":
                    asset = Assets.horizontalWall;
                    break;

                case "Corner Wall":
                    asset = Assets.cornerWall;
                    break;
                case "City":
                    asset = Assets.city;
                    break;
            }
        }

        private void GenerateModel()
        {
            switch (asset)
            {
                case Assets.tile:
                    m_ModelName = "Tile/tile";
                    break;

                case Assets.horizontalWall:
                    m_ModelName = "Wall/horizontalWall";
                    break;

                case Assets.cornerWall:
                    m_ModelName = "Wall/cornerWall";
                    break;

                case Assets.city:
                    m_ModelName = "Models/City/Exterior";
                    break;
            }
        }

        //Textures
        Texture2D m_Texture;

        public override void Load()
        {
            base.Load();

            switch (asset)
            {
                case Assets.tile:
                    m_Texture = GenerateTileTexture(GameState.Get().random.Next(6));
                    break;

                case Assets.horizontalWall:
                    //m_Texture = m_Game.Content.Load<Texture2D>("Wall/brick");
                    break;

                case Assets.cornerWall:
                    //m_Texture = m_Game.Content.Load<Texture2D>("Wall/walling");
                    break;
            }

        }

        private Texture2D GenerateTileTexture(int num)
        {
            switch (num)
            {
                case 0: return m_Game.Content.Load<Texture2D>("Floor/floor-1");
                case 1: return m_Game.Content.Load<Texture2D>("Floor/floor-2");
                case 2: return m_Game.Content.Load<Texture2D>("Floor/floor-3");
                case 3: return m_Game.Content.Load<Texture2D>("Floor/floor-4");
                case 4: return m_Game.Content.Load<Texture2D>("Floor/floor-5");
                case 5: return m_Game.Content.Load<Texture2D>("Floor/floor-6");
                default: break;
            }
            return null;
        }

        public override void Draw(float fDeltaTime)
        {
            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (asset != Assets.none)
                    {
                        if (asset == Assets.tile)
                        effect.Texture = m_Texture;
                    }

                }
            }

            base.Draw(fDeltaTime);
        }
   

    }
}
