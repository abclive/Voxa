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
            public Texture PrimitiveTexture;
            public Guid GUID;

            public Primitive(List<TexturedVertex> vertices, List<ushort> indices)
            {
                this.Vertices = vertices;
                this.Indices = indices;
                this.PrimitiveTexture = null;
                this.GUID = Guid.NewGuid();
            }

            public Primitive(List<TexturedVertex> vertices, List<ushort> indices, Texture primitiveTexture)
            {
                this.Vertices = vertices;
                this.Indices = indices;
                this.PrimitiveTexture = primitiveTexture;
                this.GUID = Guid.NewGuid();
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
