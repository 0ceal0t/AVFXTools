using AVFXTools.Views;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using Veldrid;
using Veldrid.Utilities;

using VeldridKey = Veldrid.Key;
using InputKey = System.Windows.Input.Key;
using VeldridModifier = Veldrid.ModifierKeys;
using InputModifier = System.Windows.Input.ModifierKeys;
using VeldridMouse = Veldrid.MouseButton;
using InputMouse = System.Windows.Input.MouseButton;
using System.Windows.Input;

namespace AVFXTools.ApplicationBase
{
    public class VeldridComponent : Win32HwndControl, ApplicationWindow
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private Swapchain _swapchainSource;
        private GraphicsDevice _device;
        public DisposeCollectorResourceFactory _resources;
        public VFXViewer _window;

        public BasicInputSnapshot SnapShot = new BasicInputSnapshot();
        public ImGuiRenderer igr;

        public event Action<float> Rendering;
        public event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        public event Action GraphicsDeviceDestroyed;
        public event Action Resized;
        public event Action<KeyEvent> KeyPressed;

        public float WindowWidth;
        public float WindowHeight;

        uint ApplicationWindow.Width => (uint)WindowWidth;
        uint ApplicationWindow.Height => (uint)WindowHeight;

        public bool _IsRendering { get; private set; }

        protected override sealed void Initialize()
        {
            _device = GraphicsDevice.CreateD3D11(new GraphicsDeviceOptions());
            _resources = new DisposeCollectorResourceFactory(_device.ResourceFactory);
            CreateSwapchain();
            igr = new ImGuiRenderer(_device, _swapchainSource.Framebuffer.OutputDescription, (int)WindowWidth, (int)WindowHeight, ColorSpaceHandling.Linear);
            GraphicsDeviceCreated?.Invoke(_device, _resources, _swapchainSource);

            _IsRendering = true;
            CompositionTarget.Rendering += OnCompositionTargetRendering;

            _OnMouseDown += Event_MouseDown;
            _OnMouseUp += Event_MouseUp;
            _OnMouseMove += Event_MouseMove;
            _OnMouseWheel += Event_MouseWheel;
            _OnTextInput += Event_TextInput;
        }

        protected override sealed void Uninitialize()
        {
            _IsRendering = false;
            CompositionTarget.Rendering -= OnCompositionTargetRendering;

            if (_device != null)
            {
                GraphicsDeviceDestroyed?.Invoke();
                _device.WaitForIdle();
                _resources.DisposeCollector.DisposeAll();
                _device.Dispose();
                _device = null;
            }
            DestroySwapchain();
        }

        protected sealed override void OnResized()
        {
            ResizeSwapchain();
            igr.WindowResized((int)WindowWidth, (int)WindowHeight);
            Resized?.Invoke();
        }

        private void OnCompositionTargetRendering(object sender, EventArgs eventArgs)
        {
            if (!_IsRendering)
                return;
            if (!_stopwatch.IsRunning)
                _stopwatch.Start();

            var elapsedTotalSeconds = (float)_stopwatch.Elapsed.TotalSeconds;
            while (elapsedTotalSeconds < 1.0f / 30.0f)
            {
                elapsedTotalSeconds = (float)_stopwatch.Elapsed.TotalSeconds;
            }
            _stopwatch.Restart();


            if (_device != null)
            {
                BasicInputSnapshot publicSnapshot = new BasicInputSnapshot();
                SnapShot.CopyTo(publicSnapshot);
                SnapShot.Clear();

                igr.Update(elapsedTotalSeconds, publicSnapshot);
                InputTracker.UpdateFrameInput(publicSnapshot);

                Rendering?.Invoke(elapsedTotalSeconds);
                _device.WaitForIdle();
            }
        }

        private double GetDpiScale()
        {
            PresentationSource source = PresentationSource.FromVisual(this);
            return source.CompositionTarget.TransformToDevice.M11;
        }

        protected virtual void CreateSwapchain()
        {
            double dpiScale = GetDpiScale();
            WindowWidth = (float)(ActualWidth < 0 ? 0 : Math.Ceiling(ActualWidth * dpiScale));
            WindowHeight = (float)(ActualHeight < 0 ? 0 : Math.Ceiling(ActualHeight * dpiScale));
            Width = WindowWidth;
            Height = WindowHeight;

            Module mainModule = typeof(VeldridComponent).Module;
            IntPtr hinstance = Marshal.GetHINSTANCE(mainModule);
            SwapchainSource win32Source = SwapchainSource.CreateWin32(Hwnd, hinstance);
            SwapchainDescription scDesc = new SwapchainDescription(win32Source, (uint)WindowWidth, (uint)WindowHeight, Veldrid.PixelFormat.R32_Float, true);

            _swapchainSource = _device.ResourceFactory.CreateSwapchain(scDesc);
        }

        protected virtual void DestroySwapchain()
        {
            _swapchainSource.Dispose();
        }

