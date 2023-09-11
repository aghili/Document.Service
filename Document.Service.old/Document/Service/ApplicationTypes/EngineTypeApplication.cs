// Decompiled with JetBrains decompiler
// Type: Document.Service.ApplicationTypes.EngineTypeApplication
// Assembly: Document.Service, Version=0.12.1.0, Culture=neutral, PublicKeyToken=9c8f15b9f8ae24cc
// MVID: 397D359F-A322-4ECB-BA44-B758DFC28E5C
// Assembly location: D:\Mahak\Tools\MahakTray\Document.Service.dll

using System.Windows.Forms;

namespace Document.Service.ApplicationTypes
{
  internal class EngineTypeApplication : IEngineType
  {
    public string Name => Application.ProductName;
  }
}
