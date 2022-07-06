using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Launcher
{
    public class EmbeddedResources
    {
        public static async Task<byte[]> EmbeddedResourceToBytes(string fileName)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (Stream fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
                    {
                        await fileStream.CopyToAsync(memoryStream);
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