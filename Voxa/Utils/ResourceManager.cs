using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Voxa.Utils
{
    static class ResourceManager
    {
        public static string GetTextResource(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }

        public static BinaryReader GetBinaryResourceReader(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            return new BinaryReader(stream);
        }

        public static Stream GetFileStream(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            return stream;
        }
    }
}
