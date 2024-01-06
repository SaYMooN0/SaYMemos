using SaYMemos.Services.interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ILogger = SaYMemos.Services.interfaces.ILogger;

namespace SaYMemos.Services.implementations
{
    public class Encryptor : IEncryptor
    {
        private readonly byte[] _key;
        private readonly ILogger _logger;
        public Encryptor(string passwordEncKey, ILogger logger)
        {
            using var sha256 = SHA256.Create();
            _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordEncKey));
            _logger = logger;
        }

        public string EncryptPassword(string password)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.GenerateIV();
            var iv = aes.IV;
            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(password);
            }
            var encryptedContent = memoryStream.ToArray();
            var result = new byte[iv.Length + encryptedContent.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

            return Convert.ToBase64String(result);
        }

        public string DecryptPassword(string encryptedPassword)
        {
            var fullCipher = Convert.FromBase64String(encryptedPassword);
            using var aes = Aes.Create();
            aes.Key = _key;
            var iv = new byte[aes.BlockSize / 8];
            var cipher = new byte[fullCipher.Length - iv.Length];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, iv);
            using var memoryStream = new MemoryStream(cipher);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
    