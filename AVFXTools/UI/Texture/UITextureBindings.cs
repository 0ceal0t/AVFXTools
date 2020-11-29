using AVFXTools.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid;

namespace AVFXTools.UI
{
    public class UITextureBindings
    {
        public UIMain Main;
        // ====================
        public Textures TextureViews;
        public Dictionary<int, TextureInfo> IndexToPointer = new Dictionary<int, TextureInfo>();

        public UITextureBindings(UIMain main)
        {
            Main = main;
            // ==============
            TextureViews = new Textures(Main.AVFX.Textures, Main.Main.Getter, Main.Main.C);
            int idx = 0;
            foreach(var TextureV in TextureViews.Views)
            {
                IntPtr pointer = Main.I.GetOrCreateImGuiBinding(Main.Main.Factory, TextureV);
                IndexToPointer.Add(idx, new TextureInfo(pointer, TextureV.Target.Width, TextureV.Target.Height));
                idx++;
            }
        }
    }

    // ====================
    public struct TextureInfo
    {
        public IntPtr Pointer;
        public float Width;
        public float Height;

        public TextureInfo(IntPtr pointer, float width, float height)
        {
            Pointer = pointer;
            Width = width;
            Height = height;
        }
    }
}
