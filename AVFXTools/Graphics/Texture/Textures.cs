using AVFXLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veldrid;

using Pfim;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Veldrid.ImageSharp;
using AVFXTools.FFXIV;
using System.IO;

namespace AVFXTools.Main
{
    public class Textures
    {
        public ResourceGetter Getter;
        public TextureView EmptyView;
        public Core C;
        public List<ImageView> ImageViews = new List<ImageView>();
        public ImageView EmptyImageView;

        public struct ImageView
        {
            public TextureView View;
            public Image<Rgba32> Image;
            public ImageView(TextureView view, Image<Rgba32> img)
            {
                View = view;
                Image = img;
            }
        }

        public Textures(List<AVFXTexture> textures, ResourceGetter getter, Core core)
        {
            C = core;
            Getter = getter;
            // =================
            foreach (var tex in textures)
            {
                AddTexture(tex.Path.Value);
            }
            // =================
            Image<Rgba32> emptyImage = new Image<Rgba32>(64, 64);
            ImageSharpTexture EmptyTex = new ImageSharpTexture(emptyImage);
            EmptyImageView = new ImageView(C.Factory.CreateTextureView(EmptyTex.CreateDeviceTexture(C.GD, C.Factory)), emptyImage);
        }

        public void AddTexture(string path)
        {
            ImageViews.Add(GetImageView(path));
        }
        public void RemoveTexture(int idx)
        {
            ImageViews[idx].View.Dispose();
            ImageViews.RemoveAt(idx);
        }
        public void UpdateTexture(int idx, string path)
        {
            ImageViews[idx].View.Dispose();
            ImageViews[idx] = GetImageView(path);
        }

        public ImageView GetImageView(string path)
        {
            path = path.Replace("\u0000", "");
            Image<Rgba32> img = new Image<Rgba32>(64, 64);
            try
            {
                bool ddsResult = Getter.GetDDS(path, out var bytes);
                if (ddsResult)
                {
                    var image = Pfim.Pfim.FromStream(new MemoryStream(bytes));
                    img = ReadFromDDS(image);
                }
            }
            catch (Exception e)
            {
            }

            var ImgTex = new ImageSharpTexture(img);
            return new ImageView(C.Factory.CreateTextureView(ImgTex.CreateDeviceTexture(C.GD, C.Factory)), img);
        }

        public TextureView GetView(int idx)
        {
            if (idx == -1 || idx >= ImageViews.Count()) return EmptyImageView.View;
            return ImageViews[idx].View;
        }

        public Sampler GetSampler(LiteralEnum<TextureBorderType> U, LiteralEnum<TextureBorderType> V)
        {
            SamplerDescription MaskSamplerD = SamplerDescription.Aniso4x;
            if (U.Assigned)
            {
                MaskSamplerD.AddressModeU = GetSamplerMode(U.Value);
            }
            if (V.Assigned)
            {
                MaskSamplerD.AddressModeV = GetSamplerMode(V.Value);
            }
            return C.Factory.CreateSampler(MaskSamplerD);
        }

        public static SamplerAddressMode GetSamplerMode(TextureBorderType mode)
        {
            switch (mode)
            {
                case TextureBorderType.Repeat:
                    return SamplerAddressMode.Wrap;
                case TextureBorderType.Clamp:
                    return SamplerAddressMode.Clamp;
                case TextureBorderType.Mirror:
                    return SamplerAddressMode.Mirror;
            }
            return SamplerAddressMode.Wrap;
        }

        // ========== STATIC ===========

        public static Image<Rgba32> ReadFromDDS(string path)
        {
            var image = Pfim.Pfim.FromFile(path);
            return ReadFromDDS(image);
        }

        public static Image<Rgba32> ReadFromDDS(Pfim.IImage image)
        {
            var BytesPerPixel = image.BitsPerPixel / 8;
            int Width = image.Width;
            int Height = image.Height;
            byte[] NewData = new byte[4 * Width * Height];

            switch (image.Format)
            {
                case ImageFormat.Rgba32:
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            int originalPos = (y * image.Stride) + x * BytesPerPixel;
                            int newPos = (y * 4 * Width) + x * 4;
                            NewData[newPos + 0] = image.Data[originalPos + 2];
                            NewData[newPos + 1] = image.Data[originalPos + 1];
                            NewData[newPos + 2] = image.Data[originalPos + 0];
                            NewData[newPos + 3] = image.Data[originalPos + 3];
                        }
                    }
                    break;
                case ImageFormat.Rgb24:
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            int originalPos = (y * image.Stride) + x * BytesPerPixel;
                            int newPos = (y * 4 * Width) + x * 4;
                            NewData[newPos + 0] = image.Data[originalPos + 2];
                            NewData[newPos + 1] = image.Data[originalPos + 1];
                            NewData[newPos + 2] = image.Data[originalPos + 0];
                            NewData[newPos + 3] = 255;
                        }
                    }
                    break;
                case ImageFormat.Rgb8:
                    for (int y = 0; y < Height; y++)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            int originalPos = (y * image.Stride) + x * BytesPerPixel;
                            int newPos = (y * 4 * Width) + x * 4;
                            NewData[newPos + 0] = 255;
                            NewData[newPos + 1] = 255;
                            NewData[newPos + 2] = 255;
                            NewData[newPos + 3] = image.Data[originalPos];
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Unsupported Pixel Format");
                    break;
            }
            return Image.LoadPixelData<Rgba32>(NewData, Width, Height);
        }
    }
}
