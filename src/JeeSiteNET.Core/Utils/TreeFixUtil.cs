namespace JeeSiteNET.Core.Utils;

public static class TreeFixUtil
{
    public static List<TreeFixNode> FixTreeData(List<TreeFixNode> nodes)
    {
        if (nodes == null || nodes.Count == 0) return [];

        var nodeMap = nodes.ToDictionary(n => n.Id);
        var roots = nodes.Where(n => string.IsNullOrEmpty(n.ParentId) || !nodeMap.ContainsKey(n.ParentId)).ToList();
        int sort = 0;

        void FixNode(TreeFixNode node, string parentCodes, int level)
        {
            node.ParentCodes = parentCodes;
            node.TreeSort = (++sort) * 10;
            node.TreeLevel = level;
            node.TreeNames = BuildTreeNames(node, nodeMap);

            var children = nodes.Where(n => n.ParentId == node.Id).OrderBy(n => n.TreeSort).ToList();
            foreach (var child in children)
            {
                string childParentCodes = string.IsNullOrEmpty(parentCodes)
                    ? node.Id
                    : $"{parentCodes},{node.Id}";
                FixNode(child, childParentCodes, level + 1);
            }
        }

        foreach (var root in roots)
            FixNode(root, "", 0);

        return nodes;
    }

    private static string BuildTreeNames(TreeFixNode node, Dictionary<string, TreeFixNode> nodeMap)
    {
        var names = new List<string>();
        var current = node;
        while (current != null)
        {
            names.Insert(0, current.Name);
            current = !string.IsNullOrEmpty(current.ParentId) && nodeMap.TryGetValue(current.ParentId, out var p) ? p : null;
        }
        return string.Join(", ", names);
    }
}

public class TreeFixNode
{
    public string Id { get; set; } = "";
    public string ParentId { get; set; } = "";
    public string Name { get; set; } = "";
    public string ParentCodes { get; set; } = "";
    public decimal TreeSort { get; set; }
    public int TreeLevel { get; set; }
    public string TreeNames { get; set; } = "";
}
