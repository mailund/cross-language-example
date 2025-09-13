using Shouldly;
using Xunit;
using Greetings;

public class GreetingsTests
{
    [Fact]
    public void GetGreeting_Returns_NonEmptyString()
    {
        var greeting = GreetingService.GetGreeting();
        greeting.ShouldNotBeNullOrWhiteSpace();
        // Optionally print for debug
        System.Console.WriteLine($"Greeting: {greeting}");
    }
}
