namespace Document.Service;

public class ServiceResultEventArg : EventArgs
{
    public ServiceResultEventArg(ServiceResult result) => this.Result = result;

    public ServiceResult Result { get; private set; }
}