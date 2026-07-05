//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli.Annotations;

namespace BookGen.Cli.Internals;

[CommandName("")]
internal class BranchListCommand : Command
{
    internal class TreeNode
    {
        public string Name { get; }
        public List<TreeNode> Children { get; }

        public TreeNode(string name)
        {
            Name = name;
            Children = new List<TreeNode>();
        }

        public override string ToString()
            => Name;
    }

    private readonly TreeNode _tree;

    private TreeNode BuildTree(List<string> branchItems)
    {
        TreeNode rootNode = new TreeNode("");
        foreach (var item in branchItems)
        {
            var parts = item.Split(' ');
            TreeNode currentNode = rootNode;

            foreach (var part in parts)
            {
                TreeNode? existingChild = currentNode?.Children.FirstOrDefault(c => c.Name == part);
                if (existingChild == null)
                {
                    var newNode = new TreeNode(part);
                    currentNode?.Children.Add(newNode);
                    currentNode = newNode;
                }
                else
                {
                    currentNode = existingChild;
                }
            }
        }

        return rootNode;
    }

    public BranchListCommand(BranchItemsProvider branchItemsProvider)
    {
        _tree = BuildTree(branchItemsProvider.BranchItems);
    }

    public override int Execute(IReadOnlyList<string> context)
    {
        Console.WriteLine("Available subcommands: ");
        Console.WriteLine();
        foreach (TreeNode item in _tree.Children)
        {
            PrintNode(item, 0);
        }
        return 0;
    }

    private static void PrintNode(TreeNode node, int level)
    {
        string spacing = new string(' ', level * 2);
        Console.WriteLine($"{spacing}{node.Name}");
        foreach (TreeNode child in node.Children)
        {
            string childSpacing = new string(' ', level+1 * 2);
            Console.WriteLine($"{childSpacing}├ {child.Name}");
            if (child.Children.Count > 0)
            {
                PrintNode(child, level + 1);
            }
        }
    }
}
