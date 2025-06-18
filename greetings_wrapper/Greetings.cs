using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Greetings
{
    /// <summary>
    /// Interface for platform-specific Rust library interactions.
    /// </summary>
    internal interface IRustLibrary
    {
        IntPtr GetGreeting();
        void FreeGreeting(IntPtr ptr);
    }

    /// <summary>
    /// macOS implementation of the Rust library interface.
    /// </summary>
    internal class MacOSRustLibrary : IRustLibrary
    {
        [DllImport("greetings_wrapper/libgreetings_rust.dylib", EntryPoint = "get_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_greeting();

        [DllImport("greetings_wrapper/libgreetings_rust.dylib", EntryPoint = "free_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_greeting(IntPtr ptr);

        public IntPtr GetGreeting() => get_greeting();
        public void FreeGreeting(IntPtr ptr) => free_greeting(ptr);
    }

    /// <summary>
    /// Linux implementation of the Rust library interface.
    /// </summary>
    internal class LinuxRustLibrary : IRustLibrary
    {
        [DllImport("greetings_wrapper/libgreetings_rust.so", EntryPoint = "get_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_greeting();

        [DllImport("greetings_wrapper/libgreetings_rust.so", EntryPoint = "free_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_greeting(IntPtr ptr);

        public IntPtr GetGreeting() => get_greeting();
        public void FreeGreeting(IntPtr ptr) => free_greeting(ptr);
    }

    /// <summary>
    /// Windows implementation of the Rust library interface.
    /// </summary>
    internal class WindowsRustLibrary : IRustLibrary
    {
        [DllImport("greetings_wrapper/greetings_rust.dll", EntryPoint = "get_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_greeting();

        [DllImport("greetings_wrapper/greetings_rust.dll", EntryPoint = "free_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_greeting(IntPtr ptr);

        public IntPtr GetGreeting() => get_greeting();
        public void FreeGreeting(IntPtr ptr) => free_greeting(ptr);
    }

    /// <summary>
    /// Provides access to greeting functionality implemented in Rust.
    /// </summary>
    public static class GreetingService
    {
        private static readonly IRustLibrary _rustLibrary = CreateRustLibrary();

        private static IRustLibrary CreateRustLibrary()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return new MacOSRustLibrary();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return new LinuxRustLibrary();
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return new WindowsRustLibrary();
            else
                throw new PlatformNotSupportedException("Unsupported platform");
        }

        /// <summary>
        /// Gets a greeting message from the Rust library.
        /// </summary>
        /// <returns>A greeting string.</returns>
        public static string GetGreeting()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = _rustLibrary.GetGreeting();
                return Marshal.PtrToStringAnsi(ptr);
            }
            catch (DllNotFoundException ex)
            {
                Console.WriteLine($"Failed to load native library: {ex.Message}");
                Console.WriteLine($"Current platform: {RuntimeInformation.OSDescription}");
                Console.WriteLine($"Current architecture: {RuntimeInformation.OSArchitecture}");
                Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
                return "Error: Native library not found";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return "Error: " + ex.Message;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    try
                    {
                        _rustLibrary.FreeGreeting(ptr);
                    }
                    catch (Exception)
                    {
                        // Ignore errors in cleanup
                    }
                }
            }
        }
    }
}
