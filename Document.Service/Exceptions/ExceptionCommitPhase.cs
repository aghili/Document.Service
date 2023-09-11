using System;
using System.Runtime.Serialization;

namespace Document.Service.Exceptions;

[Serializable]
internal class ExceptionCommitPhase : Exception
{
    public ExceptionCommitPhase()
    {
    }

    public ExceptionCommitPhase(string? message)
        : base(message)
    {
    }

    public ExceptionCommitPhase(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionCommitPhase(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}