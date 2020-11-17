using AVFXTools.FFXIV;
using SaintCoinach.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools
{
    public class Skeleton
    {
        static class Interop
        {
            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr loadSkeleton(byte[] rigData, int rigSize);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern void unloadSkeleton(IntPtr ptr);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getNumBones(IntPtr ptr);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getBoneNames(IntPtr ptr, [In, Out] string[] output);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getReferencePose(IntPtr ptr, [In, Out] InteropTransform[] output);

            [DllImport("hkAnimationInterop.dll", CallingConvention = CallingConvention.Cdecl)]
            public static extern int getParentIndices(IntPtr ptr, [In, Out] int[] output);
        }

        internal IntPtr _UnmanagedPtr;
        public SklbFile File { get; private set; }
        public int BoneCount { get; private set; }
        public int[] ParentBoneIndices { get; private set; }

        //public Matrix[] ReferencePose { get; private set; }

        public string[] BoneNames { get; private set; }
        public InteropTransform[] ReferencePosLocal { get; private set; }

        public Skeleton(SklbFile file)
        {
            this.File = file;

            _UnmanagedPtr = HavokInterop.Execute(() => Interop.loadSkeleton(file.HavokData, file.HavokData.Length));

            BoneCount = HavokInterop.Execute(() => Interop.getNumBones(_UnmanagedPtr));

            BoneNames = new string[BoneCount];
            HavokInterop.Execute(() => Interop.getBoneNames(_UnmanagedPtr, BoneNames));

            ParentBoneIndices = new int[BoneCount];
            HavokInterop.Execute(() => Interop.getParentIndices(_UnmanagedPtr, ParentBoneIndices));
            ReferencePosLocal = new InteropTransform[BoneCount];
            HavokInterop.Execute(() => Interop.getReferencePose(_UnmanagedPtr, ReferencePosLocal));

            // https://github.com/ufx/SaintCoinach/blob/5ec1b81521fde4c79d3d80fe5f8b8c2cd4961ebf/SaintCoinach.Graphics.Viewer/Skeleton.cs
            /*for (var target = 0; target < BoneCount; ++target)
            {
                var current = target;
                ReferencePose[target] = Matrix.Identity;
                while (current >= 0)
                {
                    ReferencePose[target] = ReferencePose[target] * referencePoseLocal[current].ToTransformationMatrix();

                    current = ParentBoneIndices[current];
                }
            }*/
        }
    }
}
