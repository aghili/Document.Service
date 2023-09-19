namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

[Serializable]
public class Netsh2GeneralResult
{
    public Netsh2GeneralResult(Netsh2GeneralResult result):this(result.ExitCode,result.Message) { }
    public Netsh2GeneralResult(int exitCode, string message)
    {
        this.ExitCode = exitCode;
        if (exitCode == 0)
        {
            Result = EnFirewallResult.SUCCESS;
        }
        else
        {
            if (message.Contains("No rules match the specified criteria."))
                Result = EnFirewallResult.NO_RULE_MATCH;
            else if (message.Contains(" is not a valid argument for this command."))
                Result = EnFirewallResult.INVALID_ARGUMENTS;
            else if (message.Contains("The requested operation requires elevation"))
                Result = EnFirewallResult.ADMINISTRATOR_REQUIRED;
        }
        Message = message;
    }

    public EnFirewallResult Result { set; get; } = EnFirewallResult.FAILED;
    public string? Message { get; set; }
    public int ExitCode { get; }
}