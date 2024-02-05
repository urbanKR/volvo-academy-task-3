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
            //read file
            IEnumerable<string> data = await ReadFileAsync(FolderPath + "/" + fileName + ".txt");
            //process lines
            LineProcessor lineProcessor = new LineProcessor(data.ToArray());
            //calculate statistics
            StatisticsCounter statisticsCounter = new StatisticsCounter(lineProcessor);
            //write to file
            string newFileName = fileName + "_statistics";
            string newFilePath = ResultsPath + newFileName + ".txt";
            await WriteResultsToNewFileAsync(newFilePath, statisticsCounter);
        }

        public async Task ProcessFilesAsync()
        {
            string[] fileNames = Directory.GetFiles(FolderPath, "*.txt").Select(p => Path.GetFileNameWithoutExtension(p)).ToArray();
            var tasks = fileNames.Select(fn => ProcessFileAsync(fn)).ToArray();
            await Task.WhenAll(tasks);

        }

        public async Task<IEnumerable<string>> ReadFileAsync(string filePath)
        {
            try
            {
                string[] data = await File.ReadAllLinesAsync(filePath);
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
            string longestSentence = await statCounter.GetLongestSentenceAsync();
            string shortestSentence = await statCounter.GetShortestSentenceAsync();
            string longestWord = await statCounter.GetLongestWordAsync();
            char mostCommonLetter = await statCounter.GetMostCommonLetterAsync();
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
