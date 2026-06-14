namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 树形结构数据补全工具类（按父子关系递归补全 ParentCodes / TreeLevel / TreeNames / TreeSort）
/// </summary>
public static class TreeFixUtil
{
    /// <summary>
    /// 对一组扁平树节点（通过 Id + ParentId 关联）递归计算并补全树结构辅助字段
    /// </summary>
    /// <param name="nodes">原始节点列表（仅需 Id/ParentId/Name 字段）</param>
    /// <returns>补全后的原列表引用</returns>
    public static List<TreeFixNode> FixTreeData(List<TreeFixNode> nodes)
    {
        if (nodes == null || nodes.Count == 0) return [];

        // 构建 Id -> 节点 映射，便于按父节点编码快速查找
        var nodeMap = nodes.ToDictionary(n => n.Id);
        // 根节点：未指定父编码或父编码不在当前集合中
        var roots = nodes.Where(n => string.IsNullOrEmpty(n.ParentId) || !nodeMap.ContainsKey(n.ParentId)).ToList();
        int sort = 0;

        // 深度优先遍历：自上而下为每个节点填充层级、父路径、名称路径、先序排序号
        void FixNode(TreeFixNode node, string parentCodes, int level)
        {
            node.ParentCodes = parentCodes;
            node.TreeSort = (++sort) * 10;
            node.TreeLevel = level;
            node.TreeNames = BuildTreeNames(node, nodeMap);

            // 处理子节点（按原 TreeSort 升序）
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

    /// <summary>
    /// 沿父链向上回溯至根，拼接节点名称路径（根 → 当前节点，逗号分隔）
    /// </summary>
    /// <param name="node">起始节点</param>
    /// <param name="nodeMap">Id 映射</param>
    /// <returns>名称路径字符串，如 "总部,研发部,平台组"</returns>
    private static string BuildTreeNames(TreeFixNode node, Dictionary<string, TreeFixNode> nodeMap)
    {
        var names = new List<string>();
        var current = node;
        while (current != null)
        {
            // 头部插入以保证最终顺序为 "根 → 当前节点"
            names.Insert(0, current.Name);
            current = !string.IsNullOrEmpty(current.ParentId) && nodeMap.TryGetValue(current.ParentId, out var p) ? p : null;
        }
        return string.Join(", ", names);
    }
}

/// <summary>
/// 通用树节点模型（Id / ParentId / Name 以及补全得到的结构字段）
/// </summary>
public class TreeFixNode
{
    /// <summary>
    /// 节点唯一编码（主键）
    /// </summary>
    public string Id { get; set; } = "";

    /// <summary>
    /// 父节点编码；根节点为空字符串
    /// </summary>
    public string ParentId { get; set; } = "";

    /// <summary>
    /// 节点显示名称
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// 所有祖先节点编码（逗号分隔，以根开始）
    /// </summary>
    public string ParentCodes { get; set; } = "";

    /// <summary>
    /// 先序遍历的排序号（递增值 × 10，便于后续插入）
    /// </summary>
    public decimal TreeSort { get; set; }

    /// <summary>
    /// 节点所在层级（根为 0）
    /// </summary>
    public int TreeLevel { get; set; }

    /// <summary>
    /// 从根到当前节点的名称路径（逗号分隔，如 "总部,研发部,平台组"）
    /// </summary>
    public string TreeNames { get; set; } = "";
}
