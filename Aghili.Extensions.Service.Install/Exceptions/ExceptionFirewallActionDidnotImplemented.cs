using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionFirewallActionDidnotImplemented : Exception
{
    public ExceptionFirewallActionDidnotImplemented()
    {
    }

    public ExceptionFirewallActionDidnotImplemented(string? message) : base(message)
    {
    }

    public ExceptionFirewallActionDidnotImplemented(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ExceptionFirewallActionDidnotImplemented(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}