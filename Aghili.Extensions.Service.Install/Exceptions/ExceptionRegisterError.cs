﻿using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionRegisterError : Exception
{
    public ExceptionRegisterError()
    {
    }

    public ExceptionRegisterError(string? message) : base(message)
    {
    }

    public ExceptionRegisterError(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ExceptionRegisterError(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}