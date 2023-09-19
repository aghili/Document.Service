namespace Aghili.Extensions.Service.Install.Register.FirewallServices;

internal interface IFirewallServiceRegister
{
    bool IsFirewallEnabled { get; }
    static bool IsReady { get; }
    bool IsFirewallInstalled { get; }
    bool AppAuthorizationsAllowed { get; }

    void GrantAuthorization(string applicationFullPath, string appName);
    bool HasAuthorization(string applicationFullPath);
    void RemoveAuthorization(string applicationFullPath);
}