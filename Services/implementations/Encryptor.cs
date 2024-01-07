using SaYMemos.Services.interfaces;
using System.Security.Cryptography;
using System.Text;

namespace SaYMemos.Services.implementations
{
    public class Encryptor : IEncryptor
    {
        private const int KeySize = 16;

        private readonly byte[] _idEncKey, _confirmationEncKey, _passwordEncKey;

        public Encryptor(string idEncKey, string confirmationEncKey, string passwordEncKey)
        {
            _idEncKey = Generate16ByteKey(idEncKey);
            _confirmationEncKey = Generate16ByteKey(confirmationEncKey);
            _passwordEncKey = Generate16ByteKey(passwordEncKey);
        }

        public string EncryptPassword(string password) =>
           Encrypt(_passwordEncKey, password);

        public string DecryptPassword(string encryptedPassword) =>
            Decrypt(_passwordEncKey, encryptedPassword);

        public string EncryptId(string id) =>
            Encrypt(_idEncKey, id);

        public string DecryptId(string encryptedId) =>
            Decrypt(_idEncKey, encryptedId);

        public string EncryptConfirmationId(string confirmationId) =>
            Encrypt(_confirmationEncKey, confirmationId);

        public string DecryptConfirmationId(string encryptedConfirmationId) =>
            Decrypt(_confirmationEncKey, encryptedConfirmationId);

        private byte[] Generate16ByteKey(string key)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            Array.Resize(ref keyBytes, KeySize);
            return keyBytes;
        }

        private string Encrypt(byte[] key, string text)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(text);
                    }

                    var iv = aes.IV;
                    var encrypted = memoryStream.ToArray();
                    var result = new byte[iv.Length + encrypted.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        private string Decrypt(byte[] key, string encryptedText)
        {
            var fullCipher = Convert.FromBase64String(encryptedText);
            var iv = new byte[KeySize];
            var cipher = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream(cipher))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
