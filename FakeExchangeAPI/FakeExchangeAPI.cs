using System;
using System.Runtime.InteropServices;

public static class FakeExchangeAPI
{
    // Create a handle to a new FakeExchange instance (not C-callable)
    public static IntPtr MakeExchangeHandle(FakeExchange exchange)
    {
        GCHandle handle = GCHandle.Alloc(exchange);
        return GCHandle.ToIntPtr(handle);
    }

    // Free the handle when done (C-callable)
    [UnmanagedCallersOnly(EntryPoint = "free_exchange_handle")]
    public static void FreeExchangeHandle(IntPtr handle)
    {
        GCHandle.FromIntPtr(handle).Free();
    }

    // C-callable: add order
    [UnmanagedCallersOnly(EntryPoint = "add_order")]
    public static int AddOrder(IntPtr handle, double quantity, double price)
    {
        var exchange = (FakeExchange?)GCHandle.FromIntPtr(handle).Target;
        if (exchange == null) return -1;
        return exchange.AddOrder(quantity, price);
    }

    // C-callable: update order
    [UnmanagedCallersOnly(EntryPoint = "update_order")]
    public static int UpdateOrder(IntPtr handle, int orderId, double quantity, double price)
    {
        var exchange = (FakeExchange?)GCHandle.FromIntPtr(handle).Target;
        if (exchange == null) return 0;
        return exchange.UpdateOrder(orderId, quantity, price) ? 1 : 0;
    }

    // C-callable: get order quantity
    [UnmanagedCallersOnly(EntryPoint = "get_order_quantity")]
    public static double GetOrderQuantity(IntPtr handle, int orderId)
    {
        var exchange = (FakeExchange?)GCHandle.FromIntPtr(handle).Target;
        if (exchange == null) return -1;
        var order = exchange.GetOrder(orderId);
        return order?.Quantity ?? -1;
    }

    // C-callable: get order price
    [UnmanagedCallersOnly(EntryPoint = "get_order_price")]
    public static double GetOrderPrice(IntPtr handle, int orderId)
    {
        var exchange = (FakeExchange?)GCHandle.FromIntPtr(handle).Target;
        if (exchange == null) return -1;
        var order = exchange.GetOrder(orderId);
        return order?.Price ?? -1;
    }
}
