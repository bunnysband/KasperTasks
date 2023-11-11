using System.Text.RegularExpressions;

namespace Task1.ConsoleApp.Data
{
    internal class DataProvider
    {
        const string DefaultDataFile = "Data\\test.in";
        private readonly string[] data;
        private readonly int wordsNumber;

        public DataProvider(string? dataFilePath = null)
        {
            dataFilePath ??= Path.Combine(Environment.CurrentDirectory, DefaultDataFile);
            data = File.ReadAllLines(dataFilePath);
            wordsNumber = int.Parse(data[0]);

            Words = ParseWordsFromData();
            UserTypedTextList = ParseUserTypedTextLisFromData();
        }
        public List<Word> Words { get; }

        public List<string> UserTypedTextList { get; }

        private List<Word> ParseWordsFromData()
        {
            var regexp = new Regex("(\\w*) (\\d*)");
            var words = new List<Word>();
            for (int i = 1; i <= wordsNumber; i++)
            {
                var match = regexp.Match(data[i]);
                words.Add(new Word(int.Parse(match.Groups[2].Value), match.Groups[1].Value));
            }
            return words.ToList();
        }

        private List<string> ParseUserTypedTextLisFromData()
        {
            var userTypedTextList = new List<string>();
            for (int i = wordsNumber + 2; i < data.Length; i++)
            {
                userTypedTextList.Add(data[i]);
            }
            return userTypedTextList;
        }
    }
}
