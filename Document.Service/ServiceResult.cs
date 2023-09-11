using System.ServiceProcess;

namespace Document.Service;

[Serializable]
public class ServiceResult
{
    public ServiceControllerStatus ServiceRunStatus { set; get; }

    public bool ServiceIsInstalled { set; get; }

    public bool FirewallRuleAdded { set; get; }

    public bool FirewallIsInstall { set; get; }

    public bool FirewallIsEnable { set; get; }

    public string Message { get; set; }

    public bool Result { get; set; }

    public bool AppAuthorizationsAllowed { get; internal set; }

    internal string ToStringXml() => throw new NotImplementedException();

    internal string ToSerialize() => throw new NotImplementedException();

    internal string ToStringJson() => "{" + string.Format("\"ServiceRunStatus\":\"{0}\",", (object)this.ServiceRunStatus) + string.Format("\"ServiceIsInstalled\":{0},", (object)this.ServiceIsInstalled) + string.Format("\"FirewallIsInstall\":{0},", (object)this.FirewallIsInstall) + string.Format("\"FirewallIsEnable\":{0},", (object)this.FirewallIsEnable) + string.Format("\"FirewallRuleAdded\":{0},", (object)this.FirewallRuleAdded) + "\"Message\":\"" + this.Message + "\"," + string.Format("\"Result\":{0}", (object)this.Result) + "}";
}