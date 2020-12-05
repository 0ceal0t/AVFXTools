using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid;
using Veldrid.SPIRV;

// SOME REALLY GENERIC SHIT TO DISPLAY THE MODEL

namespace AVFXTools.Main
{
    public class WepModelItem
    {
        public static string VertexShader = @"
        #version 450
        layout(set = 0, binding = 0) uniform ProjectionBuffer { mat4 Projection; };
        layout(set = 0, binding = 1) uniform ViewBuffer { mat4 View; };
        layout(set = 0, binding = 2) uniform WorldBuffer { mat4 World; };

        layout(location = 0) in vec3 Position;
        layout(location = 1) in vec3 Normal;

        layout(location = 0) out vec3 Norm;
        layout(location = 1) out vec3 Pos;

        void main() {
            vec4 worldPosition = World * vec4(Position, 1);
            vec4 viewPosition = View * worldPosition;
            vec4 clipPosition = Projection * viewPosition;
            gl_Position = clipPosition;

            Norm = Normal;
            Pos = Position;
        }";
        public static string FragShader = @"
        #version 450
        layout(location = 0) in vec3 Norm;
        layout(location = 1) in vec3 Pos;

        layout(location = 0) out vec4 fsout_color;

        void main() {
            vec3 lightPos = vec3(0,1,0);
            vec3 norm = normalize(Norm);
            vec3 lightDir = normalize(lightPos - Pos);  

            float diff = max(dot(norm, lightDir), 0.0);
            vec3 lightColor = vec3(1,1,1);
            vec3 diffuse = diff * lightColor;

            vec3 objectColor = vec3(0.5,0.5,0.5);
            vec3 ambient = vec3(0.8, 0.8, 0.8);
            vec3 result = (ambient + diffuse) * objectColor;
            fsout_color = vec4(result, 1.0);
        }";

        public DeviceBuffer[] VertexBuffer;
        public DeviceBuffer[] IndexBuffer;
        public uint[] IndexNum;

        public Pipeline Pipe;
        public Core C;

        public WepModelItem(
            WepModel model,
            Core core
        )
        {
            C = core;

            BasicVertex[][] v_s = model.Vertices;
            ushort[][] i_s = model.Indices;

            VertexBuffer = new DeviceBuffer[v_s.Length];
            IndexBuffer = new DeviceBuffer[v_s.Length];
            IndexNum = new uint[v_s.Length];

            for (int meshIdx = 0; meshIdx < v_s.Length; meshIdx++)
            {
                IndexNum[meshIdx] = (uint)i_s[meshIdx].Length;
                VertexBuffer[meshIdx] = C.Factory.CreateBuffer(new BufferDescription((uint)(BasicVertex.SizeInBytes * v_s[meshIdx].Length), BufferUsage.VertexBuffer));
                IndexBuffer[meshIdx] = C.Factory.CreateBuffer(new BufferDescription(sizeof(ushort) * IndexNum[meshIdx], BufferUsage.IndexBuffer));
                C.GD.UpdateBuffer(VertexBuffer[meshIdx], 0, v_s[meshIdx]);
                C.GD.UpdateBuffer(IndexBuffer[meshIdx], 0, i_s[meshIdx]);
            }

            ShaderSetDescription shaderSet = new ShaderSetDescription(
                new[]
                {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
                        new VertexElementDescription("Normal", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3)
                    )
                },
                C.Factory.CreateFromSpirv(
                    new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(VertexShader), "main"),
                    new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(FragShader), "main")
                )
            );

            var pipelineDesc = new GraphicsPipelineDescription(
                BlendStateDescription.SingleOverrideBlend,
                new DepthStencilStateDescription
                {
                    DepthTestEnabled = false,
                    DepthWriteEnabled = false,
                    DepthComparison = ComparisonKind.Always
                },
                new RasterizerStateDescription()
                {
                    CullMode = FaceCullMode.None,
                    FillMode = PolygonFillMode.Solid,
                    FrontFace = FrontFace.CounterClockwise,
                    DepthClipEnabled = false,
                    ScissorTestEnabled = false,
                },
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] {
                    C.Main.GlobalLayout
                },
                C.Chain.Framebuffer.OutputDescription
            );
            pipelineDesc.RasterizerState.CullMode = FaceCullMode.None; // disable face culling :p
            Pipe = C.Factory.CreateGraphicsPipeline(pipelineDesc);
        }



        public void Draw()
        {
            C.CL.SetPipeline(Pipe);
            C.CL.SetGraphicsResourceSet(0, C.Main.ProjViewWorldSet);

            for (int meshIdx = 0; meshIdx < VertexBuffer.Length; meshIdx++)
            {
                C.CL.SetVertexBuffer(0, VertexBuffer[meshIdx]);
                C.CL.SetIndexBuffer(IndexBuffer[meshIdx], IndexFormat.UInt16);

                C.CL.DrawIndexed(
                    indexCount: IndexNum[meshIdx],
                    instanceCount: 1,
                    indexStart: 0,
                    vertexOffset: 0,
                    instanceStart: 0
                );
            }
        }
    }
}

