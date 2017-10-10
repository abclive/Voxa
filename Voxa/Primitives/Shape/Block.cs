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
    public class Block : StaticModel
    {
        public Block(float initialSize = 1)
        {
            this.generateVertices(initialSize);
        }

        private void generateVertices(float initialSize)
        {
            this.Meshes = new Mesh[1];
            this.Materials = new Material[1];

            List<TexturedVertex> verticesList = new List<TexturedVertex>();

            // Top
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, initialSize, -initialSize), Color4.Green, new Vector3(0, 1, 0))); // top right
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, initialSize, -initialSize), Color4.Green, new Vector3(0, 1, 0))); // top left
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, initialSize, initialSize), Color4.Green, new Vector3(0, 1, 0))); // bottom left
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, initialSize, initialSize), Color4.Green, new Vector3(0, 1, 0))); // bottom right

            // Bottom
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, -initialSize, -initialSize), Color4.Orange, new Vector3(0, -1, 0))); // top left
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, -initialSize, initialSize), Color4.Orange, new Vector3(0, -1, 0))); // bottom left
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, -initialSize, initialSize), Color4.Orange, new Vector3(0, -1, 0))); // bottom right
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, -initialSize, -initialSize), Color4.Orange, new Vector3(0, -1, 0))); // top right

            // Front
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, initialSize, initialSize), Color4.Red, new Vector3(0, 0, 1))); // top right
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, initialSize, initialSize), Color4.Red, new Vector3(0, 0, 1))); // top left
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, -initialSize, initialSize), Color4.Red, new Vector3(0, 0, 1))); // bottom left
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, -initialSize, initialSize), Color4.Red, new Vector3(0, 0, 1))); // bottom right

            // Back
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, initialSize, -initialSize), Color4.Yellow, new Vector3(0, 0, -1))); // top right
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, initialSize, -initialSize), Color4.Yellow, new Vector3(0, 0, -1))); // top left
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, -initialSize, -initialSize), Color4.Yellow, new Vector3(0, 0, -1))); // bottom left
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, -initialSize, -initialSize), Color4.Yellow, new Vector3(0, 0, -1))); // bottom right

            // Left
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, initialSize, initialSize), Color4.Blue, new Vector3(-1, 0, 0))); // top right
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, initialSize, -initialSize), Color4.Blue, new Vector3(-1, 0, 0))); // top left
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, -initialSize, -initialSize), Color4.Blue, new Vector3(-1, 0, 0))); // bottom left
            verticesList.Add(new TexturedVertex(new Vector3(-initialSize, -initialSize, initialSize), Color4.Blue, new Vector3(-1, 0, 0))); // bottom right

            // Right
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, initialSize, initialSize), Color4.Magenta, new Vector3(1, 0, 0))); // top right
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, initialSize, -initialSize), Color4.Magenta, new Vector3(1, 0, 0))); // top left
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, -initialSize, -initialSize), Color4.Magenta, new Vector3(1, 0, 0))); // bottom left
            verticesList.Add(new TexturedVertex(new Vector3(initialSize, -initialSize, initialSize), Color4.Magenta, new Vector3(1, 0, 0))); // bottom right

            List<ushort> indices = new List<ushort>(){
                0, 1, 2, 0, 2, 3,       // top
                4, 5, 6, 4, 6, 7,       // bottom
                8, 9, 10, 8, 10, 11,    // front
                12, 13, 14, 12, 14, 15, // back
                16, 17, 18, 16, 18, 19, // left
                20, 21, 22, 20, 22, 23  // right
            };

            Mesh.Primitive shapePrimitive = new Mesh.Primitive(verticesList, indices);
            this.Meshes[0] = new Mesh(shapePrimitive);
            this.Materials[0] = new Material(0, Color4.Red, new Color4(255, 100, 100, 255), 2);
        }
    }
}
