using Aghili.Extensions.Service.Install.Exceptions;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Aghili.Extensions.Service.Install.Register.WindowsService;

public class WindowsServiceRegisterSc : IWindowsServiceRegister
{
    private static readonly string regasmfile = "SC.EXE";

    public static bool IsReady => true;

    public string Title => "SC";

    private static WindowsServiceActionResult ServiceCommand(WindowsServiceAction Action, string ContentFolder, WindowsServiceInformation item)
    {
        string value = Path.Combine(ContentFolder, item.Filename);
        Process process = new Process();
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = regasmfile;
        process.StartInfo.Arguments = ((item.ServerName == null) ? "" : (item.ServerName + " "));
        ProcessStartInfo startInfo = process.StartInfo;
        string arguments = startInfo.Arguments;
        DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
        defaultInterpolatedStringHandler.AppendFormatted(Action);
        defaultInterpolatedStringHandler.AppendLiteral(" ");
        startInfo.Arguments = arguments + defaultInterpolatedStringHandler.ToStringAndClear();
        ProcessStartInfo startInfo2 = process.StartInfo;
        startInfo2.Arguments = startInfo2.Arguments + item.ServiceName + "  ";
        switch (Action)
        {
            case WindowsServiceAction.create:
                {
                    ProcessStartInfo startInfo3 = process.StartInfo;
                    startInfo3.Arguments = startInfo3.Arguments + "start=" + item.MapToString(item.StartType) + " ";
                    ProcessStartInfo startInfo4 = process.StartInfo;
                    string arguments2 = startInfo4.Arguments;
                    object obj;
                    if (item.ErrorType.HasValue)
                    {
                        defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(9, 1);
                        defaultInterpolatedStringHandler.AppendLiteral("error= ");
                        defaultInterpolatedStringHandler.AppendFormatted(item.ErrorType);
                        defaultInterpolatedStringHandler.AppendLiteral("  ");
                        obj = defaultInterpolatedStringHandler.ToStringAndClear();
                    }
                    else
                    {
                        obj = "";
                    }

                    startInfo4.Arguments = arguments2 + (string?)obj;
                    ProcessStartInfo startInfo5 = process.StartInfo;
                    string arguments3 = startInfo5.Arguments;
                    defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
                    defaultInterpolatedStringHandler.AppendLiteral("binpath=\"");
                    defaultInterpolatedStringHandler.AppendFormatted(value);
                    defaultInterpolatedStringHandler.AppendLiteral(" --contentRoot ");
                    defaultInterpolatedStringHandler.AppendFormatted(ContentFolder);
                    defaultInterpolatedStringHandler.AppendLiteral("\" ");
                    startInfo5.Arguments = arguments3 + defaultInterpolatedStringHandler.ToStringAndClear();
                    process.StartInfo.Arguments += ((item.Group == null) ? "" : ("group=" + item.Group + " "));
                    process.StartInfo.Arguments += ((item.UserName == null) ? "" : ("obj=" + item.UserName + " "));
                    process.StartInfo.Arguments += ((item.Password == null) ? "" : ("password=" + item.Password + " "));
                    break;
                }
        }

        process.Start();
        string text = process.StandardOutput.ReadToEnd();
        string text2 = process.StandardError.ReadToEnd();
        int exitCode = process.ExitCode;
        string[] array = text.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string message = ((array.Length < 2) ? "" : string.Join(Environment.NewLine, array.Skip(1).ToArray()));
        return new WindowsServiceActionResult
        {
            Action = Action,
            Result = (WindowsServiceResult)exitCode,
            Message = message
        };
    }

    private static void CheckResult(WindowsServiceActionResult result)
    {
        switch (result.Result)
        {
            case WindowsServiceResult.SUCCESS:
                break;
            case WindowsServiceResult.FAILURE_ACCESS_IS_DENIED:
                throw new ExceptionAdministratorPrivileges("\n\t\tCannot open Service Control Manager on computer '.'. This operation might require other privileges");
            case WindowsServiceResult.FAILURE_DELETED_MARKED:
                throw new ExceptionServiceIsDeletedMark("Service marked for delete,Please stop the service and then try to rerun installer.");
            case WindowsServiceResult.FAILURE_EXIST:
                throw new ExceptionServiceIsExist("Service exists,unistall processed.");
            case WindowsServiceResult.FAILURE_INVALID_COMMANDLINE_ARGUMENT:
                throw new ExceptionInvalidCommandLineArgument("CommandLine Did not work,Invalid CommandLine Argument!");
            default:
                throw new ExceptionUnknown(result.Message);
        }
    }

    public void Install(string ContentFolder, WindowsServiceInformation item)
    {
        WindowsServiceActionResult result = ServiceCommand(WindowsServiceAction.create, ContentFolder, item);
        CheckResult(result);
    }

    public void Uninstall(string ContentFolder, WindowsServiceInformation item)
    {
        WindowsServiceActionResult windowsServiceActionResult = ServiceCommand(WindowsServiceAction.stop, ContentFolder, item);
        if (windowsServiceActionResult.Result == WindowsServiceResult.SUCCESS || windowsServiceActionResult.Result == WindowsServiceResult.FAILURE_NOT_STARTED)
        {
            windowsServiceActionResult = ServiceCommand(WindowsServiceAction.delete, ContentFolder, item);
        }

        CheckResult(windowsServiceActionResult);
    }
}