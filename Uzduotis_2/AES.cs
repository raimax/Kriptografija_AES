using System;
using System.Security.Cryptography;

namespace Uzduotis_2
{
    public class AES : ICipher
    {
        string ICipher.AlgorithmName => "AES cipher";

        public byte[] Encode(string text, byte[] key, byte[] IV)
        {
            if (text == null || text.Length < 1) throw new ArgumentNullException($"Text not provided in {nameof(Encode)}");
            if (key == null || key.Length < 1) throw new ArgumentNullException($"Key not provided in {nameof(Encode)}");
            if (IV == null || IV.Length < 1) throw new ArgumentNullException($"IV not provided in {nameof(Encode)}");

            using Aes aes = Aes.Create();

            aes.Key = key;
            aes.IV = IV;
        }

        public string Decode(string text, byte[] key, byte[] IV)
        {
            throw new System.NotImplementedException();
        }
    }
}
