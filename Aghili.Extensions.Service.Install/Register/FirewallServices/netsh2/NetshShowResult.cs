namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class NetshShowResult: NetshGeneralResult
{
    const String Pattern = "^(?<rulename>\\r\\n)|(?<key>.+): \\s+ (?<value>.+)$";
    public NetshShowResult(NetshGeneralResult result) :base(result.ExitCode,result.Message)
    {
        switch (Result)
        {
            case EnFirewallResult.FAILED:
                break;
            case EnFirewallResult.SUCCESS:
                parseResult();
                break;
            case EnFirewallResult.NO_RULE_MATCH:
                break;
            case EnFirewallResult.INVALID_ARGUMENTS:
                break;
            case EnFirewallResult.ADMINISTRATOR_REQUIRED:
                break;
            default:
                break;
        }
    }

    private void parseResult()
    {
        var models_properties = Utilities.ModelExtention.GetModelProperties(Pattern, Message,"rulename");
        foreach (var row in models_properties)
        {
            Rules.Add(new NetshShowRuleResult(row));
        }
    }

    public List<NetshShowRuleResult> Rules { get; private set; } = new();
}
