using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{


    public record UserLinks(
        [property: Key] long Id,
        string GithubLink,
        string TelegramLink,
        string YoutubeLink,
        string DiscordLink
    )
    {
        static public UserLinks Default() => new UserLinks(0, string.Empty, string.Empty, string.Empty, string.Empty);
    }

}
