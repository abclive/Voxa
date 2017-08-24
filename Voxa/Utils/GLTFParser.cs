using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using OpenTK;
using OpenTK.Graphics;
using Voxa.Objects;
using Voxa.Rendering;

namespace Voxa.Utils
{
    class GLTFLoader
    {
        // See https://github.com/javagl/gltfOverview/releases
        readonly JObject _json;
        readonly Dictionary<int, BinaryReader> _buffers = new Dictionary<int, BinaryReader>();
        readonly string _resourcePath;

        public GLTFLoader(string resourcePath, string resourceName)
        {
            _resourcePath = resourcePath;
            _json = JObject.Parse(ResourceManager.GetTextResource(_resourcePath + "." + resourceName));

            if ((string)_json["asset"]["version"] != "2.0") throw new Exception("Unsupported GLTF format version (expected 2.0)");

            if (_json["scenes"].Count() != 1) throw new Exception("Expected a single scene");
            int sceneId = (int)_json["scene"];
            var sceneJson = _json["scenes"][sceneId];

            for (int i = 0; i < _json["buffers"].Count(); i++)
            {
                _buffers[i] = ResourceManager.GetBinaryResourceReader(_resourcePath + "." + (string)_json["buffers"][i]["uri"]);
            }
        }

        public List<Mesh> GetAllMeshes()
        {
            List<Mesh> meshes = new List<Mesh>();

            for (int i = 0; i < _json["nodes"].Count(); i++) {
                if (_json["nodes"][i]["mesh"] != null) {
                    meshes.Add(this.getMesh((JObject)_json["nodes"][i]));
                }
            }

            return meshes;
        }

        public Mesh GetMesh(string nodeName)
        {
            JObject nodeJson = null;

            for (int i = 0; i < _json["nodes"].Count(); i++) {
                if (_json["nodes"][i]["name"] != null && (string)_json["nodes"][i]["name"] == nodeName) {
                    nodeJson = (JObject)_json["nodes"][i];
                }
            }

            if (nodeJson == null)
                throw new Exception($"No node found named {nodeName}.");

            return this.getMesh(nodeJson);
        }

        public Mesh GetMesh(int nodeId)
        {
            JObject nodeJson = (JObject)_json["nodes"][nodeId];

            if (nodeJson == null)
                throw new Exception($"No node found with id {nodeId}.");

            return this.getMesh(nodeJson);
        }

