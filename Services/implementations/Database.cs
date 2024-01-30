using Microsoft.EntityFrameworkCore;
using SaYMemos.Models.data.entities.comments;
using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.form_classes;
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
            optionsBuilder.UseLazyLoadingProxies();

            _context = new MemoDbContext(optionsBuilder.Options);
            _context.Database.EnsureCreated();
        }

        public bool IsEmailTaken(string email) =>
            _context.LoginInfos.Any(li => li.Login == email);
        public async Task<long> AddUserToConfirm(UserToConfirm user)
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

        public async Task DeleteUserFromConfirm(long id)
        {
            var user = await _context.UsersToConfirm.FindAsync(id);

            if (user is not null)
            {
                _context.UsersToConfirm.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsUserToConfirmExists(long id, string confirmationCode) =>
            await _context.UsersToConfirm.AnyAsync(u => u.Id == id && u.ConfirmationCode == confirmationCode);

        public async Task<long> AddNewConfirmedUser(UserToConfirm userToConfirm)
        {
            LoginInfo loginInfo = new(0, userToConfirm.Email, userToConfirm.PasswordHash);
            _context.LoginInfos.Add(loginInfo);

            UserAdditionalInfo userAdditionalInfo = UserAdditionalInfo.Default();
            _context.UserAdditionalInfos.Add(userAdditionalInfo);

            UserLinks userLinks = UserLinks.Default();
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

        public async Task<long?> GetUserIdByEmail(string email) =>
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
        public async Task<User?> GetUserById(long id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        public async Task<Memo?> GetMemoById(Guid id) =>
            await _context.Memos.FirstOrDefaultAsync(m => m.id == id);
        public async Task<string?> GetProfilePictureById(long id) =>
            await _context.Users
            .Where(u => u.Id == id)
            .Select(u => u.ProfilePicturePath)
            .FirstOrDefaultAsync();
        public async Task SetProfilePictureForUser(long id, string imagePath)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is not null)
            {
                user.SetProfilePicture(imagePath);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateUserSettings(AccountSettingsForm data, long id)
        {
            User? user = await GetUserById(id);
            if (user is not null)
            {
                user.UpdateFromAccountSettings(data);
                await _context.SaveChangesAsync();
            }

        }

        public async Task AddNewMemo(long authorId, string authorComment, bool areCommentsAvailable, string? imagePath, List<string> tagValues)
        {
            var newMemo = Memo.CreateNew(authorId, authorComment, areCommentsAvailable, imagePath);

            foreach (var value in tagValues)
            {
                var tag = await _context.MemoTags.FirstOrDefaultAsync(t => t.Value == value);
                if (tag is null)
                {
                    tag = new MemoTag(0, value);
                    _context.MemoTags.Add(tag);
                }
                newMemo.Tags.Add(tag);
            }
            _context.Memos.Add(newMemo);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ChangeLikeState(long userId, Guid memoId)
        {
            var memo = await GetMemoById(memoId);
            User u = await GetUserById(userId);
            if (memo is null)
                return false;

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.userId == userId && l.memoGuid == memoId);

            if (like is not null)
            {
                _context.Likes.Remove(like);

                u.Likes.Remove(like);
                memo.Likes.Remove(like);
                await _context.SaveChangesAsync();
                
                return false;
            }
            else
            {
                var newLike = MemoLike.CreateNew(memoId, userId);

                _context.Likes.Add(newLike);
                u.Likes.Add(newLike);
                memo.Likes.Add(newLike);

                await _context.SaveChangesAsync();
                return true;
            }
        }

        public Task<Comment> AddCommentToMemo(Guid memoId, string memoComment, User user)
        {
            throw new NotImplementedException();
        }
    }
}
