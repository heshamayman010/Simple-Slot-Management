using System.Threading.Tasks;

namespace Vosita.Data;

public interface IVositaDbSchemaMigrator
{
    Task MigrateAsync();
}
