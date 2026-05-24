using Volo.Abp.Modularity;

namespace Vosita;

public abstract class VositaDomainTestBase<TStartupModule> : VositaTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}