using AVFXLib.AVFX;
using AVFXLib.Main;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public abstract class LiteralBase: Base
    {
        // remember to pad out avfx name to 4
        public int Size { get; set; }

        public bool InJSON;
        public bool InAVFX;

        public LiteralBase(string jsonPath, string avfxName, int size, bool inJSON, bool inAVFX) : base(jsonPath, avfxName)
        {
            Size = size;
            InJSON = inJSON;
            InAVFX = inAVFX;
        }

        public abstract void read(JValue json);
        public abstract void read(AVFXLeaf node);

        public abstract string stringValue();
        public override void Print(int level)
        {
            Console.WriteLine("{0} {1} | {2}:  {3}", new String('\t', level), String.Join("/", JSONPath), AVFXName, stringValue());
        }
    }
}
