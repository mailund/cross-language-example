using System;
using System.IO;
using Shouldly;
using Xunit;
using Greetings;

public class GreetingsTests
{
    [Fact]
    public void GetGreeting_Returns_NonEmptyString()
    {
        // Ensure the dylib is in the expected subdirectory for the test (do this before any native call)
        var runfiles = Directory.GetCurrentDirectory();
        var testSrcDir = Environment.GetEnvironmentVariable("TEST_SRCDIR");
        var dylibName = "libgreetings_rust.dylib";
        var expectedPath = Path.Combine(runfiles, "greetings_wrapper", dylibName);
        if (!string.IsNullOrEmpty(testSrcDir) && Directory.Exists(testSrcDir))
        {
            foreach (var f in Directory.GetFiles(testSrcDir, "*", SearchOption.AllDirectories))
            {
                if (f.EndsWith(dylibName))
                {
                    var targetDir = Path.Combine(runfiles, "greetings_wrapper");
                    if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
                    var targetPath = Path.Combine(targetDir, dylibName);
                    if (!File.Exists(targetPath))
                        File.Copy(f, targetPath);
                }
            }
        }

        // Print working directory and files for debugging
        Console.WriteLine($"[DEBUG] Current directory: {runfiles}");
        foreach (var f in Directory.GetFiles(runfiles))
            Console.WriteLine($"[DEBUG] File: {f}");
        foreach (var d in Directory.GetDirectories(runfiles))
            Console.WriteLine($"[DEBUG] Dir: {d}");

        var greeting = GreetingService.GetGreeting();
        greeting.ShouldNotBeNullOrWhiteSpace();
        
        greeting.ShouldBe("Hello from Rust!");
        // Optionally print for debug
        System.Console.WriteLine($"Greeting: {greeting}");
    }
}
