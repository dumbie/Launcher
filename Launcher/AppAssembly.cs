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
                string fileName = "Launcher.Assembly." + args.Name.Split(',')[0] + ".dll";
                byte[] fileBytes = EmbeddedResources.EmbeddedResourceToBytes(fileName);

                Debug.WriteLine("Resolving embedded assembly dll: " + fileName);
                return Assembly.Load(fileBytes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed resolving assembly dll: " + ex.Message);
                return null;
            }
        }
    }
}