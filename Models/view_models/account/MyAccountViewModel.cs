using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.view_models.memos;

namespace SaYMemos.Models.view_models.account
{
    public record MyAccountViewModel(
       string nickname,
       string profilePicturePath,
       Dictionary<string, string> links,
       string fullName,
       OneMemoViewModel[] memos
       )
    {
        public static MyAccountViewModel FromUser(User user)
        {
            var likedMemoIds = user.Likes.Select(l => l.id).ToHashSet();

            return new MyAccountViewModel(
                user.Nickname,
                user.ProfilePicturePath,
                user.UserLinks.ParseToNonEmptyDictionary(),
                user.FullName,
                user.PostedMemos
                    .Select(memo => OneMemoViewModel.FromMemo(memo, likedMemoIds.Contains(memo.id)))
                    .OrderByDescending(memo => memo.creationDate)
                    .ToArray()
            );
        }


    }

}
