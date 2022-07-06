using Microsoft.Win32;
using System.Diagnostics;

namespace Launcher
{
    public class Registry
    {
        public static bool SecureUIAPathsCheck()
        {
            try
            {
                using (RegistryKey registryKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    using (RegistryKey regKeyPolicies = registryKeyLocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\", true))
                    {
                        return regKeyPolicies.GetValue("EnableSecureUIAPaths").ToString() == "0" ? true : false;
                    }
                }
            }
            catch { }
            return false;
        }

        public static void SecureUIAPathsAllow()
        {
            try
            {
                using (RegistryKey registryKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    using (RegistryKey regKeyPolicies = registryKeyLocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\", true))
                    {
                        regKeyPolicies.SetValue("EnableSecureUIAPaths", 0);
                    }
                }

                Debug.WriteLine("Disabled the secure uia paths check.");
            }
            catch { }
        }

        public static void SecureUIAPathsBlock()
        {
            try
            {
                using (RegistryKey registryKeyLocalMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
                {
                    using (RegistryKey regKeyPolicies = registryKeyLocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System\", true))
                    {
                        regKeyPolicies.SetValue("EnableSecureUIAPaths", 1);
                    }
                }

                Debug.WriteLine("Enabled the secure uia paths check.");
            }
            catch { }
        }
    }
}