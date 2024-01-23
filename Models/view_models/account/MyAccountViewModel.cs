using SaYMemos.Models.data.entities.users;

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
        public static MyAccountViewModel FromUser(User user) =>
            new(
                user.Nickname,
                user.ProfilePicturePath,
                user.UserLinks.ParseToNonEmptyDictionary(),
                user.FullName,
                user.PostedMemos
                    .Select(OneMemoViewModel.FromMemo)
                    .OrderByDescending(memo => memo.creationDate)
                    .ToArray()
      );

    }

}
