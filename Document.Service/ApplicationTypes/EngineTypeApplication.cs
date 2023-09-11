namespace Document.Service.ApplicationTypes;

internal class EngineTypeApplication : IEngineType
{
    public string Name => AppDomain.CurrentDomain.FriendlyName;
}