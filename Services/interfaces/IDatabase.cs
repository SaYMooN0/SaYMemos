using Microsoft.EntityFrameworkCore;
using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Services.interfaces
{
    public interface IDatabase
    {
        public bool IsEmailTaken(string email);
        public Task<long> AddUserToConfirmAsync(UserToConfirm user);
        public Task DeleteUserFromConfirmAsync(long id);
        public Task<bool> IsUserToConfirmExistsAsync(long id, string confirmationCode);
        public Task<long> AddNewConfirmedUser(UserToConfirm user);
        public Task<UserToConfirm?> GetUserToConfirmById(long id);
        public Task<string?> GetPasswordHashForEmail(string email);
        public Task<long?> GetUserIdByEmail(string email);
        public Task UpdateLastLoginDateForUser(long id);
    }
}
