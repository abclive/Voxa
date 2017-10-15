using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Voxa.Objects
{
    public class OrthographicCamera : Component
    {
        public float Width;
        public float Height;
        public float ZNear;
        public float ZFar;

        public OrthographicCamera()
        {
        }

        public OrthographicCamera(float width, float height, float zNear, float zFar)
        {
            this.Width = width;
            this.Height = height;
            this.ZNear = zNear;
            this.ZFar = zFar;
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreateOrthographic(this.Width, this.Height, this.ZNear, this.ZFar);
        }
    }
}