        private Mesh getMesh(JObject nodeJson)
        {
            int meshId = (int)nodeJson["mesh"];
            JObject meshJson = (JObject)_json["meshes"][meshId];

            //if (((JArray)meshJson["primitives"]).Count > 1) Logger.Warning($"Found multiple primitives for mesh {meshId} but only a single one is supported at the moment.");

            List<Mesh.Primitive> primitives = new List<Mesh.Primitive>();
            foreach (JObject primitiveJson in (JArray)meshJson["primitives"]) {
                
                // Material
                int materialId = (int)primitiveJson["material"];
                var materialJson = _json["materials"][materialId];

                int textureId = -1;
                string imageResourcePath = null;

                Color4 vertexColor = Color4.White;
                if (materialJson["pbrMetallicRoughness"]["baseColorTexture"] != null) {
                    textureId = (int)materialJson["pbrMetallicRoughness"]["baseColorTexture"]["index"];
                } else if (materialJson["pbrMetallicRoughness"]["baseColorFactor"] != null) {
                    // Shader diffuse color
                    JArray color = (JArray)materialJson["pbrMetallicRoughness"]["baseColorFactor"];
                    float alpha = (color[3] != null) ? (float)color[3] : 0;
                    vertexColor = new Color4((float)color[0], (float)color[1], (float)color[2], alpha);
                }

                if (textureId >= 0) {
                    var textureJson = _json["textures"][textureId];

                    int imageId = (int)textureJson["source"];
                    var imageJson = _json["images"][imageId];
                    imageResourcePath = _resourcePath + "." + (string)imageJson["uri"];
                }

                // Geometry
                if (primitiveJson["mode"] == null || (GLTFPrimitiveMode)(int)primitiveJson["mode"] != GLTFPrimitiveMode.TRIANGLES) {
                    throw new Exception($"Found unsupported primitive mode {(int)primitiveJson["mode"]} for {meshId}, only TRIANGLES (4) is supported.");
                }

                int positionAttributeAccessorId = (int)primitiveJson["attributes"]["POSITION"];
                var positionAccessor = _json["accessors"][positionAttributeAccessorId];

                int verticesCount = (int)positionAccessor["count"];

                TexturedVertex[] staticVertices = new TexturedVertex[verticesCount];

                JToken accessor, bufferView;
                int sourceOffset, stride;
                BinaryReader buffer;

                // Position
                accessor = positionAccessor;
                if ((GLTFConst)(int)accessor["componentType"] != GLTFConst.FLOAT) {
                    throw new Exception($"Found unexpected component type {(int)accessor["componentType"]} for POSITION attribute of {meshId}, FLOAT was expected.");
                }

                bufferView = _json["bufferViews"][(int)accessor["bufferView"]];
                int accessorByteOffset = (accessor["byteOffset"] != null) ? (int)accessor["byteOffset"] : 0;
                int bufferViewByteOffset = (bufferView["byteOffset"] != null) ? (int)bufferView["byteOffset"] : 0;
                sourceOffset = bufferViewByteOffset + accessorByteOffset;
                stride = (bufferView["byteStride"] != null) ? (int)bufferView["byteStride"] : sizeof(float) * 3;

                buffer = _buffers[(int)bufferView["buffer"]];

                for (var i = 0; i < verticesCount; i++) {
                    buffer.BaseStream.Position = sourceOffset + stride * i;

                    var position = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
                    staticVertices[i].Position = position;
                    staticVertices[i].Color = vertexColor;
                }

                // Normals
                int normalAttributeAccessorId = (int)primitiveJson["attributes"]["NORMAL"];
                var normalAccessor = _json["accessors"][normalAttributeAccessorId];
                accessor = normalAccessor;
                if ((GLTFConst)(int)accessor["componentType"] != GLTFConst.FLOAT) {
                    throw new Exception($"Found unexpected component type {(int)accessor["componentType"]} for NORMAL attribute of {meshId}, FLOAT was expected.");
                }

                bufferView = _json["bufferViews"][(int)accessor["bufferView"]];
                accessorByteOffset = (accessor["byteOffset"] != null) ? (int)accessor["byteOffset"] : 0;
                bufferViewByteOffset = (bufferView["byteOffset"] != null) ? (int)bufferView["byteOffset"] : 0;
                sourceOffset = bufferViewByteOffset + accessorByteOffset;
                stride = (bufferView["byteStride"] != null) ? (int)bufferView["byteStride"] : sizeof(float) * 3;

                buffer = _buffers[(int)bufferView["buffer"]];

                for (var i = 0; i < verticesCount; i++) {
                    buffer.BaseStream.Position = sourceOffset + stride * i;

                    staticVertices[i].Normal = new Vector3(buffer.ReadSingle(), buffer.ReadSingle(), buffer.ReadSingle());
                }

                // Texture coordinates
                if (imageResourcePath != null) {
                    int attributeAccessorId = (int)primitiveJson["attributes"]["TEXCOORD_0"];

                    if (primitiveJson["attributes"]["TEXCOORD_1"] != null)
                        throw new Exception("Dual textures primitives are not supported");

                    accessor = _json["accessors"][attributeAccessorId];
                    if ((GLTFConst)(int)accessor["componentType"] != GLTFConst.FLOAT) {
                        throw new Exception($"Found unexpected component type {(int)accessor["componentType"]} for TEXCOORD_0 attribute of {meshId}, FLOAT was expected.");
                    }

                    bufferView = _json["bufferViews"][(int)accessor["bufferView"]];
                    accessorByteOffset = (accessor["byteOffset"] != null) ? (int)accessor["byteOffset"] : 0;
                    bufferViewByteOffset = (bufferView["byteOffset"] != null) ? (int)bufferView["byteOffset"] : 0;
                    sourceOffset = bufferViewByteOffset + accessorByteOffset;
                    stride = (bufferView["byteStride"] != null) ? (int)bufferView["byteStride"] : sizeof(float) * 2;

                    buffer = _buffers[(int)bufferView["buffer"]];

                    for (var i = 0; i < verticesCount; i++) {
                        buffer.BaseStream.Position = sourceOffset + stride * i;

                        staticVertices[i].TextureCoord = new Vector2(buffer.ReadSingle(), buffer.ReadSingle());
                    }
                }

                // Indices
                ushort[] indices = new ushort[0];
                if (primitiveJson["indices"] != null) {
                    int indicesAccessorId = (int)primitiveJson["indices"];
                    var indicesAccessor = _json["accessors"][indicesAccessorId];

                    indices = new ushort[(int)indicesAccessor["count"]];
                    accessor = indicesAccessor;

                    bufferView = _json["bufferViews"][(int)accessor["bufferView"]];
                    accessorByteOffset = (accessor["byteOffset"] != null) ? (int)accessor["byteOffset"] : 0;
                    bufferViewByteOffset = (bufferView["byteOffset"] != null) ? (int)bufferView["byteOffset"] : 0;
                    sourceOffset = bufferViewByteOffset + accessorByteOffset;

                    buffer = _buffers[(int)bufferView["buffer"]];

                    for (var i = 0; i < indices.Length; i++) {
                        buffer.BaseStream.Position = sourceOffset + sizeof(ushort) * i;
                        indices[i] = buffer.ReadUInt16();
                    }
                }

                Mesh.Primitive primitive = new Mesh.Primitive(staticVertices.ToList(), indices.ToList());

                if (imageResourcePath != null) {
                    primitive.PrimitiveTexture = new Texture(imageResourcePath);
                }

                primitives.Add(primitive);
            }

            Mesh mesh = new Mesh(primitives.ToArray());

            if (nodeJson["matrix"] != null) {
                JArray m = (JArray)nodeJson["matrix"];
                Matrix4 initialMatrix = new Matrix4((float)m[0], (float)m[1], (float)m[2], (float)m[3], (float)m[4], (float)m[5], (float)m[6], (float)m[7], (float)m[8], (float)m[9], (float)m[10], (float)m[11], (float)m[12], (float)m[13], (float)m[14], (float)m[15]);
                mesh.LocalMatrix = initialMatrix;
            }

            if (nodeJson["name"] != null) {
                mesh.Name = (string)nodeJson["name"];
            }

            return mesh;
        }

        enum GLTFConst
        {
            BYTE = 5120,
            UNSIGNED_BYTE = 5121,
            SHORT = 5122,
            UNSIGNED_SHORT = 5123,
            UNSIGNED_INT = 5125,
            FLOAT = 5126
        }

        enum GLTFPrimitiveMode
        {
            POINTS = 0,
            LINES = 1,
            LINE_LOOP = 2,
            LINE_STRIP = 3,
            TRIANGLES = 4,
            TRIANGLE_STRIP = 5,
            TRIANGLE_FAN = 6
        }
    }
}
