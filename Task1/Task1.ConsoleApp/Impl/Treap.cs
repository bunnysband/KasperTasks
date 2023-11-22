namespace Task1.ConsoleApp.Impl
{
    public class Treap
    {
        private Treap(string key, int priority, Treap? left = null, Treap? right = null, Treap? parent = null)
        {
            Key = key;
            Priority = priority;
            Left = left;
            Right = right;
            Parent = parent;
        }

        public string Key { get; private set; }
        public int Priority { get; private set; }
        public Treap? Left { get; private set; }
        public Treap? Right { get; private set; }
        public Treap? Parent { get; private set; }

        public void Split(string key, out Treap? leftTree, out Treap? rightTree)
        {
            Treap? newTree = null;
            if (Key.CompareTo(key) <= 0)
            {
                if (Right == null)
                    rightTree = null;
                else
                    Right.Split(key, out newTree, out rightTree);
                leftTree = new Treap(Key, Priority, Left, newTree);
            }
            else
            {
                if (Left == null)
                    leftTree = null;
                else
                    Left.Split(key, out leftTree, out newTree);
                rightTree = new Treap(Key, Priority, newTree, Right);
            }
        }

        public Treap? Search(string key)
        {
            if (Key.StartsWith(key))
                return this;
            else if (Right is null && Left is null)
                return null;
            else if (Key.CompareTo(key) < 0)
                return Right is null ? this : Right.Search(key);
            else
                return Left is null ? this : Left.Search(key);
        }

        public Treap? Add(string key, int priority)
        {
            Split(key, out Treap? left, out Treap? right);
            Treap added = new Treap(key, priority);
            return Merge(Merge(left, added), right);
        }

        public static Treap? Merge(Treap? leftTree, Treap? rightTree)
        {
            if (leftTree == null) return rightTree;
            if (rightTree == null) return leftTree;

            if (leftTree.Priority > rightTree.Priority)
            {
                var newRight = Merge(leftTree.Right, rightTree);
                return new Treap(leftTree.Key, leftTree.Priority, leftTree.Left, newRight);
            }
            else
            {
                var newLeft = Merge(leftTree, rightTree.Left);
                return new Treap(rightTree.Key, rightTree.Priority, newLeft, rightTree.Right);
            }
        }

        public static Treap Build(string[] keys, int[] priorities)
        {
            var tree = new Treap(keys[0], priorities[0]);
            var last = tree;

            for (int i = 1; i < keys.Length; ++i)
            {
                if (last.Priority > priorities[i])
                {
                    last.Right = new Treap(keys[i], priorities[i], parent: last);
                    last = last.Right;
                }
                else
                {
                    Treap? current = last;
                    while (current.Parent != null && current.Priority <= priorities[i])
                        current = current.Parent;
                    if (current.Priority <= priorities[i])
                    {
                        last = new Treap(keys[i], priorities[i], current);
                        current.Parent = last;
                    }
                    else
                    {
                        last = new Treap(keys[i], priorities[i], current.Right, null, current);
                        current.Right = last;
                    }
                }
            }

            while (last.Parent != null)
                last = last.Parent;
            return last;
        }
    }
}
