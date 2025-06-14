using System;
using System.Runtime.InteropServices;

namespace Greetings
{
    /// <summary>
    /// Provides access to greeting functionality implemented in Rust.
    /// </summary>
    public static class GreetingService
    {
        // Import the native function from the Rust shared library
        [DllImport("greetings_rust", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_greeting();

        [DllImport("greetings_rust", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_greeting(IntPtr ptr);

        /// <summary>
        /// Gets a greeting message from the Rust library.
        /// </summary>
        /// <returns>A greeting string.</returns>
        public static string GetGreeting()
        {
            // Get the pointer from the Rust function
            IntPtr ptr = get_greeting();
            
            try
            {
                // Convert the unmanaged string to a managed string
                return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                // Free the memory allocated by the Rust code
                if (ptr != IntPtr.Zero)
                {
                    free_greeting(ptr);
                }
            }
        }
    }
}

