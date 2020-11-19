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
        public bool OpenFromGameDialogOpen = false;
        public static uint INPUT_SIZE = 40;
        public byte[] avfxPathBytes = new byte[INPUT_SIZE];
        public byte[] mdlPathBytes = new byte[INPUT_SIZE];

        public ImgUIControls(ImgUIMain main)
        {
            Main = main;
        }

        public void Draw()
        {
            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
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
                        OpenFromGameDialogOpen = true;
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
                ImGui.EndMainMenuBar();
            }
            //=========================
            ImGui.SetNextWindowSize(new Vector2(600, 200));
            if (OpenFromGameDialogOpen)
            {
                ImGui.OpenPopup("Choose In-Game File");
            }
            
            if (ImGui.BeginPopupModal("Choose In-Game File", ref OpenFromGameDialogOpen))
            {
                if (ImGui.InputText("AVFX Path", avfxPathBytes, INPUT_SIZE))
                {

                }
                if (ImGui.InputText("MDL Path", mdlPathBytes, INPUT_SIZE))
                {

                }
                ImGui.EndPopup();
            }
        }
    }
}
