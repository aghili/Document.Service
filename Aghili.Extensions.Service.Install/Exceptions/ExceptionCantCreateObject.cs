﻿using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionCantCreateObject : Exception
{
    public ExceptionCantCreateObject()
    {
    }

    public ExceptionCantCreateObject(string message)
        : base(message)
    {
    }

    public ExceptionCantCreateObject(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionCantCreateObject(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}