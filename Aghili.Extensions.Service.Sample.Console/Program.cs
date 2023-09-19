internal class Program
{
    private static void Main(string[] args)
    {
        new Aghili.Extensions.Service.Install.Engine("TestApp").Run(args);
        Console.ReadLine();
    }
}