using System;
using System.Runtime.InteropServices;

// Delegate type for the C callback (event handler)
[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate void AgentOnEventCallback(IntPtr cAgent, int eventId);

// Agent subclass that wraps a C object and dispatches to C
public class CAgent : Agent
{
    private readonly IntPtr _cAgentPtr;
    private readonly AgentOnEventCallback _onEventCallback;

    public CAgent(int id, string name, IntPtr cAgentPtr, AgentOnEventCallback onEventCallback)
        : base(id, name)
    {
        _cAgentPtr = cAgentPtr;
        _onEventCallback = onEventCallback;
    }

    public override void OnEvent(int eventId)
    {
        _onEventCallback(_cAgentPtr, eventId);
    }
}

// C-callable API for creating a CAgent and connecting it to the exchange
public static class CAgentAPI
{
    // C-callable: create and connect a CAgent to the exchange
    [UnmanagedCallersOnly(EntryPoint = "connect_c_agent")]
    public static int ConnectCAgent(
        IntPtr exchangeHandle,
        int agentId,
        [MarshalAs(UnmanagedType.LPStr)] string name,
        IntPtr cAgentPtr,
        IntPtr onEventCallbackPtr)
    {
        var exchange = (FakeExchange?)GCHandle.FromIntPtr(exchangeHandle).Target;
        if (exchange == null) return -1;

        var onEventCallback = Marshal.GetDelegateForFunctionPointer<AgentOnEventCallback>(onEventCallbackPtr);
        var agent = new CAgent(agentId, name, cAgentPtr, onEventCallback);
        return exchange.ConnectAgent(agent);
    }
}
