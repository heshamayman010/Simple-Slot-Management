// using Microsoft.Data.Sqlite;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Infrastructure;
// using Microsoft.EntityFrameworkCore.Storage;
// using Microsoft.Extensions.DependencyInjection;
// using Volo.Abp;
// using Volo.Abp.EntityFrameworkCore;
// using Volo.Abp.EntityFrameworkCore.Sqlite;
// using Volo.Abp.FeatureManagement;
// using Volo.Abp.Modularity;
// using Volo.Abp.PermissionManagement;
// using Volo.Abp.SettingManagement;
// using Volo.Abp.Uow;

// namespace Vosita.EntityFrameworkCore;

// [DependsOn(
//     typeof(VositaEntityFrameworkCoreModule),
//     typeof(AbpEntityFrameworkCoreSqliteModule)
//     )]
// public class VositaEntityFrameworkCoreTestModule : AbpModule
// {
//     private SqliteConnection? _sqliteConnection;

//     public override void ConfigureServices(ServiceConfigurationContext context)
//     {
//         Configure<FeatureManagementOptions>(options =>
//         {
//             options.SaveStaticFeaturesToDatabase = false;
//             options.IsDynamicFeatureStoreEnabled = false;
//         });
//         Configure<PermissionManagementOptions>(options =>
//         {
//             options.SaveStaticPermissionsToDatabase = false;
//             options.IsDynamicPermissionStoreEnabled = false;
//         });
//         Configure<SettingManagementOptions>(options =>
//         {
//             options.SaveStaticSettingsToDatabase = false;
//             options.IsDynamicSettingStoreEnabled = false;
//         });
//         context.Services.AddAlwaysDisableUnitOfWorkTransaction();

//         ConfigureInMemorySqlite(context.Services);
//     }

//     private void ConfigureInMemorySqlite(IServiceCollection services)
//     {
//         _sqliteConnection = CreateDatabaseAndGetConnection();

//         services.Configure<AbpDbContextOptions>(options =>
//         {
//             options.Configure(context =>
//             {
//                 context.DbContextOptions.UseSqlite(_sqliteConnection);
//             });
//         });
//     }

//     public override void OnApplicationShutdown(ApplicationShutdownContext context)
//     {
//         _sqliteConnection?.Dispose();
//     }

//     private static SqliteConnection CreateDatabaseAndGetConnection()
//     {
//         var connection = new AbpUnitTestSqliteConnection("Data Source=:memory:");
//         connection.Open();

//         var options = new DbContextOptionsBuilder<VositaDbContext>()
//             .UseSqlite(connection)
//             .Options;

//         using (var context = new VositaDbContext(options))
//         {
//             context.GetService<IRelationalDatabaseCreator>().CreateTables();
//         }

//         return connection;
//     }
// }

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Sqlite;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.Uow;
using Vosita.EntityFrameworkCore;

namespace Vosita.EntityFrameworkCore;

[DependsOn(
    typeof(VositaEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqliteModule)
    )]
public class VositaEntityFrameworkCoreTestModule : AbpModule
{
    private SqliteConnection? _sqliteConnection;

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<FeatureManagementOptions>(options =>
        {
            options.SaveStaticFeaturesToDatabase = false;
            options.IsDynamicFeatureStoreEnabled = false;
        });
        Configure<PermissionManagementOptions>(options =>
        {
            options.SaveStaticPermissionsToDatabase = false;
            options.IsDynamicPermissionStoreEnabled = false;
        });
        Configure<SettingManagementOptions>(options =>
        {
            options.SaveStaticSettingsToDatabase = false;
            options.IsDynamicSettingStoreEnabled = false;
        });
        
        context.Services.AddAlwaysDisableUnitOfWorkTransaction();

        ConfigureInMemorySqlite(context.Services);
    }

    private void ConfigureInMemorySqlite(IServiceCollection services)
    {
        // Keep a single persistent connection open so the in-memory DB doesn't wipe between scopes
        _sqliteConnection = new SqliteConnection("Data Source=:memory:");
        _sqliteConnection.Open();

        services.Configure<AbpDbContextOptions>(options =>
        {
            options.Configure(context =>
            {
                context.DbContextOptions.UseSqlite(_sqliteConnection);
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        // Trigger schema creation after all modules have completely mapped their models
        using (var scope = context.ServiceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<VositaDbContext>();
            
            // This forces EF to map every single DbSet property (including Slots) into the live connection
            dbContext.Database.EnsureCreated();
        }
    }

    public override void OnApplicationShutdown(ApplicationShutdownContext context)
    {
        _sqliteConnection?.Dispose();
    }
}