        private void ResizeSwapchain()
        {
            double dpiScale = GetDpiScale();
            WindowWidth = (float)(ActualWidth < 0 ? 0 : Math.Ceiling(ActualWidth * dpiScale));
            WindowHeight = (float)(ActualHeight < 0 ? 0 : Math.Ceiling(ActualHeight * dpiScale));
            Width = WindowWidth;
            Height = WindowHeight;

            _swapchainSource.Resize((uint)WindowWidth, (uint)WindowHeight);
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public void Event_KeyUp(KeyEventArgs e, bool m_Ctl, bool m_Shift, bool m_Alt)
        {
            VeldridKey key = MapKeys(e.Key);
            VeldridModifier mod = GetKeyModifiers(m_Ctl, m_Shift, m_Alt);
            KeyEvent keyEvent = new KeyEvent(key, false, mod);
            SnapShot.KeyEventsList.Add(keyEvent);
        }

        public void Event_KeyDown(KeyEventArgs e, bool m_Ctl, bool m_Shift, bool m_Alt)
        {
            VeldridKey key = MapKeys(e.Key);
            VeldridModifier mod = GetKeyModifiers(m_Ctl, m_Shift, m_Alt);
            KeyEvent keyEvent = new KeyEvent(key, true, mod);
            SnapShot.KeyEventsList.Add(keyEvent);
        }

        public void Event_TextInput(string text)
        {
            foreach (char c in text)
            {
                SnapShot.KeyCharPressesList.Add(c);
            }
        }

        public void Event_MouseMove(double X, double Y)
        {
            Vector2 mousePos = new Vector2((float)X, (float)Y);
            SnapShot.MousePosition = mousePos;
        }

        public void Event_MouseDown(InputMouse b)
        {
            VeldridMouse button = MapMouseButton(b);
            SnapShot.MouseDown[(int)button] = true;
            MouseEvent mouseEvent = new MouseEvent(button, true);
            SnapShot.MouseEventsList.Add(mouseEvent);
        }

        public void Event_MouseUp(InputMouse b)
        {
            VeldridMouse button = MapMouseButton(b);
            SnapShot.MouseDown[(int)button] = false;
            MouseEvent mouseEvent = new MouseEvent(button, false);
            SnapShot.MouseEventsList.Add(mouseEvent);
        }

        public void Event_MouseWheel(int d)
        {
            SnapShot.WheelDelta += (float)d / 120;
        }

        private VeldridMouse MapMouseButton(InputMouse button)
        {
            switch (button)
            {
                case InputMouse.Left:
                    return VeldridMouse.Left;
                case InputMouse.Right:
                    return VeldridMouse.Right;
                case InputMouse.Middle:
                    return VeldridMouse.Middle;
                case InputMouse.XButton1:
                    return VeldridMouse.Button1;
                case InputMouse.XButton2:
                    return VeldridMouse.Button2;
                default:
                    return VeldridMouse.Left;
            }
        }

        private VeldridKey MapKeys(InputKey key)
        {
            switch (key)
            {
                case InputKey.Back:
                    return VeldridKey.Back;
                case InputKey.Tab:
                    return VeldridKey.Tab;
                case InputKey.Clear:
                    return VeldridKey.Clear;
                case InputKey.Enter:
                    return VeldridKey.Enter;
                case InputKey.LeftShift:
                    return VeldridKey.ShiftLeft;
                case InputKey.LeftCtrl:
                    return VeldridKey.ControlLeft;
                case InputKey.RightShift:
                    return VeldridKey.ShiftRight;
                case InputKey.RightCtrl:
                    return VeldridKey.ControlRight;
                case InputKey.Pause:
                    return VeldridKey.Pause;
                case InputKey.CapsLock:
                    return VeldridKey.CapsLock;
                case InputKey.Escape:
                    return VeldridKey.Escape;
                case InputKey.Space:
                    return VeldridKey.Space;
                case InputKey.PageUp:
                    return VeldridKey.PageUp;
                case InputKey.PageDown:
                    return VeldridKey.PageDown;
                case InputKey.End:
                    return VeldridKey.End;
                case InputKey.Home:
                    return VeldridKey.Home;
                case InputKey.Left:
                    return VeldridKey.Left;
                case InputKey.Up:
                    return VeldridKey.Up;
                case InputKey.Right:
                    return VeldridKey.Right;
                case InputKey.Down:
                    return VeldridKey.Down;
                case InputKey.PrintScreen:
                    return VeldridKey.PrintScreen;
                case InputKey.Insert:
                    return VeldridKey.Insert;
                case InputKey.Delete:
                    return VeldridKey.Delete;
                case InputKey.D0:
                    return VeldridKey.Number0;
                case InputKey.D1:
                    return VeldridKey.Number1;
                case InputKey.D2:
                    return VeldridKey.Number2;
                case InputKey.D3:
                    return VeldridKey.Number3;
                case InputKey.D4:
                    return VeldridKey.Number4;
                case InputKey.D5:
                    return VeldridKey.Number5;
                case InputKey.D6:
                    return VeldridKey.Number6;
                case InputKey.D7:
                    return VeldridKey.Number7;
                case InputKey.D8:
                    return VeldridKey.Number8;
                case InputKey.D9:
                    return VeldridKey.Number9;
                case InputKey.A:
                    return VeldridKey.A;
                case InputKey.B:
                    return VeldridKey.B;
                case InputKey.C:
                    return VeldridKey.C;
                case InputKey.D:
                    return VeldridKey.D;
                case InputKey.E:
                    return VeldridKey.E;
                case InputKey.F:
                    return VeldridKey.F;
                case InputKey.G:
                    return VeldridKey.G;
                case InputKey.H:
                    return VeldridKey.H;
                case InputKey.I:
                    return VeldridKey.I;
                case InputKey.J:
                    return VeldridKey.J;
                case InputKey.K:
                    return VeldridKey.K;
                case InputKey.L:
                    return VeldridKey.L;
                case InputKey.M:
                    return VeldridKey.M;
                case InputKey.N:
                    return VeldridKey.N;
                case InputKey.O:
                    return VeldridKey.O;
                case InputKey.P:
                    return VeldridKey.P;
                case InputKey.Q:
                    return VeldridKey.Q;
                case InputKey.R:
                    return VeldridKey.R;
                case InputKey.S:
                    return VeldridKey.S;
                case InputKey.T:
                    return VeldridKey.T;
                case InputKey.U:
                    return VeldridKey.U;
                case InputKey.V:
                    return VeldridKey.V;
                case InputKey.W:
                    return VeldridKey.W;
                case InputKey.X:
                    return VeldridKey.X;
                case InputKey.Y:
                    return VeldridKey.Y;
                case InputKey.Z:
                    return VeldridKey.Z;
                case InputKey.LWin:
                    return VeldridKey.LWin;
                case InputKey.RWin:
                    return VeldridKey.RWin;
                case InputKey.Sleep:
                    return VeldridKey.Sleep;
                case InputKey.NumPad0:
                    return VeldridKey.Keypad0;
                case InputKey.NumPad1:
                    return VeldridKey.Keypad1;
                case InputKey.NumPad2:
                    return VeldridKey.Keypad2;
                case InputKey.NumPad3:
                    return VeldridKey.Keypad3;
                case InputKey.NumPad4:
                    return VeldridKey.Keypad4;
                case InputKey.NumPad5:
                    return VeldridKey.Keypad5;
                case InputKey.NumPad6:
                    return VeldridKey.Keypad6;
                case InputKey.NumPad7:
                    return VeldridKey.Keypad7;
                case InputKey.NumPad8:
                    return VeldridKey.Keypad8;
                case InputKey.NumPad9:
                    return VeldridKey.Keypad9;
                case InputKey.Multiply:
                    return VeldridKey.KeypadMultiply;
                case InputKey.Add:
                    return VeldridKey.KeypadAdd;
                case InputKey.Subtract:
                    return VeldridKey.KeypadSubtract;
                case InputKey.Decimal:
                    return VeldridKey.KeypadDecimal;
                case InputKey.Divide:
                    return VeldridKey.KeypadDivide;
                case InputKey.F1:
                    return VeldridKey.F1;
                case InputKey.F2:
                    return VeldridKey.F2;
                case InputKey.F3:
                    return VeldridKey.F3;
                case InputKey.F4:
                    return VeldridKey.F4;
                case InputKey.F5:
                    return VeldridKey.F5;
                case InputKey.F6:
                    return VeldridKey.F6;
                case InputKey.F7:
                    return VeldridKey.F7;
                case InputKey.F8:
                    return VeldridKey.F8;
                case InputKey.F9:
                    return VeldridKey.F9;
                case InputKey.F10:
                    return VeldridKey.F10;
                case InputKey.F11:
                    return VeldridKey.F11;
                case InputKey.F12:
                    return VeldridKey.F12;
                case InputKey.F13:
                    return VeldridKey.F13;
                case InputKey.F14:
                    return VeldridKey.F14;
                case InputKey.F15:
                    return VeldridKey.F15;
                case InputKey.F16:
                    return VeldridKey.F16;
                case InputKey.F17:
                    return VeldridKey.F17;
                case InputKey.F18:
                    return VeldridKey.F18;
                case InputKey.F19:
                    return VeldridKey.F19;
                case InputKey.F20:
                    return VeldridKey.F20;
                case InputKey.F21:
                    return VeldridKey.F21;
                case InputKey.F22:
                    return VeldridKey.F22;
                case InputKey.F23:
                    return VeldridKey.F23;
                case InputKey.F24:
                    return VeldridKey.F24;
                case InputKey.NumLock:
                    return VeldridKey.NumLock;
                case InputKey.Scroll:
                    return VeldridKey.ScrollLock;
                case InputKey.LeftAlt:
                    return VeldridKey.LAlt;
                case InputKey.RightAlt:
                    return VeldridKey.RAlt;
                default:
                    return VeldridKey.Unknown;
            }
        }

        private VeldridModifier GetKeyModifiers(bool m_Ctl, bool m_Shift, bool m_Alt)
        {
            var modifiers = VeldridModifier.None;
            if (m_Ctl)
                modifiers |= VeldridModifier.Control;
            if (m_Shift)
                modifiers |= VeldridModifier.Shift;
            if (m_Alt)
                modifiers |= VeldridModifier.Alt;
            return modifiers;
        }
    }
}