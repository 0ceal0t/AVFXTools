using System;
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

        public ResourceGetter(string gamePath)
        {
            realm = new ARealmReversed(gamePath, SaintCoinach.Ex.Language.English);
            HavokInterop.InitializeMTA();
        }

        public ModelDefinition GetModel(string file)
        {
            File mdlBase;
            bool result = realm.Packs.TryGetFile(file, out mdlBase);
            return ((ModelFile)mdlBase).GetModelDefinition();
        }

        public Skeleton GetSkeleton(string file)
        {
            File mdlBase;
            bool result = realm.Packs.TryGetFile(file, out mdlBase);
            if(result == false)
            {
                return null;
            }
            return new Skeleton(new SklbFile(mdlBase));
        }

        public byte[] GetData(string file)
        {
            SaintCoinach.IO.File fileOut;
            bool result = realm.Packs.TryGetFile(file, out fileOut);
            return result ? fileOut.GetData() : new byte[0];
        }

        public byte[] GetDDS(string file) // .atex -> .dds
        {
            SaintCoinach.IO.File fileOut;
            bool result = realm.Packs.TryGetFile(file, out fileOut);
            FileCommonHeader header = fileOut.CommonHeader;
            return DDSConverter.GetDDS(header.GetBuffer(), fileOut.GetData());
        }

        public SaintCoinach.Xiv.IXivSheet<SaintCoinach.Xiv.XivRow> GetModelSkeleton()
        {
            return realm.GameData.GetSheet("ModelSkeleton");
        }
    }
}
