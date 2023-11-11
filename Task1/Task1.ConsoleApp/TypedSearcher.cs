using System.Collections.Immutable;
using Task1.ConsoleApp.Data;

namespace Task1.ConsoleApp
{
    internal class TypedSearcher
    {
        private readonly List<Word> words;
        private object lockObj = new();

        public TypedSearcher(DataProvider data, int maxWordsNumber)
        {
            words = data.Words;
            MaxWordsNumber = maxWordsNumber;
        }

        public int MaxWordsNumber { get; set; }

        public void SearchMatchedWords(string typedText)
        {
            var typeMatchedWords = new List<string>();
            var wordsCount = 0;
            var sortedWords = words.OrderBy(x => x.Count).ThenByDescending(x => x.Value).ToList();
            foreach (var word in sortedWords)
            {
                if (word.Value.StartsWith(typedText))
                {
                    typeMatchedWords.Add(word.Value);
                    wordsCount++;
                }
                if (wordsCount >= MaxWordsNumber)
                    break;
            }

            lock (lockObj)
            {
                Console.WriteLine();
                Console.WriteLine($"Typed: {typedText}");
                typeMatchedWords.ForEach(Console.WriteLine);
            }

        }

        //медленнее
        public void SearchWithBinaryMatchedWords(string typedText)
        {
            var sortedWords = words.ToImmutableSortedDictionary(x => x.Value, x => x.Count);
            var keys = sortedWords.Keys.ToArray();
            var startIndex = Array.BinarySearch(keys, typedText);
            if (startIndex < 0)
            {
                if (~startIndex > keys.Length)
                    return;
                startIndex = ~startIndex;
            }

            var matched = new List<(string, int)>();
            for ( var i = startIndex; i < keys.Length && keys[i].StartsWith(typedText); i++)
            {
                matched.Add(new (keys[i], sortedWords[keys[i]]));
            }

            matched = matched.OrderByDescending(x => x.Item2).Take(MaxWordsNumber).ToList();

            lock (lockObj)
            {
                Console.WriteLine();
                Console.WriteLine($"Typed: {typedText}");
                matched.ForEach(x => Console.WriteLine(x.Item1));
            }

        }
    }
}
