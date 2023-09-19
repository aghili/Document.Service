namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public enum EnFirewallResult
{
    FAILED = -1,
    SUCCESS = 0,
    NO_RULE_MATCH = 1,
    INVALID_ARGUMENTS = 2,
    ADMINISTRATOR_REQUIRED = 3,
}