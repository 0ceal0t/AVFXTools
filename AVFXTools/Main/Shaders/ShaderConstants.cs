using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.Main.Shaders
{
    public class ShaderConstants
    {
        public const string VertexCode = @"
#version 450

struct ParticleInstanceData
{{
    mat4 TransformMatrix;
    vec4 Color;
    vec4 ColorScale;
    vec2 UV1_Scroll;
    vec2 UV1_Scale;
    vec2 UV2_Scroll;
    vec2 UV2_Scale;
    vec2 UV3_Scroll;
    vec2 UV3_Scale;
    vec2 UV4_Scroll;
    vec2 UV4_Scale;
    float UV1_Rot;
    float UV2_Rot;
    float UV3_Rot;
    float UV4_Rot;
    float ColorBri;
    float NPow;
    float DPow;
    float _Padding;
}};
layout(std140, set = 0, binding = 0) readonly buffer InstanceBuffer
{{
    ParticleInstanceData Instances[];
}};

layout(set = 1, binding = 0) uniform ProjectionBuffer {{ mat4 Projection; }};
layout(set = 1, binding = 1) uniform ViewBuffer {{ mat4 View; }};
layout(set = 1, binding = 2) uniform WorldBuffer {{ mat4 World; }};

layout(location = 0) in vec3 Position;
layout(location = 1) in vec2 TexCoords;
layout(location = 2) in vec2 UV2;
layout(location = 3) in vec4 Color;

layout(location = 0) out vec2 fsin_texCoords;
layout(location = 1) out vec2 UV;
layout(location = 2) out vec2 UV1_Scroll;
layout(location = 3) out vec2 UV1_Scale;
layout(location = 4) out float UV1_Rot;
layout(location = 5) out vec2 UV2_Scroll;
layout(location = 6) out vec2 UV2_Scale;
layout(location = 7) out float UV2_Rot;
layout(location = 8) out vec2 UV3_Scroll;
layout(location = 9) out vec2 UV3_Scale;
layout(location = 10) out float UV3_Rot;
layout(location = 11) out vec2 UV4_Scroll;
layout(location = 12) out vec2 UV4_Scale;
layout(location = 13) out float UV4_Rot;
layout(location = 14) out vec4 Color_RGBA;
layout(location = 15) out float Color_Bri;
layout(location = 16) out vec4 Color_Scale;
layout(location = 17) out float N_Pow;
layout(location = 18) out float D_Pow;
layout(location = 19) flat out int idx;
layout(location = 20) out vec4 Vert_Color;

void main() {{
    idx = 0;
    vec3 Pos = Position;
    mat4 SRTMatrix = Instances[idx].TransformMatrix;

    {0}

    vec4 SRT_Position = (SRTMatrix * vec4(Pos, 1));

    vec4 worldPosition = World * SRT_Position;
    vec4 viewPosition = View * worldPosition;
    vec4 clipPosition = Projection * viewPosition;
    gl_Position = clipPosition;

    fsin_texCoords = TexCoords;
    UV = UV2;
    UV1_Scroll = Instances[idx].UV1_Scroll;
    UV1_Scale = Instances[idx].UV1_Scale;
    UV1_Rot = Instances[idx].UV1_Rot;
    UV2_Scroll = Instances[idx].UV2_Scroll;
    UV2_Scale = Instances[idx].UV2_Scale;
    UV2_Rot = Instances[idx].UV2_Rot;
    UV3_Scroll = Instances[idx].UV3_Scroll;
    UV3_Scale = Instances[idx].UV3_Scale;
    UV3_Rot = Instances[idx].UV3_Rot;
    UV4_Scroll = Instances[idx].UV4_Scroll;
    UV4_Scale = Instances[idx].UV4_Scale;
    UV4_Rot = Instances[idx].UV4_Rot;
    Color_RGBA = Instances[idx].Color;
    Color_Bri = Instances[idx].ColorBri;
    Color_Scale = Instances[idx].ColorScale;
    N_Pow = Instances[idx].NPow;
    D_Pow = Instances[idx].DPow;
    Vert_Color = Color;
}}";

        public const string RotateDirectionNormal = @"";

        public const string RotateDirectionBillboardCamera = @"
        //Pos = vec3(Pos.x, -1 * Pos.y, Pos.z);
";

        public const string FragmentCode = @"
#version 450
layout(location = 0) in vec2 fsin_texCoords;
layout(location = 1) in vec2 UV;
layout(location = 2) in vec2 UV1_Scroll;
layout(location = 3) in vec2 UV1_Scale;
layout(location = 4) in float UV1_Rot;
layout(location = 5) in vec2 UV2_Scroll;
layout(location = 6) in vec2 UV2_Scale;
layout(location = 7) in float UV2_Rot;
layout(location = 8) in vec2 UV3_Scroll;
layout(location = 9) in vec2 UV3_Scale;
layout(location = 10) in float UV3_Rot;
layout(location = 11) in vec2 UV4_Scroll;
layout(location = 12) in vec2 UV4_Scale;
layout(location = 13) in float UV4_Rot;
layout(location = 14) in vec4 Color_RGBA;
layout(location = 15) in float Color_Bri;
layout(location = 16) in vec4 Color_Scale;
layout(location = 17) in float N_Pow;
layout(location = 18) in float D_Pow;
layout(location = 19) flat in int idx;
layout(location = 20) in vec4 Vert_Color;

layout(set = 2, binding = 0) uniform texture2D TC1_Texture;
layout(set = 2, binding = 1) uniform sampler TC1_Sampler;
layout(set = 2, binding = 2) uniform texture2D TC2_Texture;
layout(set = 2, binding = 3) uniform sampler TC2_Sampler;
layout(set = 2, binding = 4) uniform texture2D TC3_Texture;
layout(set = 2, binding = 5) uniform sampler TC3_Sampler;
layout(set = 2, binding = 6) uniform texture2D TC4_Texture;
layout(set = 2, binding = 7) uniform sampler TC4_Sampler;
layout(set = 2, binding = 8) uniform texture2D TN_Texture;
layout(set = 2, binding = 9) uniform sampler TN_Sampler;
layout(set = 2, binding = 10) uniform texture2D TD_Texture;
layout(set = 2, binding = 11) uniform sampler TD_Sampler;

layout(location = 0) out vec4 fsout_color;

// =======================
vec2 rotateUV(vec2 uv, float rotation) {{ float mid = 0.5; return vec2( cos(rotation) * (uv.x - mid) + sin(rotation) * (uv.y - mid) + mid, cos(rotation) * (uv.y - mid) - sin(rotation) * (uv.x - mid) + mid);}}
vec2 rotateUV(vec2 uv, float rotation, vec2 mid) {{ return vec2( cos(rotation) * (uv.x - mid.x) + sin(rotation) * (uv.y - mid.y) + mid.x, cos(rotation) * (uv.y - mid.y) - sin(rotation) * (uv.x - mid.x) + mid.y ); }}
vec2 rotateUV(vec2 uv, float rotation, float mid) {{ return vec2( cos(rotation) * (uv.x - mid) + sin(rotation) * (uv.y - mid) + mid, cos(rotation) * (uv.y - mid) - sin(rotation) * (uv.x - mid) + mid ); }}

float Lumin(vec4 color) {{ return 0.3*color.x + 0.59*color.y + 0.11*color.z; }}

vec3 normalizeColor(vec3 color) {{ return color / max(color.x, max(color.y, color.z)); }}
// ======================

void main() {{
    vec2 UV1_Coords = vec2(0.5, 0.5) + UV1_Scale * (rotateUV(fsin_texCoords, UV1_Rot) - vec2(0.5, 0.5)) + UV1_Scroll;
    vec2 UV2_Coords = vec2(0.5, 0.5) + UV2_Scale * (rotateUV(fsin_texCoords, UV2_Rot) - vec2(0.5, 0.5)) + UV2_Scroll;
    vec2 UV3_Coords = vec2(0.5, 0.5) + UV3_Scale * (rotateUV(fsin_texCoords, UV3_Rot) - vec2(0.5, 0.5)) + UV3_Scroll;
    vec2 UV4_Coords = vec2(0.5, 0.5) + UV4_Scale * (rotateUV(fsin_texCoords, UV4_Rot) - vec2(0.5, 0.5)) + UV4_Scroll;

    //vec3 Color = vec3(Color_RGBA[0], Color_RGBA[1], Color_RGBA[2]);
    //vec3 ColorScale = normalizeColor(vec3(Color_Scale.x, Color_Scale.y, Color_Scale.z));
    //float Lum = Lumin(Color);
    //float Alpha = 0;
    // -------------------------
    //Alpha = Alpha * Color_Scale[3] * Color_RGBA[3];
    //Color = Color * ColorScale;
    //if(Alpha < 0.001)
    //    discard;
    //Color = Color * vec3(Vert_Color[0], Vert_Color[1], Vert_Color[2]);
    //Alpha = Alpha * Vert_Color[3];

    vec4 Color = vec4(1,1,1,1);

{0}
{1}
{2}
{3}
{4}
{5}
{6}
{7}

    
    Color = Color * Color_RGBA * Vert_Color;
    Color.xyz = Color.xyz * Color.w;
    Color = clamp(Color, 0, 1);
    if (Color.w < 0.0039)
        discard;

    fsout_color = Color;
}}";
    }
}