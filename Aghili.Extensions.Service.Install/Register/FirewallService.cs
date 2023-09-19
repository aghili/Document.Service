using Aghili.Extensions.Service.Install.Register.FirewallServices;

namespace Aghili.Extensions.Service.Install.Register;

public class FirewallService
{
    private IFirewallServiceRegister? _InstallerEngine;

    public bool IsFirewallEnabled => InstallerEngine.IsFirewallEnabled;
    public bool IsFirewallInstalled => InstallerEngine.IsFirewallInstalled;
    public bool AppAuthorizationsAllowed => InstallerEngine.AppAuthorizationsAllowed;
    private IFirewallServiceRegister InstallerEngine => _InstallerEngine ??= FirewallServiceFwMgrRegister.IsReady ? new FirewallServiceFwMgrRegister() : new FirewallServices.netsh2.FirewallServiceNetsh2Register();

    internal void GrantAuthorization(string processPath, string name)
    {
        InstallerEngine.GrantAuthorization(processPath, name);
    }

    internal bool HasAuthorization(string processPath)
    {
        return InstallerEngine.HasAuthorization(processPath);
    }

    internal void RemoveAuthorization(string processPath)
    {
        InstallerEngine.RemoveAuthorization(processPath);
    }
}