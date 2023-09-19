using System.Text.Json;

namespace Aghili.Extensions.Service.Install.Register.WindowsServices;

public class WindowsServiceInformation
{
    public string? ServerName { get; set; }

    public string? DisplayName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Description { get; set; }

    public string Filename { get; set; }

    public EnWindowsServiceStartType StartType { get; set; } = EnWindowsServiceStartType.auto;


    public EnWindowsServiceErrorType? ErrorType { get; set; }

    public string? Group { get; set; }

    public string ServiceName { get; set; }

    public WindowsServiceInformation(string filename, string serviceName)
    {
        Filename = filename;
        ServiceName = serviceName;
    }

    internal static string MapToString(EnWindowsServiceStartType startType)
    {
        if (startType == EnWindowsServiceStartType.delayed_auto)
        {
            return "delay-auto";
        }

        return Enum.GetName(typeof(EnWindowsServiceStartType), startType) ?? startType.ToString();
    }

    internal static string MapToString(EnWindowsServiceErrorType startType)
    {
        return Enum.GetName(typeof(EnWindowsServiceErrorType), startType) ?? startType.ToString();
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}