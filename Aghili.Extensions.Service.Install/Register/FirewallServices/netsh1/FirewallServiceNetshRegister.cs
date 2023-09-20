using Aghili.Extensions.Service.Install.Exceptions;
using System.Diagnostics;
using System.Reflection;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh1;

internal class FirewallServiceNetshRegister : IFirewallServiceRegister
{
    private static readonly string programEngineFilename = "NETSH.EXE";
    private static readonly string programEngineFilenameCategory = " firewall ";
    private static Dictionary<EnFirewallAction, List<string>> ActionProperties = new()
    {
        { EnFirewallAction.add_allowedprogram                 ,new List<string>(){ nameof(FirewallInformation.Program), nameof(FirewallInformation.Name), nameof(FirewallInformation.Mode), nameof(FirewallInformation.Scope), nameof(FirewallInformation.Addresses), nameof(FirewallInformation.Profile) } },
        { EnFirewallAction.add_portopening                    ,new List<string>(){ nameof(FirewallInformation.Protocol), nameof(FirewallInformation.Port), nameof(FirewallInformation.Name), nameof(FirewallInformation.Mode), nameof(FirewallInformation.Scope), nameof(FirewallInformation.Addresses), nameof(FirewallInformation.Profile), nameof(FirewallInformation.Interface) } },
        { EnFirewallAction.set_allowedprogram                 ,new List<string>(){ nameof(FirewallInformation.Program), nameof(FirewallInformation.Name), nameof(FirewallInformation.Mode), nameof(FirewallInformation.Scope), nameof(FirewallInformation.Addresses), nameof(FirewallInformation.Profile) } },
        { EnFirewallAction.set_portopening                    ,new List<string>(){ nameof(FirewallInformation.Protocol), nameof(FirewallInformation.Port), nameof(FirewallInformation.Name), nameof(FirewallInformation.Mode), nameof(FirewallInformation.Scope), nameof(FirewallInformation.Addresses), nameof(FirewallInformation.Profile), nameof(FirewallInformation.Interface) } },
        { EnFirewallAction.delete_allowedprogram              ,new List<string>(){ nameof(FirewallInformation.Program),  nameof(FirewallInformation.Profile) } },
        { EnFirewallAction.delete_portopening                 ,new List<string>(){ nameof(FirewallInformation.Protocol), nameof(FirewallInformation.Port), nameof(FirewallInformation.Profile), nameof(FirewallInformation.Interface) } },
        { EnFirewallAction.set_icmpsetting                    ,new List<string>(){ nameof(FirewallInformation.Type),  nameof(FirewallInformation.Mode), nameof(FirewallInformation.Profile), nameof(FirewallInformation.Interface) } },
        { EnFirewallAction.set_multicastbroadcastresponse     ,new List<string>(){nameof(FirewallInformation.Mode), nameof(FirewallInformation.Profile) } },
        { EnFirewallAction.set_notifications                  ,new List<string>(){nameof(FirewallInformation.Mode), nameof(FirewallInformation.Profile) } },
        { EnFirewallAction.set_opmode                         ,new List<string>(){ nameof(FirewallInformation.Mode), nameof(FirewallInformation.Exceptions), nameof(FirewallInformation.Profile), nameof(FirewallInformation.Interface) } },
      };

    private static FirewallServiceActionResult ServiceCommand(EnFirewallAction Action, FirewallInformation item)
    {
        Process process = new Process();
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = programEngineFilename;

        process.StartInfo.Arguments = ConvertPropertiesToArguments(Action, item);

        process.Start();
        string text = process.StandardOutput.ReadToEnd();
        string text2 = process.StandardError.ReadToEnd();
        int exitCode = process.ExitCode;
        string[] array = text.Split(new char[2] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string message = array.Length < 2 ? "" : string.Join(Environment.NewLine, array.Skip(1).ToArray());

        return new FirewallServiceActionResult(exitCode, message);
    }

    private static string ConvertPropertiesToArguments(EnFirewallAction Action, FirewallInformation item)
    {
        if (!ActionProperties.ContainsKey(Action))
            throw new ExceptionFirewallActionDidnotImplemented($"Firewall Action {Action} did not implemented!");
        var classType = typeof(FirewallInformation);
        var properties = classType.GetProperties();
        string result = " ";
        foreach (var argument in ActionProperties[Action])
        {

            var property = classType.GetProperty(argument);
            if (property == null)
                throw new ExceptionFirewallActionPropertyMissimg($"Firewall Action {Action} need property {argument} ,but did not defined in arguments!");
            result += ConvertPropertyToArgument(item, property);
        }
        return result;
    }

    private static string ConvertPropertyToArgument(object item, PropertyInfo property)
    {
        object? value = property.GetValue(item);
        string argument_value = " ";

        if (value == null)
            return argument_value;

        if (property.PropertyType.IsEnum)
            argument_value = GetEnumString($"{value}");
        else if (property.PropertyType.IsArray)
            argument_value = GetArrayString(value);
        else if (property.PropertyType == typeof(string))
            argument_value = $"\"{value}\"";
        else
            argument_value = $"{value}";

        return $"{property.Name} = {argument_value}";
    }

    private static string GetArrayString(object value)
    {
        throw new NotImplementedException();
    }

    private static string GetEnumString(string value)
    {
        return value.Replace('_', ' ');
    }

    public bool IsFirewallEnabled => throw new NotImplementedException();

    public bool IsFirewallInstalled => throw new NotImplementedException();

    public bool AppAuthorizationsAllowed => throw new NotImplementedException();

    public void GrantAuthorization(string applicationFullPath, string appName)
    {
        throw new NotImplementedException();
    }

    public bool HasAuthorization(string applicationFullPath)
    {
        throw new NotImplementedException();
    }

    public void RemoveAuthorization(string applicationFullPath)
    {
        throw new NotImplementedException();
    }
}