using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Vosita.Data;

/* This is used if database provider does't define
 * IVositaDbSchemaMigrator implementation.
 */
public class NullVositaDbSchemaMigrator : IVositaDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
