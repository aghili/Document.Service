using Aghili.Extensions.Service.Install.Utilities;
using System.Net;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class NetshAddParameterCommand : Netsh2BaseCommand
{
    public NetshAddParameterCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }
    public NetshAddParameterCommand Program(string value)
    {
        parameters["program="] = $"\"{value}\"";
        return this;
    }
    public NetshAddParameterCommand Service(string value = "any")
    {
        parameters["service="] = value;
        return this;
    }
    public NetshAddParameterCommand Description(string value)
    {
        parameters["description="] = $"\"{value}\"";
        return this;
    }
    public NetshAddParameterCommand LocalIp(string value = "any")
    {
        parameters["localip="] = value;
        return this;
    }
    public NetshAddParameterCommand LocalIp(IPAddress value)
    {
        parameters["localip="] = value.ToString();
        return this;
    }
    public NetshAddParameterCommand RemoteIp(string value = "any")
    {
        parameters["Remoteip="] = value;
        return this;
    }
    public NetshAddParameterCommand RmtComputerGroup(string value = "any")
    {
        parameters["rmtcomputergrp="] = value;
        return this;
    }
    public NetshAddParameterCommand RmtUserGroup(string value = "any")
    {
        parameters["rmtusrgrp="] = value;
        return this;
    }
    public NetshAddParameterCommand RemoteIp(IPAddress value)
    {
        parameters["Remoteip="] = value.ToString();
        return this;
    }
    public NetshAddParameterCommand RemoteIp(EnFirewallRemoteip value)
    {
        parameters["Remoteip="] = value.ConvertToString();
        return this;
    }
    public NetshAddParameterCommand LocalPort(EnFirewallLocalPort value)
    {
        parameters["localport="] = value.ConvertToString();
        return this;
    }
    public NetshAddParameterCommand LocalPort(uint value)
    {
        parameters["localport="] = $"{value}";
        return this;
    }
    public NetshAddParameterCommand LocalPort(uint min, uint max)
    {
        parameters["localport="] = $"{min}-{max}";
        return this;
    }
    public NetshAddParameterCommand RemotePort(uint value)
    {
        parameters["remoteport="] = $"{value}";
        return this;
    }
    public NetshAddParameterCommand RemotePort(uint min, uint max)
    {
        parameters["remoteport="] = $"{min}-{max}";
        return this;
    }
    public NetshAddParameterCommand Protocol(EnFirewallProtocol value, string? type = null, string? code = null)
    {
        string type_code = "";
        if (type != null)
            type_code = $"{type}: {code}";
        parameters["protocol="] = value.ConvertToString();
        return this;
    }
    public NetshAddParameterCommand Protocol(byte value)
    {
        parameters["protocol="] = $"{value}";
        return this;
    }

    public NetshDoCommand<NetshGeneralResult> End()
    {
        return new(this);
    }
}