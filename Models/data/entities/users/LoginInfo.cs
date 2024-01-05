using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public record LoginInfo(
        [property: Key] long Id,
        string Login,
        string PasswordHash
    );

 
}
