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
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w1501/obj/body/b0036/vfx/eff/vw0002.avfx")); // rav
            AVFXNode node = Reader.readAVFX(getter.GetData(@"vfx/common/eff/z3of_stlp1_c0c.avfx")); // omega

            /*
             * 
             *  Arcane Bulwark vfx/common/eff/pc_skin01ah.avfx 
                Artificially Enhanced (Annia) vfx/common/eff/c601_red_c0v.avfx
                Artificially Enhanced (Julia) vfx/common/eff/c601_blue_c0v.avfx 
                Diamondback vfx/common/eff/st_hard_aoz0f.avfx
                Light in the Dark vfx/common/eff/m0565_stlplight01_c0p.avfx 
                Local Resonance vfx/common/eff/z3of_stlp1_c0c.avfx   <--------------
                Mummification vfx/common/eff/noroi_awl0f.avfx 
                Ominous Wind vfx/common/eff/curse_wind_stloop_c0i.avfx
                Remote Resonance vfx/common/eff/z3of_stlp2_c0c.avfx -

                Astral Effect vfx/common/eff/n4g7_stlp1_c0x.avfx 
                Blood of the Dragon vfx/common/eff/dk10ht_drg0c.avfx
                Cursekeeper vfx/common/eff/m0532_stlp1c0x.avfx 
                Grudge vfx/common/eff/st_akama_kega0j.avfx
                Life of the Dragon - vfx/common/eff/dk10ht_rdg0c.avfx 
                Mortal Flame vfx/common/eff/m0073statuslp01t0w.avfx
                Resonant vfx/common/eff/dkst_over_p0f.avfx 
                Umbral Effect vfx/common/eff/n4g7_stlp1_c1x.avfx 
                Unrelenting Anguish vfx/common/eff/aramidama_stloop_c0i.avfx

                Fury's Bolt (boss) vfx/common/eff/daiichiram_stlp01p.avfx
                Greased Lightning IV vfx/common/eff/dk10ht_sip3t.avfx 
                Guardian Spirit vfx/common/eff/c0101_aura_c0t.avfx
                Inner Release vfx/common/eff/dk10ht_ang0c.avfx 
                Neutral Sect vfx/common/eff/abi_as027c1h.avfx
                Temperance - vfx/common/eff/cnj17wing_c0h.avfx
                The One Dragon vfx/common/eff/m0501_statusloop01c0i.avfx
                Unwavering Will vfx/common/eff/m0648statuslp01c0w.avfx 
                Waxing Nocturne vfx/common/eff/st_berserk_aoz0f.avfx 
             */

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
