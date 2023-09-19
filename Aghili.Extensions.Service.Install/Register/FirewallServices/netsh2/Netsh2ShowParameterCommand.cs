using Aghili.Extensions.Service.Install.Utilities;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2ShowParameterCommand : Netsh2BaseCommand
{
    public Netsh2ShowParameterCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }
    public Netsh2ShowParameterCommand Profile(EnFirewallProfile value)
    {
        parameters["profile="] = value.ConvertToString();
        return this;
    }
    public Netsh2ShowParameterCommand Profile(EnFirewallType value)
    {
        parameters["type="] = value.ConvertToString();
        return this;
    }
    public Netsh2ShowParameterCommand Verbose()
    {
        parameters["verbose"] = "";
        return this;
    }
    public Netsh2DoCommand<Netsh2ShowResult> End()
    {
        return new(this);
    }
}