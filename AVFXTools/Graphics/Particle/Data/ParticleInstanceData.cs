using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ParticleInstanceData
    {
        public Matrix4x4 TransformMatrix;

        public Vector4 Color;
        public Vector4 ColorScale;

        public Vector2 UV1_Scroll;
        public Vector2 UV1_Scale;

        public Vector2 UV2_Scroll;
        public Vector2 UV2_Scale;

        public Vector2 UV3_Scroll;
        public Vector2 UV3_Scale;

        public Vector2 UV4_Scroll;
        public Vector2 UV4_Scale;

        public float UV1_Rot;
        public float UV2_Rot;
        public float UV3_Rot;
        public float UV4_Rot;

        public float ColorBri;
        public float NPow;
        public float DPow;
        public float _Padding;

        public ParticleInstanceData(
            Matrix4x4 _TMatrix,

            Vector4 _Color,
            float _ColorBri,
            Vector4 _ColorScale,

            Vector2 _UV1_Scroll,
            Vector2 _UV1_Scale,
            float _UV1_Rot,
            Vector2 _UV2_Scroll,
            Vector2 _UV2_Scale,
            float _UV2_Rot,
            Vector2 _UV3_Scroll,
            Vector2 _UV3_Scale,
            float _UV3_Rot,
            Vector2 _UV4_Scroll,
            Vector2 _UV4_Scale,
            float _UV4_Rot,

            float _NPow,
            float _DPow
        )
        {
            TransformMatrix = _TMatrix;

            Color = _Color;
            ColorBri = _ColorBri;
            ColorScale = _ColorScale;

            UV1_Scroll = _UV1_Scroll;
            UV1_Scale = _UV1_Scale;
            UV1_Rot = _UV1_Rot;
            UV2_Scroll = _UV2_Scroll;
            UV2_Scale = _UV2_Scale;
            UV2_Rot = _UV2_Rot;
            UV3_Scroll = _UV3_Scroll;
            UV3_Scale = _UV3_Scale;
            UV3_Rot = _UV3_Rot;
            UV4_Scroll = _UV4_Scroll;
            UV4_Scale = _UV4_Scale;
            UV4_Rot = _UV4_Rot;

            NPow = _NPow;
            DPow = _DPow;

            _Padding = 0.0f;
        }
    }
}
