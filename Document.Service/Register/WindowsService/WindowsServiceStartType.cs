namespace Document.Service.Register.WindowsService;

public enum WindowsServiceStartType
{
    boot,
    system,
    auto,
    demand,
    disabled,
    delayed_auto
}