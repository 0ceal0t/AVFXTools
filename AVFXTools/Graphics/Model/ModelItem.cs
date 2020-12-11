using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public class ModelItem
    {
        public VertexPositionTexture[] Verts;
        public ushort[] Indexes;
        public VertexEmitter[] VEmits;

        public ModelItem(AVFXModel model)
        {
            Verts = GetVertexData(model);
            Indexes = GetIndexData(model);
            VEmits = GetVEmitData(model);
        }

        // ========== ARRAY ============
        public static ModelItem[] GetArray(List<AVFXModel> models)
        {
            ModelItem[] ret = new ModelItem[models.Count];
            for (int idx = 0; idx < models.Count; idx++)
            {
                ret[idx] = new ModelItem(models[idx]);
            }
            return ret;
        }

        // VERTS =========================
        public static VertexPositionTexture[] GetVertexData(AVFXModel model)
        {
            VertexPositionTexture[] ret = new VertexPositionTexture[model.Vertices.Count];
            int idx = 0;
            foreach (Vertex v in model.Vertices)
            {
                ret[idx] = new VertexPositionTexture(
                    new Vector3(v.Position[0], v.Position[1], v.Position[2]),
                    new Vector4(v.UV1[0], v.UV1[1], v.UV1[2], v.UV1[3]) + new Vector4(0.5f),
                    new Vector4(v.UV2[0], v.UV2[1], v.UV2[2], v.UV2[3]) + new Vector4(0.5f),
                    new Vector4(v.Color[0], v.Color[1], v.Color[2], v.Color[3]) / 255.0f
                );
                idx++;
            }
            return ret;
        }

        // IDX =======================
        public static ushort[] GetIndexData(AVFXModel model)
        {
            ushort[] ret = new ushort[model.Indexes.Count * 3];
            int idx = 0;
            foreach (AVFXLib.Models.Index i in model.Indexes)
            {
                ret[(idx * 3) + 0] = (ushort)i.I1;
                ret[(idx * 3) + 1] = (ushort)i.I2;
                ret[(idx * 3) + 2] = (ushort)i.I3;
                idx++;
            }
            return ret;
        }
        // ==========================
        public static VertexEmitter[] GetVEmitData(AVFXModel model)
        {
            VertexEmitter[] ret = new VertexEmitter[model.EmitVertices.Count];
            for(int idx = 0; idx < model.VNums.Count; idx++)
            {
                int OrderNum = model.VNums[idx].Num;
                var EmitV = model.EmitVertices[idx];
                Vector3 Pos = new Vector3(EmitV.Position[0], EmitV.Position[1], EmitV.Position[2]);
                Vector3 Norm = new Vector3(EmitV.Normal[0], EmitV.Normal[1], EmitV.Normal[2]);
                ret[OrderNum] = new VertexEmitter(Pos, Norm);
            }
            return ret;
        }
    }

    public struct VertexEmitter
    {
        public Vector3 Pos;
        public Vector3 Normal;
        public VertexEmitter(Vector3 _Pos, Vector3 _Norm)
        {
            Pos = _Pos;
            Normal = _Norm;
        }
    }

    public struct VertexPositionTexture
    {
        public const uint SizeInBytes = 60;

        public Vector3 Pos;
        public Vector4 UV;
        public Vector4 UV2;
        public Vector4 Color;

        public VertexPositionTexture(Vector3 _Pos, Vector4 _UV, Vector4 _Color) : this(_Pos, _UV, _UV, _Color) {}

        public VertexPositionTexture(Vector3 _Pos, Vector4 _UV, Vector4 _UV2, Vector4 _Color)
        {
            Pos = _Pos;
            UV = _UV;
            UV2 = _UV2;
            Color = _Color;
        }
    }
}
