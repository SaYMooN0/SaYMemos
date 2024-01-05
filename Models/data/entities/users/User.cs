using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public class User
    {
        [Key]
        public long Id { get; init; }
        public string Nickname { get; private set; }
        public bool IsAccountPrivate { get; private set; }
        public bool IsLastLoginDatePrivate { get; private set; }
        public DateTime LastLoginDate { get; private set; }

        public long AdditionalInfoId { get; init; }
        public long LoginInfoId { get; init; }
        public long UserLinksId { get; init; }


        public virtual UserAdditionalInfo AdditionalInfo { get; set; }
        public virtual LoginInfo LoginInfo { get; set; }
        public virtual UserLinks UserLinks { get; set; }
    }
}
