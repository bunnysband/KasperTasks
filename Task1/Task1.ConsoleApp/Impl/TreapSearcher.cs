using System.Text;
using Task1.ConsoleApp.Data;

namespace Task1.ConsoleApp.Impl
{
    internal class TreapSearcher : ITypedSearcher
    {
        private const int DefaultMaxWordsNumber = 10;
        private readonly DataProvider _data;
        private readonly int _maxWordsNumber;
        private readonly Treap treapStructure;

        public TreapSearcher(DataProvider data, int maxWordsNumber = DefaultMaxWordsNumber)
        {
            _data = data;
            _maxWordsNumber = maxWordsNumber;
            treapStructure = CreateTreapStructure();
        }

        public void Search(string typedText)
        {
            var text = new StringBuilder();
            text.AppendLine($"Typed: {typedText}");

            var queue = new PriorityQueue<Word, int>(_maxWordsNumber);
            var startNode = treapStructure.Search(typedText);

            if (startNode != null)
            {
                CheckNode(startNode);
            }

            void CheckNode(Treap node)
            {
                if (node.Key.StartsWith(typedText) && (queue.Count < _maxWordsNumber || queue.Peek().Count <= node.Priority))
                {
                    if (queue.Count < _maxWordsNumber)
                        queue.Enqueue(new Word(node.Priority, node.Key), node.Priority);
                    else
                    {
                        if (queue.Peek().Count == node.Priority && node.Key.CompareTo(queue.Peek().Value) < 0 || queue.Peek().Count < node.Priority)
                        {
                            queue.EnqueueDequeue(new Word(node.Priority, node.Key), node.Priority);
                        }
                    }
                }

                if (node.Left != null && (queue.Count < _maxWordsNumber || queue.Peek().Count <= node.Left?.Priority))
                {
                    CheckNode(node.Left);
                }

                if (node.Right != null && (queue.Count < _maxWordsNumber || queue.Peek().Count <= node.Right?.Priority))
                {
                    CheckNode(node.Right);
                }
            }

            queue.UnorderedItems.ToList().Select(x => x.Element).OrderByDescending(x => x.Count).ThenBy(x => x.Value).ToList().ForEach(x => text.AppendLine($"{x.Value} {x.Count}"));

            Console.WriteLine(text.ToString());
        }
        private Treap CreateTreapStructure()
        {
            var sortedWords = _data.Words.OrderBy(x => x.Value);
            var keys = sortedWords.Select(w => w.Value).ToArray();
            var priorities = sortedWords.Select(w => w.Count).ToArray();

            return Treap.Build(keys, priorities);
        }
    }
}
