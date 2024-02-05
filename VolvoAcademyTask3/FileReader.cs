using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace VolvoAcademyTask3
{
    internal class FileReader
    {
        private string FilePath { get; }

        public FileReader(string filePath)
        {
            FilePath = filePath;
        }

        public async Task ProcessFileAsync()
        {
            //read file
            IEnumerable<string> data = await ReadFileAsync();
            //process lines
            LineProcessor lineProcessor = new LineProcessor(data.ToArray());
            //calculate statistics
            StatisticsCounter statisticsCounter = new StatisticsCounter(lineProcessor);
            //write to file
            await WriteResultsToNewFileAsync(@"C:\Users\Krzysztof\Downloads\test_file_write.txt", statisticsCounter);
        }

        public async Task<IEnumerable<string>> ReadFileAsync()
        {
            try
            {
                string[] data = await File.ReadAllLinesAsync(FilePath);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while reading data from file {FilePath}: {e.Message}");
                return new List<string> { };
            }
        }

        public async Task WriteResultsToNewFileAsync(string filePath, StatisticsCounter statCounter)
        {
            string longestSentence = await statCounter.GetLongestSentenceAsync();
            string shortestSentence = await statCounter.GetShortestSentenceAsync();
            string longestWord = await statCounter.GetLongestWordAsync();
            char mostCommonLetter = await statCounter.GetMostCommonLetterAsync();
            string[] sortedWords = await statCounter.GetWordsSortedByNumberOfUsesAsync();
            if (!File.Exists(filePath))
            {
                var writeFile = File.CreateText(filePath);
                writeFile.Close();
                using (StreamWriter streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    await streamWriter.WriteAsync($"Longest sentence by number of characters: {longestSentence}\n");
                    await streamWriter.WriteAsync($"Shortest sentence by numbers of words: {shortestSentence}\n");
                    await streamWriter.WriteAsync($"Longest word: {longestWord}\n");
                    await streamWriter.WriteAsync($"Most common letter: {mostCommonLetter} \n");
                    await streamWriter.WriteAsync("Words sorted by the number of uses in descending order:\n");

                    foreach (string word in sortedWords)
                    {
                        await streamWriter.WriteAsync($"{word}\n");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Can not create this file, because it already exists!");
            }

        }
    }
}
