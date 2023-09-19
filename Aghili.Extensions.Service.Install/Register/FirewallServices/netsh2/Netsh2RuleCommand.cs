using Aghili.Extensions.Service.Install.Utilities;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2RuleCommand : Netsh2BaseCommand
{
    public Netsh2RuleCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }

    public Netsh2AddParameterCommand Add(string name, EnFirewallDirection dir, EnFirewallAction action)
    {
        parameters.Add("add rule", "");
        parameters.Add("name=", name);
        parameters.Add("dir=", dir.ConvertToString());
        parameters.Add("action=", action.ConvertToString());
        return new(this);
    }


    public Netsh2DeleteParameterCommand Delete(string name)
    {
        parameters.Add("delete rule", "");
        parameters.Add("name=", name);
        return new(this);
    }

    public Netsh2ShowParameterCommand Show(string name = "all")
    {
        parameters.Add("show rule", "");
        parameters.Add("name=", name);
        return new(this);
    }
}
