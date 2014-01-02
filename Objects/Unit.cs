using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380.Objects
{
    public abstract class Unit : GameObject
    {
        protected string m_name = "";

        protected int m_maxHP = 50;
        protected int m_baseAttackDamage = 20;
        protected int m_dexterity = 15;
        protected int m_defense = 20;

        protected int m_maxMovementDistance = 4;
        protected int m_range = 1;
        protected int m_inventorySize = 4;

        public enum WeaponType { none, staff, sword, spear, axe, bow };
        protected WeaponType m_weaponType = WeaponType.none;

        protected Player m_owner;
        protected List<Item> m_inventory;
        protected Objects.Items.Weapon m_equippedWeapon;
        protected Objects.Items.Armor m_equippedArmor;
        protected Point m_gridLoc;
        protected int m_HP;
        protected bool m_isKing;
        protected List<Texture2D> m_textures;
        protected List<Texture2D> m_walkAnim = new List<Texture2D>();
        protected List<Texture2D> m_attackAnim = new List<Texture2D>();
        protected List<Texture2D> m_idleAnim = new List<Texture2D>();

        public enum AnimState { none, walk, attack, selected, done, block, gotHit };
        AnimState animState = AnimState.none;

        protected string m_attackSound = "Sword";
        protected string m_dodgeSound = "Shield";
        protected string m_damageSound = "Hit";

        protected Texture2D m_defaultTexture;
        protected Texture2D m_selectedTexture;
        protected Texture2D m_grayTexture;
        protected Texture2D m_blockTexture;
        protected Texture2D m_hitTexture;

        protected Texture2D m_currentTexture;

        protected int animIndex = 0;

        private Vector3 unitForward;

        public Unit(Game game, Player owner, float z)
            : base(game)
        {
            m_gridLoc = new Point(-1, -1);
            m_inventory = new List<Item>(m_inventorySize);
            m_equippedWeapon = null;
            m_equippedArmor = null;
            m_owner = owner;
            m_isKing = false;
            m_performedActionThisTurn = false;
            m_movedThisTurn = false;
            m_z = z;
            m_textures = new List<Texture2D>();
           
            m_ModelName = "Models/UnitCube";

            try
            {
                CreateWalkAnim();
                CreateIdleAnim();
                CreateAttackAnim();
                CreateDefaultTexture();
            }
            catch { }
        }

        public virtual void CreateDefaultTexture()
        {
            if (m_owner == null) return;
            if (m_Game == null) return;
            if (m_defaultTexture == null) return;
            if (m_grayTexture == null) return;
        }

        public override void Load()
        {
            base.Load();
        }

        public abstract void CreateWalkAnim();
        public abstract void CreateIdleAnim();
        public abstract void CreateAttackAnim();

        public void InitializeUnit()
        {
            m_HP = m_maxHP;

            Vector3 defaultDirection = GameState.Get().Camera.Position - Position;
            defaultDirection.Normalize();
            unitForward = defaultDirection;
        }

        private float m_z = 0;
        public float z
        {
            get { return m_z; }
            set { m_z = value; }
        }

        public Player getOwner()
        {
            return m_owner;
        }

        public bool isKing
        {
            get { return m_isKing; }
        }
        
        public List<Item> getInventory()
        {
            return m_inventory;
        }

        public Items.Weapon equippedWeapon
        {
            get { return m_equippedWeapon; }
            set { m_equippedWeapon = value; }
        }

        public Items.Armor equippedArmor
        {
            get { return m_equippedArmor; }
            set { m_equippedArmor = value; }
        }
         
        public Point gridLocation
        {
            get { return m_gridLoc; }
            set { m_gridLoc = value; }
        }

        public int maxHP
        {
            get { return m_maxHP; }
        }

        public int HP
        {
            get { return m_HP; }
            set { m_HP = value; }
        }

        public int moveDistance
        {
            get { return m_maxMovementDistance; }
            set { m_maxMovementDistance = value; }
        }

        public string getName()
        {
            return m_name;
        }

        public int baseAttackDamage
        {
            get { return m_baseAttackDamage; }
        }

        public int dexterity
        {
            get { return m_dexterity; }
        }

        public int defense
        {
            get { return m_defense; }
        }

        public int range
        {
            get { return m_range; }
        }

        public int inventorySize
        {
            get { return m_inventorySize; }
        }

        public WeaponType weaponType
        {
            get { return m_weaponType; }
        }

        private bool m_performedActionThisTurn;
        public bool performedActionThisTurn
        {
            get { return m_performedActionThisTurn; }
            set { m_performedActionThisTurn = value; }
        }

        private bool m_movedThisTurn;
        public bool movedThisTurn
        {
            get { return m_movedThisTurn; }
            set { m_movedThisTurn = value; }
        }

        // Code for moving a Unit to a new GridLocation
        #region A* movement stuff

        private bool m_moving = false;
        public bool isMoving()
        {
            return m_moving;
        }

        private static float m_animationSpeed = 2.6f;
        private float m_elapsedTime = 0.0f;
        private float m_travelTime = 0.0f;
        private Vector3 m_startingPosition = new Vector3();
        private List<Point> m_path;
        private int m_currentTarget = 0;

        private Vector3 m_targetWorldSpace = new Vector3();
        private Point m_target = new Point();

        protected virtual void setNextMovementTarget()
        {  
            m_target = m_path[m_currentTarget];
            //m_targetWorldSpace = GameState.Get().toWorldSpace(m_target, z);
            m_targetWorldSpace = new Vector3(m_target.X - 8, m_target.Y - 4, z);

            m_startingPosition = Position;
            m_travelTime = Vector3.Distance(m_startingPosition, m_targetWorldSpace) / m_animationSpeed;
            m_elapsedTime = 0.0f;

            // rotation stuff, fix later
            float relativeY = m_targetWorldSpace.Y - Position.Y;
            float angleOfRotation = (float)Math.Acos(relativeY /
                Vector3.Distance(Vector3.Zero, m_targetWorldSpace - Position));

            //Rotation = Quaternion.CreateFromAxisAngle(-Vector3.UnitZ, angleOfRotation);

            m_currentTarget++;
        }

        // Default function for moving a Unit to a new location on the grid given an A* path
        public void moveTo(List<Point> path)
        {
            if (path.Count == 0) return;

            //animation here ##
            animate(AnimState.walk, true);

            m_path = path;
            m_currentTarget = 0;
            setNextMovementTarget();
            m_gridLoc = m_path[m_path.Count - 1];
            m_moving = true;
        }
        #endregion

        private Quaternion GetRotation(Vector3 source, Vector3 dest, Vector3 up)
        {
            float dot = Vector3.Dot(source, dest);

            if (Math.Abs(dot - (-1.0f)) < 0.000001f)
            {
                // vector a and b point exactly in the opposite direction, 
                // so it is a 180 degrees turn around the up-axis
                return new Quaternion(up, MathHelper.ToRadians(180.0f));
            }
            if (Math.Abs(dot - (1.0f)) < 0.000001f)
            {
                // vector a and b point exactly in the same direction
                // so we return the identity quaternion
                return Quaternion.Identity;
            }

            float rotAngle = (float)Math.Acos(dot);
            Vector3 rotAxis = Vector3.Cross(source, dest);
            rotAxis = Vector3.Normalize(rotAxis);
            return Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
        }

        
        public override void Update(float fDeltaTime)
        {
            base.Update(fDeltaTime);

            Camera camera = GameState.Get().Camera;

            Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, camera.Yaw);
            Quaternion newRot = Quaternion.CreateFromYawPitchRoll(0, camera.Pitch + MathHelper.PiOver2, 0);
            //newRot *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, camera.Yaw);
            Rotation = Quaternion.Multiply(Rotation, newRot);

            if (m_moving)
            {
                float lerpFactor = Math.Min(1, m_elapsedTime / m_travelTime);
                Position = Vector3.Lerp(m_startingPosition, m_targetWorldSpace, lerpFactor);
                m_elapsedTime += fDeltaTime;

                if (lerpFactor == 1)
                {
                    if (m_currentTarget < m_path.Count)
                    {
                        setNextMovementTarget();
                    }
                    else
                    {
                        m_moving = false;
                        //stop walk animation here ##
                        animate(AnimState.walk, false);
                        GameState.Get().UnitDoneMoving();
                    }
                }
            }
        }

        public override void Draw(float fDeltaTime)
        {
            foreach (ModelMesh mesh in m_Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (animState == AnimState.none)
                        effect.Texture = m_defaultTexture;
                    if (animState == AnimState.walk && m_walkAnim.Count > 0)
                        effect.Texture = m_walkAnim[animIndex];
                    if (animState == AnimState.attack)
                        effect.Texture = m_attackAnim[animIndex];
                    if (animState == AnimState.block)
                        effect.Texture = m_blockTexture;
                    if (animState == AnimState.gotHit)
                        effect.Texture = m_hitTexture;
                    if (animState == AnimState.done)
                        effect.Texture = m_grayTexture;   
                }
            }

            base.Draw(fDeltaTime);
        }

        private AnimState tempState;

        public void hitOrMiss(bool hitTrue)
        {
             tempState = animState;

            if (hitTrue)
            {
                animState = AnimState.gotHit;
                SoundManager.Get().PlaySoundCue(m_damageSound);
            }
            else
            {
                animState = AnimState.block;
                SoundManager.Get().PlaySoundCue(m_dodgeSound);
            }
            m_Timer.AddTimer("revert state", 1.0f, revert, false);

        }

        private void revert()
        {
            animState = tempState;
        }

        public void animate(AnimState animst, bool run = true)
        {
            animState = animst;
            //Console.WriteLine((run ? "Start " : "End ") + animState.ToString());
            if (animst == AnimState.none || animst == AnimState.done)
            {
                return;
            }

            if (run)
            {
                if (animst == AnimState.attack)
                {
                    SoundManager.Get().PlaySoundCue(m_attackSound);
                }
                m_Timer.AddTimer("t_anim", 0.2f, animate, true);
            }
            else
            {
                animState = AnimState.none;
                animIndex = 0;
                m_Timer.RemoveTimer("t_anim");
                GameState.Get().CheckToUpdateActiveUnit();
            }   
        }

        public AnimState getAnimState()
        {
            return animState;
        }

        public void grayOut()
        {

        }

        private void animate()
        {
            switch (animState)
            {
                case AnimState.walk: 
                    if (animIndex < m_walkAnim.Count)
                        animIndex++;
                    if (animIndex >= m_walkAnim.Count)
                        animIndex = 0;
                    break;
                case AnimState.attack:
                    if (animIndex < m_attackAnim.Count)
                        animIndex++;
                    if (animIndex >= m_attackAnim.Count)
                    {
                        animIndex = 0;
                        animate(AnimState.attack, false);
                    }
                    break;
                case AnimState.none:
                    animIndex = 0;
                    break;
                default: break;
            }
 
        }
    }
}