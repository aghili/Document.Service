using Aghili.Extensions.Service.Install.Exceptions;
using System.Diagnostics;

namespace Aghili.Extensions.Service.Install.Register.WindowsServices;

public class WindowsServiceRegisterDotNet4 : IWindowsServiceRegister
{
    private static readonly string regasmfile = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\Microsoft.NET\\Framework\\v4.0.30319\\InstallUtil.exe";

    public static bool IsReady => File.Exists(regasmfile);

    public string Title => "DotNet4";

    private static void InstallServiceCommand(string ContentFolder, WindowsServiceInformation item, out string output, out string error, bool uninstall)
    {
        string text = Path.Combine(ContentFolder, item.Filename);
        Process process = new Process();
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = regasmfile;
        process.StartInfo.Arguments = "\"" + text + "\" " + (uninstall ? "/u" : "");
        process.Start();
        output = process.StandardOutput.ReadToEnd();
        error = process.StandardError.ReadToEnd();
    }

    public void Install(string ContentFolder, WindowsServiceInformation item)
    {
        InstallServiceCommand(ContentFolder, item, out var output, out var _, uninstall: false);
        List<string> list = output.Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        int num = list.IndexOf("An exception occurred during the Install phase.");
        if (num > -1)
        {
            string result = list[num + 1];
            if (!(result == "System.InvalidOperationException: Cannot open Service Control Manager on computer '.'. This operation might require other privileges."))
            {
                if (result == "System.ComponentModel.Win32Exception: The specified service already exists")
                {
                    throw new ExceptionServiceIsExist("Service exists,unistall processed.");
                }

                throw new ExceptionUnknown(list[num + 1]);
            }

            throw new ExceptionAdministratorPrivileges("\n\t\tCannot open Service Control Manager on computer '.'. This operation might require other privileges");
        }

        if (list.Contains("The Install phase completed successfully, and the Commit phase is beginning."))
        {
            if (list.Contains("The Commit phase completed successfully."))
            {
                if (list.Contains("The transacted install has completed."))
                {
                    return;
                }

                throw new ExceptionCommitPhase("Transacted has error.");
            }

            throw new ExceptionCommitPhase("Commit phase has error.");
        }

        throw new ExceptionUnknown("Install phase did not completed.");
    }

    public void Uninstall(string ContentFolder, WindowsServiceInformation item)
    {
        throw new NotImplementedException();
    }
}