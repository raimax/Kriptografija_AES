namespace Uzduotis_2
{
    public interface ICipher
    {
        public string AlgorithmName { get; }
        public byte[] Encode(string text, byte[] key, byte[] IV);
        public string Decode(string text, byte[] key, byte[] IV);
    }
}
