using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionArgumentRequired : Exception
{
    public ExceptionArgumentRequired()
    {
    }

    public ExceptionArgumentRequired(string? message)
      : base("Argumunt " + message + " is Required!")
    {
    }

    public ExceptionArgumentRequired(string? message, Exception? innerException)
      : base(message, innerException)
    {
    }

    protected ExceptionArgumentRequired(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
}
