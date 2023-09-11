using System;
using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

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