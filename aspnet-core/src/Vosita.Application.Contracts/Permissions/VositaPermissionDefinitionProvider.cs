using Vosita.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Vosita.Permissions;

public class VositaPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(VositaPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(VositaPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<VositaResource>(name);
    }
}
