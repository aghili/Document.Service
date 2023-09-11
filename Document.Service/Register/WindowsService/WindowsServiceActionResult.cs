namespace Document.Service.Register.WindowsService;

public class WindowsServiceActionResult
{
    public WindowsServiceAction Action { get; set; }

    public WindowsServiceResult Result { get; set; }

    public string Message { get; set; } = "";

}