using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa;

namespace Voxa.Objects
{
    public class Camera : Component
    {
        public Vector3 Orientation = new Vector3((float)Math.PI, 0f, 0f);
        public float   MoveSpeed = 0.2f;
        public float   MouseSensitivity = 0.005f;

        public Camera()
        {
            
        }

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookAt = new Vector3();

            lookAt.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            lookAt.Y = (float)Math.Sin((float)Orientation.Y);
            lookAt.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));

            return Matrix4.LookAt(this.gameObject.Transform.Position, this.gameObject.Transform.Position + lookAt, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            Vector3 forward = new Vector3((float)Math.Sin((float)Orientation.X), 0, (float)Math.Cos((float)Orientation.X));
            Vector3 right = new Vector3(-forward.Z, 0, forward.X);

            offset += x * right;
            offset += y * forward;
            offset.Y += z;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, MoveSpeed);

            this.gameObject.Transform.Position += offset;
        }

        public void AddRotation(float x, float y)
        {
            x = x * MouseSensitivity;
            y = y * MouseSensitivity;

            Orientation.X = (Orientation.X + x) % ((float)Math.PI * 2.0f);
            Orientation.Y = Math.Max(Math.Min(Orientation.Y + y, (float)Math.PI / 2.0f - 0.1f), (float)-Math.PI / 2.0f + 0.1f);
        }
    }
}
