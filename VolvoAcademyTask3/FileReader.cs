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
        private string FolderPath { get; }
        private string ResultsPath => FolderPath + "\\results\\";

        public FileReader(string folderPath)
        {
            FolderPath = folderPath;
        }

        public async Task ProcessFileAsync(string fileName)
        {
            Console.WriteLine($"Starting Processing file: {fileName}");
            //read file
            IEnumerable<string> data = await ReadFileAsync(FolderPath + "/" + fileName + ".txt");
            //process lines
            LineProcessor lineProcessor = new LineProcessor(data.ToArray());
            //calculate statistics
            StatisticsCounter statisticsCounter = new StatisticsCounter(lineProcessor);
            //write to file
            string newFileName = lineProcessor.Title;
            string newFilePath = ResultsPath + newFileName + ".txt";
            await WriteResultsToNewFileAsync(newFilePath, statisticsCounter);
            Console.WriteLine($"Finished Processing file: {fileName}");
        }

        public async Task ProcessFilesAsync()
        {
            string[] fileNames = Directory.GetFiles(FolderPath, "*.txt").Select(p => Path.GetFileNameWithoutExtension(p)).ToArray();
            var tasks = fileNames.Select(fn => ProcessFileAsync(fn)).ToArray();
            await Task.WhenAll(tasks);

        }

        public async Task<IEnumerable<string>> ReadFileAsync(string filePath)
        {
            Console.WriteLine($"Starting reading file: {filePath.Split('/').Last()}");
            try
            {
                string[] data = await File.ReadAllLinesAsync(filePath);
                Console.WriteLine($"Finished reading file: {filePath.Split('/').Last()}");
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while reading data from file {filePath}: {e.Message}");
                return new List<string> { };
            }
        }

        public async Task WriteResultsToNewFileAsync(string filePath, StatisticsCounter statCounter)
        {
            Console.WriteLine($"Starting writing to file: {filePath.Split('\\').Last()}");
            string[] longestSentences = await statCounter.Get10LongestSentencesAsync();
            string[] shortestSentences = await statCounter.Get10ShortestSentencesAsync();
            string[] longestWords = await statCounter.Get10LongestWordsAsync();
            char[] mostCommonLetters = await statCounter.Get10MostCommonLetterAsync();
            string[] sortedWords = await statCounter.GetWordsSortedByNumberOfUsesAsync();
            if (!File.Exists(filePath))
            {
                if (!Directory.Exists(ResultsPath))
                {
                    var newFlder = Directory.CreateDirectory(FolderPath + "/results");
                }
                var writeFile = File.CreateText(filePath);
                writeFile.Close();
                using (StreamWriter streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    await streamWriter.WriteAsync("******Longest sentences by number of characters:******\n");
                    foreach (var s in longestSentences)
                    {
                        await streamWriter.WriteLineAsync(s + "\n");
                    }
                    await streamWriter.WriteAsync("******Shortest sentences by numbers of words:******\n");
                    foreach (var s in shortestSentences)
                    {
                        await streamWriter.WriteLineAsync(s + "\n");
                    }
                    await streamWriter.WriteAsync("******Longest words:******\n");
                    foreach (var s in longestWords)
                    {
                        await streamWriter.WriteLineAsync(s + "\n");
                    }
                    await streamWriter.WriteAsync("******Most common letters:******\n");
                    foreach (var s in mostCommonLetters)
                    {
                        await streamWriter.WriteLineAsync(s + "\n");
                    }
                    await streamWriter.WriteAsync("******Words sorted by the number of uses in descending order:******\n");

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
            Console.WriteLine($"Finished writing to file: {filePath.Split('\\').Last()}");
        }
    }
}
