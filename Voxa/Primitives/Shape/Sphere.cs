using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using Voxa.Objects;
using Voxa.Rendering;
using Voxa.Utils;

namespace Voxa.Primitives.Shape
{
    public class Sphere : StaticModel
    {
        private struct Face
        {
            public Vector3 V1;
            public Vector3 V2;
            public Vector3 V3;

            public Face(Vector3 v1, Vector3 v2, Vector3 v3)
            {
                V1 = v1;
                V2 = v2;
                V3 = v3;
            }
        }

        private List<Vector3> points;
        private int index;
        private Dictionary<long, int> middlePointIndexCache;

        public Sphere()
        {
            this.generateVertices(1, false);
        }

        public Sphere(int recursionLevel, bool perFaceLighting = false)
        {
            this.generateVertices(recursionLevel, perFaceLighting);
        }

        private void generateVertices(int recursionLevel, bool perFaceLighting)
        {
            this.Meshes = new Mesh[1];
            this.Materials = new Material[1];
            List<TexturedVertex> verticesList = new List<TexturedVertex>();

            this.middlePointIndexCache = new Dictionary<long, int>();
            this.points = new List<Vector3>();
            this.index = 0;
            float t = (float)((1.0 + Math.Sqrt(5.0)) / 2.0);
            float s = 1;
            List<Face> faces = new List<Face>();

            this.addVertex(new Vector3(-s, t, 0));
            this.addVertex(new Vector3(s, t, 0));
            this.addVertex(new Vector3(-s, -t, 0));
            this.addVertex(new Vector3(s, -t, 0));

            this.addVertex(new Vector3(0, -s, t));
            this.addVertex(new Vector3(0, s, t));
            this.addVertex(new Vector3(0, -s, -t));
            this.addVertex(new Vector3(0, s, -t));

            this.addVertex(new Vector3(t, 0, -s));
            this.addVertex(new Vector3(t, 0, s));
            this.addVertex(new Vector3(-t, 0, -s));
            this.addVertex(new Vector3(-t, 0, s));

            // 5 faces around point 0
            faces.Add(new Face(this.points[0], this.points[11], this.points[5]));
            faces.Add(new Face(this.points[0], this.points[5], this.points[1]));
            faces.Add(new Face(this.points[0], this.points[1], this.points[7]));
            faces.Add(new Face(this.points[0], this.points[7], this.points[10]));
            faces.Add(new Face(this.points[0], this.points[10], this.points[11]));

            // 5 adjacent faces 
            faces.Add(new Face(this.points[1], this.points[5], this.points[9]));
            faces.Add(new Face(this.points[5], this.points[11], this.points[4]));
            faces.Add(new Face(this.points[11], this.points[10], this.points[2]));
            faces.Add(new Face(this.points[10], this.points[7], this.points[6]));
            faces.Add(new Face(this.points[7], this.points[1], this.points[8]));

            // 5 faces around point 3
            faces.Add(new Face(this.points[3], this.points[9], this.points[4]));
            faces.Add(new Face(this.points[3], this.points[4], this.points[2]));
            faces.Add(new Face(this.points[3], this.points[2], this.points[6]));
            faces.Add(new Face(this.points[3], this.points[6], this.points[8]));
            faces.Add(new Face(this.points[3], this.points[8], this.points[9]));

            // 5 adjacent faces 
            faces.Add(new Face(this.points[4], this.points[9], this.points[5]));
            faces.Add(new Face(this.points[2], this.points[4], this.points[11]));
            faces.Add(new Face(this.points[6], this.points[2], this.points[10]));
            faces.Add(new Face(this.points[8], this.points[6], this.points[7]));
            faces.Add(new Face(this.points[9], this.points[8], this.points[1]));

            // refine triangles
            for (int i = 0; i < recursionLevel; i++) {
                var faces2 = new List<Face>();
                foreach (Face tri in faces) {
                    // replace triangle by 4 triangles
                    int a = this.getMiddlePoint(tri.V1, tri.V2);
                    int b = this.getMiddlePoint(tri.V2, tri.V3);
                    int c = this.getMiddlePoint(tri.V3, tri.V1);

                    faces2.Add(new Face(tri.V1, this.points[a], this.points[c]));
                    faces2.Add(new Face(tri.V2, this.points[b], this.points[a]));
                    faces2.Add(new Face(tri.V3, this.points[c], this.points[b]));
                    faces2.Add(new Face(this.points[a], this.points[b], this.points[c]));
                }
                faces = faces2;
            }

            foreach (Face tri in faces) {
                Vector2 uv1 = tri.V1.GetSphereCoord();
                Vector2 uv2 = tri.V2.GetSphereCoord();
                Vector2 uv3 = tri.V3.GetSphereCoord();
                this.fixColorStrip(ref uv1, ref uv2, ref uv3);

                if (perFaceLighting) {
                    Vector3 dir = Vector3.Cross(tri.V2 - tri.V1, tri.V3 - tri.V1);
                    Vector3 normal = Vector3.Normalize(dir);

                    verticesList.Add(new TexturedVertex(tri.V1, uv1, Color4.White, normal));
                    verticesList.Add(new TexturedVertex(tri.V2, uv2, Color4.White, normal));
                    verticesList.Add(new TexturedVertex(tri.V3, uv3, Color4.White, normal));
                } else {
                    Vector3 normalV1 = tri.V1.Normalized();
                    normalV1 = Vector3.Divide(normalV1, normalV1.Length);
                    Vector3 normalV2 = tri.V2.Normalized();
                    normalV2 = Vector3.Divide(normalV2, normalV2.Length);
                    Vector3 normalV3 = tri.V3.Normalized();
                    normalV3 = Vector3.Divide(normalV3, normalV3.Length);

                    verticesList.Add(new TexturedVertex(tri.V1, uv1, Color4.White, normalV1));
                    verticesList.Add(new TexturedVertex(tri.V2, uv2, Color4.White, normalV2));
                    verticesList.Add(new TexturedVertex(tri.V3, uv3, Color4.White, normalV3));
                }
            }

            Mesh.Primitive shapePrimitive = new Mesh.Primitive(verticesList);
            this.Meshes[0] = new Mesh(shapePrimitive);
            this.Materials[0] = new Material(0, Color4.Blue, Color4.White, 2);
            this.Materials[0].AmbientColor = Color4.Red;
        }

