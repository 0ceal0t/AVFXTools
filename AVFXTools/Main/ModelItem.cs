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
                ret[idx] = new VertexPositionTexture(new Vector3(v.Position[0], v.Position[1], v.Position[2]), new Vector2(v.UV1[0] + 0.5f, v.UV1[1] + 0.5f), new Vector2(v.UV2[2], v.UV2[3]));
                idx++;
            }
            return ret;
        }

        // IDX =======================
        public static ushort[] GetIndexData(AVFXModel model)
        {
            ushort[] ret = new ushort[model.Indexes.Count * 3];
            int idx = 0;
            foreach (Index i in model.Indexes)
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
        public const uint SizeInBytes = 28;

        public Vector3 Pos;
        public Vector2 UV;
        public Vector2 UV2;

        public VertexPositionTexture(Vector3 _Pos, Vector2 _UV) : this(_Pos, _UV, _UV)
        {
        }

        public VertexPositionTexture(Vector3 _Pos, Vector2 _UV, Vector2 _UV2)
        {
            Pos = _Pos;
            UV = _UV;
            UV2 = _UV2;
        }
    }
}
