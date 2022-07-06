using System;
using System.Diagnostics;
using System.Reflection;

namespace Launcher
{
    public partial class App
    {
        //Resolve missing assembly dll files
        private Assembly AppAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                string fileName = "Resources\\" + args.Name.Split(',')[0] + ".dll";
                Debug.WriteLine("Resolving assembly dll: " + fileName);
                return Assembly.LoadFrom(fileName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed resolving assembly dll: " + ex.Message);
                return null;
            }
        }
    }
}