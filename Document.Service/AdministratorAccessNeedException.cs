using System.Runtime.Serialization;

namespace Document.Service;

[Serializable]
internal class AdministratorAccessNeedException : Exception
{
    public AdministratorAccessNeedException()
    {
    }

    public AdministratorAccessNeedException(string message)
        : base(message)
    {
    }

    public AdministratorAccessNeedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected AdministratorAccessNeedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}