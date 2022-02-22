﻿using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Uzduotis_2
{
    public class Menu
    {
        public Option SelectedOption { get; private set; } = Option.Undefined;
        public string Result { get; private set; }
        private readonly ICipher _cipher;

        public Menu(ICipher cipher)
        {
            _cipher = cipher;
        }

        public enum Option
        {
            Undefined = 0,
            Encode = 1,
            Decode = 2
        }

        public void Start()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{_cipher.AlgorithmName}\n");
            Console.ResetColor();

            DisplayOptions();

            BeginProcess();
        }

        public void DisplayOptions()
        {
            while (SelectedOption == Option.Undefined)
            {
                Console.WriteLine("Select option:\n");
                Console.WriteLine("1. Encode");
                Console.WriteLine("2. Decode");

                string inputOption = Console.ReadLine();

                switch (inputOption)
                {
                    case "1":
                        SelectedOption = Option.Encode;
                        break;
                    case "2":
                        SelectedOption = Option.Decode;
                        break;
                    default:
                        Console.WriteLine("Option doesn't exist\n");
                        break;
                }
            }
        }

        public void BeginProcess()
        {
            string textToDecode = "";
            
            if (SelectedOption == Option.Decode)
            {
                string selectedOption = InputText("Would you like to decode from file? y/n");

                if (selectedOption == "y")
                {
                    string fileName = InputText("Enter file name: ");
                    textToDecode = ReadFromFile(fileName).Result;
                }
                else
                {
                    textToDecode = InputText("Enter your text: ");
                }
            }
            else
            {
                textToDecode = InputText("Enter your text: ");
            }
            
            byte[] key = InputKey("Enter your key (16): ");
            CipherMode mode = SelectMode();

            switch (SelectedOption)
            {
                case Option.Encode:
                    Result = _cipher.Encode(textToDecode, key, mode);
                    break;
                case Option.Decode:
                    Result = _cipher.Decode(Convert.FromBase64String(textToDecode), key, mode);
                    break;
                default:
                    break;
            }

            DisplayResult();
            SelectedOption = Option.Undefined;
            Start();
        }

        private static CipherMode SelectMode()
        {
            string text = "";
            int mode = 0;

            while (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Select mode:\n");
                Console.WriteLine("1: CBC");
                Console.WriteLine("2: ECB");
                Console.WriteLine("3: OFB");
                Console.WriteLine("4: CFB");
                Console.WriteLine("5: CTS");
                text = Console.ReadLine();

                if (!int.TryParse(text, out mode) || !Enum.IsDefined(typeof(CipherMode), mode))
                {
                    text = "";
                }
            }

            return (CipherMode)mode;
        }

        private static string InputText(string title)
        {
            string inputText = "";

            while (string.IsNullOrWhiteSpace(inputText))
            {
                Console.WriteLine(title);
                inputText = Console.ReadLine();
            }

            return inputText;
        }

        private static byte[] InputKey(string title)
        {
            string inputText = "";

            while (string.IsNullOrWhiteSpace(inputText) || inputText.Length != 16)
            {
                Console.WriteLine(title);
                inputText = Console.ReadLine();
            }

            return System.Text.Encoding.UTF8.GetBytes(inputText);
        }

        private void DisplayResult()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Result: " + Result + "\n");
            Console.ResetColor();

            if (SelectedOption == Option.Encode)
            {
                string inputText = InputText("Would you like to save result to a file? y/n");

                if (inputText == "y")
                {
                    inputText = InputText("Enter file name: ");
                    WriteToFile(inputText, Result);
                }
            }
        }

        private static async void WriteToFile(string fileName, string data)
        {
            try
            {
                await FileManager.WriteToFile(fileName, data);
                Console.WriteLine($"Result saved to file \"{fileName}\"\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<string> ReadFromFile(string fileName)
        {
            try
            {
                return await FileManager.ReadFromFile(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return "";
        }
    }
}
