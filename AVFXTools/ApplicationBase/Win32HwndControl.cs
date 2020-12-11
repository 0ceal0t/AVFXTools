using System;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace AVFXTools.ApplicationBase
{
    /// <summary>
    /// Creates internal Hwnd to host DirectXComponent within a control in the window.
    /// </summary>

    public abstract class Win32HwndControl : HwndHost
    {

        protected IntPtr Hwnd { get; private set; }
        private IntPtr _HwndPrev;

        protected bool HwndInitialized { get; private set; }
        private bool _mouseInWindow;
        private Point _previousPosition;
        private bool _isMouseCaptured;
        private bool _applicationHasFocus;
        public new bool IsMouseCaptured
        {
            get { return _isMouseCaptured; }
        }

        public Action<MouseButton> _OnMouseUp;
        public Action<MouseButton> _OnMouseDown;
        public Action<double, double> _OnMouseMove;
        public Action<int> _OnMouseWheel;

        private const string WindowClass = "HwndWrapper";

        protected Win32HwndControl()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            System.Windows.Application.Current.Activated += OnApplicationActivated;
            System.Windows.Application.Current.Deactivated += OnApplicationDeactivated;

            foreach (var w in System.Windows.Application.Current.Windows)
            {
                if (((Window)w).IsActive)
                {
                    _applicationHasFocus = true;
                    break;
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Initialize();
            HwndInitialized = true;
            Loaded -= OnLoaded;
        }
        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Uninitialize();
            HwndInitialized = false;
            Unloaded -= OnUnloaded;
            Dispose();
        }

        protected abstract void Initialize();
        protected abstract void Uninitialize();
        protected abstract void OnResized();

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            var wndClass = new NativeMethods.WndClassEx();
            wndClass.cbSize = (uint)Marshal.SizeOf(wndClass);
            wndClass.hInstance = NativeMethods.GetModuleHandle(null);
            wndClass.lpfnWndProc = NativeMethods.DefaultWindowProc;
            wndClass.lpszClassName = WindowClass;
            wndClass.hCursor = NativeMethods.LoadCursor(IntPtr.Zero, NativeMethods.IDC_ARROW);
            NativeMethods.RegisterClassEx(ref wndClass);
            Hwnd = NativeMethods.CreateWindowEx(
                0, WindowClass, "", NativeMethods.WS_CHILD | NativeMethods.WS_VISIBLE,
                0, 0, (int)Width, (int)Height, hwndParent.Handle, IntPtr.Zero, IntPtr.Zero, 0);
            return new HandleRef(this, Hwnd);
        }
        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            NativeMethods.DestroyWindow(hwnd.Handle);
            Hwnd = IntPtr.Zero;
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            UpdateWindowPos();
            base.OnRenderSizeChanged(sizeInfo);
            if (HwndInitialized)
            {
                System.Diagnostics.Debug.WriteLine("resized");
                OnResized();
            }
        }

        private void OnApplicationActivated(object sender, EventArgs e)
        {
            _applicationHasFocus = true;
        }
        private void OnApplicationDeactivated(object sender, EventArgs e)
        {
            _applicationHasFocus = false;
            if (_mouseInWindow)
            {
                _mouseInWindow = false;
            }
            ReleaseMouseCapture();
        }
        public new void CaptureMouse()
        {
            if (_isMouseCaptured)
                return;
            NativeMethods.SetCapture(Hwnd);
            _isMouseCaptured = true;
        }
        public new void ReleaseMouseCapture()
        {
            if (!_isMouseCaptured)
                return;
            NativeMethods.ReleaseCapture();
            _isMouseCaptured = false;
        }

        protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case NativeMethods.WM_LBUTTONDOWN:
                    RaiseMouseDownEvent(MouseButton.Left);
                    break;
                case NativeMethods.WM_LBUTTONUP:
                    RaiseMouseUpEvent(MouseButton.Left);
                    break;
                case NativeMethods.WM_RBUTTONDOWN:
                    RaiseMouseDownEvent(MouseButton.Right);
                    break;
                case NativeMethods.WM_RBUTTONUP:
                    RaiseMouseUpEvent(MouseButton.Right);
                    break;
                case NativeMethods.WM_MOSEWHEEL:
                    if (_mouseInWindow)
                    {
                        int delta = NativeMethods.GetWheelDeltaWParam(wParam.ToInt32());
                        RaiseMouseWheelEvent(delta);
                    }
                    break;
                case NativeMethods.WM_MOUSEMOVE:
                    if (!_applicationHasFocus)
                        break;
                    Point currentMousePosition = new Point(
                        NativeMethods.GetXLParam((int)lParam),
                        NativeMethods.GetYLParam((int)lParam));
                    if (!_mouseInWindow)
                    {
                        _mouseInWindow = true;
                        _HwndPrev = NativeMethods.GetFocus();
                        NativeMethods.SetFocus(Hwnd);

                        // send the track mouse event so that we get the WM_MOUSELEAVE message
                        var tme = new NativeMethods.TRACKMOUSEEVENT
                        {
                            cbSize = Marshal.SizeOf(typeof(NativeMethods.TRACKMOUSEEVENT)),
                            dwFlags = NativeMethods.TME_LEAVE,
                            hWnd = hwnd
                        };
                        NativeMethods.TrackMouseEvent(ref tme);
                    }

                    if (currentMousePosition != _previousPosition)
                        RaiseMouseMoveEvent(currentMousePosition);

                    _previousPosition = currentMousePosition;
                    break;
            }
            return base.WndProc(hwnd, msg, wParam, lParam, ref handled);
        }

        private void RaiseMouseMoveEvent(Point p)
        {
            _OnMouseMove?.Invoke(p.X, p.Y);
        }
        private void RaiseMouseWheelEvent(int delta)
        {
            _OnMouseWheel?.Invoke(delta);
        }
        private void RaiseMouseUpEvent(MouseButton button)
        {
            _OnMouseUp?.Invoke(button);
        }
        private void RaiseMouseDownEvent(MouseButton button)
        {
            _OnMouseDown?.Invoke(button);
        }
    }

    internal class NativeMethods
    {
        // ReSharper disable InconsistentNaming
        public const int WS_CHILD = 0x40000000;
        public const int WS_CLIPCHILDREN = 0x20000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_MOSEWHEEL = 0x020A;
        public const int IDC_ARROW = 32512;
        public const int WM_MOUSEMOVE = 512;
        public const int WM_MOUSELEAVE = 0x02A3;
        public const int WM_MBUTTONDBLCLK = 0x0209;

        public const uint TME_LEAVE = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        public struct WndClassEx
        {
            public uint cbSize;
            public uint style;
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public WndProc lpfnWndProc;
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;

        }

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        public static readonly WndProc DefaultWindowProc = DefWindowProc;

        [DllImport("user32.dll", EntryPoint = "CreateWindowEx", CharSet = CharSet.Auto)]
        public static extern IntPtr CreateWindowEx(
            int exStyle,
            string className,
            string windowName,
            int style,
            int x, int y,
            int width, int height,
            IntPtr hwndParent,
            IntPtr hMenu,
            IntPtr hInstance,
            [MarshalAs(UnmanagedType.AsAny)] object pvParam);

        [DllImport("user32.dll", EntryPoint = "DestroyWindow", CharSet = CharSet.Auto)]
        public static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string module);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.U2)]
        public static extern short RegisterClassEx([In] ref WndClassEx lpwcx);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);
        // ReSharper restore InconsistentNaming

        [DllImport("user32.dll")]
        public static extern int SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        public static extern IntPtr SetCapture(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [StructLayout(LayoutKind.Sequential)]
        public struct NativePoint
        {
            public int X;
            public int Y;
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref NativePoint point);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern int ShowCursor(bool bShow);

        [StructLayout(LayoutKind.Sequential)]
        public struct TRACKMOUSEEVENT
        {
            public int cbSize;
            public uint dwFlags;
            public IntPtr hWnd;
            public uint dwHoverTime;
        }
        [DllImport("user32.dll")]
        public static extern int TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        public static int GetXLParam(int lParam)
        {
            return LowWord(lParam);
        }
        public static int GetYLParam(int lParam)
        {
            return HighWord(lParam);
        }
        public static int GetWheelDeltaWParam(int wParam)
        {
            return HighWord(wParam);
        }
        public static int LowWord(int input)
        {
            return (short)(input & 0xffff);
        }
        public static int HighWord(int input)
        {
            return (short)(input >> 16);
        }
    }
}