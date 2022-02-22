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
            System.Console.ForegroundColor = System.ConsoleColor.Green;
            System.Console.WriteLine($"{_cipher.AlgorithmName}\n");
            System.Console.ResetColor();

            DisplayOptions();

            BeginProcess();
        }

        public void DisplayOptions()
        {
            while (SelectedOption == Option.Undefined)
            {
                System.Console.WriteLine("Select option:\n");
                System.Console.WriteLine("1. Encode");
                System.Console.WriteLine("2. Decode");

                string inputOption = System.Console.ReadLine();

                switch (inputOption)
                {
                    case "1":
                        SelectedOption = Option.Encode;
                        break;
                    case "2":
                        SelectedOption = Option.Decode;
                        break;
                    default:
                        System.Console.WriteLine("Option doesn't exist\n");
                        break;
                }
            }
        }

        public void BeginProcess()
        {
            string inputText = InputText();

            switch (SelectedOption)
            {
                case Option.Encode:
                    Result = _cipher.Encode(inputText);
                    break;
                case Option.Decode:
                    Result = _cipher.Decode(inputText);
                    break;
                default:
                    break;
            }

            DisplayResult();
            SelectedOption = Option.Undefined;
            Start();
        }

        private static string InputText()
        {
            string inputText = "";

            while (string.IsNullOrWhiteSpace(inputText))
            {
                System.Console.WriteLine("Enter your text:");
                inputText = System.Console.ReadLine();
            }

            return inputText;
        }

        private void DisplayResult()
        {
            System.Console.Clear();
            System.Console.BackgroundColor = System.ConsoleColor.White;
            System.Console.ForegroundColor = System.ConsoleColor.Black;
            System.Console.WriteLine("Result: " + Result + "\n");
            System.Console.ResetColor();
        }
    }
}
