namespace Aghili.Extensions.Service.Install.Register.WindowsService;

public interface IWindowsServiceRegister
{
    string Title { get; }

    void Install(string ContentFolder, WindowsServiceInformation item);

    void Uninstall(string ContentFolder, WindowsServiceInformation item);
}