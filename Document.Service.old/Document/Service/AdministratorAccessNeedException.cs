// Decompiled with JetBrains decompiler
// Type: Document.Service.AdministratorAccessNeedException
// Assembly: Document.Service, Version=0.12.1.0, Culture=neutral, PublicKeyToken=9c8f15b9f8ae24cc
// MVID: 397D359F-A322-4ECB-BA44-B758DFC28E5C
// Assembly location: D:\Mahak\Tools\MahakTray\Document.Service.dll

using System;
using System.Runtime.Serialization;

namespace Document.Service
{
  [Serializable]
  internal class AdministratorAccessNeedException : Exception
  {
    public AdministratorAccessNeedException()
    {
    }

    public AdministratorAccessNeedException(string message)
      : base(message)
    {
    }

    public AdministratorAccessNeedException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    protected AdministratorAccessNeedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
