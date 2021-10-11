using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace XapienTest.UnitTests
{
    [TestClass]
    public class CompaniesReducerTest
    {
        readonly string businessSuffixes = File.ReadAllText("business_suffixes.json");

        [TestMethod]
        public void ParseAndClean_AllUnique_ReturnsAllCompanies()
        {
            string companiesJson = @"[""One"", ""Two"", ""Three""]";
            List<string> expected = new() { "one", "two", "three" };
            var reducer = new CompaniesReducer(businessSuffixes);

            List<string> actual = reducer.ParseAndClean(companiesJson);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ParseAndClean_ExactDuplicates_ReturnsUniqueCompanies()
        {
            string companiesJson = @"[""One"", ""One"", ""Two"", ""Three""]";
            List<string> expected = new() { "one", "two", "three" };
            var reducer = new CompaniesReducer(businessSuffixes);

            List<string> actual = reducer.ParseAndClean(companiesJson);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ParseAndClean_CaseInsensitiveDuplicates_ReturnsUniqueCompanies()
        {
            string companiesJson = @"[""One"", ""one"", ""Two"", ""Three""]";
            List<string> expected = new() { "one", "two", "three" };
            var reducer = new CompaniesReducer(businessSuffixes);

            List<string> actual = reducer.ParseAndClean(companiesJson);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ParseAndClean_PunctuatedDuplicates_ReturnsCleanUniqueCompanies()
        {
            string companiesJson = @"[""One"", ""One."", ""Two"", ""Three"", ""Four Five"", ""Four, Five""]";
            List<string> expected = new() { "one", "two", "three", "four five" };
            var reducer = new CompaniesReducer(businessSuffixes);

            List<string> actual = reducer.ParseAndClean(companiesJson);

            foreach (string company in expected)
            {
                Console.WriteLine(company);
            }
            foreach (string company in actual)
            {
                Console.WriteLine(company);
            }
            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ParseAndClean_LeadingWordThe_ReturnsTrimmedCompanies()
        {
            string companiesJson = @"[""The One"", ""the Two"", ""Three""]";
            List<string> expected = new() { "one", "two", "three" };
            var reducer = new CompaniesReducer(businessSuffixes);

            List<string> actual = reducer.ParseAndClean(companiesJson);

            CollectionAssert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ParseAndClean_nSuffixes_ReturnsTrimmedUniqueCompanies()
        {
            string companiesJson = @"[""One Ltd"", ""One company ltd"", ""Two"", ""Three""]";
            List<string> expected = new() { "one", "two", "three" };
            var reducer = new CompaniesReducer(businessSuffixes);

            List<string> actual = reducer.ParseAndClean(companiesJson);

            CollectionAssert.AreEqual(actual, expected);
        }
    }
}
