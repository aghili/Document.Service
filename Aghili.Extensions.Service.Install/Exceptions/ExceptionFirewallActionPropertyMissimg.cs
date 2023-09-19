using System.Runtime.Serialization;

namespace Aghili.Extensions.Service.Install.Exceptions;

[Serializable]
internal class ExceptionFirewallActionPropertyMissimg : Exception
{
    public ExceptionFirewallActionPropertyMissimg()
    {
    }

    public ExceptionFirewallActionPropertyMissimg(string? message) : base(message)
    {
    }

    public ExceptionFirewallActionPropertyMissimg(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected ExceptionFirewallActionPropertyMissimg(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}