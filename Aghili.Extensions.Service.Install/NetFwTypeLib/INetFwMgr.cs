using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib;

[Guid("F7898AF5-CAC4-4632-A2EC-DA06E5111AF2")]
[TypeIdentifier]
[ComImport]
public interface INetFwMgr
{
[DispId(1)]
INetFwPolicy LocalPolicy { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
}
