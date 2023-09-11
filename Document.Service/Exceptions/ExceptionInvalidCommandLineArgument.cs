﻿using System;
using System.Runtime.Serialization;

namespace Document.Service.Exceptions;

[Serializable]
internal class ExceptionInvalidCommandLineArgument : Exception
{
    public ExceptionInvalidCommandLineArgument()
    {
    }

    public ExceptionInvalidCommandLineArgument(string? message)
        : base(message)
    {
    }

    public ExceptionInvalidCommandLineArgument(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionInvalidCommandLineArgument(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}