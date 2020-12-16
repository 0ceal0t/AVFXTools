using AVFXLib.Main;
using AVFXTools.ApplicationBase;
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

        public void TestLog()
        {
            Logger.WriteInfo("info test");
            Logger.WriteWarning("warning test");
            Logger.WriteError("error test");
            Logger.WriteInfo("longlinetest longlinetest longlinetest longlinetest longlinetest longlinetest longlinetest longlinetest ");
            for(int i = 0; i < 100; i++)
            {
                Logger.WriteInfo(i.ToString());
            }
        }

        public void Draw()
        {
            if (!_DrawOnce)
            {
                ImGui.SetNextWindowPos(new Vector2(Main.Main.Window.WindowWidth - 610, Main.Main.Window.WindowHeight - 410));
                ImGui.SetNextWindowSize(new Vector2(600, 400));
                _DrawOnce = true;
            }

            ImGui.Begin("Log");
            ImGui.BeginChild("Log##Lines");

            foreach (var logLine in Logger.Items)
            {
                switch (logLine.Type)
                {
                    case LoggerType.INFO:
                        ImGui.TextColored(new Vector4(0.0f, 1.0f, 0.0f, 1.0f), "[INFO]: ");
                        break;
                    case LoggerType.WARNING:
                        ImGui.TextColored(new Vector4(1.0f, 1.0f, 0.0f, 1.0f), "[INFO]: ");
                        break;
                    case LoggerType.ERROR:
                        ImGui.TextColored(new Vector4(1.0f, 0.0f, 0.0f, 1.0f), "[INFO]: ");
                        break;
                }
                ImGui.SameLine();
                ImGui.TextWrapped(logLine.Text);
            }

            ImGui.EndChild();
            ImGui.End();
        }
    }
}
