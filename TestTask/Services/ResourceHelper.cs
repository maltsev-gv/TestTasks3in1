using System.IO;
using System.Reflection;
using TestTask.Models;

namespace TestTask.Services
{
    public class ResourceHelper
    {
        public static string GetSqlScript(DisplayingScripts sqlScript)
        {
            return GetTextFile($"SqlScripts.{sqlScript}.sql");
        }

        public static string GetTextFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            resourceName = $"{assembly.GetName().Name}.{resourceName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static byte[] GetBinaryFile(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            resourceName = $"{assembly.GetName().Name}.{resourceName}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }

                byte[] ba = new byte[stream.Length];
                stream.Read(ba, 0, ba.Length);
                return ba;
            }
        }
    }
}
