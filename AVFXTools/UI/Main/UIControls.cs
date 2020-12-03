using AVFXLib.AVFX;
using AVFXLib.Main;
using AVFXLib.Models;
using ImGuiNET;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ImGui.SetNextWindowPos(new Vector2(Main.Main.Window.Width / 2 - 100, 40));
            if (ImGui.Begin("Just An Approximation", ref AlwaysOpen, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground))
            {
                ImGui.Text("Just An Approximation");
                ImGui.End();
            }
            //=========================
            if (ImGui.BeginMainMenuBar())
            {
                // === AVFX ====
                if (ImGui.BeginMenu("AVFX##Menu"))
                {
                    if (ImGui.MenuItem("Open Local AVFX", "Ctrl+O"))
                    {
                        Stream stream;
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "AVFX files (*.avfx)|*.avfx|All files (*.*)|*.*";
                        fileDialog.RestoreDirectory = true;
                        if (fileDialog.ShowDialog() == DialogResult.OK)
                        {
                            if ((stream = fileDialog.OpenFile()) != null)
                            {
                                AVFXNode node = Reader.readAVFX(new BinaryReader(stream));
                                Main.Main.AVFX = new AVFXBase();
                                Main.Main.AVFX.read(node);
                                Main.Main.refreshGraphics();
                                Main.Main.refreshUI();
                                stream.Close();
                            }
                        }
                    }
                    if (ImGui.MenuItem("Open AVFX From Game", null))
                    {
                        AVFXFromGameDialog = true;
                    }
                    if (ImGui.MenuItem("Export", "Ctrl+S"))
                    {
                        AVFXNode node = Main.AVFX.toAVFX();
                        byte[] bytes = node.toBytes();
                        Save("AVFX files (*.avfx)|*.avfx|All files (*.*)|*.*", bytes);
                    }
                    if (ImGui.MenuItem("Export to JSON", null))
                    {
                        JObject json = (JObject)Main.AVFX.toJSON();
                        byte[] bytes = Util.StringToBytes(json.ToString());
                        Save("JSON files (*.json)|*.json|All files (*.*)|*.*", bytes);
                    }
                    ImGui.EndMenu();
                }
                // === MODEL ====
                if (ImGui.BeginMenu("Model##Menu"))
                {
                    if (ImGui.MenuItem("Open Model From Game", null))
                    {
                        MDLFromGameDialog = true;
                    }
                    ImGui.EndMenu();
                }
                // === DEBUG ===
                if (ImGui.BeginMenu("Debug##Menu"))
                {
                    if (ImGui.MenuItem("Export Raw Structure", null))
                    {
                        byte[] bytes = Util.StringToBytes(Main.Main.LastImportNode.exportString(0));
                        Save("TXT files (*.txt)|*.txt|All files (*.*)|*.*", bytes);
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndMainMenuBar();
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

        public static void Save(string Filter, byte[] bytes)
        {
            Stream stream;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = Filter;
            saveDialog.RestoreDirectory = true;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if ((stream = saveDialog.OpenFile()) != null)
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }
        }
    }
}
