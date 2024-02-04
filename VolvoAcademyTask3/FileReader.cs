using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task WriteResultsToNewFileAsync()
        {
            throw new NotImplementedException();
        }
    }
}
