using AVFXLib.Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AVFXLib.AVFX
{
    public class AVFXLeaf: AVFXNode
    {
        public byte[] Contents { get; set; }
        public int Size { get; set; }

        public AVFXLeaf(string n, int s, byte[] c): base(n)
        {
            Name = n;
            Size = s;
            Contents = c;
        }

        public override byte[] toBytes()
        {
            byte[] bytes = new byte[8 + Util.RoundUp(Size)];
            byte[] name = Util.NameTo4Bytes(Name);
            byte[] size = Util.IntTo4Bytes(Size);
            Buffer.BlockCopy(name, 0, bytes, 0, 4);
            Buffer.BlockCopy(size, 0, bytes, 4, 4);
            Buffer.BlockCopy(Contents, 0, bytes, 8, Contents.Length);
            return bytes;
        }

        public override bool EqualsNode(AVFXNode node)
        {
            if(!(node is AVFXLeaf))
            {
                Console.WriteLine("Wrong Type {0}", node.Name);
                return false;
            }
            AVFXLeaf leaf = (AVFXLeaf)node;
            if (Name != leaf.Name)
            {
                Console.WriteLine("Wrong Name {0} {1}", Name, leaf.Name);
                return false;
            }
            if(Size != leaf.Size)
            {
                Console.WriteLine("Wrong Leaf Size {0} : {1} / {2} : {3}", Name, Size.ToString(), leaf.Name, leaf.Size.ToString());
                return false;
            }
            if(Contents.Length != leaf.Contents.Length)
            {
                Console.WriteLine("Wrong Contents Size {0} : {1} / {2} : {3}", Name, Contents.Length.ToString(), leaf.Name, leaf.Contents.Length.ToString());
                return false;
            }
            for(int idx = 0; idx < Contents.Length; idx++)
            {
                if(Contents[idx] != leaf.Contents[idx])
                {
                    Console.WriteLine("Wrong Contents in {0} byte {1} : {2} / {3}", Name, idx, Contents[idx].ToString(), leaf.Contents[idx].ToString());
                    Util.PrintBytes(Contents);
                    Util.PrintBytes(leaf.Contents);
                    return false;
                }
            }
            return true;
        }

        public override string exportString(int level)
        {
            return String.Format("{0}> {1} {2}\n", new String('\t', level), Name, Size);
        }
    }
}
