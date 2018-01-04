using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Voxa.Utils
{
    public static class ResourceManager
    {
        public static string GetTextAssetContent(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream)) {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        public static string GetTextResource(string resourcePath)
        {
            Assembly assembly = GetAssemblyContext(resourcePath);

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        public static BinaryReader GetBinaryResourceReader(string resourcePath)
        {
            Assembly assembly = GetAssemblyContext(resourcePath);

            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            return new BinaryReader(stream);
        }

        public static Stream GetFileStream(string resourcePath)
        {
            Assembly assembly = GetAssemblyContext(resourcePath);

            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            return stream;
        }

        private static Assembly GetAssemblyContext(string resourcePath)
        {
            string assemblyName = resourcePath.Split('.')[0];
            Assembly assembly;
            if (assemblyName == "Voxa") {
                assembly = Assembly.GetExecutingAssembly();
            } else {
                assembly = Assembly.GetEntryAssembly();
            }
            return assembly;
        }
    }
}
