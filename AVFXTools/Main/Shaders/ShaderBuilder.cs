using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main.Shaders
{
    public class ShaderBuilder
    {
        public string FRAG_SHADER = "";
        public string VERT_SHADER = "";

        public AVFXParticle Particle;

        public ShaderBuilder(AVFXParticle particle)
        {
            Particle = particle;
            // ==============
            BuildFrag();
            BuildVert();
        }

        public void BuildVert()
        {
            // ROTATION DIRECTION
            string rotationDirection = ShaderConstants.RotateDirectionNormal;
            switch (Particle.RotationDirectionBase.Value)
            {
                case RotationDirectionBase.CameraBillboard:
                    rotationDirection = ShaderConstants.RotateDirectionBillboardCamera;
                    break;
                case RotationDirectionBase.TreeBillboard:
                    rotationDirection = ShaderConstants.RotateDirectionBillboardCamera;
                    break;
            }

            VERT_SHADER = String.Format(ShaderConstants.VertexCode, rotationDirection);
        }

        public void BuildFrag()
        {
            string TC1 = (Particle.TC1.Enabled.Value == true) ? GetTextureColor("TC1", Particle.TC1.UvSetIdx, Particle.TC1.TextureCalculateColor, Particle.TC1.TextureCalculateAlpha, Particle.TC1.ColorToAlpha, Particle.TC1.TextureFilter) : "";
            string TC2 = (Particle.TC2.Enabled.Value == true) ? GetTextureColor("TC2", Particle.TC2.UvSetIdx, Particle.TC2.TextureCalculateColor, Particle.TC2.TextureCalculateAlpha, Particle.TC2.ColorToAlpha, Particle.TC2.TextureFilter) : "";
            string TC3 = (Particle.TC3.Enabled.Value == true) ? GetTextureColor("TC3", Particle.TC3.UvSetIdx, Particle.TC3.TextureCalculateColor, Particle.TC3.TextureCalculateAlpha, Particle.TC3.ColorToAlpha, Particle.TC3.TextureFilter) : "";
            string TC4 = (Particle.TC4.Enabled.Value == true) ? GetTextureColor("TC4", Particle.TC4.UvSetIdx, Particle.TC4.TextureCalculateColor, Particle.TC4.TextureCalculateAlpha, Particle.TC4.ColorToAlpha, Particle.TC4.TextureFilter) : "";

            string TN = "";
            string TR = "";
            string TD = (Particle.TD.Enabled.Value == true) ? GetTextureDistort(Particle.TD) : "";
            string TP = "";

            FRAG_SHADER = String.Format(ShaderConstants.FragmentCode, TD, TC1, TC2, TC3, TC4, TN, TR, TP);
        }

        public string GetVertexCode()
        {
            return VERT_SHADER;
        }

        public string GetFragCode()
        {
            return FRAG_SHADER;
        }

        // ============================
        public string GetTextureDistort(AVFXTextureDistortion distort)
        {
            // WHICH UVs ARE BEING DISTORTED
            string prefix = "TD";
            string uvPrefix = "UV" + (distort.UvSetIdx.Value + 1).ToString();

            string OffsetCode = "";
            if (distort.TargetUV1.Value == true)
            {
                OffsetCode += "UV1_Coords = UV1_Coords + Offset;";
            }
            if (distort.TargetUV2.Value == true)
            {
                OffsetCode += "UV2_Coords = UV2_Coords + Offset;";
            }
            if (distort.TargetUV3.Value == true)
            {
                OffsetCode += "UV3_Coords = UV3_Coords + Offset;";
            }
            if (distort.TargetUV4.Value == true)
            {
                OffsetCode += "UV4_Coords = UV4_Coords + Offset;";
            }

            return String.Format(@"
                vec2 {0}_Coords = MoveUV(UV2_C.xy, {1}_Scale, {1}_Scroll, {1}_Rot);
                vec4 {0}_Val = texture(sampler2D({0}_Texture, {0}_Sampler), {0}_Coords);
                vec2 Offset = D_Pow * ({0}_Val.xy - 0.5f);
                
                {2}
                ", prefix, uvPrefix, OffsetCode);
        }

        public string GetTextureColor(
            string prefix,
            LiteralInt uvIdx,
            LiteralEnum<TextureCalculateColor> colorMix,
            LiteralEnum<TextureCalculateAlpha> alphaMix,
            LiteralBool colorToAlpha,
            LiteralEnum<TextureFilterType> textureFilter
        )
        {
            string uvPrefix = "UV" + (uvIdx.Value + 1).ToString();

            // COLOR ==================
            string colorCode = "";
            TextureCalculateColor colorCalc = colorMix.Value;
            switch (colorCalc)
            {
                case TextureCalculateColor.Add:
                    colorCode += String.Format(@"Color.xyz = {0}_Color.xyz + Color.xyz;", prefix);
                    break;
                case TextureCalculateColor.Max:
                    colorCode += String.Format(@"Color.xyz = max({0}_Color.xyz, Color.xyz);", prefix);
                    break;
                case TextureCalculateColor.Min:
                    colorCode += String.Format(@"Color.xyz = min({0}_Color.xyz, Color.xyz);", prefix);
                    break;
                case TextureCalculateColor.Multiply:
                    colorCode += String.Format(@"Color.xyz = Color.xyz * {0}_Color.xyz;", prefix);
                    break;
                case TextureCalculateColor.Subtract:
                    colorCode += String.Format(@"Color.xyz = Color.xyz - {0}_Color.xyz;", prefix);
                    break;
            }

            // ALPHA =======================
            string alphaCode = "";
            TextureCalculateAlpha alphaCalc = alphaMix.Value;
            switch (alphaCalc)
            {
                case TextureCalculateAlpha.Max:
                    alphaCode += String.Format(@"Color.w = max({0}_Color.w, Color.w);", prefix);
                    break;
                case TextureCalculateAlpha.Min:
                    alphaCode += String.Format(@"Color.w = min({0}_Color.w, Color.w);", prefix);
                    break;
                case TextureCalculateAlpha.Multiply:
                    alphaCode += String.Format(@"Color.w = Color.w * {0}_Color.w;", prefix);
                    break;
            }

            // ==============================
            string colorToAlphaCode = (colorToAlpha.Value == true) ? String.Format(@"{0}_Color.w = {0}_Color.x;", prefix) : "";
            if (prefix == "TC1")
            {
                colorCode = String.Format(@"Color = {0}_Color;", prefix);
                alphaCode = "";
            }

            // FINAL ASSEMBLY ==============
            return String.Format(@"
                vec2 {0}_Coords = {1}_Coords;
                vec4 {0}_Color = texture(sampler2D({0}_Texture, {0}_Sampler), {0}_Coords);
                float {0}_Lum = Lumin({0}_Color);
                {2}

                {3}
                {4}
                ", prefix, uvPrefix, colorToAlphaCode, alphaCode, colorCode);
        }
    }
}