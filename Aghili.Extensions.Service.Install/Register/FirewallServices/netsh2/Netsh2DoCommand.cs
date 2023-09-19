using Aghili.Extensions.Service.Install.Utilities;
using System.Diagnostics;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2DoCommand<T>
{
    private static readonly string programEngineFIlename = "NETSH.EXE";

    private static Netsh2GeneralResult ServiceCommand(string command)
    {
        Process process = new Process();
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = programEngineFIlename;

        process.StartInfo.Arguments = command;

        process.Start();
        string text = process.StandardOutput.ReadToEnd();
        string text2 = process.StandardError.ReadToEnd();
        int exitCode = process.ExitCode;
        //string[] array = text.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        //string message = ((array.Length < 2) ? "" : string.Join(Environment.NewLine, array.Skip(1).ToArray()));

        return new Netsh2GeneralResult(exitCode, text);
    }

    private Netsh2BaseCommand parent;

    public Netsh2DoCommand(Netsh2BaseCommand parent)
    {
        this.parent = parent;
    }

    public T Do()
    {
        return (T)Activator.CreateInstance(typeof(T),ServiceCommand(parent.GetCommand()));
    }
}
