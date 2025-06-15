using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Greetings
{
    /// <summary>
    /// Provides access to greeting functionality implemented in Rust.
    /// </summary>
    public static class GreetingService
    {
        // Import the native functions from the Rust shared library with full paths

        // macOS
        [DllImport("src/greetings/libgreetings_rust.dylib", EntryPoint = "get_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_greeting_macos();

        [DllImport("src/greetings/libgreetings_rust.dylib", EntryPoint = "free_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_greeting_macos(IntPtr ptr);

        // Linux
        [DllImport("src/greetings/libgreetings_rust.so", EntryPoint = "get_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_greeting_linux();

        [DllImport("src/greetings/libgreetings_rust.so", EntryPoint = "free_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_greeting_linux(IntPtr ptr);

        // Windows
        [DllImport("src/greetings/greetings_rust.dll", EntryPoint = "get_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_greeting_windows();

        [DllImport("src/greetings/greetings_rust.dll", EntryPoint = "free_greeting", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_greeting_windows(IntPtr ptr);

        /// <summary>
        /// Gets a greeting message from the Rust library.
        /// </summary>
        /// <returns>A greeting string.</returns>
        public static string GetGreeting()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                // Get the pointer from the appropriate platform-specific function
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Console.WriteLine("Detected macOS, loading libgreetings_rust.dylib");
                    ptr = get_greeting_macos();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.WriteLine("Detected Linux, loading libgreetings_rust.so");
                    ptr = get_greeting_linux();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Console.WriteLine("Detected Windows, loading greetings_rust.dll");
                    ptr = get_greeting_windows();
                }
                else
                {
                    throw new PlatformNotSupportedException("Unsupported platform");
                }

                // Convert the unmanaged string to a managed string
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
                // Free the memory allocated by the Rust code
                if (ptr != IntPtr.Zero)
                {
                    try
                    {
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                            free_greeting_macos(ptr);
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                            free_greeting_linux(ptr);
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                            free_greeting_windows(ptr);
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
