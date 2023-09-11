// Decompiled with JetBrains decompiler
// Type: NetFwTypeLib.INetFwPolicy
// Assembly: Document.Service, Version=0.12.1.0, Culture=neutral, PublicKeyToken=9c8f15b9f8ae24cc
// MVID: 397D359F-A322-4ECB-BA44-B758DFC28E5C
// Assembly location: D:\Mahak\Tools\MahakTray\Document.Service.dll

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib
{
  [CompilerGenerated]
  [Guid("D46D2478-9AC9-4008-9DC7-5563CE5536CC")]
  [TypeIdentifier]
  [ComImport]
  public interface INetFwPolicy
  {
    [DispId(1)]
    INetFwProfile CurrentProfile { [DispId(1), MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)] [return: MarshalAs(UnmanagedType.Interface)] get; }
  }
}
