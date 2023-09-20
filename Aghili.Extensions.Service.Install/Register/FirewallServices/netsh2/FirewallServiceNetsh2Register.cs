using Aghili.Extensions.Service.Install.Exceptions;

namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

internal class FirewallServiceNetshRegister : IFirewallServiceRegister
{
    public bool IsFirewallEnabled => FirewallEnabled();

    private bool FirewallEnabled()
    {
        var command =
                     new NetshCommand()
                     .AdvFirewall()
                     .Show()
                     .CurrentProfile();
        var result = command.Do();
        return result.Items.Any(item => item.State == EnProfileState.On);
    }

    public bool IsFirewallInstalled => true;

    public bool AppAuthorizationsAllowed => true;

    public void GrantAuthorization(string applicationFullPath, string appName)
    {
        var command =
            new NetshCommand()
            .AdvFirewall()
            .Firewall()
            .Rule()
            .Add(appName, EnFirewallDirection.IN, EnFirewallAction.allow)
            .Program(applicationFullPath)
            .End();
        HandleResult(command.Do());
    }

    private static void HandleResult(NetshGeneralResult result)
    {
        if (result == null)
            throw new ExceptionEngineRequirementsDidNotExist();

        switch (result.Result)
        {
            case EnFirewallResult.INVALID_ARGUMENTS:
            case EnFirewallResult.ADMINISTRATOR_REQUIRED:
            case EnFirewallResult.FAILED:
                throw new ExceptionFirewallHelper(result.Message);
            case EnFirewallResult.NO_RULE_MATCH:
            case EnFirewallResult.SUCCESS:
                break;
        }
    }

    public bool HasAuthorization(string applicationFullPath)
    {
        var command =
             new NetshCommand()
             .AdvFirewall()
             .Firewall()
             .Rule()
             .Show()
             .Verbose()
             .End();
        var result = command.Do();
        return result.Rules.Any(item => item.Program == applicationFullPath);
    }

    public void RemoveAuthorization(string applicationFullPath)
    {

        var command =
             new NetshCommand()
             .AdvFirewall()
             .Firewall()
             .Rule()
             .Show()
             .Verbose()
             .End();
        var result = command.Do();
        var rules = result.Rules.Where(item => item.Program == applicationFullPath).ToList();
        foreach ( var rule in rules )
        HandleResult(new NetshCommand()
          .AdvFirewall()
          .Firewall()
          .Rule()
          .Delete(rule.Rule_Name)
          .End().Do());
    }
}