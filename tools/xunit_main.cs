using System;
using System.Threading.Tasks;
using Xunit.Runner.InProc.SystemConsole;

public static class Program
{
    public static int Main(string[] args)
        => ConsoleRunner.Run(Array.Empty<string>()).GetAwaiter().GetResult();
}