using SaintCoinach.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.FFXIV
{
    public class DDSConverter // .atex -> .dds
    {
        struct DDS_PIXELFORMAT
        {
            public uint dwSize;
            public uint dwFlags;
            public uint dwFourCC;
            public uint dwRGBBitCount;
            public uint dwRBitMask;
            public uint dwGBitMask;
            public uint dwBBitMask;
            public uint dwABitMask;
        };

        struct DDS_HEADER
        {
            public uint dwSize;
            public uint dwFlags;
            public uint dwHeight;
            public uint dwWidth;
            public uint dwPitchOrLinearSize;
            public uint dwDepth;
            public uint dwMipMapCount;
            public uint dwReserved0;
            public uint dwReserved1;
            public uint dwReserved2;
            public uint dwReserved3;
            public uint dwReserved4;
            public uint dwReserved5;
            public uint dwReserved6;
            public uint dwReserved7;
            public uint dwReserved8;
            public uint dwReserved19;
            public uint dwReserved10;

            public DDS_PIXELFORMAT ddspf;
            public uint dwCaps; //
            public uint dwCaps2;
            public uint dwCaps3;
            public uint dwCaps4;
            public uint dwReserved11;
        };

        enum DDSD_ENUM : uint
        {
            DDSD_CAPS = 0x1,//Required in every.dds file. 	
            DDSD_HEIGHT = 0x2,//Required in every.dds file.
            DDSD_WIDTH = 0x4,//Required in every.dds file.
            DDSD_PITCH = 0x8,//Required when pitch is provided for an uncompressed texture.
            DDSD_PIXELFORMAT = 0x1000,//Required in every.dds file.
            DDSD_MIPMAPCOUNT = 0x20000,//Required in a mipmapped texture.
            DDSD_LINEARSIZE = 0x80000,//Required when pitch is provided for a compressed texture.
            DDSD_DEPTH = 0x800000,//Required in a depth texture
        }

        enum DDPF_ENUM : uint
        {
            DDPF_ALPHAPIXELS = 0x1, // Texture contains alpha data; dwRGBAlphaBitMask contains valid data.
            DDPF_ALPHA = 0x2, // Used in some older DDS files for alpha channel only uncompressed data (dwRGBBitCount contains the alpha channel bitcount; dwABitMask contains valid data)
            DDPF_FOURCC = 0x4, // Texture contains compressed RGB data; dwFourCC contains valid data.
            DDPF_RGB = 0x40, // Texture contains uncompressed RGB data; dwRGBBitCountand the RGB masks(dwRBitMask, dwGBitMask, dwBBitMask) contain valid data.
        }

        public static byte[] GetDDS(byte[] headerdata, byte[] dataMain)
        {
            BinaryReader reader2 = new BinaryReader(new MemoryStream(dataMain));

            reader2.ReadInt32(); // idk
            int compressionType = reader2.ReadInt32();
            int width = reader2.ReadInt16();
            int height = reader2.ReadInt16();
            reader2.ReadInt16();
            int numMipMap = reader2.ReadInt16();

            DDS_HEADER header = new DDS_HEADER();
            DDS_PIXELFORMAT format = header.ddspf;
            format.dwFlags = (uint)(DDPF_ENUM.DDPF_ALPHAPIXELS | DDPF_ENUM.DDPF_FOURCC);
            header.dwFlags |= (uint)(DDSD_ENUM.DDSD_CAPS | DDSD_ENUM.DDSD_HEIGHT | DDSD_ENUM.DDSD_WIDTH | DDSD_ENUM.DDSD_PIXELFORMAT | DDSD_ENUM.DDSD_LINEARSIZE);
            header.dwFlags |= (uint)DDSD_ENUM.DDSD_MIPMAPCOUNT;

            format.dwSize = 32;
            header.dwSize = 124; // why set this if it MUST be 124?
            header.dwHeight = (uint)height;
            header.dwWidth = (uint)width;
            header.dwMipMapCount = (uint)numMipMap;
            header.dwCaps = 0x08 | 0x400000 | 0x1000; // DDSCAPS_COMPLEX | DDSCAPS_MIPMAP | DDSCAPS_TEXTURE

            // -----------------
            switch (compressionType){
                case 0x3420: // DX1
                    format.dwFourCC = 0x31545844;
                    header.dwPitchOrLinearSize = (uint)(Math.Max((uint)1, ((width + 3) / 4)) * Math.Max((uint)1, ((height + 3) / 4)) * 8);
                    break;
                case 0x3430: // DX3
                    format.dwFourCC = 0x33545844;
                    header.dwPitchOrLinearSize = (uint)(Math.Max((uint)1, ((width + 3) / 4)) * Math.Max((uint)1, ((height + 3) / 4)) * 16);
                    break;
                case 0x3431: // DX5
                    format.dwFourCC = 0x35545844;
                    header.dwPitchOrLinearSize = (uint)(Math.Max((uint)1, ((width + 3) / 4)) * Math.Max((uint)1, ((height + 3) / 4)) * 16);
                    break;
                default: // hopefully this always works. I kinda doubt it
                    //format.dwFlags = (uint)(DDPF_ENUM.DDPF_ALPHA | DDPF_ENUM.DDPF_RGB);

                    header.dwPitchOrLinearSize = (uint)(width * height);
                    format.dwFlags = (uint)(DDPF_ENUM.DDPF_ALPHA);
                    format.dwRGBBitCount = (uint)8;
                    format.dwABitMask = (uint)255;
                    header.dwCaps = 0x1000;

                    Console.WriteLine("non-DX compression type");
                    break;
            }

            header.ddspf = format;

            List<byte> data = new List<byte>();

            int size = System.Runtime.InteropServices.Marshal.SizeOf<DDS_HEADER>();
            byte[] headerBytes = new byte[size];

            var ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.StructureToPtr(header, ptr, false);
            System.Runtime.InteropServices.Marshal.Copy(ptr, headerBytes, 0, size);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);

            data.AddRange(System.Text.ASCIIEncoding.UTF8.GetBytes("DDS "));
            data.AddRange(headerBytes);

            reader2.BaseStream.Position = 0x50;
            byte[] restOfData = reader2.ReadBytes(dataMain.Length - (int)reader2.BaseStream.Position);

            data.AddRange(restOfData);

            return data.ToArray();

        }
    }
}
