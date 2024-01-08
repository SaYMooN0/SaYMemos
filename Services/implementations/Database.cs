using Microsoft.EntityFrameworkCore;
using SaYMemos.Models.data.entities.users;
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

        public bool IsEmailTaken(string email) =>
            _context.LoginInfos.Any(li => li.Login == email);
        public async Task<long> AddUserToConfirmAsync(UserToConfirm user)
        {
            var existingUser = await _context.UsersToConfirm
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser is not null)
            {
                var updatedUser = existingUser.UpdateWithSavedEmailAndId(user);
                _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
            }
            else
                _context.UsersToConfirm.Add(user);

            await _context.SaveChangesAsync();
            return existingUser?.Id ?? user.Id;
        }


        public async Task DeleteUserFromConfirmAsync(long id)
        {
            var user = await _context.UsersToConfirm.FindAsync(id);

            if (user is not null)
            {
                _context.UsersToConfirm.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserToConfirmExistsAsync(long id, string confirmationCode) =>
            await _context.UsersToConfirm.AnyAsync(u => u.Id == id && u.ConfirmationCode == confirmationCode);

        public async Task<long> AddNewConfirmedUser(UserToConfirm userToConfirm)
        {
            LoginInfo loginInfo = new(0, userToConfirm.Email, userToConfirm.PasswordHash);
            _context.LoginInfos.Add(loginInfo);

            var userAdditionalInfo = UserAdditionalInfo.Default();
            _context.UserAdditionalInfos.Add(userAdditionalInfo);

            var userLinks = UserLinks.Default();
            _context.UserLinks.Add(userLinks);

            await _context.SaveChangesAsync();

            var user = User.CreateNewUser(userToConfirm.Nickname, loginInfo.Id, userAdditionalInfo.Id, userLinks.Id);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.Info("New user added " + user);
            return user.Id;
        }
        public async Task<UserToConfirm?> GetUserToConfirmById(long id)
            => await _context.UsersToConfirm.FindAsync(id);
        public async Task<string?> GetPasswordHashForEmail(string email) =>
            await _context.LoginInfos.Where(li => li.Login == email).Select(li => li.PasswordHash).FirstOrDefaultAsync();

        public async Task<long?> GetUserIdByEmail(string email)=>
            await _context.Users
                .Where(u => u.LoginInfo.Login == email)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();
        public async Task UpdateLastLoginDateForUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user is not null)
            {
                user.UpdateLastLoginDate();
                await _context.SaveChangesAsync();
            }
        }
        public async Task<User?> GetUserByIdAsync(long id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
