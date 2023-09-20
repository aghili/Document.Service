using System.Runtime.InteropServices;
using System.Security;

namespace Aghili.Extensions.Service.Install.Extensions
{
    internal static class VersionExtension
    {
        [SecurityCritical]
        [DllImport("ntdll.dll", SetLastError = true)]
        internal static extern bool RtlGetVersion(ref OSVERSIONINFOEX versionInfo);
        [StructLayout(LayoutKind.Sequential)]
        internal struct OSVERSIONINFOEX
        {
            // The OSVersionInfoSize field must be set to Marshal.SizeOf(typeof(OSVERSIONINFOEX))
            internal int OSVersionInfoSize;
            internal int MajorVersion;
            internal int MinorVersion;
            internal int BuildNumber;
            internal int PlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            internal string CSDVersion;
            internal ushort ServicePackMajor;
            internal ushort ServicePackMinor;
            internal short SuiteMask;
            internal byte ProductType;
            internal byte Reserved;
        }
        
        public static OSVERSIONINFOEX RealWindowVersion(this Version version)
        {
            var osVersionInfo = new OSVERSIONINFOEX { OSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX)) };
            if (!RtlGetVersion(ref osVersionInfo))
            {
                if (osVersionInfo.BuildNumber == 0)
                    throw new Exception("Windows version not found!");
            }
            return osVersionInfo;
        }
    }
}
