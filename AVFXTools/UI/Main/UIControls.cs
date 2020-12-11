using AVFXLib.Main;
using ImGuiNET;
using System;
using System.IO;
using System.Numerics;

namespace AVFXTools.UI
{
    public class UIControls
    {
        public UIMain Main;
        //=============
        public bool AVFXFromGameDialog = false;
        public bool MDLFromGameDialog = false;

        public static uint INPUT_SIZE = 80;
        public byte[] avfxPathBytes = new byte[INPUT_SIZE];
        public byte[] mdlPathBytes = new byte[INPUT_SIZE];
        public bool AlwaysOpen = true;

        public UIControls(UIMain main)
        {
            Main = main;
        }

        public void Draw()
        {
            ImGui.SetNextWindowSize(new Vector2(400, 100));
            ImGui.SetNextWindowPos(new Vector2(Main.Main.Window.WindowWidth / 2 - 100, 40));
            if (ImGui.Begin("Just An Approximation", ref AlwaysOpen, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground))
            {
                ImGui.Text("Just An Approximation");
                ImGui.End();
            }
            //=========================
            ImGui.SetNextWindowSize(new Vector2(600, 200));
            if (AVFXFromGameDialog)
            {
                ImGui.OpenPopup("Game AVFX");
            }

            if (ImGui.BeginPopupModal("Game AVFX", ref AVFXFromGameDialog))
            {
                ImGui.InputText("AVFX Path", avfxPathBytes, INPUT_SIZE);
                if (ImGui.Button("OK##AVFX path"))
                {
                    string str = Util.BytesToString(avfxPathBytes).Trim('\0').Trim(' ');
                    AVFXFromGameDialog = false;
                    Main.Main.OpenGameAVFX(str);
                    Main.Main.refreshGraphics();
                    Main.Main.refreshUI();
                }
                ImGui.EndPopup();
            }
            //=========================
            ImGui.SetNextWindowSize(new Vector2(600, 200));
            if (MDLFromGameDialog)
            {
                ImGui.OpenPopup("Game MDL");
            }

            if (ImGui.BeginPopupModal("Game MDL", ref MDLFromGameDialog))
            {
                ImGui.InputText("MDL Path", mdlPathBytes, INPUT_SIZE);
                if (ImGui.Button("OK##MDL path"))
                {
                    string str = Util.BytesToString(mdlPathBytes).Trim('\0').Trim(' ');
                    AVFXFromGameDialog = false;
                    Main.Main.OpenGameMdl(str);
                    Main.Main.refreshGraphics();
                }
                ImGui.EndPopup();
            }
        }
    }
}
