    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Entities
using JeeSiteNET.Modules.Test.Domain.Entities;

// 定义 JeeSiteNET.Modules.Test.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Domain.Interfaces
namespace JeeSiteNET.Modules.Test.Domain.Interfaces;

// 定义接口 ITestTreeRepository
// 定义接口：ITestTreeRepository
public interface ITestTreeRepository : IRepository<TestTree>
{
}
