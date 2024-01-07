namespace SaYMemos.Services.interfaces
{
    public interface IEncryptor
    {
        public string EncryptPassword(string password);
        public string DecryptPassword(string encryptedPassword);
        public string EncryptId(string id);
        public string DecryptId(string encryptedId);
        public string EncryptConfirmationId(string confirmationId);
        public string DecryptConfirmationId(string encryptedConfirmationId);
    }
}
