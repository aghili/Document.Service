// Decompiled with JetBrains decompiler
// Type: Document.Service.ApplicationTypes.EngineTypeService
// Assembly: Document.Service, Version=0.12.1.0, Culture=neutral, PublicKeyToken=9c8f15b9f8ae24cc
// MVID: 397D359F-A322-4ECB-BA44-B758DFC28E5C
// Assembly location: D:\Mahak\Tools\MahakTray\Document.Service.dll

using System.ServiceProcess;

namespace Document.Service.ApplicationTypes
{
  internal class EngineTypeService : IEngineType
  {
    private ServiceBase application;

    public EngineTypeService(ServiceBase application) => this.application = application;

    public string Name => this.application.ServiceName;
  }
}
