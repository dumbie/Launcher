using System;
using System.Diagnostics;
using System.Security.Principal;

namespace Launcher
{
    public class Administrator
    {
        //Check administrator permission
        public static bool AdminCheck()
        {
            try
            {
                using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check admin permission: " + ex.Message);
                return false;
            }
        }
    }
}