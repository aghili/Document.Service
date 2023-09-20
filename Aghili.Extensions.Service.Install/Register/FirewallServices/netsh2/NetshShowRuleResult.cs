namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2
{
    public class NetshShowRuleResult
    {
        public NetshShowRuleResult(Dictionary<string, string> properties)
        {
            foreach (var property in properties)
                Utilities.ModelExtention.SetModelProperty(this, property.Key, property.Value);
        }
        public string Rule_Name { get; set; }
        public EnFirewallEnable Enabled { get; set; }
        public EnFirewallDirection Direction { get; set; }
        public EnFirewallProfile[] Profiles { get; set; }
        public string LocalIP { get; set; }
        public string RemoteIP { get; set; }
        public string Protocol { get; set; }
        public string Program { get; set; }
        public string Description { get; set; }
        public string Service { get; set; }
        public string Security { get; set; }
        public EnFirewallAction Action { get; set; }
    }
}