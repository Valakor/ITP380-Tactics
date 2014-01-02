//-----------------------------------------------------------------------------
// Camera Singleton that for now, doesn't do much.
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

namespace itp380
{
	public class Camera
	{
        private const float CAMERA_SPEED = 5.0f;
        private const float ROT_SPEED = 2.0f;
        private const float LERP_FACTOR = 6.0f;

        private const float MIN_PITCH = -MathHelper.Pi / 2.5f;
        private const float MAX_PITCH = -MathHelper.Pi / 8;

        private const float X_CLAMP = 7;
        private const float Y_CLAMP = 7;
        private Vector2 constrainOffset = new Vector2(-4, 3);

        private const float MIN_ZOOM = 1.0f;
        private const float MAX_ZOOM = 8.0f;

        private Game m_Game;
        private bool m_bTransformDirty = false;

        private Vector3 m_fPosition;
        public Vector3 Position
        {
            get
            {
                //if (m_bTransformDirty)
                    //ComputeMatrix();

                return m_fPosition;
            }
            set { m_fPosition = value; m_bTransformDirty = true; }
        }

        private Vector3 m_fTarget;
        public Vector3 Target
        {
            get { return m_fTarget; }
            set { m_fTarget = value; m_bTransformDirty = true; }
        }

        private float m_fYaw = 0.0f;
        public float Yaw
        {
            get { return m_fYaw; }
            set { m_bTransformDirty = true; m_fYaw = value; }
        }

        private float m_fPitch = MIN_PITCH;
        public float Pitch
        {
            get { return m_fPitch; }
            set { m_bTransformDirty = true; m_fPitch = MathHelper.Clamp(value, MIN_PITCH, MAX_PITCH); }
        }

        private float m_fZoom = MAX_ZOOM;
        public float Zoom
        {
            get { return m_fZoom; }
            set { m_bTransformDirty = true; m_fZoom = MathHelper.Clamp(value, MIN_ZOOM, MAX_ZOOM); }
        }

		private Matrix m_Camera;
		public Matrix CameraMatrix
		{
			get
            {
                //if (m_bTransformDirty)
                    //ComputeMatrix();

                return m_Camera;
            }
		}

		public Camera(Game game)
		{
            m_Game = game;
            Target = new Vector3(0, 0, 0.5f); 
		    //ComputeMatrix();
		}

		public void Update(float fDeltaTime)
		{

            ComputeMatrix(fDeltaTime);
		}

		void ComputeMatrix(float fDeltaTime)
		{
            Vector3 newPosition = -Vector3.UnitY;

            newPosition *= m_fZoom;
            newPosition = Vector3.Transform(newPosition, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, m_fYaw));
            Vector3 vLeft = Vector3.Transform(Vector3.UnitX, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, m_fYaw));
            newPosition = Vector3.Transform(newPosition, Quaternion.CreateFromAxisAngle(vLeft, m_fPitch));
            newPosition += Target;

            Position = Vector3.Lerp(Position, newPosition, fDeltaTime * LERP_FACTOR);

            m_Camera = Matrix.CreateLookAt(Position, Target, Vector3.UnitZ);

