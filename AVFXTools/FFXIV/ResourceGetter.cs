using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SaintCoinach;
using SaintCoinach.Graphics;
using SaintCoinach.Imaging;
using SaintCoinach.IO;

namespace AVFXTools.FFXIV
{
    public class ResourceGetter
    {
        public ARealmReversed realm;
        private readonly ConcurrentDictionary<PackIdentifier, Pack> _Packs = new ConcurrentDictionary<PackIdentifier, Pack>();

        public ResourceGetter(string gamePath)
        {
            realm = new ARealmReversed(gamePath, SaintCoinach.Ex.Language.English);
            realm.Packs.GetPack(new SaintCoinach.IO.PackIdentifier("exd", SaintCoinach.IO.PackIdentifier.DefaultExpansion, 0)).KeepInMemory = true;

            HavokInterop.InitializeMTA();
        }

        public ModelDefinition GetModel(string file)
        {
            File mdlBase;
            bool result = GetFile(file, out mdlBase);
            return ((ModelFile)mdlBase).GetModelDefinition();
        }

        public Skeleton GetSkeleton(string file)
        {
            File mdlBase;
            bool result = GetFile(file, out mdlBase);
            if(result == false)
            {
                return null;
            }
            return new Skeleton(new SklbFile(mdlBase));
        }

        public byte[] GetData(string file)
        {
            File fileOut;
            bool result = GetFile(file, out fileOut);
            return result ? fileOut.GetData() : new byte[0];
        }

        public byte[] GetDDS(string file) // .atex -> .dds
        {
            File fileOut;
            bool result = GetFile(file, out fileOut);
            FileCommonHeader header = fileOut.CommonHeader;
            return DDSConverter.GetDDS(header.GetBuffer(), fileOut.GetData());
        }

        public bool GetFile(string path, out File fileOut)
        {
            if (GetPack(path, out var pack))
                return pack.TryGetFile(path, out fileOut);

            fileOut = null;
            return false;
        }

        public bool GetPack(string path, out Pack pack) // thanks a lot, SaintCoinach
        {
            pack = null;

            if (!PackIdentifier.TryGet(path, out var id))
                return false;

            byte number = id.Number;
            if (number > 100)
                number = 0;
            // I'm honestly confused why I needed to do this
            //it happens with paths like "vfx/action/ab_rampart/textures/soli011bm.atex", since the "ab" gets treated like pack offset of 171 or something, when it's not
            PackIdentifier ident = new PackIdentifier(id.Type, id.Expansion, number);

            pack = _Packs.GetOrAdd(ident, i => new Pack(realm.Packs, realm.Packs.DataDirectory, ident));
            return true;
        }

        public SaintCoinach.Xiv.IXivSheet<SaintCoinach.Xiv.XivRow> GetModelSkeleton()
        {
            return realm.GameData.GetSheet("ModelSkeleton");
        }
    }
}
