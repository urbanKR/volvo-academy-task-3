using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace VolvoAcademyTask3
{
    internal class LineProcessor
    {
        public string[] Sentences { get; set; }
        public Dictionary<string, int> Words { get; set; }
        public Dictionary<char, int> Letters { get; set; }
        public string[] Punctuation { get; set; }

        public LineProcessor(string[] lines)
        {
            string data = string.Join(" ", lines);
            Sentences = Regex.Split(data, @"(?<=[.!?])\s+").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            string[] words = Regex.Split(data, @"\b\w+\b").Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();
            Words = words.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
            Letters = CountLetters(words);
            Punctuation = Regex.Matches(data, @"\P{L}").Select(m => m.Value).ToArray();
        }
        private Dictionary<char, int> CountLetters(string[] words)
        {
            Dictionary<char, int> letters = new();
            foreach (string s in words)
            {
                foreach (char c in s)
                {
                    if (letters.ContainsKey(c))
                    {
                        letters[c] = letters[c] + 1;
                    }
                    else
                    {
                        letters[c] = 1;
                    }
                }
            }
            return letters;
        }
    }
}
