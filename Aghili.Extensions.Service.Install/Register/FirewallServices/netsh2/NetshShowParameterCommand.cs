using Aghili.Extensions.Service.Install.Utilities;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class NetshShowParameterCommand : Netsh2BaseCommand
{
    public NetshShowParameterCommand(Netsh2BaseCommand parent) : base(parent)
    {
    }
    public NetshShowParameterCommand Profile(EnFirewallProfile value)
    {
        parameters["profile="] = value.ConvertToString();
        return this;
    }
    public NetshShowParameterCommand Profile(EnFirewallType value)
    {
        parameters["type="] = value.ConvertToString();
        return this;
    }
    public NetshShowParameterCommand Verbose()
    {
        parameters["verbose"] = "";
        return this;
    }
    public NetshDoCommand<NetshShowResult> End()
    {
        return new(this);
    }
}