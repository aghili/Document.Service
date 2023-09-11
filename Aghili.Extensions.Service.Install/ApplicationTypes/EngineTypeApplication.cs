namespace Aghili.Extensions.Service.Install.ApplicationTypes;

internal class EngineTypeApplication : IEngineType
{
    public string Name => AppDomain.CurrentDomain.FriendlyName;
}