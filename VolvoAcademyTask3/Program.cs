﻿namespace VolvoAcademyTask3
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            FileReader fileReader = new FileReader(@"C:\Users\Krzysztof\Downloads\test_file.txt");
            await fileReader.ProcessFileAsync();
        }
    }
}
