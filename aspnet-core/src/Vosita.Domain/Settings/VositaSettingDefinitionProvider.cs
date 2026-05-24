using Volo.Abp.Settings;

namespace Vosita.Settings;

public class VositaSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(VositaSettings.MySetting1));
    }
}
