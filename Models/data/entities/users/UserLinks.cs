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
        public Dictionary<string, string> ParseToNonEmptyDictionary() =>
            new Dictionary<string, string>
            {
                ["Github"] = GithubLink,
                ["Telegram"] = TelegramLink,
                ["Youtube"] = YoutubeLink,
                ["Discord"] = DiscordLink
            }
            .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    }

}
