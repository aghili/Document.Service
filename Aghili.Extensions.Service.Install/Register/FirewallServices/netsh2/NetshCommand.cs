namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class NetshCommand : Netsh2BaseCommand
{
    public NetshCommand() : base(null) { }
    public NetshAdvFirewallCommand AdvFirewall()
    {
        return new NetshAdvFirewallCommand(this);
    }
}