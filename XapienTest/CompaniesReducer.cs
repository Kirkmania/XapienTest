using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace XapienTest
{
    public class CompaniesReducer
    {
        private readonly List<string> suffixes;

        public CompaniesReducer(string businessSuffixes)
        {
            suffixes = JsonSerializer.Deserialize<List<string>>(businessSuffixes);
        }

        /// <summary>
        /// Parses json input and returns a list of company names with detected duplicates removed.
        /// </summary>
        /// <param name="jsonInput">{string} JSON object of a list of companies.</param>
        /// <returns>{list} Cleaned list of companies.</returns>
        public List<string> ParseAndClean(string jsonInput)
        {
            HashSet<string> companies = JsonSerializer.Deserialize<HashSet<string>>(jsonInput);
            HashSet<string> reducedCompanies = new();
            foreach (string company in companies)
            {
                string noPuncLower = RemoveCapsPunctuation(company);
                Stack<string> words = new(noPuncLower.Split(" "));
                List<string> noSuffixes = CleanSuffixes(words).ToList();
                noSuffixes.Reverse();
                noSuffixes.Remove("the");
                reducedCompanies.Add(string.Join(" ", noSuffixes.ToArray()));
            }
            return reducedCompanies.ToList();
        }

        /// <summary>
        /// Remove any punctuation that isn't '&' or '-' from a company string.
        /// </summary>
        /// <param name="company">{string} company name.</param>
        /// <returns>{string} Company name with puncuation removed.</returns>
        public string RemoveCapsPunctuation(string company)
        {
            company = new(company.Where(c => !char.IsPunctuation(c) || c == '&' || c == '-').ToArray());
            return company.ToLower();
        }

        /// <summary>
        /// Recursively removes business suffixes from a company listing. Suffixes to remove can be altered from business_suffixes.json file.
        /// Note: '&' is included in suffixes list to handle cases such as "john smith & co".
        /// </summary>
        /// <param name="words">{Stack} Tokenised company name.</param>
        /// <returns>{Stack} Company listing with appropriate suffixes removed.</returns>
        public Stack<string> CleanSuffixes(Stack<string> words)
        {
            if (words.Count == 0 || !suffixes.Contains(words.Peek()))
            {
                return words;
            }
            else
            {
                words.Pop();
                return CleanSuffixes(words);
            }
        }

        /// <summary>
        /// UNUSED function to generate common (stop) words from suffixes of a list of company names.
        /// Would be more useful with a much larger list but does not produce very useful results here.
        /// </summary>
        /// <param name="companies">List{string} of company names.</param>
        /// <returns>List of stop words that meet the threshold proportion.</returns>
        public List<string> GenerateStopWords(List<string> companies)
        {
            // Tokenise companies list
            List<string> tokens = new();
            foreach (string s in companies)
            {
                string[] words = s.Split(' ');
                if (words.Length > 1)
                {
                    tokens.Add(words.Last());
                }
            }
            // Use LINQ to build frequency dictionary
            var frequencies = tokens.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            // Add words that pass a threshold proportion to list of stop words
            List<string> stopWords = new();
            double threshold = 0.002;
            foreach (var s in frequencies)
            {
                if ((float)s.Value / companies.Count > threshold)
                {
                    stopWords.Add(s.Key);
                    Console.WriteLine($"ID: {s.Key}, Freq: {s.Value}");
                }
            }
            return stopWords;
        }
    }
}
