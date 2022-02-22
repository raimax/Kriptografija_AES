namespace Uzduotis_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new(new AES());

            menu.Start();
        }
    }
}
