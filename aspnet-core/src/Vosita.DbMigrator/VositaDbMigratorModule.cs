using Vosita.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Vosita.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(VositaEntityFrameworkCoreModule),
    typeof(VositaApplicationContractsModule)
    )]
public class VositaDbMigratorModule : AbpModule
{
}
