using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Forms;
using Squirrel;

namespace AVFXTools.Configuration
{
    public class AppUpdates
    {
        public EventHandler OnUpdateCheckFinished;

        public AppUpdates()
        {

        }

        public async Task Run()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            try
            {
                ApplicationBase.Logger.WriteInfo("Checking for updates....");

                using (var updateManager = await UpdateManager.GitHubUpdateManager(repoUrl: "https://github.com/mkaminsky11/AVFXTools", applicationName: "AVFXTools", prerelease: true))
                {
                    /*SquirrelAwareApp.HandleEvents(
                        onInitialInstall: v => updateManager.CreateShortcutForThisExe(),
                        onAppUpdate: v => updateManager.CreateShortcutForThisExe(),
                        onAppUninstall: v => updateManager.RemoveShortcutForThisExe());

                    var downloadedRelease = await updateManager.UpdateApp();

                    if (downloadedRelease != null)
                    {
                        Console.WriteLine("Updates found, restarting");
                        UpdateManager.RestartApp();
                    }
                    else
                    {
                        Console.WriteLine("No updates found");
                    }*/
                }

                await Task.Delay(200);

                OnUpdateCheckFinished?.Invoke(this, null);
            }
            catch
            {
                MessageBox.Show("An Error Occurred When Installing Updates", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                App.ShutdownAll(1);
            }

            // Reset security protocol after updating
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
        }
    }
}
