﻿//-----------------------------------------------------------------------------
// Base GameObject class that every other class in the Objects namespace
// inherits from. It's both drawable and updatable.
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
	public class GameObject
	{
		protected Game m_Game;
		public Model m_Model;
		protected Matrix[] m_ModelBones;
		protected string m_ModelName;

		protected Matrix m_WorldTransform = Matrix.Identity;
		protected bool m_bTransformDirty = false;

        protected BoundingBox m_ModelBounds;
        protected BoundingBox m_WorldBounds;
        public BoundingBox WorldBounds
        {
            get { return m_WorldBounds; }
            set { m_WorldBounds = value; }
        }

		protected eDrawOrder m_DrawOrder = eDrawOrder.Default;
		public eDrawOrder DrawOrder
		{
			get { return m_DrawOrder; }
            set { m_DrawOrder = value; }
		}

		// Anything that's timer logic is assumed to be affected by time factor
		protected Utils.Timer m_Timer = new Utils.Timer();

        protected Quaternion m_Rotation = Quaternion.Identity;
        public Quaternion Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; m_bTransformDirty = true; }
        }

		Vector3 m_vPos = Vector3.Zero;
		public Vector3 Position
		{
			get { return m_vPos; }
			set { m_vPos = value; m_bTransformDirty = true; }
		}

		float m_fScale = 1.0f;
		public float Scale
		{
			get { return m_fScale; }
			set { m_fScale = value; m_bTransformDirty = true; }
		}

		public void RebuildWorldTransform()
		{
            m_WorldBounds = m_ModelBounds;
            CalculateWorldBounds();  
            
            m_bTransformDirty = false;
			// Scale, rotation, translation
			m_WorldTransform = Matrix.CreateScale(m_fScale) *
                Matrix.CreateFromQuaternion(m_Rotation) * Matrix.CreateTranslation(m_vPos);
		}

        private void CalculateWorldBounds()
        {
            Vector3 max = TransformPoint(m_WorldBounds.Max);
            Vector3 min = TransformPoint(m_WorldBounds.Min);

            if (max.X < min.X)
            {
                float temp = max.X;
                max.X = min.X;
                min.X = temp;
            }
            if (max.Y < min.Y)
            {
                float temp = max.Y;
                max.Y = min.Y;
                min.Y = temp;
            }
            if (max.Z < min.Z)
            {
                float temp = max.Z;
                max.Z = min.Z;
                min.Z = temp;
            }

            m_WorldBounds.Max = max;
            m_WorldBounds.Min = min;
        }

        public Vector3 TransformPoint(Vector3 p)
        {
            Matrix transform = Matrix.CreateScale(m_fScale) *
                Matrix.CreateFromQuaternion(m_Rotation) * Matrix.CreateTranslation(m_vPos);

            Vector3 rp = Vector3.Transform(p, transform);
            //System.Diagnostics.Debug.Print("Point " + p.ToString() + " transformed to " + rp.ToString());
            return rp;
        }

		public bool m_bEnabled = true;
		public bool Enabled
		{
			get { return m_bEnabled; }
			set { m_bEnabled = value; }
		}

		public GameObject(Game game)
		{
			m_Game = game;
		}

		public virtual void Load()
		{
			if (m_ModelName != "")
			{
				m_Model = m_Game.Content.Load<Model>(m_ModelName);
				m_ModelBones = new Matrix[m_Model.Bones.Count];
				m_Model.CopyAbsoluteBoneTransformsTo(m_ModelBones);
			}

            m_ModelBounds = PhysicsManager.Get().GetBoundingBox(m_ModelName);
            
            RebuildWorldTransform();
		}

		public virtual void Unload()
		{

		}

		public virtual void Update(float fDeltaTime)
		{
			if (m_bTransformDirty)
			{
				RebuildWorldTransform();
			}

			m_Timer.Update(fDeltaTime);
		}

		public virtual void Draw(float fDeltaTime)
		{
			if (m_bTransformDirty)
			{
				RebuildWorldTransform();
			}

			foreach (ModelMesh mesh in m_Model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = m_ModelBones[mesh.ParentBone.Index] * m_WorldTransform;
					effect.View = GameState.Get().CameraMatrix;
					effect.Projection = GraphicsManager.Get().Projection;
					effect.EnableDefaultLighting();
					effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
					effect.DirectionalLight0.Enabled = false;
					effect.DirectionalLight1.Enabled = false;
					effect.DirectionalLight2.Enabled = false;
					effect.PreferPerPixelLighting = true;
				}
				mesh.Draw();
			}
		}
	}
}
