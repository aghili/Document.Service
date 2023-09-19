﻿using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2AdvFirewallCommand:Netsh2BaseCommand
{
    public Netsh2AdvFirewallCommand(Netsh2BaseCommand parent):base(parent)
    {
        parameters.Add("AdvFirewall", "");
    }
    public Netsh2FirewallCommand Firewall()
    {
        return new Netsh2FirewallCommand(this);
    }

    public Netsh2ShowCommand Show()
    {
        return new Netsh2ShowCommand(this);
    }
}

public class Netsh2ShowCommand : Netsh2BaseCommand
{
    public Netsh2ShowCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }

    public Netsh2DoCommand<Netsh2ShowProfilesResult> AllProfiles()
    {
        parameters.Add("Show", "All");
        return new Netsh2DoCommand<Netsh2ShowProfilesResult>(this);
    }
    public Netsh2DoCommand<Netsh2ShowProfilesResult> Profile(EnFirewallProfile profile)
    {
        parameters.Add("Show", $"{profile}Profile");
        return new Netsh2DoCommand<Netsh2ShowProfilesResult>(this);
    }
    public Netsh2DoCommand<Netsh2ShowProfilesResult> CurrentProfile()
    {
        parameters.Add("Show", $"CurrentProfile");
        return new Netsh2DoCommand<Netsh2ShowProfilesResult>(this);
    }
}

public class Netsh2ShowProfilesResult:Netsh2GeneralResult
{
    const string Pattern = "^((?<profile>\\w+) Profile Settings:)|(?<key>.+) \\s+ (?<value>.+)$";//"(\\w*) Profile Settings:\\s-*\\s(((.*) \\s* (.*))\\s)*\\sLogging:\\s(((.*) \\s* (.*))\\s)*";

    public Netsh2ShowProfilesResult(Netsh2GeneralResult result):base(result.ExitCode,result.Message)
    {
        var models_properties = Utilities.ModelExtention.GetModelProperties(Pattern, Message, "profile",true);

        foreach (var model in models_properties)
        {
            var item = new Netsh2ShowProfileResult(model);
            Items.Add(item);
        }
    }
    public List<Netsh2ShowProfileResult> Items { get; set; } = new();

}

public class Netsh2ShowProfileResult
{
    public Netsh2ShowProfileResult(Dictionary<string, string> properties)
    {
        foreach (var property in properties)
            Utilities.ModelExtention.SetModelProperty(this, property.Key, property.Value);
    }

    public string Profile { get; set; }
    public EnProfileState State { get; set; }
    public EnFirewallPolicy[] FirewallPolicies { get; set; }
    public string LocalFirewallRules { get; set; }
    public string LocalConSecRules { get; set; }
    public EnFirewallStatus InboundUserNotification { get; set; }
    public EnFirewallStatus RemoteManagement { get; set; }
    public EnFirewallStatus UnicastResponseToMulticast { get; set; }

}