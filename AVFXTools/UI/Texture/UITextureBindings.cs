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

        public List<TextureInfo> IdxToPointer = new List<TextureInfo>();

        public UITextureBindings(UIMain main)
        {
            Main = main;
            // ==============
            TextureViews = new Textures(Main.AVFX.Textures, Main.Main.Getter, Main.Main.C);

            foreach(var TextureImageView in TextureViews.ImageViews)
            {
                var TextureV = TextureImageView.View;
                IntPtr pointer = Main.I.GetOrCreateImGuiBinding(Main.Main.Factory, TextureV);
                IdxToPointer.Add(new TextureInfo(pointer, TextureV.Target.Width, TextureV.Target.Height));
            }
        }

        public void addTextureBinding(string path)
        {
            TextureViews.AddTexture(path);
            var TextureV = TextureViews.ImageViews[TextureViews.ImageViews.Count() - 1].View; // the new one
            IntPtr pointer = Main.I.GetOrCreateImGuiBinding(Main.Main.Factory, TextureV);
            IdxToPointer.Add(new TextureInfo(pointer, TextureV.Target.Width, TextureV.Target.Height));
        }
        public void removeTextureBinding(int idx)
        {
            var TextureV = TextureViews.ImageViews[idx].View;
            Main.I.RemoveImGuiBinding(TextureV);
            TextureViews.RemoveTexture(idx);
            IdxToPointer.RemoveAt(idx);
        }
        public void updateTextureBinding(string path, int idx)
        {
            var TextureV = TextureViews.ImageViews[idx].View;
            Main.I.RemoveImGuiBinding(TextureV);
            TextureViews.UpdateTexture(idx, path);
            TextureV = TextureViews.ImageViews[idx].View;
            IntPtr pointer = Main.I.GetOrCreateImGuiBinding(Main.Main.Factory, TextureV);
            IdxToPointer[idx] = new TextureInfo(pointer, TextureV.Target.Width, TextureV.Target.Height);
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
