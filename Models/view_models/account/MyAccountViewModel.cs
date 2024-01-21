using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.account
{
    public record MyAccountViewModel(
       string nickname,
       string profilePicturePath,
       Dictionary<string, string> links,
       string fullName,
       string registrationDate,
       string aboutMe,
       string hobbies,
       OneMemoViewModel[] memos
       )
    {
        public static MyAccountViewModel FromUser(User user) =>
            new(user.Nickname,
                user.ProfilePicturePath,
                user.UserLinks.ParseToNonEmptyDictionary(),
                user.FullName,
                user.AdditionalInfo.RegistrationDate.ToString("D"),
                user.AdditionalInfo.AboutMe,
                user.AdditionalInfo.Hobbies, 
                user.PostedMemos.Select(OneMemoViewModel.FromMemo).ToArray());
    }

}
