using System;
using AVFXLib;
using AVFXLib.AVFX;
using AVFXLib.Models;
using AVFXLib.Main;
using Newtonsoft.Json.Linq;
using System.IO;
using AVFXTools.GraphicsBase;
using AVFXTools.FFXIV;
using AVFXTools.Main;

namespace AVFXTools
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //JObject obj = Reader.readJSON("D:\\FFXIV\\TOOLS\\DataPuller\\AVFXStudio\\test\\test.json");
            //AVFXBase avfx = new AVFXBase();
            //avfx.read(obj);
            //avfx.Print(0);

            //avfx2.Print(0);
            //AVFXNode outNode = avfx2.toAVFX(); // TEST AVFX EQUALITY
            //Console.WriteLine(node.EqualsNode(outNode));

            //JObject obj = (JObject)avfx2.toJSON(); // TEST JSON EXPORT
            //File.WriteAllText("D:\\FFXIV\\TOOLS\\DataPuller\\AVFXStudio\\test\\test.json", obj.ToString());

            //ModelReader.ReadModel("D:\\FFXIV\\models\\script.glb");
            //ModelReader.ExportAllModels("D:\\FFXIV\\TOOLS\\DataPuller\\avfx\\test.gltf", avfx2);





            const string GameDirectory = @"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn";
            ResourceGetter getter = new ResourceGetter(GameDirectory);

            //AVFXNode node = Reader.readAVFX("D:\\FFXIV\\TOOLS\\DataPuller\\avfx\\BASE_2.avfx");

            // dead hive -> @"chara/weapon/w1501/obj/body/b0036/vfx/eff/vw0002.avfx"
            // seeing horde -> @"chara/weapon/w1501/obj/body/b0050/vfx/eff/vw0002.avfx"
            // ucob -> @"chara/weapon/w2101/obj/body/b0006/vfx/eff/vw0002.avfx"
            // tea -> @"chara/weapon/w2201/obj/body/b0049/vfx/eff/vw0001.avfx"

            //var model = new WepModel(@"chara/weapon/w2201/obj/body/b0049/model/w2201b0049.mdl", getter); // tea
            //var model = new WepModel(@"chara/weapon/w2101/obj/body/b0006/model/w2101b0006.mdl", getter); // ucob
            //var model = new WepModel(@"chara/weapon/w1501/obj/body/b0050/model/w1501b0050.mdl", getter); // nid
            var model = new WepModel(@"chara/weapon/w1501/obj/body/b0036/model/w1501b0036.mdl", getter); // rav


            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w2201/obj/body/b0049/vfx/eff/vw0001.avfx")); // tea
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w2101/obj/body/b0006/vfx/eff/vw0002.avfx")); // ucob
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w1501/obj/body/b0050/vfx/eff/vw0002.avfx")); // nid
            AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w1501/obj/body/b0036/vfx/eff/vw0002.avfx")); // rav

            AVFXBase avfx2 = new AVFXBase();
            avfx2.read(node);

            //JObject obj = (JObject)avfx2.toJSON(); // TEST JSON EXPORT
            //File.WriteAllText(@"D:\FFXIV\TOOLS\AVFXTools\AVFXTools\out\rav.json", obj.ToString());


            // -------------------
            VeldridStartupWindow window = new VeldridStartupWindow("AVFX");
            //Viewer viewer = new Viewer(window, avfx2, getter, model);

            MainViewer mv = new MainViewer(window, avfx2, getter, model);
            window.Run();
        }
    }
}
