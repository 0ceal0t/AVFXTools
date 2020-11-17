using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Schema2;
using SharpGLTF.Memory;

namespace AVFXLib.Main
{
    using VERTEX = SharpGLTF.Geometry.VertexTypes.VertexPositionNormalTangent;

    public class ModelReader
    {
        public static void ReadModel(string path)
        {
            var model = SharpGLTF.Schema2.ModelRoot.Load(path);
            foreach(var mesh in model.LogicalMeshes)
            {
                var primitives = mesh.Primitives[0];

                var indexes = primitives.GetIndices();
                var positions = primitives.GetVertices("POSITION").AsVector3Array();
                var normal = primitives.GetVertices("NORMAL").AsVector3Array();
                var tangnets = primitives.GetVertices("TANGENT").AsVector4Array();

                var colors = primitives.GetVertexAccessor("COLOR_0")?.AsColorArray();
                var uv1 = primitives.GetVertexAccessor("TEXCOORD_0")?.AsVector2Array();
                var uv2 = primitives.GetVertexAccessor("TEXCOORD_1")?.AsVector2Array();
            }
        }

        public static void ExportAllModels(string path, AVFXBase avfx)
        {
            var ROOT_MODEL = ModelRoot.CreateModel();
            var SCENE = ROOT_MODEL.DefaultScene = ROOT_MODEL.UseScene("Default");

            var idx = 0;
            foreach (AVFXModel model in avfx.Models)
            {
                // https://github.com/vpenades/SharpGLTF/blob/4c752452cde584cd3f125e7f23074ada81cc8333/tests/SharpGLTF.Tests/Schema2/Authoring/BasicSceneCreationTests.cs
                var NODE = SCENE.CreateNode(idx.ToString());
                var MESH = NODE.Mesh = ROOT_MODEL.CreateMesh(idx.ToString());

                var material = ROOT_MODEL.CreateMaterial("Material_" + idx.ToString())
                .WithDefault(new Vector4(0.5f, 0.5f, 0.5f, 1))
                .WithDoubleSide(true);

                var positions = new Vector3[model.Vertices.Count];
                var normals = new Vector3[model.Vertices.Count];
                var tangents = new Vector4[model.Vertices.Count];
                var colors = new Vector4[model.Vertices.Count];
                var uv1 = new Vector2[model.Vertices.Count];
                var uv2 = new Vector2[model.Vertices.Count];
                var indices = new int[3 * model.Indexes.Count];

                var vIdx = 0;
                foreach (Vertex vertex in model.Vertices)
                {
                    positions[vIdx] = new Vector3(vertex.Position[0], vertex.Position[1], vertex.Position[2]);

                    normals[vIdx] = new Vector3(vertex.Normal[0], vertex.Normal[1], vertex.Normal[2]);
                    normals[vIdx] = normals[vIdx] / normals[vIdx].Length();

                    var tangent3 = new Vector3(vertex.Tangent[0], vertex.Tangent[1], vertex.Tangent[2]);
                    tangent3 = tangent3 / tangent3.Length();
                    tangents[vIdx] = new Vector4(tangent3, 1);

                    colors[vIdx] = new Vector4(vertex.Color[0], vertex.Color[1], vertex.Color[2], vertex.Color[3]);

                    uv1[vIdx] = new Vector2(vertex.UV1[0], vertex.UV1[1]);
                    uv2[vIdx] = new Vector2(vertex.UV2[0], vertex.UV2[1]);

                    vIdx++;
                }
                var iIdx = 0;
                foreach (Index index in model.Indexes)
                {
                    indices[3 * iIdx + 0] = index.I1;
                    indices[3 * iIdx + 1] = index.I2;
                    indices[3 * iIdx + 2] = index.I3;
                    iIdx++;
                }

                // create mesh primitive
                var primitive = MESH.CreatePrimitive()
                    .WithVertexAccessor("POSITION", positions)
                    .WithVertexAccessor("NORMAL", normals)
                    .WithVertexAccessor("TANGENT", tangents)
                    .WithVertexAccessor("COLOR_0", colors)
                    .WithVertexAccessor("TEXCOORD_0", uv1)
                    .WithVertexAccessor("TEXCOORD_1", uv2)
                    .WithIndicesAccessor(PrimitiveType.TRIANGLES, indices)
                    .WithMaterial(material);

                idx++;
            }

            ROOT_MODEL.SaveGLTF(path);
        }
    }
}
