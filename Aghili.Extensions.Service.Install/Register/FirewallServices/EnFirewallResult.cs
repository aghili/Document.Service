namespace Aghili.Extensions.Service.Install.Register.FirewallServices;

public enum EnFirewallResult
{
    FAILED = 1,
    SUCCESS = 0,
    NO_RULE_MATCH = 2,
    INVALID_ARGUMENTS = 3,
    ADMINISTRATOR_REQUIRED = 4,
}