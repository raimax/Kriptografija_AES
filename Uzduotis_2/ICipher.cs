using System.Security.Cryptography;

namespace Uzduotis_2
{
    public interface ICipher
    {
        public string AlgorithmName { get; }
        public string Encode(string text, byte[] key, CipherMode mode);
        public string Decode(byte[] text, byte[] key, CipherMode mode);
    }
}
