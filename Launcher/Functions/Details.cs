using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static Launcher.AppVariables;
using static Launcher.Strings;

namespace Launcher
{
    public class Details
    {
        //Set application details
        public static bool AppSetDetails()
        {
            try
            {
                Launcher_LauncherExePath = AVFunctions.ApplicationPathExecutable();
                Launcher_LauncherRootPath = AVFunctions.ApplicationPathRoot();
                Launcher_TargetName = ReplaceLauncherName(Path.GetFileNameWithoutExtension(Launcher_LauncherExePath));

                //Set working directory to executable path
                Directory.SetCurrentDirectory(Launcher_LauncherRootPath);

                //Check target application name
                if (Launcher_TargetName.ToLower() == "launcher")
                {
                    Debug.WriteLine("Looking for valid executable files.");
                    string exeFile = Directory.GetFiles(Launcher_LauncherRootPath, "*.exe").Where(x => !Path.GetFileNameWithoutExtension(x).ToLower().Contains("updater") && !Path.GetFileNameWithoutExtension(x).ToLower().Contains("launcher")).FirstOrDefault();
                    if (string.IsNullOrWhiteSpace(exeFile))
                    {
                        Debug.WriteLine("Invalid app name detected: " + Launcher_TargetName);
                        return false;
                    }
                    else
                    {
                        Launcher_TargetName = ReplaceLauncherName(Path.GetFileNameWithoutExtension(exeFile));
                    }
                }

                Launcher_TargetExeName = Launcher_TargetName + ".exe";
                Launcher_TargetExePath = Path.Combine(Launcher_LauncherRootPath, Launcher_TargetExeName);
                Launcher_Author = "Arnold Vink";
                Launcher_TaskName = "ArnoldVink_" + Launcher_TargetName;
                Launcher_Description = Launcher_TargetName + " Launcher";
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