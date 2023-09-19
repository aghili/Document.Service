using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionUnknown : Exception
{
    public ExceptionUnknown()
    {
    }

    public ExceptionUnknown(string? message)
        : base(message)
    {
    }

    public ExceptionUnknown(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ExceptionUnknown(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}