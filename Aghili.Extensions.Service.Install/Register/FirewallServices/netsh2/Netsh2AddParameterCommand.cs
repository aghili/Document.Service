﻿using Aghili.Extensions.Service.Install.Utilities;
using System.Net;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2AddParameterCommand : Netsh2BaseCommand
{
    public Netsh2AddParameterCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }
    public Netsh2AddParameterCommand Program(string value)
    {
        parameters["program="] = $"\"{value}\"";
        return this;
    }
    public Netsh2AddParameterCommand Service(string value = "any")
    {
        parameters["service="] = value;
        return this;
    }
    public Netsh2AddParameterCommand Description(string value)
    {
        parameters["description="] = $"\"{value}\"";
        return this;
    }
    public Netsh2AddParameterCommand LocalIp(string value = "any")
    {
        parameters["localip="] = value;
        return this;
    }
    public Netsh2AddParameterCommand LocalIp(IPAddress value)
    {
        parameters["localip="] = value.ToString();
        return this;
    }
    public Netsh2AddParameterCommand RemoteIp(string value = "any")
    {
        parameters["Remoteip="] = value;
        return this;
    }
    public Netsh2AddParameterCommand RmtComputerGroup(string value = "any")
    {
        parameters["rmtcomputergrp="] = value;
        return this;
    }
    public Netsh2AddParameterCommand RmtUserGroup(string value = "any")
    {
        parameters["rmtusrgrp="] = value;
        return this;
    }
    public Netsh2AddParameterCommand RemoteIp(IPAddress value)
    {
        parameters["Remoteip="] = value.ToString();
        return this;
    }
    public Netsh2AddParameterCommand RemoteIp(EnFirewallRemoteip value)
    {
        parameters["Remoteip="] = value.ConvertToString();
        return this;
    }
    public Netsh2AddParameterCommand LocalPort(EnFirewallLocalPort value)
    {
        parameters["localport="] = value.ConvertToString();
        return this;
    }
    public Netsh2AddParameterCommand LocalPort(uint value)
    {
        parameters["localport="] = $"{value}";
        return this;
    }
    public Netsh2AddParameterCommand LocalPort(uint min, uint max)
    {
        parameters["localport="] = $"{min}-{max}";
        return this;
    }
    public Netsh2AddParameterCommand RemotePort(uint value)
    {
        parameters["remoteport="] = $"{value}";
        return this;
    }
    public Netsh2AddParameterCommand RemotePort(uint min, uint max)
    {
        parameters["remoteport="] = $"{min}-{max}";
        return this;
    }
    public Netsh2AddParameterCommand Protocol(EnFirewallProtocol value, string? type = null, string? code = null)
    {
        string type_code = "";
        if (type != null)
            type_code = $"{type}: {code}";
        parameters["protocol="] = value.ConvertToString();
        return this;
    }
    public Netsh2AddParameterCommand Protocol(byte value)
    {
        parameters["protocol="] = $"{value}";
        return this;
    }

    public Netsh2DoCommand<Netsh2GeneralResult> End()
    {
        return new(this);
    }
}