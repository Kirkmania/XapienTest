using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace XapienTest
{
    class Runner
    {
        public static void Main(string[] args)
        {
            string jsonInput = File.ReadAllText("org_names.json");
            string businessSuffixes = File.ReadAllText("business_suffixes.json");
            CompaniesReducer reducer = new(businessSuffixes);

            List<string> companies = JsonSerializer.Deserialize<List<string>>(jsonInput);
            List<string> reducedCompanies = reducer.ParseAndClean(jsonInput);

            Console.WriteLine($"Original list length: {companies.Count}");
            Console.WriteLine($"Reduced list length: {reducedCompanies.Count}");
            //List<string> stopWords = reducer.GenerateStopWords(reducedCompanies);
            string jsonOutput = JsonSerializer.Serialize(reducedCompanies);
            File.WriteAllText("output.json", jsonOutput);
        }
    }
}
