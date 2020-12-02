using AVFXLib.Models;
using AVFXTools.Main.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Veldrid;
using Veldrid.SPIRV;

namespace AVFXTools.Main
{
    public class ParticlePipeline
    {
        public AVFXParticle Particle;
        public ParticleItem Item;
        public Core C;
        // ==============
        public uint IndexNum;
        public DeviceBuffer VertexBuffer;
        public DeviceBuffer IndexBuffer;
        // ===============
        public int InstanceNum;
        public uint InstanceSize;
        public DeviceBuffer InstanceBuffer;
        public ResourceLayout InstanceLayout;
        public ResourceSet InstanceSet;
        // ============
        public ResourceSet TextureSet;
        public Pipeline Pipe;

        public ParticlePipeline(AVFXParticle particle, ParticleItem item, Core core)
        {
            Particle = particle;
            Item = item;
            C = core;
            // ============ VERTEX + INDEX ========
            IndexNum = (uint)Item.Indexes.Length;
            VertexBuffer = C.Factory.CreateBuffer(new BufferDescription((uint)(VertexPositionTexture.SizeInBytes * Item.Verts.Length), BufferUsage.VertexBuffer));
            IndexBuffer = C.Factory.CreateBuffer(new BufferDescription(sizeof(ushort) * IndexNum, BufferUsage.IndexBuffer));
            C.GD.UpdateBuffer(VertexBuffer, 0, Item.Verts);
            C.GD.UpdateBuffer(IndexBuffer, 0, Item.Indexes);
            // =========== INSTANCES ==========
            InstanceNum = Item.Instances.Length;
            InstanceSize = (uint)Unsafe.SizeOf<ParticleInstanceData>();
            InstanceBuffer = C.Factory.CreateBuffer(new BufferDescription(InstanceSize * (uint)InstanceNum, BufferUsage.StructuredBufferReadOnly, InstanceSize));
            InstanceLayout = C.Factory.CreateResourceLayout(new ResourceLayoutDescription(new ResourceLayoutElementDescription("InstanceBuffer", ResourceKind.StructuredBufferReadOnly, ShaderStages.Vertex)));
            InstanceSet = C.Factory.CreateResourceSet(new ResourceSetDescription(InstanceLayout, InstanceBuffer));
            // ========= TEXTURES =============
            TextureView TC1_View = C.Tex.GetView(particle.TC1.MaskTextureIdx.Value);
            TextureView TC2_View = C.Tex.GetView(particle.TC2.TextureIdx.Value);
            TextureView TC3_View = C.Tex.GetView(particle.TC3.TextureIdx.Value);
            TextureView TC4_View = C.Tex.GetView(particle.TC4.TextureIdx.Value);
            TextureView TN_View = C.Tex.GetView(particle.TN.TextureIdx.Value);
            TextureView TD_View = C.Tex.GetView(particle.TD.TextureIdx.Value);
            Sampler TC1_Sampler = C.Tex.GetSampler(particle.TC1.TextureBorderU, particle.TC1.TextureBorderV);
            Sampler TC2_Sampler = C.Tex.GetSampler(particle.TC2.TextureBorderU, particle.TC2.TextureBorderV);
            Sampler TC3_Sampler = C.Tex.GetSampler(particle.TC3.TextureBorderU, particle.TC3.TextureBorderV);
            Sampler TC4_Sampler = C.Tex.GetSampler(particle.TC4.TextureBorderU, particle.TC4.TextureBorderV);
            Sampler TN_Sampler = C.Tex.GetSampler(particle.TN.TextureBorderU, particle.TN.TextureBorderV);
            Sampler TD_Sampler = C.Tex.GetSampler(particle.TD.TextureBorderU, particle.TD.TextureBorderV);
            ResourceLayout TextureLayout = C.Factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("TC1_Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TC1_Sampler", ResourceKind.Sampler, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TC2_Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TC2_Sampler", ResourceKind.Sampler, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TC3_Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TC3_Sampler", ResourceKind.Sampler, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TC4_Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TC4_Sampler", ResourceKind.Sampler, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TN_Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TN_Sampler", ResourceKind.Sampler, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TD_Texture", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
                    new ResourceLayoutElementDescription("TD_Sampler", ResourceKind.Sampler, ShaderStages.Fragment)
                )
            );
            TextureSet = C.Factory.CreateResourceSet(new ResourceSetDescription(
                    TextureLayout,
                    TC1_View,
                    TC1_Sampler,
                    TC2_View,
                    TC2_Sampler,
                    TC3_View,
                    TC3_Sampler,
                    TC4_View,
                    TC4_Sampler,
                    TN_View,
                    TN_Sampler,
                    TD_View,
                    TD_Sampler
                )
            );
            // ====== SHADER ============
            ShaderBuilder shader = new ShaderBuilder(particle);
            ShaderSetDescription shaderSet = new ShaderSetDescription(
                new[] {
                    new VertexLayoutDescription(
                        new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
                        new VertexElementDescription("UV1", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4),
                        new VertexElementDescription("UV2", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4),
                        new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4)
                    )
                },
                C.Factory.CreateFromSpirv(
                    new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(shader.GetVertexCode()), "main"),
                    new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(shader.GetFragCode()), "main")
                )
            );
            // ====== BLEND MODE ===========
            BlendStateDescription BlendDrawMode = new BlendStateDescription
            {
                AttachmentStates = new BlendAttachmentDescription[]
                {
                    new BlendAttachmentDescription
                    {
                        BlendEnabled = true,
                        SourceColorFactor = BlendFactor.SourceAlpha,
                        DestinationColorFactor = BlendFactor.InverseSourceAlpha,
                        ColorFunction = BlendFunction.Add,
                        SourceAlphaFactor = BlendFactor.One,
                        DestinationAlphaFactor = BlendFactor.InverseSourceAlpha,
                        AlphaFunction = BlendFunction.Add
                    }
                }
            };
            BlendStateDescription ReverseDrawMode = new BlendStateDescription
            {
                AttachmentStates = new BlendAttachmentDescription[]
                {
                    new BlendAttachmentDescription
                    {
                        BlendEnabled = true,
                        SourceColorFactor = BlendFactor.One,
                        DestinationColorFactor = BlendFactor.InverseSourceColor,
                        ColorFunction = BlendFunction.Add,
                        SourceAlphaFactor = BlendFactor.Zero,
                        DestinationAlphaFactor = BlendFactor.One,
                        AlphaFunction = BlendFunction.Add
                    }
                }
            };
            BlendStateDescription AddDrawMode = new BlendStateDescription
            {
                AttachmentStates = new BlendAttachmentDescription[]
                {
                    new BlendAttachmentDescription
                    {
                        BlendEnabled = true,
                        SourceColorFactor = BlendFactor.SourceAlpha,
                        DestinationColorFactor = BlendFactor.One,
                        ColorFunction = BlendFunction.Add,
                        SourceAlphaFactor = BlendFactor.Zero,
                        DestinationAlphaFactor = BlendFactor.One,
                        AlphaFunction = BlendFunction.Add
                    }
                }
            };
            BlendStateDescription MultiplyDrawMode = new BlendStateDescription
            {
                AttachmentStates = new BlendAttachmentDescription[]
                {
                    new BlendAttachmentDescription
                    {
                        BlendEnabled = true,
                        SourceColorFactor = BlendFactor.DestinationColor,
                        DestinationColorFactor = BlendFactor.InverseSourceAlpha,
                        ColorFunction = BlendFunction.Add,
                        SourceAlphaFactor = BlendFactor.Zero,
                        DestinationAlphaFactor = BlendFactor.One,
                        AlphaFunction = BlendFunction.Add
                    }
                }
            };

            BlendStateDescription BlendStateDrawMode = BlendDrawMode;
            DrawMode MODE = particle.DrawMode.Value;
            switch (MODE)
            {
                case DrawMode.Reverse:
                    BlendStateDrawMode = ReverseDrawMode;
                    break;
                case DrawMode.Screen:
                    BlendStateDrawMode = ReverseDrawMode;
                    break;
                case DrawMode.Add:
                    BlendStateDrawMode = AddDrawMode;
                    break;
                case DrawMode.Blend:
                    BlendStateDrawMode = BlendDrawMode;
                    break;
                case DrawMode.Multiply:
                    BlendStateDrawMode = MultiplyDrawMode;
                    break;
            }
            // ======== CULLINE MODE ======
            FaceCullMode PipelineCullMode = FaceCullMode.None;
            CullingType CullMode = particle.CullingType.Value;
            switch (CullMode)
            {
                case CullingType.Back:
                    PipelineCullMode = FaceCullMode.Back;
                    break;
                case CullingType.Double:
                    break;
                case CullingType.Front:
                    PipelineCullMode = FaceCullMode.Front;
                    break;
                case CullingType.Max:
                    break;
                case CullingType.None:
                    break;
            }
            // ======= DEPTH TEST ======
            bool PipelineDepthTest = (particle.IsDepthTest.Value == true);
            // ======= DEPTH WRITE =====
            bool PipelineDepthWrite = (particle.IsDepthWrite.Value == true);
            // ======= PIPELINE =======
            var pipelineDesc = new GraphicsPipelineDescription(
                BlendStateDrawMode,
                new DepthStencilStateDescription
                {
                    DepthTestEnabled = PipelineDepthTest,
                    DepthWriteEnabled = PipelineDepthWrite,
                    DepthComparison = ComparisonKind.Always
                },
                new RasterizerStateDescription()
                {
                    CullMode = PipelineCullMode,
                    FillMode = PolygonFillMode.Solid,
                    FrontFace = FrontFace.CounterClockwise,
                    DepthClipEnabled = false,
                    ScissorTestEnabled = false,
                },
                PrimitiveTopology.TriangleList,
                shaderSet,
                new[] {
                    InstanceLayout,
                    C.Main.GlobalLayout,
                    TextureLayout
                },
                C.Chain.Framebuffer.OutputDescription
            );
            Pipe = C.Factory.CreateGraphicsPipeline(pipelineDesc);
        }

        public void Draw()
        {
            C.CL.SetPipeline(Pipe);
            C.CL.SetGraphicsResourceSet(2, TextureSet);
            C.CL.SetGraphicsResourceSet(1, C.Main.ProjViewWorldSet);
            C.CL.SetGraphicsResourceSet(0, InstanceSet);

            C.CL.SetVertexBuffer(0, VertexBuffer);
            C.CL.SetIndexBuffer(IndexBuffer, IndexFormat.UInt16);

            for (int instanceIdx = 0; instanceIdx < InstanceNum; instanceIdx++) { 
                if(Item.Instances[instanceIdx] != null && Item.Instances[instanceIdx].Dead != true)
                {
                    if (Item.Instances[instanceIdx].IsSpawner) continue; // a powder spawner, don't render it
                    C.CL.UpdateBuffer(InstanceBuffer, 0, Item.Instances[instanceIdx].GetData());
                    C.CL.DrawIndexed(
                        indexCount: IndexNum,
                        instanceCount: 1,
                        indexStart: 0,
                        vertexOffset: 0,
                        instanceStart: 0
                    );
                }
            }
        }
    }
}
