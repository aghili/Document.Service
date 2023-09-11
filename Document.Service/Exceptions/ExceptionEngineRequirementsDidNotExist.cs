using System;
using System.Runtime.Serialization;

namespace Document.Service.Exceptions;

[Serializable]
internal class ExceptionEngineRequirementsDidNotExist : Exception
{
    public ExceptionEngineRequirementsDidNotExist()
    {
    }

    public ExceptionEngineRequirementsDidNotExist(string? message)
        : base(message)
    {
    }

    public ExceptionEngineRequirementsDidNotExist(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionEngineRequirementsDidNotExist(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}