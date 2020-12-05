using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid;

namespace AVFXTools.GraphicsBase
{
    public interface ApplicationWindow
    {
        event Action<float> Rendering;
        event Action<GraphicsDevice, ResourceFactory, Swapchain> GraphicsDeviceCreated;
        event Action GraphicsDeviceDestroyed;
        event Action Resized;
        event Action<KeyEvent> KeyPressed;

        uint Width { get; }
        uint Height { get; }

        void Run();
    }
}
