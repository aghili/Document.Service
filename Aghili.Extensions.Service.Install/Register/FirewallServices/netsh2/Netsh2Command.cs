namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2Command : Netsh2BaseCommand
{
    public Netsh2Command() : base(null) { }
    public Netsh2AdvFirewallCommand AdvFirewall()
    {
        return new Netsh2AdvFirewallCommand(this);
    }
}