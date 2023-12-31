﻿using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionAdministratorPrivileges : Exception
{
    public ExceptionAdministratorPrivileges()
    {
    }

    public ExceptionAdministratorPrivileges(string? message)
        : base(message)
    {
    }

    public ExceptionAdministratorPrivileges(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionAdministratorPrivileges(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}