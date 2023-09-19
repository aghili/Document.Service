namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2FirewallCommand : Netsh2BaseCommand
{
    public Netsh2FirewallCommand(Netsh2BaseCommand parent) : base(parent)
    {
        parameters.Add("Firewall", "");
    }

    public Netsh2RuleCommand Rule()
    {
        return new Netsh2RuleCommand(this);
    }
}
