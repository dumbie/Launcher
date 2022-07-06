using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using static Launcher.AppVariables;
using static Launcher.Strings;

namespace Launcher
{
    public partial class App
    {
        //Set application details
        private bool AppSetDetails()
        {
            try
            {
                Launcher_FilePath = Assembly.GetEntryAssembly().Location;
                Launcher_WorkingPath = Path.GetDirectoryName(Launcher_FilePath);
                Launcher_AppName = ReplaceLauncherName(Path.GetFileNameWithoutExtension(Launcher_FilePath));

                //Set working directory to executable path
                Directory.SetCurrentDirectory(Launcher_WorkingPath);

                //Check application name
                if (Launcher_AppName.ToLower() == "launcher")
                {
                    Debug.WriteLine("Looking for valid executable files.");
                    string exeFile = Directory.GetFiles(Launcher_WorkingPath, "*.exe").Where(x => !Path.GetFileNameWithoutExtension(x).ToLower().Contains("updater") && !Path.GetFileNameWithoutExtension(x).ToLower().Contains("launcher")).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(exeFile))
                    {
                        Debug.WriteLine("Invalid app name detected: " + Launcher_AppName);
                        return false;
                    }
                    else
                    {
                        Launcher_AppName = ReplaceLauncherName(Path.GetFileNameWithoutExtension(exeFile));
                    }
                }

                Launcher_ExecutableFile = Launcher_AppName + ".exe";
                Launcher_Author = "Arnold Vink";
                Launcher_TaskName = "ArnoldVink_" + Launcher_AppName;
                Launcher_Description = Launcher_AppName + " Launcher";

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed setting app details: " + ex.Message);
                return false;
            }
        }
    }
}