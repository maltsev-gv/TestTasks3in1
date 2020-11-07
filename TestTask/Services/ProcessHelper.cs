using System.Diagnostics;

namespace TestTask.Services
{
    public class ProcessHelper
    {
        public static void StartProcess(string processName)
        {
            Process.Start(new ProcessStartInfo(processName));
        }
    }
}
