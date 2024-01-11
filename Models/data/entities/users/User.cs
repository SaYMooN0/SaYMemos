using System.ComponentModel.DataAnnotations;
namespace SaYMemos.Models.data.entities.users
{
    public class User
    {
        [Key]
        public long Id { get; init; }
        public string Nickname { get; private set; }
        public string ProfilePicturePath { get; private set; } = "default_profile_picture.png";
        public bool IsAccountPrivate { get; private set; }
        public bool IsLastLoginDatePrivate { get; private set; }
        public DateTime LastLoginDate { get; private set; }
        public bool AreLinksPrivate { get; private set; }

        public long UserLinksId { get; init; }
        public long AdditionalInfoId { get; init; }
        public long LoginInfoId { get; init; }

        public virtual UserAdditionalInfo AdditionalInfo { get; set; }
        public virtual LoginInfo LoginInfo { get; set; }
        public virtual UserLinks UserLinks { get; set; }
        public static User CreateNewUser(string nickname, long loginInfoId, long additionalInfoId, long userLinksId)
        {
            return new User
            {
                Nickname = nickname,
                ProfilePicturePath = "",
                IsAccountPrivate = false,
                LastLoginDate = DateTime.UtcNow,
                IsLastLoginDatePrivate = false,
                AreLinksPrivate = false,

                UserLinksId = userLinksId,
                LoginInfoId = loginInfoId,
                AdditionalInfoId = additionalInfoId,

            };
        }
        public override string ToString() =>
            $"ID: {Id}, Nickname: {Nickname}";
        public void UpdateLastLoginDate() => LastLoginDate = DateTime.UtcNow;
        public string GetLastTimeOnlineString() => IsLastLoginDatePrivate
            ? (DateTime.Now - LastLoginDate).TotalDays < 7 ? "Was online a long time ago" : "Was online recently"
            : LastLoginDate.Date == DateTime.Today ? "Was online today at " + LastLoginDate.ToString("HH:mm")
            : LastLoginDate.Date == DateTime.Today.AddDays(-1) ? "Was online yesterday at " + LastLoginDate.ToString("HH:mm")
            : LastLoginDate.Year == DateTime.Now.Year ? "Was online on " + LastLoginDate.ToString("dd MMM")
            : "Was online on " + LastLoginDate.ToString("dd MMM yyyy");

        public Dictionary<string,string> GetUserLinksDictionary() =>UserLinks.ParseToNonEmptyDictionary();
        public void SetProfilePicture(string picturePath) => ProfilePicturePath = picturePath;
    }

}
