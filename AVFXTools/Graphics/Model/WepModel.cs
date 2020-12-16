using AVFXTools.FFXIV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using System.Text.RegularExpressions;

namespace AVFXTools.Main
{
    public struct BasicVertex
    {
        public const uint SizeInBytes = 24;

        public Vector3 Pos;
        public Vector3 Normal;

        public BasicVertex(Vector3 _Pos, Vector3 _Normal)
        {
            Pos = _Pos;
            Normal = _Normal;
        }
    }

    public struct BindPoint
    {
        public Vector3 Point1;
        public Vector3 Point2;

        public BindPoint(Vector3 p1, Vector3 p2)
        {
            Point1 = p1;
            Point2 = p2;
        }
    }

    public class WepModel
    {
        public bool Valid = false;
        public BasicVertex[][] Vertices;
        public ushort[][] Indices;
        public Dictionary<int, BindPoint> BindPoints = new Dictionary<int, BindPoint>();

        public WepModel(SaintCoinach.Graphics.ModelDefinition mdlDef, ResourceGetter getter)
        {
            var model = mdlDef.GetModel(0);
            string file = mdlDef.File.Path;
            Regex regex = new Regex("w[0-9]+");
            Match match = regex.Match(file);
            string skeletonPath = String.Format(@"chara/weapon/{0}/skeleton/base/b0001/skl_{0}b0001.sklb", match.Value);
            var skeletonResult = getter.GetSkeleton(skeletonPath, out var skeleton);
            if (!skeletonResult)
                return;

            int LAST_NUM = 0; // this whole thing is probably wrong
            int BONE_IDX = 0;

            foreach (var u in mdlDef.UnknownStructs1)
            {
                int bindIdx = (int)u.Unknown1;
                int sheetIdx = (int)u.Unknown2;

                Vector3 P1 = new Vector3(UIntToFloat(u.Unknown3), UIntToFloat(u.Unknown4), UIntToFloat(u.Unknown5));
                Vector3 P2 = new Vector3(UIntToFloat(u.Unknown6), UIntToFloat(u.Unknown7), UIntToFloat(u.Unknown8));

                if (sheetIdx != LAST_NUM)
                {
                    LAST_NUM = sheetIdx;
                    BONE_IDX++;
                }
                var bone = skeleton.ReferencePosLocal[BONE_IDX];
                P1 = P1 + new Vector3(bone.Translation.X, bone.Translation.Y, bone.Translation.Z);

                BindPoints[bindIdx] = new BindPoint(P1, P2);

                //var sheet = getter.GetModelSkeleton();
                //var row = sheet[sheetIdx]; // goes from row[0] to row[16]
            }

            for (int idx = 0; idx < skeleton.BoneCount; idx++)
            {
                ApplicationBase.Logger.WriteInfo(string.Format("BONE| {0} {1} : {2} {3} {4}", skeleton.BoneNames[idx], skeleton.ParentBoneIndices[idx], skeleton.ReferencePosLocal[idx].Translation.X, skeleton.ReferencePosLocal[idx].Translation.Y, skeleton.ReferencePosLocal[idx].Translation.Z));
            }


            // ==========
            // SET UP MODEL VERTICES
            // ==========
            Vertices = new BasicVertex[model.Meshes.Length][];
            Indices = new ushort[model.Meshes.Length][];

            for (int meshIdx = 0; meshIdx < model.Meshes.Length; meshIdx++)
            {
                var mesh = model.Meshes[meshIdx];

                Vertices[meshIdx] = new BasicVertex[mesh.Vertices.Length];
                int idx = 0;
                foreach (var v in mesh.Vertices)
                {
                    var pos = (SaintCoinach.Graphics.Vector4)v.Position;
                    var norm = (SaintCoinach.Graphics.Vector3)v.Normal;
                    Vertices[meshIdx][idx] = new BasicVertex(new Vector3(pos.X, pos.Y, pos.Z), new Vector3(norm.X, norm.Y, norm.Z));
                    idx++;
                }

                Indices[meshIdx] = new ushort[mesh.Indices.Length];
                for (idx = 0; idx < mesh.Indices.Length; idx++)
                {
                    Indices[meshIdx][idx] = mesh.Indices[idx];
                }
            }

            Valid = true;
        }

        public static float UIntToFloat(uint i)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
        }
    }
}
