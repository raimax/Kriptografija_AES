using System;
using System.IO;
using System.Threading.Tasks;

namespace Uzduotis_2
{
    public static class FileManager
    {
        /// <summary>
        /// Nuskaito tekstą iš failo ir jį grąžina.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>Nuskaitytą tekstą</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static async Task<string> ReadFromFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException($"File name not provided in {nameof(ReadFromFile)}");

            if (!File.Exists(Directory.GetCurrentDirectory() + "\\" + fileName))
            {
                throw new FileNotFoundException($"Cannot read file. File \"{fileName}\" doesn't exist in \"{Directory.GetCurrentDirectory()}\"");
            }

            return await File.ReadAllTextAsync(Directory.GetCurrentDirectory() + "\\" + fileName);
        }

        /// <summary>
        /// Įrašo duomenis į failą
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task WriteToFile(string fileName, string data)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException($"File name not provided in {nameof(WriteToFile)}");
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentNullException($"Data not provided in {nameof(WriteToFile)}");

            await File.WriteAllTextAsync(Directory.GetCurrentDirectory() + "\\" + fileName, data);
        }
    }
}
