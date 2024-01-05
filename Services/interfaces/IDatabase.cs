namespace SaYMemos.Services.interfaces
{
    public interface IDatabase
    {
        public bool IsEmailTaken(string email);
    }
}
