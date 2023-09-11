using System;
using System.Runtime.Serialization;

namespace Document.Service.Exceptions;

[Serializable]
internal class ExceptionTransactionPhase : Exception
{
    public ExceptionTransactionPhase()
    {
    }

    public ExceptionTransactionPhase(string? message)
        : base(message)
    {
    }

    public ExceptionTransactionPhase(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionTransactionPhase(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}