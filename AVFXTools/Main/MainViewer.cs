using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using Veldrid;
using Veldrid.SPIRV;
using Veldrid.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using AVFXTools.GraphicsBase;
using AVFXLib.Models;
using AVFXTools.FFXIV;
using AVFXTools.UI;
using AVFXLib.Main;
using AVFXLib.AVFX;

namespace AVFXTools.Main
{
    public class MainViewer : Application
    {
        public DeviceBuffer ProjectionBuffer;
        public DeviceBuffer ViewBuffer;
        public DeviceBuffer WorldBuffer;

        public ResourceLayout GlobalLayout;

        public static Vector3 CameraPos;
        public static Vector3 CameraLook;
        public static Vector2 Size;

        public CommandList CL;
        public ResourceSet ProjViewWorldSet;
        public ResourceFactory Factory;

        public AVFXBase AVFX;
        public ResourceGetter Getter;
        public WepModel Model;
        public Core C;
        public ImgUIMain UI;


        public MainViewer(VeldridStartupWindow window, AVFXBase b, ResourceGetter g, WepModel baseM) : base(window)
        {
            AVFX = b;
            Getter = g;
            Model = baseM;
        }

        protected unsafe override void CreateResources(ResourceFactory factory)
        {
            // SHARED
            ProjectionBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            ViewBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            WorldBuffer = factory.CreateBuffer(new BufferDescription(64, BufferUsage.UniformBuffer));
            GlobalLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription("ProjectionBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("ViewBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex),
                    new ResourceLayoutElementDescription("WorldBuffer", ResourceKind.UniformBuffer, ShaderStages.Vertex)
                )
            );
            ProjViewWorldSet = factory.CreateResourceSet(new ResourceSetDescription(
                GlobalLayout,
                ProjectionBuffer,
                ViewBuffer,
                WorldBuffer
            ));
            Factory = factory;
            CL = factory.CreateCommandList();
            refreshGraphics();
            refreshUI();
        }


        public void refreshGraphics()
        {
            if (AVFX != null)
            {
                C = new Core(AVFX.clone(), Getter, Model, this, GraphicsDevice, Factory, CL, MainSwapchain, _camera);
            }
        }

        public void refreshUI()
        {
            if (AVFX != null)
            {
                UI = new ImgUIMain(AVFX, this, Window.igr, GraphicsDevice, CL);
            }
        }

        public void OpenLocalAVFX(string path)
        {
            AVFXNode node = Reader.readAVFX(path);
            AVFX = new AVFXBase();
            AVFX.read(node);
        }

        public void OpenGameAVFX(string path)
        {
            AVFXNode node = Reader.readAVFX(Getter.GetData(path));
            AVFX = new AVFXBase();
            AVFX.read(node);
        }

        public void clearAll()
        {
            AVFX = null;
            C = null;
            UI = null;
        }

        public void OpenGameMdl(string path)
        {
            Model = new WepModel(path, Getter);
        }

        protected override void OnDeviceDestroyed()
        {
            base.OnDeviceDestroyed();
        }

        protected override void Draw(float deltaSeconds)
        {
            CL.Begin();

            // PROJECTION
            CL.UpdateBuffer(ProjectionBuffer, 0, Matrix4x4.CreatePerspectiveFieldOfView(
                1.0f,
                (float)Window.Width / Window.Height,
                0.5f,
                100f));
            // CAMERA + VIEW
            CL.UpdateBuffer(ViewBuffer, 0, _camera.ViewMatrix);
            CameraPos = _camera.Position;
            CameraLook = _camera.Forward;
            Size = new Vector2((float)Window.Width, (float)Window.Height);
            // WORLD
            Matrix4x4 rotation = Matrix4x4.Identity;
            CL.UpdateBuffer(WorldBuffer, 0, ref rotation);

            CL.SetFramebuffer(MainSwapchain.Framebuffer);
            CL.ClearColorTarget(0, new RgbaFloat(0.3f, 0.3f, 0.3f, 1.0f));
            CL.ClearDepthStencil(1f);

            if (C != null)
            {
                C.Update(deltaSeconds);
                C.Draw();
                UI.Draw();
            }

            CL.End();
            GraphicsDevice.SubmitCommands(CL);
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
