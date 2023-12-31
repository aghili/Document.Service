﻿using Aghili.Extensions.Service.Install.Exceptions;
using Aghili.Extensions.Service.Install.Register.WindowsServices;

namespace Aghili.Extensions.Service.Install.Register;

public class WindowsService
{
    private readonly WindowsServiceInformation windowsServiceInformation;
    private IWindowsServiceRegister InstallerEngine { get; }

    public WindowsService(WindowsServiceInformation windowsServiceInformation)
    {
        if (WindowsServices.SC.WindowsServiceRegister.IsReady)
        {
            InstallerEngine = new WindowsServices.SC.WindowsServiceRegister();
        }
        else
        {
            InstallerEngine = new WindowsServices.Dotnet.WindowsServiceRegister();
        }

        this.windowsServiceInformation = windowsServiceInformation;
    }

    public void Install()
    {
        string currentDirectory = Environment.CurrentDirectory;
        if (string.IsNullOrEmpty(currentDirectory))
            throw new ExceptionEngineRequirementsDidNotExist($"Can not extract path from {Environment.CurrentDirectory}");

        if (InstallerEngine == null)
            throw new ExceptionEngineRequirementsDidNotExist($"Installer engine did not ready!");

        try
        {
            try
            {
                InstallerEngine.Install(currentDirectory, windowsServiceInformation);
            }
            catch (ExceptionServiceIsExist)
            {
                InstallerEngine.Uninstall(currentDirectory, windowsServiceInformation);
                InstallerEngine.Install(currentDirectory, windowsServiceInformation);
            }
        }
        catch (Exception ex)
        {
            ExceptionCantCreateObject exceptionCantCreateObject = new ExceptionCantCreateObject("Can't Register Service ", ex);
            throw exceptionCantCreateObject;
        }
    }

    public void Uninstall()
    {
        string currentDirectory = Environment.CurrentDirectory;
        InstallerEngine.Uninstall(currentDirectory, windowsServiceInformation);
    }
}