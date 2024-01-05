using Microsoft.EntityFrameworkCore;
using SaYMemos.Models.Data;
using SaYMemos.Services.interfaces;

namespace SaYMemos.Services.implementations
{
    public class Database : IDatabase
    {
        private readonly string _connectionString;
        private readonly interfaces.ILogger _logger;
        private readonly MemoDbContext _context; 

        public Database(string connectionString, interfaces.ILogger logger)
        {
            _connectionString = connectionString;
            _logger = logger;

            var optionsBuilder = new DbContextOptionsBuilder<MemoDbContext>();
            optionsBuilder.UseNpgsql(_connectionString);

            _context = new MemoDbContext(optionsBuilder.Options);
            _context.Database.EnsureCreated();
        }

        public bool IsEmailTaken(string email)=>
            _context.LoginInfos.Any(li => li.Login == email);
    }

}
