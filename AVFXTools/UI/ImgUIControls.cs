using AVFXLib.AVFX;
using AVFXLib.Main;
using AVFXLib.Models;
using ImGuiNET;
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
    public class ImgUIControls
    {
        public ImgUIMain Main;
        //=============
        public bool AVFXFromGameDialog = false;
        public static uint INPUT_SIZE = 40;
        public byte[] avfxPathBytes = new byte[INPUT_SIZE];
        public byte[] mdlPathBytes = new byte[INPUT_SIZE];
        public bool AlwaysOpen = true;

        public ImgUIControls(ImgUIMain main)
        {
            Main = main;
        }

        public void Draw()
        {
            ImGui.SetNextWindowSize(new Vector2(400, 100));
            ImGui.SetNextWindowPos(new Vector2(Main.Main.Window.Width / 2 - 100, 40));
            if(ImGui.Begin("Just An Approximation", ref AlwaysOpen, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoBackground))
            {
                ImGui.Text("Just An Approximation");
                ImGui.End();
            }
            //=========================
            if (ImGui.BeginMainMenuBar())
            {
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
                        Stream stream;
                        SaveFileDialog saveDialog = new SaveFileDialog();
                        saveDialog.Filter = "AVFX files (*.avfx)|*.avfx|All files (*.*)|*.*";
                        saveDialog.RestoreDirectory = true;
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            if ((stream = saveDialog.OpenFile()) != null)
                            {
                                AVFXNode node = Main.AVFX.toAVFX();
                                byte[] bytes = node.toBytes();
                                stream.Write(bytes, 0, bytes.Length);
                                stream.Close();
                            }
                        }
                    }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Model##"))
                {
                    if (ImGui.MenuItem("Open Model From Game", null))
                    {
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
                if (ImGui.InputText("AVFX Path", avfxPathBytes, INPUT_SIZE))
                {

                }
                ImGui.EndPopup();
            }
        }
    }
}
