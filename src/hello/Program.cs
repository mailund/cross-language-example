using System;
using Greetings;

class Program
{
    static void Main()
    {
        string message = GreetingService.GetGreeting();
        Console.WriteLine(message);
    }
}
