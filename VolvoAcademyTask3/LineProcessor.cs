using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;

namespace VolvoAcademyTask3
{
    internal class LineProcessor
    {
        public string[] Sentences { get; }
        public Dictionary<string, int> Words { get; }
        public Dictionary<char, int> Letters { get; }
        public string[] Punctuation { get; }
        public string Title { get; }

        public LineProcessor(string[] lines)
        {
            string data = string.Join(" ", lines);
            Sentences = Regex.Split(data, @"(?<=[.!?])\s+").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            string[] words = GetWords(data);
            Words = words.GroupBy(w => w).ToDictionary(g => g.Key, g => g.Count());
            Letters = GetLetters(words);
            Punctuation = Regex.Matches(data, @"\P{L}").Select(m => m.Value).Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();
            string lineWithTitle = lines.FirstOrDefault(line => line.StartsWith("Title:"));
            Match m = Regex.Match(lineWithTitle, @"Title:\s*(.*)");
            if (m.Success)
            {
                Title = m.Groups[1].Value.Trim();
            }
            else
            {
                Title = "Title not given";
            }
        }

        private string[] GetWords(string txt)
        {
            var match = Regex.Matches(txt, @"\b\w+\b");
            string[] words = new string[match.Count];
            for (int i = 0; i < match.Count; i++)
            {
                words[i] = match[i].Value.ToLower();
            }
            return words;
        }
        private Dictionary<char, int> GetLetters(string[] words)
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
