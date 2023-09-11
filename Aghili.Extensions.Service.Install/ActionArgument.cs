namespace Aghili.Extensions.Service.Install;

public class ActionArgument
{
    public EnCommand Command { get; set; }

    public Dictionary<EnArgument, string> Arguments { get; set; }

    public ActionArgument(ref Stack<string> args)
    {
        Command = ExtractCommand(ref args);
    }

    private EnCommand ExtractCommand(ref Stack<string> args)
    {
        if (args.Count != 0 && Enum.TryParse<EnCommand>(args.Pop(), out var result))
        {
            return result;
        }

        return EnCommand.none;
    }
}