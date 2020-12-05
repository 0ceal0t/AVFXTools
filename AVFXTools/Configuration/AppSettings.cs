using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;

namespace AVFXTools.Configuration
{
    public class AppSettings
    {
        public string GamePath { get; set; }
        public bool ModelOnLoad { get; set; }
        public bool VFXOnLoad { get; set; }
        public string ModelPath { get; set; }
        public string VFXPath { get; set; }

        public AppSettings()
        {
            GamePath = Properties.Settings.Default.GamePath;
            ModelOnLoad = Properties.Settings.Default.ModelOnLoad;
            VFXOnLoad = Properties.Settings.Default.VFXOnLoad;
            ModelPath = Properties.Settings.Default.ModelPath;
            VFXPath = Properties.Settings.Default.VFXPath;
        }

        public void RequestGamePath()
        {
            if (!GetGamePath())
            {
                Program.Shutdown(1);
                return;
            }
        }

        public bool GetGamePath()
        {
            var path = GamePath;
            while (!IsValidGamePath(path))
            {
                FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                folderDialog.Description = "Choose your FFXIV installation folder (contains /boot and /game)";
                folderDialog.ShowNewFolderButton = false;
                if(folderDialog.ShowDialog() == DialogResult.OK)
                {
                    path = folderDialog.SelectedPath;
                }
                else
                {
                    DialogResult wantToClose = MessageBox.Show("An installation folder is required. Exit the application?", "Empty path", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if(wantToClose == DialogResult.Yes)
                    {
                        return false;
                    }
                }
            }
            GamePath = path;
            Save();

            return true;
        }

        public static bool IsValidGamePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;
            if (!Directory.Exists(path))
                return false;
            return File.Exists(Path.Combine(path, "game", "ffxivgame.ver"));
        }

        public void Save()
        {
            Properties.Settings.Default.GamePath = GamePath;
            Properties.Settings.Default.ModelOnLoad = ModelOnLoad;
            Properties.Settings.Default.VFXOnLoad = VFXOnLoad;
            Properties.Settings.Default.ModelPath = ModelPath;
            Properties.Settings.Default.VFXPath = VFXPath;
            Properties.Settings.Default.Save();
        }
    }
}
