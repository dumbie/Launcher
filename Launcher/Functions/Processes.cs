using System;
using System.Diagnostics;

namespace Launcher
{
    public class Processes
    {
        //Launch process as administrator
        public static bool ProcessStartAdmin(string fileName)
        {
            try
            {
                using (Process startProcess = new Process())
                {
                    startProcess.StartInfo.FileName = fileName;
                    startProcess.StartInfo.Verb = "RunAs";
                    return startProcess.Start();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to start process: " + ex.Message);
                return false;
            }
        }
    }
}