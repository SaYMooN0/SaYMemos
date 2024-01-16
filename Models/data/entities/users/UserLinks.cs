using System.ComponentModel.DataAnnotations;

namespace SaYMemos.Models.data.entities.users
{
    public class UserLinks
    {
   

        [Key]
        public long Id { get; init; }

        public string GitHub { get; private set; }

        public string Telegram { get; private set; }

        public string Youtube { get; private set; }

        public string Discord { get; private set; }
        public UserLinks(long id, string gitHub, string telegram, string youtube, string discord)
        {
            Id = id;
            GitHub = gitHub;
            Telegram = telegram;
            Youtube = youtube;
            Discord = discord;
        }
        public void Update(string newGithubLink, string newTelegramLink, string newYoutubeLink, string newDiscordLink)
        {
            GitHub = newGithubLink ?? string.Empty;
            Telegram = newTelegramLink ?? string.Empty;
            Youtube = newYoutubeLink ?? string.Empty;
            Discord = newDiscordLink ?? string.Empty;
        }

        public static UserLinks Default() =>
            new UserLinks(0, string.Empty, string.Empty, string.Empty, string.Empty);

        public Dictionary<string, string> ParseToNonEmptyDictionary()
        {
            return new Dictionary<string, string>
            {
                ["Github"] = GitHub,
                ["Telegram"] = Telegram,
                ["Youtube"] = Youtube,
                ["Discord"] = Discord
            }
            .Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
