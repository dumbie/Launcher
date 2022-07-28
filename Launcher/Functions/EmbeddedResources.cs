using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Launcher
{
    public class EmbeddedResources
    {
        public static byte[] EmbeddedResourceToBytes(string fileName)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Stream fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
                    {
                        fileStream.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to convert embedded resource: " + ex.Message);
                return null;
            }
        }
    }
}