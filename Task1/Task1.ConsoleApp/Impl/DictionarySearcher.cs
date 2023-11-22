using System.Text;
using Task1.ConsoleApp.Data;

namespace Task1.ConsoleApp.Impl
{
    internal class DictionarySearcher : ITypedSearcher
    {
        private const int DefaultMaxWordsNumber = 10;
        private readonly DataProvider _data;
        private readonly int _maxWordsNumber;
        private readonly Dictionary<string, List<Word>> dictionaryStructure;

        public DictionarySearcher(DataProvider data, int maxWordsNumber = DefaultMaxWordsNumber)
        {
            _data = data;
            _maxWordsNumber = maxWordsNumber;
            dictionaryStructure = CreateDictionaryStructure();
        }

        public void Search(string typedText)
        {
            var text = new StringBuilder();
            text.AppendLine($"Typed: {typedText}");
            if (dictionaryStructure.TryGetValue(typedText, out List<Word>? value))
            {
                foreach (var item in value)
                {
                    text.AppendLine($"{item.Value} {item.Count}");
                }
            }

            Console.WriteLine(text.ToString());
        }

        private Dictionary<string, List<Word>> CreateDictionaryStructure()
        {
            var sortedWords = _data.Words.OrderByDescending(x => x.Count).ThenBy(x => x.Value).ToList();
            var dictionary = new Dictionary<string, List<Word>>();
            foreach (var item in _data.UserTypedTextList)
            {
                if (dictionary.ContainsKey(item))
                    continue;
                var wordsCount = 0;
                var wordsList = new List<Word>();

                foreach (var word in sortedWords)
                {
                    if (word.Value.StartsWith(item))
                    {
                        wordsList.Add(word);
                        wordsCount++;
                        if (wordsCount >= _maxWordsNumber)
                            break;
                    }
                }

                if (wordsList.Count > 0)
                    dictionary.Add(item, wordsList);
            }
            return dictionary;
        }
    }
}
