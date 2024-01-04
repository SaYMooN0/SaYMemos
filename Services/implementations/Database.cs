using SaYMemos.Services.interfaces;

namespace SaYMemos.Services.implementations
{
    public class Database : IDatabase
    {
        private string ConnectionString { get; }

        public Database(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public bool TryOpenConnection()
        {
            return true;
        }
    }
}
