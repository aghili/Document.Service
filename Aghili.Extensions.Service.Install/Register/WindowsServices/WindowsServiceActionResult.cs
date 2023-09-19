namespace Aghili.Extensions.Service.Install.Register.WindowsServices;

public class WindowsServiceActionResult
{
    public EnWindowsServiceAction Action { get; set; }

    public EnWindowsServiceResult Result { get; set; }

    public string Message { get; set; } = "";

}