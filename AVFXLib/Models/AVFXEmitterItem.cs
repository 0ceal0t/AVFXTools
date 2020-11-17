using AVFXLib.AVFX;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVFXLib.Models
{
    public class AVFXEmitterItem : AVFXEmitterIteration // how are these different than ItPr ? I have no idea
    {
        public const string NAME = "ItEm";

        public AVFXEmitterItem() : base()
        {
        }

        public override AVFXNode toAVFX()
        {
            AVFXNode res = base.toAVFX();
            res.Name = NAME;
            return res;
        }

        public override void Print(int level)
        {
            Console.WriteLine("{0}------- ItEm --------", new String('\t', level));
            foreach (AVFXEmitterIterationItem Item in Items)
            {
                Output(Item, level);
            }
        }
    }
}
