using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AVFXLib.AVFX
{
    public class AVFXBlank : AVFXNode
    {
        public AVFXBlank() : base("")
        {
        }

        public override byte[] toBytes()
        {
            return new byte[] { };
        }

        public override bool EqualsNode(AVFXNode node)
        {
            if(!(node is AVFXBlank))
            {
                Console.WriteLine("Not Blank");
                return false;
            }
            return true;
        }

        public override void Print(int level)
        {
        }
    }
}
