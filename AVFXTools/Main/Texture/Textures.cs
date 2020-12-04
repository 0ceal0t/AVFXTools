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
        public Image<Rgba32>[] Images;
        public TextureView[] Views;
        public TextureView EmptyView;
        public Core C;

        public Textures(List<AVFXTexture> textures, ResourceGetter getter, Core core)
        {
            C = core;
            // =================
            Images = new Image<Rgba32>[textures.Count];
            for (int idx = 0; idx < textures.Count; idx++)
            {
                string p = textures[idx].Path.Value.Replace("\u0000", "");
                try
                {
                    byte[] bytes = getter.GetDDS(p);
                    var image = Pfim.Pfim.FromStream(new MemoryStream(bytes));
                    Images[idx] = ReadFromDDS(image);
                }
                catch(Exception e)
                {
                    Images[idx] = new Image<Rgba32>(64, 64);
                }
            }
            // ===================
            Views = new TextureView[textures.Count];
            for(int texIdx = 0; texIdx < textures.Count; texIdx++)
            {
                ImageSharpTexture ImgTex = new ImageSharpTexture(Images[texIdx]);
                Views[texIdx] = C.Factory.CreateTextureView(ImgTex.CreateDeviceTexture(C.GD, C.Factory));
            }
            ImageSharpTexture EmptyTex = new ImageSharpTexture(new Image<Rgba32>(64, 64));
            EmptyView = C.Factory.CreateTextureView(EmptyTex.CreateDeviceTexture(C.GD, C.Factory));
        }

        public TextureView GetView(int idx)
        {
            if (idx == -1 || idx >= Views.Length) return EmptyView;
            return Views[idx];
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
