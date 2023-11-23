namespace Task1.ConsoleApp.Impl
{
    internal class Trie
    {
        public Trie(string prefix)
        {
            Prefix = prefix;
            Children = new List<Trie>();
        }

        public bool IsWord { get; private set; }
        public string Prefix { get; }
        public int Value { get; private set; }

        public List<Trie> Children { get; private set; }

        public Trie? Search(string text)
        {
            Trie current = this;
            for (int i = 0; i < text.Length; i++)
            {
                var child = current.Children.SingleOrDefault(c => c.Prefix.Equals(text[..(i + 1)]));
                if (child == null) 
                    return null;
                current = child;
            }
            return current;
        }

        public static Trie Build(params Word[] words)
        {
            var root = new Trie("");
            foreach (var word in words)
            {
                string value = word.Value;
                Trie current = root;
                for (int i = 0; i < value.Length; i++)
                {
                    var child = current.Children.SingleOrDefault(c => c.Prefix.Equals(value[..(i + 1)]));
                    if (child == null)
                    {
                        child = new Trie(value[..(i + 1)]);
                        current.Children.Add(child);
                    }

                    current = child;
                }
                current.IsWord = true;
                current.Value = word.Count;
            }
            return root;
        }
    }
}
