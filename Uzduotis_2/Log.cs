using System;

namespace Uzduotis_2
{
    public static class Log
    {
        public static void Error(string message)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
