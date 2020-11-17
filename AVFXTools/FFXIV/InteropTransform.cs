using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXTools.FFXIV
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct InteropTransform
    {
        public InteropVector4 Translation;
        public InteropVector4 Scale;
        public InteropVector4 Rotation;
    }
}
