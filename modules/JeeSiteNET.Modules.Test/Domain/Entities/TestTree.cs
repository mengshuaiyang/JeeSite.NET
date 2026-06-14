    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;

// 定义 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Domain.Entities
namespace JeeSiteNET.Modules.Test.Domain.Entities;

// 定义class TestTree
// 定义类：TestTree
public class TestTree : TreeEntity
{
    // 属性 TreeCode
    // 属性：TreeCode
    public string TreeCode { get; set; } = string.Empty;
    // 属性 TreeName
    // 属性：TreeName
    public string TreeName { get; set; } = string.Empty;

    // 方法：GetName
    public override string GetName() => TreeName;
}
