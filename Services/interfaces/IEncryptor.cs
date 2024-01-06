namespace SaYMemos.Services.interfaces
{
    public interface IEncryptor
    {
        public string EncryptPassword(string password);
        public string DecryptPassword(string encryptedPassword);
    }
}
