using SaYMemos.Models.form_classes;
using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public record UserToConfirm(
        [property: Key] long Id,
        string Nickname,
        string Email,
        string PasswordHash,
        string ConfirmationCode
    )
    {
        public static UserToConfirm FromRegistrationForm(RegistrationForm form, Func<string, string> encryptionFunc, string confirmationCode) =>
             new UserToConfirm(0, //default value then it will be changed by entity framework
                 form.Nickname, form.Email,
                 encryptionFunc(form.Password),
                 confirmationCode);
        public UserToConfirm UpdateWithSavedEmailAndId(UserToConfirm newInstance) =>
            new UserToConfirm(this.Id, newInstance.Nickname, this.Email, newInstance.PasswordHash, newInstance.ConfirmationCode);

    }

}
