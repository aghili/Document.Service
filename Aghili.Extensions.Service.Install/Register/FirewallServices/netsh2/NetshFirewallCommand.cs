namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class NetshFirewallCommand : Netsh2BaseCommand
{
    public NetshFirewallCommand(Netsh2BaseCommand parent) : base(parent)
    {
        parameters.Add("Firewall", "");
    }

    public NetshRuleCommand Rule()
    {
        return new NetshRuleCommand(this);
    }
}
