using Vosita.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Vosita.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class VositaController : AbpControllerBase
{
    protected VositaController()
    {
        LocalizationResource = typeof(VositaResource);
    }
}
