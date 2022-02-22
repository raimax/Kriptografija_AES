using System;
using System.IO;
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

        /// <summary>
        /// Galimos šifravimo užduotys
        /// </summary>
        public enum Option
        {
            Undefined = 0,
            Encode = 1,
            Decode = 2
        }

        /// <summary>
        /// Programos pradžia.
        /// </summary>
        public void Start()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{_cipher.AlgorithmName}\n");
            Console.ResetColor();

            DisplayOptions();

            try
            {
                BeginProcess();
            }
            catch (CryptographicException)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Selected mode doesn't match cipher text");
                Console.ResetColor();
                Start();
            }
            catch (FormatException)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Entered text is not in Base64 format");
                Console.ResetColor();
                Start();
            }
        }

        /// <summary>
        /// Šifravimo uždoties pasirinkimo meniu.
        /// </summary>
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

        /// <summary>
        /// Pasirinktos užduoties pradžia. Rezultato rodymas.
        /// </summary>
        public void BeginProcess()
        {
            string textToProcess;
            
            if (SelectedOption == Option.Decode)
            {
                string selectedOption = InputText("Would you like to decode from file? y/n");

                if (selectedOption == "y")
                {
                    string fileName = InputText("Enter file name: ");
                    textToProcess = ReadFromFile(fileName).Result;

                    // jei pasirinktas failas nerastas, procesas prasideda iš naujo
                    if (string.IsNullOrWhiteSpace(textToProcess))
                    {
                        BeginProcess();
                    }
                }
                else
                {
                    textToProcess = InputText("Enter your text (Base64): ");
                }
            }
            else
            {
                textToProcess = InputText("Enter your text: ");
            }
            
            byte[] key = InputKey("Enter your key (16): ");
            CipherMode mode = SelectMode();

            switch (SelectedOption)
            {
                case Option.Encode:
                    Result = _cipher.Encode(textToProcess, key, mode);
                    break;
                case Option.Decode:
                    Result = _cipher.Decode(Convert.FromBase64String(textToProcess), key, mode);
                    break;
                default:
                    break;
            }

            DisplayResult();
            SelectedOption = Option.Undefined;
            Start();
        }

        /// <summary>
        /// Šifravimo režimo pasirinkimas
        /// </summary>
        /// <returns>Pasirinktą CipherMode</returns>
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

                // tekstas nuskaitomas tol kol neįvestas skaičius nuo 1 iki 5
                if (!int.TryParse(text, out mode) || !Enum.IsDefined(typeof(CipherMode), mode))
                {
                    text = "";
                }
            }

            return (CipherMode)mode;
        }

        /// <summary>
        /// Teksto įvestis
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Iš klaviatūros įvestą tekstą.</returns>
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

        /// <summary>
        /// Slapto rakto įvestis
        /// </summary>
        /// <param name="title"></param>
        /// <returns>Slaptą raktą baitų masyvo pavidalu</returns>
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

        /// <summary>
        /// Parodo rezultatą gautą po užšifravimo ar iššifravimo.
        /// Leidžia pasirinkti resultato saugojimą į failą po užšifravimo.
        /// </summary>
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

        /// <summary>
        /// Įrašo duomenis į failą.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
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

        /// <summary>
        /// Nuskaito failą asinchroniškai ir grąžina rezultatą string formatu.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Nuskaitytą tekstą iš failo</returns>
        private static async Task<string> ReadFromFile(string fileName)
        {
            try
            {
                return await FileManager.ReadFromFile(fileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }
    }
}