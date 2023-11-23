using System.Text;
using Task1.ConsoleApp.Data;

namespace Task1.ConsoleApp.Impl
{
    internal class TrieSearcher : ITypedSearcher
    {
        private const int DefaultMaxWordsNumber = 10;
        private readonly DataProvider _data;
        private readonly int _maxWordsNumber;
        private readonly Trie trieStructure;

        public TrieSearcher(DataProvider data, int maxWordsNumber = DefaultMaxWordsNumber)
        {
            _data = data;
            _maxWordsNumber = maxWordsNumber;
            trieStructure = CreateTrieStructure();
        }

        private Trie CreateTrieStructure()
        {
            return Trie.Build(_data.Words.ToArray());
        }

        public void Search(string typedText)
        {
            var text = new StringBuilder();
            text.AppendLine($"Typed: {typedText}");

            var queue = new PriorityQueue<Word, int>(_maxWordsNumber);
            var startNode = trieStructure.Search(typedText);

            if (startNode != null)
            {
                CheckNode(startNode);
            }

            void CheckNode(Trie node)
            {
                if (node.IsWord)
                {
                    if (queue.Count < _maxWordsNumber)
                    {
                        queue.Enqueue(new Word(node.Value, node.Prefix), node.Value);
                    }
                    else
                    {
                        queue.EnqueueDequeue(new Word(node.Value, node.Prefix), node.Value);
                    }
                }
                foreach (var child in node.Children)
                {
                    CheckNode(child);
                }
            }

            queue.UnorderedItems.ToList().Select(x => x.Element).OrderByDescending(x => x.Count).ThenBy(x => x.Value).ToList().ForEach(x => text.AppendLine($"{x.Value} {x.Count}"));

            Console.WriteLine(text.ToString());
        }
    }
}