            m_bTransformDirty = false;
		}

        void PanLeft(float fDeltaTime)
        {
            Vector3 vLeft = Vector3.Transform(Vector3.UnitX, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, m_fYaw));
            Target -= vLeft * fDeltaTime * CAMERA_SPEED;
            if ((Target.X < -X_CLAMP + constrainOffset.X || X_CLAMP + constrainOffset.X < Target.X) || (Target.Y < -Y_CLAMP + constrainOffset.Y || Y_CLAMP + constrainOffset.Y < Target.Y))
            Target = new Vector3(MathHelper.Clamp(Target.X, -X_CLAMP + constrainOffset.X, X_CLAMP + constrainOffset.X), MathHelper.Clamp(Target.Y, -Y_CLAMP + constrainOffset.Y, Y_CLAMP + constrainOffset.Y), 0);
  
        }

        void PanRight(float fDeltaTime)
        {
            Vector3 vLeft = Vector3.Transform(Vector3.UnitX, Quaternion.CreateFromAxisAngle(Vector3.UnitZ, m_fYaw));
            Target += vLeft * fDeltaTime * CAMERA_SPEED;
            if ((Target.X < -X_CLAMP + constrainOffset.X || X_CLAMP + constrainOffset.X < Target.X) || (Target.Y < -Y_CLAMP + constrainOffset.Y || Y_CLAMP + constrainOffset.Y < Target.Y))
            Target = new Vector3(MathHelper.Clamp(Target.X, -X_CLAMP + constrainOffset.X, X_CLAMP + constrainOffset.X), MathHelper.Clamp(Target.Y, -Y_CLAMP + constrainOffset.Y, Y_CLAMP + constrainOffset.Y), 0);
        }

        void PanForward(float fDeltaTime)
        {
            Vector3 vForward = Vector3.Transform(Vector3.UnitY, Matrix.CreateFromYawPitchRoll(0, m_fPitch, m_fYaw));
            vForward.Z = 0;
            vForward = Vector3.Normalize(vForward);
            Target += vForward * fDeltaTime * CAMERA_SPEED;
            if ((Target.X < -X_CLAMP + constrainOffset.X || X_CLAMP + constrainOffset.X < Target.X) || (Target.Y < -Y_CLAMP + constrainOffset.Y || Y_CLAMP + constrainOffset.Y < Target.Y))
            Target = new Vector3(MathHelper.Clamp(Target.X, -X_CLAMP + constrainOffset.X, X_CLAMP + constrainOffset.X), MathHelper.Clamp(Target.Y, -Y_CLAMP+constrainOffset.Y, Y_CLAMP + constrainOffset.Y), 0);
        }

        void PanBackward(float fDeltaTime)
        {
            Vector3 vForward = Vector3.Transform(Vector3.UnitY, Matrix.CreateFromYawPitchRoll(0, m_fPitch, m_fYaw));
            vForward.Z = 0;
            vForward = Vector3.Normalize(vForward);
            Target -= vForward * fDeltaTime * CAMERA_SPEED;
            if ((Target.X < -X_CLAMP + constrainOffset.X || X_CLAMP + constrainOffset.X < Target.X) || (Target.Y < -Y_CLAMP + constrainOffset.Y || Y_CLAMP + constrainOffset.Y < Target.Y))
            Target = new Vector3(MathHelper.Clamp(Target.X, -X_CLAMP + constrainOffset.X, X_CLAMP + constrainOffset.X), MathHelper.Clamp(Target.Y, -Y_CLAMP + constrainOffset.Y, Y_CLAMP + constrainOffset.Y), 0);
        }

        void RotateUp(float fDeltaTime)
        {
            Pitch -= fDeltaTime * ROT_SPEED;
        }

        void RotateDown(float fDeltaTime)
        {
            Pitch += fDeltaTime * ROT_SPEED;
        }

        void RotateLeft(float fDeltaTime)
        {
            Yaw -= fDeltaTime * ROT_SPEED;
        }

        void RotateRight(float fDeltaTime)
        {
            Yaw += fDeltaTime * ROT_SPEED;
        }

        public void ZoomIn(float fDeltaTime)
        {
            Zoom += fDeltaTime * CAMERA_SPEED;
        }

        public void ZoomOut(float fDeltaTime)
        {
            Zoom -= fDeltaTime * CAMERA_SPEED;
        }


        public void KeyboardInput(SortedList<eBindings, BindInfo> binds, float fDeltaTime)
        {
            if (!binds.ContainsKey(eBindings.CAMERA_ROTATE))
            {
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_FORWARD))
                {
                    PanForward(fDeltaTime);
                }
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_BACKWARD))
                {
                    PanBackward(fDeltaTime);
                }
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_RIGHT))
                {
                    PanRight(fDeltaTime);
                }
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_LEFT))
                {
                    PanLeft(fDeltaTime);
                }

                if (binds.ContainsKey(eBindings.CAMERA_ZOOM_IN))
                {
                    ZoomIn(fDeltaTime);
                }

                if (binds.ContainsKey(eBindings.CAMERA_ZOOM_OUT))
                {
                    ZoomOut(fDeltaTime);
                }
            }
            else
            {
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_FORWARD))
                {
                    RotateUp(fDeltaTime);
                }
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_BACKWARD))
                {
                    RotateDown(fDeltaTime);
                }
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_RIGHT))
                {
                    RotateRight(fDeltaTime);
                }
                if (binds.ContainsKey(eBindings.CAMERA_CONTROL_LEFT))
                {
                    RotateLeft(fDeltaTime);
                }
            }
   
        }
    }
}

