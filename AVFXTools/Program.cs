using System;
using AVFXLib;
using AVFXLib.AVFX;
using AVFXLib.Models;
using AVFXLib.Main;
using AVFXTools.GraphicsBase;
using AVFXTools.FFXIV;
using AVFXTools.Configuration;
using AVFXTools.Main;

using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace AVFXTools
{
    public class Program
    {
        public static AppSettings Settings;
        public static AppUpdates updateManager;
        static ManualResetEvent ResetEvent = new ManualResetEvent(false);

        [STAThread]
        static void Main(string[] args)
        {
            //ModelReader.ReadModel("D:\\FFXIV\\models\\script.glb");
            //ModelReader.ExportAllModels("D:\\FFXIV\\TOOLS\\DataPuller\\avfx\\test.gltf", avfx2);

            Settings = new AppSettings();
            Settings.RequestGamePath();

#if !AVFX_NOAUTOUPDATE
            updateManager = new AppUpdates();
            updateManager.OnUpdateCheckFinished += DoneWithUpdates;
            updateManager.Run();
            ResetEvent.WaitOne();
#else
           DoneWithUpdates(null, null);
#endif
        }

        public static void DoneWithUpdates(object sender, EventArgs e)
        {
            ResourceGetter getter = new ResourceGetter(Settings.GamePath);

            //var model = new WepModel(@"chara/weapon/w2201/obj/body/b0049/model/w2201b0049.mdl", getter); // tea
            //var model = new WepModel(@"chara/weapon/w2101/obj/body/b0006/model/w2101b0006.mdl", getter); // ucob
            //var model = new WepModel(@"chara/weapon/w1501/obj/body/b0050/model/w1501b0050.mdl", getter); // nid
            var model = new WepModel(@"chara/weapon/w1501/obj/body/b0036/model/w1501b0036.mdl", getter); // rav


            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w2201/obj/body/b0049/vfx/eff/vw0001.avfx")); // tea
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w2101/obj/body/b0006/vfx/eff/vw0002.avfx")); // ucob
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w1501/obj/body/b0050/vfx/eff/vw0002.avfx")); // nid
            AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w1501/obj/body/b0036/vfx/eff/vw0002.avfx")); // rav

            //AVFXNode node = Reader.readAVFX(getter.GetData(@"vfx/common/eff/z3of_stlp1_c0c.avfx")); // omega
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"vfx/common/eff/cnj17wing_c0h.avfx")); // misc
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"vfx/common/eff/daiichiram_stlp01p.avfx")); // misc

            AVFXBase avfx2 = new AVFXBase();
            avfx2.read(node);

            // -------------------
            VeldridStartupWindow window = new VeldridStartupWindow("AVFXTools");
            MainViewer mv = new MainViewer(window, avfx2, getter, model);
            mv.LastImportNode = node;
            window.Run();
        }

        public static void Shutdown(int i)
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(i);
            }
        }
    }
}
