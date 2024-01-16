using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public class UserAdditionalInfo
    {
        [Key]
        public long Id { get; init; }

        public DateTime RegistrationDate { get; init; }

        public string AboutMe { get; private set; }

        public string Hobbies { get; private set; }
        public static UserAdditionalInfo Default() =>
             new(0, DateTime.UtcNow, string.Empty, string.Empty);

        public UserAdditionalInfo(long id, DateTime registrationDate, string aboutMe, string hobbies)
        {
            Id = id;
            RegistrationDate = registrationDate;
            AboutMe = aboutMe;
            Hobbies = hobbies;
        }
        public void Update(string newAboutMe, string newHobbies)
        {
            AboutMe=newAboutMe??string.Empty;
            Hobbies = newHobbies ?? string.Empty;
        }
    }

}
