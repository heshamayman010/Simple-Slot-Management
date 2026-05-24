using Volo.Abp.Modularity;

namespace Vosita;

[DependsOn(
    typeof(VositaDomainModule),
    typeof(VositaTestBaseModule)
)]
public class VositaDomainTestModule : AbpModule
{

}
