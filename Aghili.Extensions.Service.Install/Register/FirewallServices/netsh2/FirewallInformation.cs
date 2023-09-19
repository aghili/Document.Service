namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2
{
    public class FirewallInformation
    {
        public EnIcmpSettingType? Type { get; internal set; }
        public string? Program { get; set; }
        public string? Name { get; set; }
        public EnFirewallStatus? Mode { get; set; }
        public EnFirewallScope? Scope { get; set; }
        public List<string>? Addresses { get; set; }
        public EnFirewallProfile? Profile { get; set; }
        public EnFirewallStatus? Exceptions { get; set; }
        public EnFirewallNetworkProtocol? Protocol { get; set; }
        public int? Port { get; set; }
        public string? Interface { get; set; }
    }
}