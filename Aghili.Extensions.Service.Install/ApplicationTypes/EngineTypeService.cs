namespace Aghili.Extensions.Service.Install.ApplicationTypes;

internal class EngineTypeService : IEngineType
{
    //private ServiceBase application;

    public string Name { set; get; }// => application.get_ServiceName();

    public EngineTypeService(string applicationName)
    {
        this.Name= applicationName;
    }
}