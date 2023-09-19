using System.Diagnostics;

namespace Aghili.Extensions.Service.Install.Utilities
{
    internal class ApplicationInfo
    {
        public static string ProcessPath
        {
            get
            {
#if !NETSTANDARD
                string path = Environment.ProcessPath ?? Process.GetCurrentProcess().MainModule?.FileName;
#else
                string path = Process.GetCurrentProcess().MainModule?.FileName;
#endif
                return path ?? "";
            }
        }
    }
}
