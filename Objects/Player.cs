using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace itp380.Objects
{
    public class Player
    {
        public Player(int ID, int maxUnits = 5)
        {
            m_playerID = ID;
            m_maxUnits = maxUnits;
            m_units = new List<Unit>(m_maxUnits);
            m_activeUnit = null;
            m_dead = false;
        }

        private bool m_dead;
        public bool dead
        {
            get { return m_dead; }
            set { m_dead = value; }
        }

        private List<Unit> m_units;
        public List<Unit> units
        {
            get { return m_units; }
        }

        private Unit m_activeUnit;
        public Unit activeUnit
        {
            get { return m_activeUnit; }
            set { m_activeUnit = value; }
        }

        private int m_playerID;
        public int playerID
        {
            get { return m_playerID; }
        }

        private int m_maxUnits;
        public int maxUnits
        {
            get { return m_maxUnits; }
        }

        public int activeUnits
        {
            get { return m_units.Count; }
        }

        private List<Point> m_currentPossibleMoves = new List<Point>();
        public List<Point> currentPossibleMoves
        {
            get { return m_currentPossibleMoves; }
            set { m_currentPossibleMoves = value; }
        }

        private List<Point> m_currentPossibleAttacks = new List<Point>();
        public List<Point> currentPossibleAttacks
        {
            get { return m_currentPossibleAttacks; }
            set { m_currentPossibleAttacks = value; }
        }

        public bool hasKing()
        {
            foreach (Unit u in m_units)
            {
                if (u.isKing) return true;
            }
            return false;
        }
    }
}
