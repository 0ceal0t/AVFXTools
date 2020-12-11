using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using AVFXTools.Configuration;

namespace AVFXTools.Views
{
    public class ConfigWindowViewModel : INotifyPropertyChanged
    {
        public string GamePath
        {
            get => App.Settings.GamePath;
            set {
                App.Settings.GamePath = value;
                NotifyPropertyChanged(nameof(GamePath));
            }
        }
        public string VFXPath
        {
            get => App.Settings.VFXPath;
            set {
                App.Settings.VFXPath = value;
                NotifyPropertyChanged(nameof(VFXPath));
            }
        }
        public string ModelPath
        {
            get => App.Settings.ModelPath;
            set => NotifyPropertyChanged(nameof(ModelPath));
        }
        public bool VFXOnLoad
        {
            get => App.Settings.VFXOnLoad;
            set {
                if(VFXOnLoad != value)
                {
                    App.Settings.VFXOnLoad = value;
                }
                NotifyPropertyChanged(nameof(VFXOnLoad));
            }
        }
        public bool ModelOnLoad
        {
            get => App.Settings.ModelOnLoad;
            set {
                if (ModelOnLoad != value)
                {
                    App.Settings.ModelOnLoad = value;
                }
                NotifyPropertyChanged(nameof(ModelOnLoad));
            }
        }

        public ICommand GamePath_Select => new RelayCommand(GamePathSelect);
        public void GamePathSelect(object obj)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.Description = "Choose your FFXIV installation folder (contains /boot and /game)";
            folderDialog.ShowNewFolderButton = false;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                GamePath = folderDialog.SelectedPath;
            }
        }
        public ICommand Save => new RelayCommand(SaveSettings);
        public void SaveSettings(object obj)
        {
            App.Settings.Save();
            MessageBox.Show("Settings Saved");
        }

        private ConfigWindow _view;
        public ConfigWindowViewModel(ConfigWindow view)
        {
            _view = view;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
