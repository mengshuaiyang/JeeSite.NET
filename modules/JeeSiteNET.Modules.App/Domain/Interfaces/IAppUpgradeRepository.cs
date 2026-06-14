    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.App.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Entities
using JeeSiteNET.Modules.App.Domain.Entities;

// 定义 JeeSiteNET.Modules.App.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Domain.Interfaces
namespace JeeSiteNET.Modules.App.Domain.Interfaces;

// 定义接口 IAppUpgradeRepository
// 定义接口：IAppUpgradeRepository
public interface IAppUpgradeRepository : IRepository<AppUpgrade>
{
}
