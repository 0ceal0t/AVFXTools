using System;
using AVFXLib.AVFX;
using AVFXLib.Models;
using AVFXLib.Main;
using AVFXTools.FFXIV;
using AVFXTools.Configuration;
using AVFXTools.Main;
using AVFXTools.Views;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections.Generic;

namespace AVFXTools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static AppSettings Settings;
        public static AppUpdates UpdateManager;
        public static UpdateWindow UpdateView;
        public MainViewer Viewer;

        public App()
        {
        }

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            UpdateManager = new AppUpdates();
            UpdateManager.OnUpdateCheckFinished += DoneWithUpdates;
            UpdateView = new UpdateWindow();
            UpdateView.Show();
            UpdateManager.Run();
        }

        public void DoneWithUpdates(object sender, EventArgs e)
        {

            Settings = new AppSettings();
            Settings.RequestGamePath();
            ResourceGetter getter = new ResourceGetter(Settings.GamePath);

            //var model = new WepModel(@"chara/weapon/w2201/obj/body/b0049/model/w2201b0049.mdl", getter); // tea
            //var model = new WepModel(@"chara/weapon/w2101/obj/body/b0006/model/w2101b0006.mdl", getter); // ucob
            //var model = new WepModel(@"chara/weapon/w1501/obj/body/b0050/model/w1501b0050.mdl", getter); // nid
            // @"chara/weapon/w1501/obj/body/b0036/model/w1501b0036.mdl"

            WepModel model = null;
            if (Settings.ModelOnLoad)
            {
                var mdlResult = getter.GetModel(Settings.ModelPath, out var mdlDef);
                if (mdlResult)
                {
                    model = new WepModel(mdlDef, getter);
                }
                else
                {
                    ApplicationBase.Logger.WriteError("Unable to find model");
                }
            }


            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w2201/obj/body/b0049/vfx/eff/vw0001.avfx")); // tea
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w2101/obj/body/b0006/vfx/eff/vw0002.avfx")); // ucob
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"chara/weapon/w1501/obj/body/b0050/vfx/eff/vw0002.avfx")); // nid
            // @"chara/weapon/w1501/obj/body/b0036/vfx/eff/vw0002.avfx"
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"vfx/common/eff/z3of_stlp1_c0c.avfx")); // omega
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"vfx/common/eff/cnj17wing_c0h.avfx")); // misc
            //AVFXNode node = Reader.readAVFX(getter.GetData(@"vfx/common/eff/daiichiram_stlp01p.avfx")); // misc

            AVFXBase avfx = null;
            AVFXNode node = null;
            if (Settings.VFXOnLoad)
            {
                var dataResult = getter.GetData(Settings.VFXPath, out var bytes);
                if (dataResult)
                {
                    node = Reader.readAVFX(bytes, out List<string> messages);
                    foreach (var m in messages)
                    {
                        ApplicationBase.Logger.WriteWarning(m);
                    }
                    avfx = new AVFXBase();
                    avfx.read(node);
                }
                else
                {
                    ApplicationBase.Logger.WriteError("Unable to find VFX");
                }
            }

            VFXViewer mainW = new VFXViewer(avfx, getter, model);
            mainW._viewer.LastImportNode = node;
            mainW.Show();

            UpdateView.Close();
        }

        // ===== UTILS =====
        public static void ShutdownAll(int i)
        {
            if (System.Windows.Forms.Application.MessageLoop)
                System.Windows.Forms.Application.Exit();
            else
                Environment.Exit(i);
        }
        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else { }
        }
    }
}
