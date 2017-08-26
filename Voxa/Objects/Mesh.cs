using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Voxa.Rendering;

namespace Voxa.Objects
{
    class Mesh
    {
        public struct Primitive
        {
            public List<TexturedVertex> Vertices;
            public List<ushort> Indices;
            public Guid GUID;
            public int MaterialModelId;

            public Primitive(List<TexturedVertex> vertices, List<ushort> indices, int materialModelId = 0)
            {
                this.Vertices = vertices;
                this.Indices = indices;
                this.GUID = Guid.NewGuid();
                this.MaterialModelId = materialModelId;
            }
        }

        public Primitive[] Primitives;
        public Matrix4 LocalMatrix  = Matrix4.Identity;
        public string Name;

        public Mesh(Primitive primitive)
        {
            this.Primitives = new Primitive[1];
            this.Primitives[0] = primitive;
        }

        public Mesh(Primitive[] primitives)
        {
            this.Primitives = primitives;
        }
    }
}
