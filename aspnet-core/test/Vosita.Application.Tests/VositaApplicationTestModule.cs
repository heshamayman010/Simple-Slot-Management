using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Autofac;
using Volo.Abp.Castle;

namespace Vosita.Application.Tests;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpCastleCoreModule), 
    typeof(VositaApplicationModule),
    typeof(EntityFrameworkCore.VositaEntityFrameworkCoreTestModule) 
)]public class VositaApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<PermissionManagementOptions>(options =>
        {
            options.IsDynamicPermissionStoreEnabled = false;
            options.SaveStaticPermissionsToDatabase = false;
        });

        Configure<SettingManagementOptions>(options =>
        {
            options.IsDynamicSettingStoreEnabled = false;
            options.SaveStaticSettingsToDatabase = false;
        });

        Configure<FeatureManagementOptions>(options =>
        {
            options.IsDynamicFeatureStoreEnabled = false;
            options.SaveStaticFeaturesToDatabase = false;
        });
    }
}