using System.ComponentModel;
using System.ServiceProcess;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aghili.Extensions.Service.Install;

[Serializable]
public class ServiceResult
{
    [Description("Service running status.")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ServiceControllerStatus ServiceRunStatus { set; get; }

    public bool ServiceIsInstalled { set; get; }

    public bool FirewallRuleAdded { set; get; }

    public bool FirewallIsInstall { set; get; }

    public bool FirewallIsEnable { set; get; }

    public string Message { get; set; } = "";

    public bool Result { get; set; }

    public bool AppAuthorizationsAllowed { get; internal set; }

    internal string ToStringXml() => throw new NotImplementedException();

    internal string ToSerialize() => throw new NotImplementedException();

    internal string ToStringJson() => JsonSerializer.Serialize(this);
    }