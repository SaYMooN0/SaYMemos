namespace SaYMemos.Services.interfaces
{
    public interface IEmailService
    {
        public bool TrySendMessage(string email, string theme, string body);
        public bool TrySendConfirmationCode(string email, string code);
    }
}
