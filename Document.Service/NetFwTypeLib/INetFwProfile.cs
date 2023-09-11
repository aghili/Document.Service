using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib;

[Guid("174A0DDA-E9F9-449D-993B-21AB667CA456")]
[TypeIdentifier]
[ComImport]
public interface INetFwProfile
{
[SpecialName]
[MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
sealed extern void _VtblGap1_1();

[DispId(2)]
bool FirewallEnabled { [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(2), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

[DispId(3)]
bool ExceptionsNotAllowed { [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] get; [DispId(3), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [param: In] set; }

[SpecialName]
[MethodImpl(MethodCodeType = MethodCodeType.Runtime)]
sealed extern void _VtblGap2_8();

[DispId(10)]
INetFwAuthorizedApplications AuthorizedApplications { [DispId(10), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
