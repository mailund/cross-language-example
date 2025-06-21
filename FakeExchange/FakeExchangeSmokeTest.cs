using System;

public class TestAgent : Agent
{
    public int LastEventId { get; private set; } = -1;

    public TestAgent(int id, string name) : base(id, name) { }

    public override void OnEvent(int eventId)
    {
        LastEventId = eventId;
    }
}

public static class FakeExchangeSmokeTest
{
    public static void Main()
    {
        var exchange = new FakeExchange();
        int orderId = exchange.AddOrder(10, 100);
        var order = exchange.GetOrder(orderId);

        if (order == null || order.Quantity != 10 || order.Price != 100)
            throw new Exception("AddOrder or GetOrder failed");

        bool updated = exchange.UpdateOrder(orderId, 20, 200);
        if (!updated)
            throw new Exception("UpdateOrder failed");

        var updatedOrder = exchange.GetOrder(orderId);
        if (updatedOrder.Quantity != 20 || updatedOrder.Price != 200)
            throw new Exception("UpdateOrder did not update values");

        var agent = new TestAgent(1, "TestAgent");
        exchange.ConnectAgent(agent);

        exchange.Event(42);

        if (agent.LastEventId != 42)
            throw new Exception("Agent did not receive event");

        Console.WriteLine("All FakeExchange smoke tests passed!");
    }
}
