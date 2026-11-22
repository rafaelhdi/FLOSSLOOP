namespace FLOSSLOOP.Database
{
    public class DatabaseSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Load database settings dari environment variables (dari .env file)
        /// </summary>
        public static DatabaseSettings LoadFromEnvironment()
        {
            return new DatabaseSettings
            {
                Host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost",
                Port = int.TryParse(Environment.GetEnvironmentVariable("DB_PORT"), out var port) ? port : 5432,
                Database = Environment.GetEnvironmentVariable("DB_NAME") ?? "FLOSSLOOP_db",
                Username = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "postgres",
                Password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password"
            };
        }
    }
}
