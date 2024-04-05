using System;
using System.Windows;

namespace Launcher
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Resolve missing assembly dll files
                AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolveEmbedded;

                //Run application startup code
                await AppStartup();
            }
            catch { }
        }
    }
}