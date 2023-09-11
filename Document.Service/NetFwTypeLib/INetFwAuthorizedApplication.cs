using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib;

[Guid("B5E64FFA-C2C5-444E-A301-FB5E00018050")]
[TypeIdentifier]
[ComImport]
public interface INetFwAuthorizedApplication
{
    [DispId(1)]
    string Name { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)][return: MarshalAs(UnmanagedType.BStr)] get; [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)][param: MarshalAs(UnmanagedType.BStr), In] set; }

    [DispId(2)]
    string ProcessImageFileName { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)][return: MarshalAs(UnmanagedType.BStr)] get; [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)][param: MarshalAs(UnmanagedType.BStr), In] set; }
}