        private int addVertex(Vector3 point)
        {
            points.Add(point.Normalized());
            return index++;
        }

        // return index of point in the middle of p1 and p2
        private int getMiddlePoint(Vector3 point1, Vector3 point2)
        {
            long i1 = this.points.IndexOf(point1);
            long i2 = this.points.IndexOf(point2);
            
            // check if we have it already
            var firstIsSmaller = i1 < i2;
            long smallerIndex = firstIsSmaller ? i1 : i2;
            long greaterIndex = firstIsSmaller ? i2 : i1;
            long key = (smallerIndex << 32) + greaterIndex;

            int ret;
            if (this.middlePointIndexCache.TryGetValue(key, out ret)) {
                return ret;
            }

            // not in cache, calculate it
            var middle = new Vector3(
                (point1.X + point2.X) / 2.0f,
                (point1.Y + point2.Y) / 2.0f,
                (point1.Z + point2.Z) / 2.0f);

            // add vertex makes sure point is on unit sphere
            int i = this.addVertex(middle);

            // store it, return index
            this.middlePointIndexCache.Add(key, i);
            return i;
        }

        private void fixColorStrip(ref Vector2 uv1, ref Vector2 uv2, ref Vector2 uv3)
        {
            if ((uv1.X - uv2.X) >= 0.8f)
                uv1.X -= 1;
            if ((uv2.X - uv3.X) >= 0.8f)
                uv2.X -= 1;
            if ((uv3.X - uv1.X) >= 0.8f)
                uv3.X -= 1;

            if ((uv1.X - uv2.X) >= 0.8f)
                uv1.X -= 1;
            if ((uv2.X - uv3.X) >= 0.8f)
                uv2.X -= 1;
            if ((uv3.X - uv1.X) >= 0.8f)
                uv3.X -= 1;
        }
    }
}
