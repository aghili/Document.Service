using System;
using System.Runtime.Serialization;

namespace Document.Service.Exceptions;

[Serializable]
internal class ExceptionServiceIsDeletedMark : Exception
{
    public ExceptionServiceIsDeletedMark()
    {
    }

    public ExceptionServiceIsDeletedMark(string? message)
        : base(message)
    {
    }

    public ExceptionServiceIsDeletedMark(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionServiceIsDeletedMark(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}