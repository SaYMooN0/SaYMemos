using System.ComponentModel.DataAnnotations;
namespace SaYMemos.Models.data.entities.users
{
    public record UserToConfirm(
        [property: Key] long Id,
        string Name,
        string Email,
        string Nickname,
        string PasswordHash
    );
}
