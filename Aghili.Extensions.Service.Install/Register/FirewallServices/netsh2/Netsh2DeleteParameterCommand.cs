using Aghili.Extensions.Service.Install.Utilities;
using System.Net;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;


public class Netsh2DeleteParameterCommand : Netsh2BaseCommand
{
    public Netsh2DeleteParameterCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }
    public Netsh2DeleteParameterCommand Program(string value)
    {
        parameters["program="] = $"\"{value}\"";
        return this;
    }
    public Netsh2DeleteParameterCommand Service(string value = "any")
    {
        parameters["service="] = value;
        return this;
    }
    public Netsh2DeleteParameterCommand LocalIp(string value = "any")
    {
        parameters["localip="] = value;
        return this;
    }
    public Netsh2DeleteParameterCommand LocalIp(IPAddress value)
    {
        parameters["localip="] = value.ToString();
        return this;
    }
    public Netsh2DeleteParameterCommand RemoteIp(string value = "any")
    {
        parameters["Remoteip="] = value;
        return this;
    }
    public Netsh2DeleteParameterCommand RemoteIp(IPAddress value)
    {
        parameters["Remoteip="] = value.ToString();
        return this;
    }
    public Netsh2DeleteParameterCommand RemoteIp(EnFirewallRemoteip value)
    {
        parameters["Remoteip="] = value.ConvertToString();
        return this;
    }
    public Netsh2DeleteParameterCommand LocalPort(EnFirewallLocalPort value)
    {
        parameters["localport="] = value.ConvertToString();
        return this;
    }
    public Netsh2DeleteParameterCommand LocalPort(uint value)
    {
        parameters["localport="] = $"{value}";
        return this;
    }
    public Netsh2DeleteParameterCommand LocalPort(uint min, uint max)
    {
        parameters["localport="] = $"{min}-{max}";
        return this;
    }
    public Netsh2DeleteParameterCommand RemotePort(uint value)
    {
        parameters["remoteport="] = $"{value}";
        return this;
    }
    public Netsh2DeleteParameterCommand RemotePort(uint min, uint max)
    {
        parameters["remoteport="] = $"{min}-{max}";
        return this;
    }
    public Netsh2DeleteParameterCommand Protocol(EnFirewallProtocol value, string? type = null, string? code = null)
    {
        string type_code = "";
        if (type != null)
            type_code = $"{type}: {code}";
        parameters["protocol="] = value.ConvertToString();
        return this;
    }
    public Netsh2DeleteParameterCommand Protocol(byte value)
    {
        parameters["protocol="] = $"{value}";
        return this;
    }
    public Netsh2DeleteParameterCommand Profile(EnFirewallProfile value)
    {
        parameters["profile="] = value.ConvertToString();
        return this;
    }

    public Netsh2DoCommand<Netsh2GeneralResult> End()
    {
        return new(this);
    }
}