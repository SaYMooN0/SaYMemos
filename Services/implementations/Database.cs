using SaYMemos.Services.interfaces;

namespace SaYMemos.Services.implementations
{
    public class Database : IDatabase
    {
        private string ConnectionString { get; }
        private interfaces.ILogger Logger { get; }
        public Database(string connectionString, interfaces.ILogger logger)
        {
            ConnectionString = connectionString;
            Logger = logger;
        }
        public bool TryOpenConnection()
        {
            return true;
        }
    }
}
