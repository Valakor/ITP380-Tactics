using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Board : GameObject
    {
        Objects.Tile [,] m_Tiles;
        public Objects.Tile[,] tiles
        {
            get { return m_Tiles; }
        }

        public Board(Game game, Point size)
            : base(game)
        {
            m_Tiles = new Objects.Tile[size.X, size.Y]; //Creates a new board and sets its size.
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    m_Tiles[i, j] = new Objects.Tile(game, new Point(i, j));
                    m_Tiles[i, j].Position = GameState.Get().toWorldSpace(m_Tiles[i,j].GridLocation);
                    GameState.Get().SpawnGameObject(m_Tiles[i, j]);
                }
            }
            SetupAdjacency();
        }

        public Board(Game game, Objects.Tile[,] tiles)
            : base(game)
        {
            m_Tiles = tiles;
            SetupAdjacency();
        }

        public static Objects.Tile[,] CreateDefaultBoard(Game game, Point size)
        {
            Objects.Tile[,] tiles = new Objects.Tile[size.X, size.Y];

            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    tiles[i, j] = new Objects.Tile(game, new Point(i, j));
                    //tiles[i, j].Position = GameState.Get().toWorldSpace(tiles[i, j].GridLocation);
                    tiles[i, j].Position = new Vector3(tiles[i, j].GridLocation.X - 8,
                                                       tiles[i, j].GridLocation.Y - 4,
                                                       0.1f);
                }
            }

            return tiles;
        }

        private void SetupAdjacency()
        {
            foreach (Objects.Tile t in m_Tiles)
            {
                if (t == null) continue;

                int x = t.GridLocation.X;
                int y = t.GridLocation.Y;

                if (t.GridLocation.Y > 0)
                {
                    // North
                    if (m_Tiles[x, y - 1] != null)
                    {
                        m_Tiles[x, y].getNeighbors().Add(m_Tiles[x, y - 1]);
                    } 
                }
                if (y < m_Tiles.GetLength(1) - 1)
                {
                    // South
                    if (m_Tiles[x, y + 1] != null)
                    {
                        m_Tiles[x, y].getNeighbors().Add(m_Tiles[x, y + 1]);
                    }
                }
                if (x < m_Tiles.GetLength(0) - 1)
                {
                    // East
                    if (m_Tiles[x + 1, y] != null)
                    {
                        m_Tiles[x, y].getNeighbors().Add(m_Tiles[x + 1, y]);
                    }
                }
                if (x > 0)
                {
                    // West
                    if (m_Tiles[x - 1, y] != null)
                    {
                        m_Tiles[x, y].getNeighbors().Add(m_Tiles[x - 1, y]);
                    }
                }
            }
        }

        public List<Point> PossiblePath(Point start)
        {
            resetALLTiles(true);
            List<Objects.Tile> openList = new List<Objects.Tile>();
            List<Objects.Tile> closedList = new List<Objects.Tile>();
            List<Point> possiblePaths = new List<Point>();

            Objects.Tile originTile = m_Tiles[start.X, start.Y];
            if (originTile == null) return null;
            if (originTile.Occupant == null) return null;
            int maxMove = originTile.Occupant.moveDistance;

            //First place the originTile in the openList and set its g to its movement cost.
            PQAdd(originTile, openList);
            //originTile.g = originTile.MoveCost;
            //maxMove = maxMove - originTile.g; //Reduce the maxMoves you can make by the tile you are standing on.

            //While there is something in the openlist.
            while (openList.Count > 0)
            {             
                //Take the first thing out of the openList
                Objects.Tile tile = openList[0];
                openList.RemoveAt(0);
                
                //if the tile has not been checked yet or if it doesn't have any unit on it.
                // -- Possible cases for tile to be closed:
                //      -- Unit is on the tile.
                //      -- Has been checked by 
                if (tile.open == true){
                 
                    closedList.Add (tile);                        
                    tile.open = false;                           
                                        
                    foreach (Objects.Tile i in tile.getNeighbors()){
                        //If there is no occupant in the tile
                        if (i.Occupant == null || i.Occupant.getOwner() == GameState.Get().activePlayer)
                            if (i.open && !i.inPriorityQ)
                            {
                                int newG = tile.g + tile.MoveCost; //What is the distance from the selected origin?
                                if (newG <= maxMove)
                                {                                //Is the total distance more than the player can move?
                                    i.g = newG;        //If it is less, make the distance from origin variable on the adjacent tile to the distance.
                                    PQAdd(i, openList);                       //Place the adjacent tile into the priorityQ.
                                    i.inPriorityQ = true;
                                    if (i != originTile)
                                    {
                                        if (i.Occupant == null)
                                        {
                                            //Make different color.
                                            i.changeState(Tile.TileState.move);
                                            // Add to the possible paths list.
                                            possiblePaths.Add(new Point(i.GridLocation.X, i.GridLocation.Y));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int newG = tile.g + tile.MoveCost;
                                if (newG < i.g)
                                {
                                    i.g = newG;
                                }
                            }
                    }
                }
            }

            return possiblePaths;
        }

        public List<Point> AttackRange(Point start)
        {
            resetALLTiles(true);
            List<Objects.Tile> openList = new List<Objects.Tile>();
            List<Objects.Tile> closedList = new List<Objects.Tile>();
            List<Point> possibleAttackRange = new List<Point>();

            Objects.Tile originTile = m_Tiles[start.X, start.Y];
            if (originTile == null) return null;
            if (originTile.Occupant == null) return null;
            int maxMove = originTile.Occupant.range;

            //First place the originTile in the openList and set its g to its movement cost.
            PQAdd(originTile, openList);

            //While there is something in the openlist.
            while (openList.Count > 0)
            {
                //Take the first thing out of the openList
                Objects.Tile tile = openList[0];
                openList.RemoveAt(0);

                //if the tile has not been checked yet or if it doesn't have any unit on it.
                // -- Possible cases for tile to be closed:
                //      -- Unit is on the tile.
                //      -- Has been checked by 
                if (tile.open == true)
                {

                    closedList.Add(tile);
                    tile.open = false;

                    foreach (Objects.Tile i in tile.getNeighbors())
                    {
                        //If there is no occupant in the tile
                        //if (i.Occupant == null || (i.Occupant != null && i.Occupant.getOwner() != GameState.Get().activePlayer))
                            if (i.open && !i.inPriorityQ)
                            {
                                int newG = tile.g + tile.MoveCost; //What is the distance from the selected origin?
                                if (newG <= maxMove)
                                {                                //Is the total distance more than the player can move?
                                    i.g = newG;        //If it is less, make the distance from origin variable on the adjacent tile to the distance.
                                    PQAdd(i, openList);                       //Place the adjacent tile into the priorityQ.
                                    i.inPriorityQ = true;
                                    if (i != originTile)
                                    {
                                        if (i.Occupant == null || (i.Occupant != null && i.Occupant.getOwner() != GameState.Get().activePlayer))
                                        {
                                            //Make different color.
                                            i.changeState(Tile.TileState.atk);
                                            // Add to the possible paths list.
                                            possibleAttackRange.Add(new Point(i.GridLocation.X, i.GridLocation.Y));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                int newG = tile.g + tile.MoveCost;
                                if (newG < i.g)
                                {
                                    i.g = newG;
                                }
                            }
                    }
                }
            }

            return possibleAttackRange;
        }

        public List<Point> HealRange(Point start)
        {
            resetALLTiles(true);
            List<Objects.Tile> openList = new List<Objects.Tile>();
            List<Objects.Tile> closedList = new List<Objects.Tile>();
            List<Point> possibleAttackRange = new List<Point>();

            Objects.Tile originTile = m_Tiles[start.X, start.Y];
            if (originTile == null) return null;
            if (originTile.Occupant == null) return null;
            int maxMove = originTile.Occupant.range;

            //First place the originTile in the openList and set its g to its movement cost.
            PQAdd(originTile, openList);

            //While there is something in the openlist.
            while (openList.Count > 0)
            {
                //Take the first thing out of the openList
                Objects.Tile tile = openList[0];
                openList.RemoveAt(0);

                //if the tile has not been checked yet or if it doesn't have any unit on it.
                // -- Possible cases for tile to be closed:
                //      -- Unit is on the tile.
                //      -- Has been checked by 
                if (tile.open == true)
                {

                    closedList.Add(tile);
                    tile.open = false;

                    foreach (Objects.Tile i in tile.getNeighbors())
                    {
                        //If there is no occupant in the tile
                        if (i.Occupant == null || (i.Occupant != null && i.Occupant.getOwner() == GameState.Get().activePlayer))
                            if (i.open && !i.inPriorityQ)
                            {
                                int newG = tile.g + tile.MoveCost; //What is the distance from the selected origin?
                                if (newG <= maxMove)
                                {                                //Is the total distance more than the player can move?
                                    i.g = newG;        //If it is less, make the distance from origin variable on the adjacent tile to the distance.
                                    PQAdd(i, openList);                       //Place the adjacent tile into the priorityQ.
                                    i.inPriorityQ = true;
                                    if (i != originTile)
                                    {
                                        //Make different color.
                                        i.changeState(Tile.TileState.atk);
                                        // Add to the possible paths list.
                                        possibleAttackRange.Add(new Point(i.GridLocation.X, i.GridLocation.Y));
                                    }
                                }
                            }
                            else
                            {
                                int newG = tile.g + tile.MoveCost;
                                if (newG < i.g)
                                {
                                    i.g = newG;
                                }
                            }
                    }
                }
            }

            return possibleAttackRange;
        }

        private bool inPQ(Tile t, List<Tile> pq)
        {
            //Insert a search method here.
            //currently using a linear search algorithm
            foreach (Tile tile in pq){
                if (tile == t)
                {
                    return true;
                }
            }
            return false;
        }

        // Calculate A* path from start to end (the actual calculation is computed from end to start so we don't have to reverse the stack)
        public List<Point> ShortestPath(Point start, Point end)
        {
            resetALLTiles(false);
            List<Objects.Tile> openList = new List<Objects.Tile>();
            List<Objects.Tile> closedList = new List<Objects.Tile>();
            Objects.Tile currentNode = m_Tiles[end.X, end.Y];
            closedList.Add(currentNode);

            do
            {
                foreach (Objects.Tile n in currentNode.getNeighbors())
                {
                    if (closedList.Contains(n)) continue;
                    else if (openList.Contains(n))
                    {
                        //If the node is in the open list, check if we got to the node faster. ie g(x) is smaller.
                        //Calculate g(x)
                        int new_g = currentNode.g + n.MoveCost;
                        if (new_g < n.g)
                        {
                            n.parent = currentNode; //Set the parent for future backtracking.
                            n.g = new_g; //computer and set new g value;
                            n.f = n.g + n.h;
                        }
                    }
                    else
                    {
                        if (n.Occupant != null && n.Occupant.getOwner() != GameState.Get().activePlayer && n != m_Tiles[start.X, start.Y]) 
                        {
                            closedList.Add(n);
                            continue;
                        }
                        
                        // System.Diagnostics.Debug.Print(currentNode.position.ToString());
                        n.parent = currentNode;
                        //System.Diagnostics.Debug.Print(n.parent.position.ToString());
                        n.g = currentNode.g + n.MoveCost;
                        //System.Diagnostics.Debug.Print(n.position.ToString());
                        n.h = Math.Abs(n.GridLocation.X - start.X) + Math.Abs(n.GridLocation.Y - start.Y);
                        n.f = n.g + n.h;
                        PQAdd(n, openList);
                    }

                }

                if (openList.Count == 0)
                {
                    //TODO--implement what if no path available?
                    return null;
                }

                currentNode = openList[0];
                openList.Remove(currentNode);
                closedList.Add(currentNode);
            }
            while (currentNode != m_Tiles[start.X, start.Y]);

            //Run a for loop to go through all the nodes you just calculated.

            List<Point> traverse = new List<Point>();
            Objects.Tile curTile = m_Tiles[start.X, start.Y];
            while (curTile != null)
            {
                //curTile.changeState(Tile.TileState.move);
                traverse.Add(curTile.GridLocation);
                curTile = curTile.parent;
            }
            
            return traverse;
        }

        #region Helper_Functions
        //A custom function to make the node list essentially a priority queue.
        private void PQAdd(Objects.Tile node, List<Objects.Tile> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (node.f < nodes[i].f)
                {
                    nodes.Insert(i, node);
                    return;
                }
            }
            nodes.Add(node);
        }

        private void PrintBoard()
        {
            for (int x = 0; x < m_Tiles.GetLength(0); x++)
            {
                String row = "";
                for (int y = 0; y < m_Tiles.GetLength(1); y++)
                {
                    String currentTile = "{";
                    currentTile += m_Tiles[x, y].g.ToString();
                    currentTile += ",";
                    currentTile += m_Tiles[x, y].h.ToString();
                    currentTile += ",";
                    currentTile += m_Tiles[x, y].f.ToString();
                    currentTile += "}";

                    row += currentTile;
                    row += " ";
                }
                System.Diagnostics.Debug.Print(row);
            }
            System.Diagnostics.Debug.Print("");
        }

        public void resetALLTiles(bool keepSelected)
        {
            foreach (Objects.Tile t in m_Tiles){
                if (t == null) continue;
                
                t.resetTile(keepSelected);
            }
        }

        public void resetTileType(Tile.TileState type)
        {
            foreach (Objects.Tile t in m_Tiles)
            {
                if (t == null) continue;

                if (t.tileState == type)
                {
                    t.resetTile(true);
                }
            }
        }
        #endregion

    }
}
