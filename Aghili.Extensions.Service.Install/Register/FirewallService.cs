using Aghili.Extensions.Service.Install.Extensions;
using Aghili.Extensions.Service.Install.Register.FirewallServices;
using Aghili.Extensions.Service.Install.Register.FirewallServices.NetFwMgr;

namespace Aghili.Extensions.Service.Install.Register;

public class FirewallService
{
    private IFirewallServiceRegister? _InstallerEngine;

    public bool IsFirewallEnabled => InstallerEngine.IsFirewallEnabled;
    public bool IsFirewallInstalled => InstallerEngine.IsFirewallInstalled;
    public bool AppAuthorizationsAllowed => InstallerEngine.AppAuthorizationsAllowed;
    private IFirewallServiceRegister InstallerEngine
    {
        get
        {
            return _InstallerEngine ??=
                  FirewallServiceRegister.IsReady ?
                  new FirewallServiceRegister() :
                  Environment.OSVersion.Version.RealWindowVersion().BuildNumber >= 7600 ?
              new FirewallServices.netsh2.FirewallServiceNetshRegister() :
              new FirewallServices.netsh1.FirewallServiceNetshRegister();
        }
    }


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