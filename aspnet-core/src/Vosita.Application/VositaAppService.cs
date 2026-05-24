using System;
using System.Collections.Generic;
using System.Text;
using Vosita.Localization;
using Volo.Abp.Application.Services;

namespace Vosita;

/* Inherit your application services from this class.
 */
public abstract class VositaAppService : ApplicationService
{
    protected VositaAppService()
    {
        LocalizationResource = typeof(VositaResource);
    }
}
