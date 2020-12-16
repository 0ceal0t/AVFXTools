using AVFXLib.AVFX;
using AVFXLib.Main;
using AVFXLib.Models;
using AVFXTools.FFXIV;
using AVFXTools.Main;
using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AVFXTools.Views
{
    /// <summary>
    /// Interaction logic for VFXViewer.xaml
    /// </summary>
    public partial class VFXViewer : MetroWindow
    {
        public MainViewer _viewer;

        public VFXViewer(AVFXBase avfx, ResourceGetter getter, WepModel model)
        {
            InitializeComponent();

            _veldridControl._window = this;
            _viewer = new MainViewer(_veldridControl, avfx, getter, model);
        }

        // ============ EVENTS ===============
        private void _veldridControl_KeyUp(object sender, KeyEventArgs e)
        {
            bool m_Ctl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool m_Shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            bool m_Alt = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
            _veldridControl.Event_KeyUp(e, m_Ctl, m_Shift, m_Alt);
        }
        private void _veldridControl_KeyDown(object sender, KeyEventArgs e)
        {
            bool m_Ctl = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool m_Shift = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            bool m_Alt = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
            _veldridControl.Event_KeyDown(e, m_Ctl, m_Shift, m_Alt);
        }
        private void _veldridControl_TextInput(object sender, TextCompositionEventArgs e)
        {
            _veldridControl.Event_TextInput(e.Text);
        }

        // ============ CONTROLS =============
        private void LaunchGithub(object sender, RoutedEventArgs e)
        {
            App.OpenBrowser("https://github.com/mkaminsky11/AVFXTools");
        }

        private void Menu_OpenLocalAVFX(object sender, RoutedEventArgs e)
        {
            Stream stream;
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Filter = "AVFX files (*.avfx)|*.avfx|All files (*.*)|*.*";
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == true)
            {
                if ((stream = fileDialog.OpenFile()) != null)
                {
                    AVFXNode node = Reader.readAVFX(new BinaryReader(stream), out List<string> messages);
                    foreach (var m in messages)
                    {
                        ApplicationBase.Logger.WriteWarning(m);
                    }
                    _viewer.LastImportNode = node;
                    _viewer.AVFX = new AVFXBase();
                    _viewer.AVFX.read(node);
                    _viewer.refreshGraphics();
                    _viewer.refreshUI();
                    stream.Close();
                }
            }
        }
        private void Menu_OpenGameAVFX(object sender, RoutedEventArgs e)
        {
            _viewer.UI.Controls.AVFXFromGameDialog = true;
        }
        private void Menu_Export(object sender, RoutedEventArgs e)
        {
            AVFXNode node = _viewer.UI.AVFX.toAVFX();
            byte[] bytes = node.toBytes();
            Save("AVFX files (*.avfx)|*.avfx|All files (*.*)|*.*", bytes);
        }
        private void Menu_ExportJSON(object sender, RoutedEventArgs e)
        {
            JObject json = (JObject)_viewer.UI.AVFX.toJSON();
            byte[] bytes = Util.StringToBytes(json.ToString());
            Save("JSON files (*.json)|*.json|All files (*.*)|*.*", bytes);
        }
        private void Menu_OpenGameModel(object sender, RoutedEventArgs e)
        {
            _viewer.UI.Controls.MDLFromGameDialog = true;
        }
        private void Menu_ExportRaw(object sender, RoutedEventArgs e)
        {
            byte[] bytes = Util.StringToBytes(_viewer.LastImportNode.exportString(0));
            Save("TXT files (*.txt)|*.txt|All files (*.*)|*.*", bytes);

        }
        private void Menu_Verify(object sender, RoutedEventArgs e)
        {
            AVFXNode node = _viewer.UI.AVFX.toAVFX();
            Debug.WriteLine(_viewer.LastImportNode.EqualsNode(node));
        }
        private void Menu_Settings(object sender, RoutedEventArgs e)
        {
            var c = new ConfigWindow();
            c.Show();
        }

        // ======= UTILS ======
        public static void Save(string Filter, byte[] bytes)
        {
            Stream stream;
            var saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.Filter = Filter;
            saveDialog.RestoreDirectory = true;
            if (saveDialog.ShowDialog() == true)
            {
                if ((stream = saveDialog.OpenFile()) != null)
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                    ApplicationBase.Logger.WriteInfo("Successfully wrote file");
                }
            }
        }

        private void _veldridControl_Resized(object sender, SizeChangedEventArgs e)
        {
            _veldridControl.Width = _grid.Width;
            _veldridControl.Height = _grid.Height;
        }
    }
}
