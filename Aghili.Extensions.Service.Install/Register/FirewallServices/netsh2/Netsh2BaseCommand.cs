namespace Aghili.Extensions.Service.Install.Register.FirewallServices.netsh2;

public class Netsh2BaseCommand
{
    internal Dictionary<string, string> parameters = new();
    internal Netsh2BaseCommand? parent;

    public Netsh2BaseCommand(Netsh2BaseCommand? parent)
    {
        this.parent = parent;
    }

    public string GetCommand()
    {
        string result = " ";
        foreach (var item in parameters)
            result += $" {item.Key} {item.Value} ";
        return parent?.GetCommand()+ result;
    }
    
}
