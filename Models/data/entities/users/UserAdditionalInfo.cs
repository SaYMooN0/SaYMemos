using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public record UserAdditionalInfo(
        [property: Key] long Id,
        string FullName,
        DateTime RegistrationDate,
        string AdditionalInfo,
        string Hobbies
    )
    {
        public static UserAdditionalInfo Default() => new UserAdditionalInfo(0, string.Empty, DateTime.UtcNow, string.Empty, string.Empty);
    }

}
