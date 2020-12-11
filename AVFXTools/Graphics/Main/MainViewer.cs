using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;
using Veldrid;

using AVFXTools.ApplicationBase;
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
        public UIMain UI;

        public AVFXNode LastImportNode;

        public MainViewer(VeldridComponent window, AVFXBase b, ResourceGetter g, WepModel baseM) : base(window)
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
                UI = new UIMain(AVFX, this, Window.igr, GraphicsDevice, CL);
            }
        }

        public void OpenLocalAVFX(string path)
        {
            AVFXNode node = Reader.readAVFX(path);
            LastImportNode = node;
            AVFX = new AVFXBase();
            AVFX.read(node);
        }

        public void OpenGameAVFX(string path)
        {
            var dataResult = Getter.GetData(path, out var bytes);
            if (dataResult)
            {
                AVFXNode node = Reader.readAVFX(bytes);
                LastImportNode = node;
                AVFX = new AVFXBase();
                AVFX.read(node);
            }
            else
            {
                ApplicationBase.Logger.WriteError("Unable to find VFX");
            }
        }

        public void clearAll()
        {
            AVFX = null;
            C = null;
            UI = null;
        }

        public void OpenGameMdl(string path)
        {
            var mdlResult = Getter.GetModel(path, out var mdlDef);
            if (mdlResult)
            {
                Model = new WepModel(mdlDef, Getter);
            }
            else
            {
                ApplicationBase.Logger.WriteError("Unable to find model");
            }
        }

        protected override void OnDeviceDestroyed()
        {
            base.OnDeviceDestroyed();
        }

        protected override void Draw(float deltaSeconds)
        {
            CL.Begin();

            // PROJECTION
            CL.UpdateBuffer(ProjectionBuffer, 0, _camera.ProjectionMatrix);
            // CAMERA + VIEW
            CL.UpdateBuffer(ViewBuffer, 0, _camera.ViewMatrix);
            CameraPos = _camera.Position;
            CameraLook = _camera.Forward;
            Size = new Vector2(Window.WindowWidth, Window.WindowHeight);
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
            }
            if (UI != null)
            {
                UI.Draw();
            }

            CL.End();
            GraphicsDevice.SubmitCommands(CL);
            GraphicsDevice.SwapBuffers(MainSwapchain);
            GraphicsDevice.WaitForIdle();
        }
    }
}
