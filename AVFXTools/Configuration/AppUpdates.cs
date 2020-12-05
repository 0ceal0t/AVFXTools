using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Squirrel;

namespace AVFXTools.Configuration
{
    // #if !XL_NOAUTOUPDATE
    // #endif
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
                using (var updateManager = await UpdateManager.GitHubUpdateManager(repoUrl: "https://github.com/mkaminsky11/AVFXTools", applicationName: "AVFXTools", prerelease: true))
                {
                    SquirrelAwareApp.HandleEvents(
                        onInitialInstall: v => updateManager.CreateShortcutForThisExe(),
                        onAppUpdate: v => updateManager.CreateShortcutForThisExe(),
                        onAppUninstall: v => updateManager.RemoveShortcutForThisExe());

                    var downloadedRelease = await updateManager.UpdateApp();

                    if (downloadedRelease != null)
                        UpdateManager.RestartApp();
#if !AVFX_NOAUTOUPDATE
                    else
                    OnUpdateCheckFinished?.Invoke(this, null);
#endif
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //Program.Shutdown(1);
            }

            // Reset security protocol after updating
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
        }
    }
}
