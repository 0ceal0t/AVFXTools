using AVFXLib.Models;
using System.Numerics;
using Veldrid.ImageSharp;

namespace AVFXTools.Main
{
    class GUtil
    {
        // ROTATE ORDER ==============
        public static Matrix4x4 RotationMatrix(Vector3 XYZ, RotationOrder _RotationOrder)
        {
            Matrix4x4 X_ = Matrix4x4.CreateRotationX(XYZ.X);
            Matrix4x4 Y_ = Matrix4x4.CreateRotationY(XYZ.Y);
            Matrix4x4 Z_ = Matrix4x4.CreateRotationZ(XYZ.Z);
            switch (_RotationOrder)
            {
                case RotationOrder.XYZ:
                    return X_ * Y_ * Z_;
                case RotationOrder.XZY:
                    return X_ * Z_ * Y_;
                case RotationOrder.YXZ:
                    return Y_ * X_ * Z_;
                case RotationOrder.YZX:
                    return Y_ * Z_ * X_;
                case RotationOrder.ZXY:
                    return Z_ * X_ * Y_;
                case RotationOrder.ZYX:
                    return Z_ * Y_ * X_;
                default:
                    return Matrix4x4.Identity;
            }
        }

        // SRT ============
        public static Matrix4x4 TransformMatrix(Vector3 Rotate, Vector3 Scale, Vector3 Transform, RotationOrder _RotationOrder, CoordComputeOrder _CoordOrder)
        {
            Matrix4x4 T_ = Matrix4x4.CreateTranslation(new Vector3(Transform.X, Transform.Y, Transform.Z));
            Matrix4x4 R_ = RotationMatrix(new Vector3(Rotate.X, Rotate.Y, Rotate.Z), _RotationOrder);
            Matrix4x4 S_ = Matrix4x4.CreateScale(new Vector3(Scale.X, Scale.Y, Scale.Z));

            switch (_CoordOrder)
            {
                case CoordComputeOrder.Rot_Scale_Translate:
                    return R_ * S_ * T_;
                case CoordComputeOrder.Rot_Translate_Scale:
                    return R_ * T_ * S_;
                case CoordComputeOrder.Scale_Rot_Translate:
                    return S_ * R_ * T_;
                case CoordComputeOrder.Scale_Translate_Rot:
                    return S_ * T_ * R_;
                case CoordComputeOrder.Translate_Rot_Scale:
                    return T_ * R_ * S_;
                case CoordComputeOrder.Translate_Scale_Rot:
                    return T_ * S_ * R_;
                default:
                    return Matrix4x4.Identity;
            }
        }

        // DDS ====================================
        public static ImageSharpTexture GetTextureData(string rootPath, string file)
        {
            string[] fileSplit = file.Split('/');
            string fileName = fileSplit[fileSplit.Length - 1].Replace("\u0000", "").Replace(".atex", ".dds");
            var t = new ImageSharpTexture(Textures.ReadFromDDS(rootPath + "\\" + fileName), false);
            return t;
        }
    }
}
