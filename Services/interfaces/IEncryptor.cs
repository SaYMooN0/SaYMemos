namespace SaYMemos.Services.interfaces
{
    public interface IEncryptor
    {
        public string EncryptId();
        public string DecryptId();
        public string EncryptConfirmationId();
        public string DecryptConfirmationId();
        public string EncryptPassword();
        public string DecryptPassword();
    }
}
