using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace AVFXTools.Main
{
    public struct MatrixTriplet
    {
        public Matrix4x4 Rotation;
        public Vector3 Position;
        public Vector3 Scale;

        public MatrixTriplet(Vector3 P, Matrix4x4 R, Vector3 S)
        {
            Position = P;
            Rotation = R;
            Scale = S;
        }
        public MatrixTriplet(Vector3 P, Vector3 R, Vector3 S, RotationOrder _RotOrder) : this(P, RotMatrix(R, _RotOrder), S)
        {
        }

        public static MatrixTriplet Pos(Vector3 P)
        {
            return new MatrixTriplet(P, DefaultR(), DefaultS());
        }
        public static MatrixTriplet Rot(Matrix4x4 R)
        {
            return new MatrixTriplet(DefaultP(), R, DefaultS());
        }
        public static MatrixTriplet Rot(Vector3 R, RotationOrder _RotOrder)
        {
            return new MatrixTriplet(DefaultP(), RotMatrix(R, _RotOrder), DefaultS());
        }
        public static MatrixTriplet Scl(Vector3 S)
        {
            return new MatrixTriplet(DefaultP(), DefaultR(), S);
        }
        public static MatrixTriplet Identity()
        {
            return new MatrixTriplet(DefaultP(), DefaultR(), DefaultS());
        }

        public static Matrix4x4 DefaultR() { return Matrix4x4.Identity; }
        public static Vector3 DefaultP() { return new Vector3(0, 0, 0); }
        public static Vector3 DefaultS() { return new Vector3(1, 1, 1); }

        public MatrixTriplet Add(MatrixTriplet parent)
        {
            return new MatrixTriplet(Position + parent.Position, Rotation * parent.Rotation, Scale * parent.Scale);
        }
        public MatrixTriplet Add(MatrixTriplet parent, EmitterCreateStruct CreateData)
        {
            if(CreateData == null)
            {
                return Add(parent);
            }

            MatrixTriplet Parent = new MatrixTriplet(
                CreateData.InfluenceCoordPos? parent.Position : DefaultP(),
                CreateData.InfluenceCoordRot? parent.Rotation : DefaultR(),
                CreateData.InfluenceCoordScale? parent.Scale : DefaultS()
            );
            return Add(Parent);
        }

        public Matrix4x4 Compress(CoordComputeOrder _CoordOrder)
        {
            Matrix4x4 T_ = Matrix4x4.CreateTranslation(Position);
            Matrix4x4 R_ = Rotation;
            Matrix4x4 S_ = Matrix4x4.CreateScale(Scale);

            switch (_CoordOrder)
            {
                case CoordComputeOrder.Rot_Scale_Translate:
                    return T_ * S_ * R_;
                case CoordComputeOrder.Rot_Translate_Scale:
                    return S_ * T_ * R_;
                case CoordComputeOrder.Scale_Rot_Translate:
                    return T_ * R_ * S_;
                case CoordComputeOrder.Scale_Translate_Rot:
                    return R_ * T_ * S_;
                case CoordComputeOrder.Translate_Rot_Scale:
                    return S_ * R_ * T_;
                case CoordComputeOrder.Translate_Scale_Rot:
                    return R_ * S_ * T_;
                default:
                    return Matrix4x4.Identity;
            }
        }

        public static Matrix4x4 RotMatrix(Vector3 XYZ, RotationOrder _RotationOrder)
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
    }
}
