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
            init(gamePath);
        }

        public void init(string gamePath)
        {
            realm = new ARealmReversed(gamePath, SaintCoinach.Ex.Language.English);
            realm.Packs.GetPack(new SaintCoinach.IO.PackIdentifier("exd", SaintCoinach.IO.PackIdentifier.DefaultExpansion, 0)).KeepInMemory = true;

            HavokInterop.InitializeMTA();
        }

        public bool GetModel(string file, out ModelDefinition data)
        {
            File mdlBase;
            bool result = GetFile(file, out mdlBase);
            data = result ? ((ModelFile)mdlBase).GetModelDefinition() : null;
            return result;
        }

        public bool GetSkeleton(string file, out Skeleton data)
        {
            File mdlBase;
            bool result = GetFile(file, out mdlBase);
            data = result ? new Skeleton(new SklbFile(mdlBase)) : null;
            return result;
        }

        public bool GetData(string file, out byte[] data)
        {
            File fileOut;
            bool result = GetFile(file, out fileOut);
            data = result ? fileOut.GetData() : new byte[0];
            return result;
        }

        public bool GetDDS(string file, out byte[] data) // .atex -> .dds
        {
            File fileOut;
            bool result = GetFile(file, out fileOut);
            if (!result)
            {
                data = new byte[0];
                return false;
            }
            FileCommonHeader header = fileOut.CommonHeader;
            data = DDSConverter.GetDDS(header.GetBuffer(), fileOut.GetData());
            return result;
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
