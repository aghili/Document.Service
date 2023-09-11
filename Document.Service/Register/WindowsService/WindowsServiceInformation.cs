using System.Text.Json;

namespace Document.Service.Register.WindowsService;

public class WindowsServiceInformation
{
    public string? ServerName { get; set; }

    public string? DisplayName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Description { get; set; }

    public string Filename { get; set; }

    public WindowsServiceStartType StartType { get; set; } = WindowsServiceStartType.auto;


    public WindowsServiceErrorType? ErrorType { get; set; }

    public string? Group { get; set; }

    public string ServiceName { get; set; }

    public WindowsServiceInformation(string filename, string serviceName)
    {
        Filename = filename;
        ServiceName = serviceName;
    }

    internal string MapToString(WindowsServiceStartType startType)
    {
        WindowsServiceStartType windowsServiceStartType = startType;
        WindowsServiceStartType windowsServiceStartType2 = windowsServiceStartType;
        if (windowsServiceStartType2 == WindowsServiceStartType.delayed_auto)
        {
            return "delay-auto";
        }

        return Enum.GetName(startType) ?? startType.ToString();
    }

    internal string MapToString(WindowsServiceErrorType startType)
    {
        WindowsServiceErrorType windowsServiceErrorType = startType;
        WindowsServiceErrorType windowsServiceErrorType2 = windowsServiceErrorType;
        return Enum.GetName(startType) ?? startType.ToString();
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}