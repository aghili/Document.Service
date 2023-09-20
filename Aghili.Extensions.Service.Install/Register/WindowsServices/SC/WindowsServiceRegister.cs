using Aghili.Extensions.Service.Install.Exceptions;
using System.Diagnostics;

namespace Aghili.Extensions.Service.Install.Register.WindowsServices.SC;

public class WindowsServiceRegister : IWindowsServiceRegister
{
    private static readonly string regasmfile = "SC.EXE";

    public static bool IsReady => true;

    public string Title => "SC";

    private static WindowsServiceActionResult ServiceCommand(EnWindowsServiceAction Action, string ContentFolder, WindowsServiceInformation item)
    {
        string value = Path.Combine(ContentFolder, item.Filename);
        Process process = new Process();
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = regasmfile;
        process.StartInfo.Arguments = item.ServerName == null ? "" : item.ServerName + " ";
        process.StartInfo.Arguments += $"{Action} ";
        process.StartInfo.Arguments += item.ServiceName + "  ";
        switch (Action)
        {
            case EnWindowsServiceAction.create:
                {
                    process.StartInfo.Arguments += $"start={WindowsServiceInformation.MapToString(item.StartType)} ";
                    process.StartInfo.Arguments += item.ErrorType != null ? $"error= {item.ErrorType} " : "";
                    process.StartInfo.Arguments += $"binpath=\"{value} --contentRoot {ContentFolder}\" ";
                    process.StartInfo.Arguments += item.Group == null ? "" : "group=" + item.Group + " ";
                    process.StartInfo.Arguments += item.UserName == null ? "" : "obj=" + item.UserName + " ";
                    process.StartInfo.Arguments += item.Password == null ? "" : "password=" + item.Password + " ";
                    break;
                }
        }

        process.Start();
        string text = process.StandardOutput.ReadToEnd();
        string text2 = process.StandardError.ReadToEnd();
        int exitCode = process.ExitCode;
        string[] array = text.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string message = array.Length < 2 ? "" : string.Join(Environment.NewLine, array.Skip(1).ToArray());
        return new WindowsServiceActionResult
        {
            Action = Action,
            Result = (EnWindowsServiceResult)exitCode,
            Message = message
        };
    }

    private static void CheckResult(WindowsServiceActionResult result)
    {
        switch (result.Result)
        {
            case EnWindowsServiceResult.SUCCESS:
                break;
            case EnWindowsServiceResult.FAILURE_ACCESS_IS_DENIED:
                throw new ExceptionAdministratorPrivileges("\n\t\tCannot open Service Control Manager on computer '.'. This operation might require other privileges");
            case EnWindowsServiceResult.FAILURE_DELETED_MARKED:
                throw new ExceptionServiceIsDeletedMark("Service marked for delete,Please stop the service and then try to rerun installer.");
            case EnWindowsServiceResult.FAILURE_EXIST:
                throw new ExceptionServiceIsExist("Service exists,unistall processed.");
            case EnWindowsServiceResult.FAILURE_INVALID_COMMANDLINE_ARGUMENT:
                throw new ExceptionInvalidCommandLineArgument("CommandLine Did not work,Invalid CommandLine Argument!");
            default:
                throw new ExceptionUnknown(result.Message);
        }
    }

    public void Install(string ContentFolder, WindowsServiceInformation item)
    {
        WindowsServiceActionResult result = ServiceCommand(EnWindowsServiceAction.create, ContentFolder, item);
        CheckResult(result);
    }

    public void Uninstall(string ContentFolder, WindowsServiceInformation item)
    {
        WindowsServiceActionResult windowsServiceActionResult = ServiceCommand(EnWindowsServiceAction.stop, ContentFolder, item);
        if (windowsServiceActionResult.Result == EnWindowsServiceResult.SUCCESS || windowsServiceActionResult.Result == EnWindowsServiceResult.FAILURE_NOT_STARTED)
        {
            windowsServiceActionResult = ServiceCommand(EnWindowsServiceAction.delete, ContentFolder, item);
        }

        CheckResult(windowsServiceActionResult);
    }
}