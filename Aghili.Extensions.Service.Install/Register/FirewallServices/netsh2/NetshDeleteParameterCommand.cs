using Aghili.Extensions.Service.Install.Utilities;
using System.Net;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;


public class NetshDeleteParameterCommand : Netsh2BaseCommand
{
    public NetshDeleteParameterCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }
    public NetshDeleteParameterCommand Program(string value)
    {
        parameters["program="] = $"\"{value}\"";
        return this;
    }
    public NetshDeleteParameterCommand Service(string value = "any")
    {
        parameters["service="] = value;
        return this;
    }
    public NetshDeleteParameterCommand LocalIp(string value = "any")
    {
        parameters["localip="] = value;
        return this;
    }
    public NetshDeleteParameterCommand LocalIp(IPAddress value)
    {
        parameters["localip="] = value.ToString();
        return this;
    }
    public NetshDeleteParameterCommand RemoteIp(string value = "any")
    {
        parameters["Remoteip="] = value;
        return this;
    }
    public NetshDeleteParameterCommand RemoteIp(IPAddress value)
    {
        parameters["Remoteip="] = value.ToString();
        return this;
    }
    public NetshDeleteParameterCommand RemoteIp(EnFirewallRemoteip value)
    {
        parameters["Remoteip="] = value.ConvertToString();
        return this;
    }
    public NetshDeleteParameterCommand LocalPort(EnFirewallLocalPort value)
    {
        parameters["localport="] = value.ConvertToString();
        return this;
    }
    public NetshDeleteParameterCommand LocalPort(uint value)
    {
        parameters["localport="] = $"{value}";
        return this;
    }
    public NetshDeleteParameterCommand LocalPort(uint min, uint max)
    {
        parameters["localport="] = $"{min}-{max}";
        return this;
    }
    public NetshDeleteParameterCommand RemotePort(uint value)
    {
        parameters["remoteport="] = $"{value}";
        return this;
    }
    public NetshDeleteParameterCommand RemotePort(uint min, uint max)
    {
        parameters["remoteport="] = $"{min}-{max}";
        return this;
    }
    public NetshDeleteParameterCommand Protocol(EnFirewallProtocol value, string? type = null, string? code = null)
    {
        string type_code = "";
        if (type != null)
            type_code = $"{type}: {code}";
        parameters["protocol="] = value.ConvertToString();
        return this;
    }
    public NetshDeleteParameterCommand Protocol(byte value)
    {
        parameters["protocol="] = $"{value}";
        return this;
    }
    public NetshDeleteParameterCommand Profile(EnFirewallProfile value)
    {
        parameters["profile="] = value.ConvertToString();
        return this;
    }

    public NetshDoCommand<NetshGeneralResult> End()
    {
        return new(this);
    }
}