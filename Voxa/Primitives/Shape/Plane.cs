using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using Voxa.Objects;
using Voxa.Rendering;

namespace Voxa.Primitives.Shape
{
    public class Plane : StaticModel
    {
        public Plane()
        {
            this.generateVertices(1, 1);
        }

        public Plane(float width, float height)
        {
            this.generateVertices(width, height);
        }

        private void generateVertices(float width, float height)
        {
            this.Meshes = new Mesh[1];
            this.Materials = new Material[1];

            List<TexturedVertex> verticesList = new List<TexturedVertex>();

            verticesList.Add(new TexturedVertex(new Vector3(width, 0, -height), Color4.White, new Vector3(0, 1, 0))); // top right
            verticesList.Add(new TexturedVertex(new Vector3(-width, 0, -height), Color4.White, new Vector3(0, 1, 0))); // top left
            verticesList.Add(new TexturedVertex(new Vector3(-width, 0, height), Color4.White, new Vector3(0, 1, 0))); // bottom left
            verticesList.Add(new TexturedVertex(new Vector3(width, 0, height), Color4.White, new Vector3(0, 1, 0))); // bottom right

            List<ushort> indices = new List<ushort>(){
                0, 1, 2, 0, 2, 3
            };

            Mesh.Primitive shapePrimitive = new Mesh.Primitive(verticesList, indices);
            this.Meshes[0] = new Mesh(shapePrimitive);
            this.Materials[0] = new Material(0, Color4.White, Color4.White, 16);
        }
    }
}
