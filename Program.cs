using FLOSSLOOP.Views;
using FLOSSLOOP.Database;

namespace FLOSSLOOP
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Load environment variables dari file .env
            LoadEnvironmentVariables();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }

        /// <summary>
        /// Load .env file dan set sebagai environment variables
        /// </summary>
        private static void LoadEnvironmentVariables()
        {
            string envFilePath = ".env";

            if (File.Exists(envFilePath))
            {
                var lines = File.ReadAllLines(envFilePath);
                foreach (var line in lines)
                {
                    // Skip empty lines dan comments
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    var parts = line.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        Environment.SetEnvironmentVariable(key, value);
                    }
                }
            }
        }
    }
}