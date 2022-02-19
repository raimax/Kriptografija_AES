namespace Uzduotis_2
{
    public interface ICipher
    {
        public string AlgorithmName { get; }
        public string Encode(string text);
        public string Decode(string text);
    }
}
