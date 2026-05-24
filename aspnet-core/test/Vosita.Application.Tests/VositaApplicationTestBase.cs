using Volo.Abp.Modularity;

namespace Vosita;

public abstract class VositaApplicationTestBase<TStartupModule> : VositaTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}