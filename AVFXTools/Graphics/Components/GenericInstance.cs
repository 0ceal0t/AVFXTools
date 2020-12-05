using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    public abstract class GenericInstance
    {
        public GenericInstance Parent;
        public Matrix4x4 CurrentTransform;
        public Matrix4x4 StartTransform;
        public EmitterCreateStruct CreateData;

        public Matrix4x4 GetCurrentTransform()
        {
            Matrix4x4 PrevTransform = (Parent == null) ? Matrix4x4.Identity : Parent.CurrentTransform;
            Vector3 Scale;
            Quaternion Rotation;
            Vector3 Translation;
            Matrix4x4.Decompose(PrevTransform, out Scale, out Rotation, out Translation);
            if (CreateData != null)
            {
                if (!CreateData.InfluenceCoordScale)
                {
                    Scale = new Vector3(1, 1, 1);
                }
                if (!CreateData.InfluenceCoordPos)
                {
                    Translation = new Vector3(0, 0, 0);
                }
                if (!CreateData.InfluenceCoordRot)
                {
                    Rotation = new Quaternion(1, 0, 0, 0);
                }
            }
            Matrix4x4 Recomposed = Matrix4x4.CreateTranslation(Translation) * Matrix4x4.CreateFromQuaternion(Rotation);
            return Recomposed * StartTransform;
        }
    }
}
