using Microsoft.EntityFrameworkCore;
using SaYMemos.Models.data.entities.comments;
using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.form_classes;

namespace SaYMemos.Services.interfaces
{
    public interface IDatabase
    {
        public bool IsEmailTaken(string email);
        public Task<long> AddUserToConfirm(UserToConfirm user);
        public Task DeleteUserFromConfirm(long id);
        public Task<bool> IsUserToConfirmExists(long id, string confirmationCode);
        public Task<long> AddNewConfirmedUser(UserToConfirm user);
        public Task<UserToConfirm?> GetUserToConfirmById(long id);
        public Task<string?> GetPasswordHashForEmail(string email);
        public Task<long?> GetUserIdByEmail(string email);
        public Task UpdateLastLoginDateForUser(long id);
        public Task<User?> GetUserById(long id);
        public Task<Memo?> GetMemoById(Guid id);
        public Task<string?> GetProfilePictureById(long id);
        public Task SetProfilePictureForUser(long id, string imagePath);
        public Task UpdateUserSettings(AccountSettingsForm data, long id);
        public Task AddNewMemo(long authorId, string authorComment,bool areCommentsAvailable, string imagePath, List<string> tagValues);
        public Task<bool> ChangeLikeState(long userId,Guid memoId);
        public Task<Comment?> AddCommentToMemo(Guid memoId, string memoComment, User user, Guid? parentCommentId = null);
        public Task<Comment?> GetCommentById(Guid id);
        public Task<bool?> ChangeCommentRatingByUser(Guid commentId, User user, bool isUp);
    }
}
