using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolvoAcademyTask3
{
    internal class StatisticsCounter
    {
        private readonly LineProcessor LineProcessor;
        public StatisticsCounter(LineProcessor lineProcessor)
        {
            LineProcessor = lineProcessor;
        }

        public async Task<string> GetLongestSentenceAsync()
        {
            return LineProcessor.Sentences.OrderByDescending(s => s.Length).First();
        }

        public async Task<string> GetShortestSentenceAsync()
        {
            return LineProcessor.Sentences.OrderBy(sentence => sentence.Split(' ').Length).First(); ;
        }
        public async Task<string> GetLongestWordAsync()
        {
            return LineProcessor.Words.OrderByDescending(w => w.Key.Length).First().Key;
        }
        public async Task<char> GetMostCommonLetterAsync()
        {
            return LineProcessor.Letters.OrderByDescending(l => l.Value).First().Key;
        }

        public async Task<string[]> GetWordsSortedByNumberOfUsesAsync()
        {
            return LineProcessor.Words.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToArray();
        }
    }
}
