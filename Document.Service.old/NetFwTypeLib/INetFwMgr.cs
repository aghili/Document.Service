// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwMgr
// Assembly: Document.Service, Version=0.12.1.0, Culture=neutral, PublicKeyToken=9c8f15b9f8ae24cc
// MVID: 397D359F-A322-4ECB-BA44-B758DFC28E5C
// Assembly location: D:\Mahak\Tools\MahakTray\Document.Service.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib
{
  [CompilerGenerated]
  [Guid("F7898AF5-CAC4-4632-A2EC-DA06E5111AF2")]
  [TypeIdentifier]
  [ComImport]
  public interface INetFwMgr
  {
    [DispId(1)]
    INetFwPolicy LocalPolicy { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
  }
}
