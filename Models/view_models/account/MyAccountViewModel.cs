using SaYMemos.Models.data.entities.users;

namespace SaYMemos.Models.view_models.account
{
    public record MyAccountViewModel(
       string nickname,
       string profilePicturePath,
       Dictionary<string, string> links,
       string fullName,
       string registrationDate,
       string additionalInfo,
       string hobbies
       )
    {
        public static MyAccountViewModel FromUser(User user) =>
            new(user.Nickname,
                user.ProfilePicturePath,
                user.UserLinks.ParseToNonEmptyDictionary(),
                user.AdditionalInfo.FullName,
                user.AdditionalInfo.RegistrationDate.ToString("D"),
                user.AdditionalInfo.AdditionalInfo,
                user.AdditionalInfo.Hobbies);
    }

}
