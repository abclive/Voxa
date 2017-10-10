using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxa.Objects
{
    public class StaticModel : Component
    {
        public Mesh[] Meshes = new Mesh[0];
        public Material[] Materials = new Material[0];

        public StaticModel(Mesh[] meshes)
        {
            this.Meshes = meshes;
        }

        public StaticModel(Mesh[] meshes, Material[] materials)
        {
            this.Meshes = meshes;
            this.Materials = materials;
        }

        public StaticModel()
        {}

        public int GetPrimitivesCount()
        {
            int primitiveTotal = 0;
            foreach (Mesh mesh in this.Meshes) {
                primitiveTotal += mesh.Primitives.Length;
            }
            return primitiveTotal;
        }
    }
}