using Aghili.Extensions.Service.Install.Utilities;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class NetshRuleCommand : Netsh2BaseCommand
{
    public NetshRuleCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }

    public NetshAddParameterCommand Add(string name, EnFirewallDirection dir, EnFirewallAction action)
    {
        parameters.Add("add rule", "");
        parameters.Add("name=", name);
        parameters.Add("dir=", dir.ConvertToString());
        parameters.Add("action=", action.ConvertToString());
        return new(this);
    }


    public NetshDeleteParameterCommand Delete(string name)
    {
        parameters.Add("delete rule", "");
        parameters.Add("name=", name);
        return new(this);
    }

    public NetshShowParameterCommand Show(string name = "all")
    {
        parameters.Add("show rule", "");
        parameters.Add("name=", name);
        return new(this);
    }
}
