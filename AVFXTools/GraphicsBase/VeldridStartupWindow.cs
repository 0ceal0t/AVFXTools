using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Veldrid.Utilities;

using ImGuiNET;

namespace AVFXTools.GraphicsBase
{
    public class VeldridStartupWindow : ApplicationWindow
    {
        private readonly Sdl2Window _window;
        private GraphicsDevice _gd;
        private DisposeCollectorResourceFactory _factory;
        private bool _windowResized = true;

        public event Action<float> Rendering;
        public event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        public event Action GraphicsDeviceDestroyed;
        public event Action Resized;
        public event Action<KeyEvent> KeyPressed;

        public uint Width => (uint)_window.Width;
        public uint Height => (uint)_window.Height;

        public ImGuiRenderer igr;

        public VeldridStartupWindow(string title)
        {
            WindowCreateInfo wci = new WindowCreateInfo
            {
                X = 100,
                Y = 100,
                WindowWidth = 2000,
                WindowHeight = 1000,
                WindowTitle = title,
            };
            _window = VeldridStartup.CreateWindow(ref wci);
            _window.Resized += () =>
            {
                _windowResized = true;
            };
            _window.KeyDown += OnKeyDown;
        }

        public void Run()
        {
            GraphicsDeviceOptions options = new GraphicsDeviceOptions(
                debug: false,
                swapchainDepthFormat: PixelFormat.R16_UNorm,
                syncToVerticalBlank: true,
                resourceBindingModel: ResourceBindingModel.Improved,
                preferDepthRangeZeroToOne: true,
                preferStandardClipSpaceYDirection: true);
#if DEBUG
            options.Debug = true;
#endif
            _gd = VeldridStartup.CreateGraphicsDevice(_window, options, GraphicsBackend.OpenGL);
            _factory = new DisposeCollectorResourceFactory(_gd.ResourceFactory);
            igr = new ImGuiRenderer(_gd, _gd.MainSwapchain.Framebuffer.OutputDescription, _window.Width, _window.Height, ColorSpaceHandling.Linear); // ============
            GraphicsDeviceCreated?.Invoke(_gd, _factory, _gd.MainSwapchain);

            Stopwatch sw = Stopwatch.StartNew();
            double previousElapsed = sw.Elapsed.TotalSeconds;
            float frameRateSeconds = 1.0f / 60.0f;

            while (_window.Exists)
            {
                double newElapsed = sw.Elapsed.TotalSeconds;
                float deltaSeconds = (float)(newElapsed - previousElapsed);

                while(deltaSeconds < frameRateSeconds)
                {
                    newElapsed = sw.Elapsed.TotalSeconds;
                    deltaSeconds = (float)(newElapsed - previousElapsed);
                }

                InputSnapshot inputSnapshot = _window.PumpEvents();
                igr.Update(deltaSeconds, inputSnapshot);
                InputTracker.UpdateFrameInput(inputSnapshot);
                if (_window.Exists)
                {
                    previousElapsed = newElapsed;

                    if (_windowResized)
                    {
                        _windowResized = false;
                        _gd.ResizeMainWindow((uint)_window.Width, (uint)_window.Height);

                        igr.WindowResized(_window.Width, _window.Height); // ===========

                        Resized?.Invoke();
                    }

                    Rendering?.Invoke(deltaSeconds);
                }
            }

            _gd.WaitForIdle();
            _factory.DisposeCollector.DisposeAll();
            _gd.Dispose();
            GraphicsDeviceDestroyed?.Invoke();
        }

        protected void OnKeyDown(KeyEvent keyEvent)
        {
            KeyPressed?.Invoke(keyEvent);
        }
    }
}
