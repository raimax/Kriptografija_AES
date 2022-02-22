using System;
using System.IO;
using System.Security.Cryptography;

namespace Uzduotis_2
{
    public class AES : ICipher
    {
        string ICipher.AlgorithmName => "AES cipher";

        public string Encode(string text, byte[] key, CipherMode mode)
        {
            if (text == null || text.Length < 1) throw new ArgumentNullException($"Text not provided in {nameof(Encode)}");
            if (key == null || key.Length < 1) throw new ArgumentNullException($"Key not provided in {nameof(Encode)}");

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = mode;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.Key);

                using (MemoryStream msEncrypt = new ())
                {
                    using (CryptoStream csEncrypt = new (msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new (csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);

        }

        public string Decode(byte[] text, byte[] key, CipherMode mode)
        {
            if (text == null || text.Length < 1) throw new ArgumentNullException($"Text not provided in {nameof(Decode)}");
            if (key == null || key.Length < 1) throw new ArgumentNullException($"Key not provided in {nameof(Decode)}");

            string decodedText = null;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = mode;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.Key);

                using (MemoryStream msDecrypt = new MemoryStream(text))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decodedText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return decodedText;
        }
    }
}
