using Microsoft.Extensions.Localization;
using Vosita.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Vosita;

[Dependency(ReplaceServices = true)]
public class VositaBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<VositaResource> _localizer;

    public VositaBrandingProvider(IStringLocalizer<VositaResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
