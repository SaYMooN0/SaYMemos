using SaYMemos.Services.interfaces;

namespace SaYMemos.Services.implementations
{
    public class Encryptor : IEncryptor
    {
        private interfaces.ILogger Logger { get; }
        private readonly string
            _idEncKey,
            _confirmationEncKey,
            _passwordEncKey;

        public Encryptor(interfaces.ILogger logger, string idEncKey, string confirmationEncKey, string passwordEncKey)
        {
            Logger = logger;
            _idEncKey = idEncKey;
            _confirmationEncKey = confirmationEncKey;
            _passwordEncKey = passwordEncKey;
        }

        public string EncryptId()
        {
            throw new NotImplementedException();
        }

        public string DecryptId()
        {
            throw new NotImplementedException();
        }

        public string EncryptConfirmationId()
        {
            throw new NotImplementedException();
        }

        public string DecryptConfirmationId()
        {
            throw new NotImplementedException();
        }

        public string EncryptPassword()
        {
            throw new NotImplementedException();
        }

        public string DecryptPassword()
        {
            throw new NotImplementedException();
        }
    }
}
