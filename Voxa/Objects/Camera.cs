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
        public float MoveSpeed = 0.2f;
        public float MouseSensitivity = 0.005f;

        public Vector3 UpVector { get { return Vector3.Cross(Vector3.Cross(this.ForwardVector, new Vector3(0, 1, 0)), this.ForwardVector).Normalized(); } }
        public Vector3 RightVector { get { return new Vector3(-this.ForwardVector.Z, 0, this.ForwardVector.X).Normalized(); } }
        public Vector3 ForwardVector { get; private set; }

        public Vector3 LookAt;
        private Vector3 lookAt;

        public Camera()
        {
        }

        public override void OnUpdate()
        {
            if (this.LookAt == Vector3.Zero) {
                this.lookAt = new Vector3();

                this.lookAt.X = (float)(Math.Sin((float)Orientation.X) * Math.Cos((float)Orientation.Y));
                this.lookAt.Y = (float)Math.Sin((float)Orientation.Y);
                this.lookAt.Z = (float)(Math.Cos((float)Orientation.X) * Math.Cos((float)Orientation.Y));
            } else if (this.lookAt != this.LookAt) {
                this.lookAt = LookAt;
            }
            this.ForwardVector = -(this.gameObject.Transform.Position - this.lookAt).Normalized();
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(this.gameObject.Transform.Position, this.gameObject.Transform.Position + this.lookAt, Vector3.UnitY);
        }

        public void Move(float x, float y, float z)
        {
            Vector3 offset = new Vector3();

            offset += x * this.RightVector;
            offset += y * this.ForwardVector;
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
