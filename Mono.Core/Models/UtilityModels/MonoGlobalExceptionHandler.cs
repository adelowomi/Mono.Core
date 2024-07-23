using System;

namespace Mono.Core
{
    public class MonoGlobalExceptionHandler
    {
        public static void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception
            LogException((Exception)e.ExceptionObject);

            // Rethrow the exception or handle it as needed
            // return MonoStandardResponse<object>.Error((Exception)e.ExceptionObject);
        }

        private static void LogException(Exception ex)
        {
            // Implement your logging logic here
            Console.WriteLine($"Global Exception: {ex.Message}");
        }
    }
}
