using System;
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

        public int CalculateLongestSentence() 
        {
            return LineProcessor.Sentences.Max(s => s.Length);
        }

        public int CalculateShortestSentence()
        {
            return LineProcessor.Sentences.Min(s => s.Split(' ').Length);
        }
        public string GetLongestWord()
        {
            return LineProcessor.Words.OrderByDescending(w => w.Key.Length).First().Key;
        }
        public char GetMostCommonLetter()
        {
            return LineProcessor.Letters.OrderByDescending(l => l.Value).First().Key;
        }
    }
}
