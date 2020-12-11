using AVFXLib.Main;
using ImGuiNET;
using System;
using System.IO;
using System.Numerics;

namespace AVFXTools.UI
{
    public class UILogger
    {
        public UIMain Main;
        public bool _DrawOnce = false;

        public UILogger(UIMain main)
        {
            Main = main;
        }

        public void Draw()
        {
            if (!_DrawOnce)
            {
                ImGui.SetNextWindowPos(new Vector2(Main.Main.Window.WindowWidth - 600, Main.Main.Window.WindowHeight - 400));
                ImGui.SetNextWindowSize(new Vector2(600, 400));
                _DrawOnce = true;
            }

            ImGui.Begin("Log");
            ImGui.BeginChild("Log##Lines");

            for(int i = 0; i < 100; i++)
            {
                ImGui.Text(i.ToString());
            }

            ImGui.EndChild();
            ImGui.End();
        }
    }
}
