namespace Uzduotis_2
{
    public class AES : ICipher
    {
        string ICipher.AlgorithmName => "AES cipher";

        public string Decode(string text)
        {
            throw new System.NotImplementedException();
        }

        public string Encode(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
