using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public record UserAdditionalInfo(
        [property: Key] long Id,
        DateTime RegistrationDate,
        string AboutMe,
        string Hobbies
    )
    {
        public static UserAdditionalInfo Default() => new UserAdditionalInfo(0, DateTime.UtcNow, string.Empty, string.Empty);
    }

}
