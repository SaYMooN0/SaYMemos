using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public class UserAdditionalInfo
    {
        [Key]
        public long Id { get; init; }
        public string FullName { get; private set; }
        public DateTime RegistrationDate { get; init; }
        public string AdditionalInfo { get; private set; }
        public string Hobbies { get; private set; }
    }
}
