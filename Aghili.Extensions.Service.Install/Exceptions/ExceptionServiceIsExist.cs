﻿using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionServiceIsExist : Exception
{
    public ExceptionServiceIsExist()
    {
    }

    public ExceptionServiceIsExist(string? message)
        : base(message)
    {
    }

    public ExceptionServiceIsExist(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionServiceIsExist(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}