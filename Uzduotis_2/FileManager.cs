using System;
using System.IO;
using System.Threading.Tasks;

namespace Uzduotis_2
{
    public static class FileManager
    {
        public static async Task<string> ReadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException($"File name not provided in {nameof(ReadFile)}");

            if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + fileName))
            {
                throw new FileNotFoundException($"Cannot read file. File \"{fileName}\" doesn't exist in \"{Directory.GetCurrentDirectory()}\"");
            }

            return await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + "\\" + fileName);
        }

        public static async Task WriteToFile(string fileName, string data)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException($"File name not provided in {nameof(WriteToFile)}");
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentNullException($"Data not provided in {nameof(WriteToFile)}");

            await File.WriteAllTextAsync(Directory.GetCurrentDirectory() + "\\" + fileName, data);
        }
    }
}
