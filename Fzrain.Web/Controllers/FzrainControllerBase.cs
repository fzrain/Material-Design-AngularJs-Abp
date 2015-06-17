using Abp.Web.Mvc.Controllers;

namespace Fzrain.Web.Controllers
{
    /// <summary>
    /// Derive all Controllers from this class.
    /// </summary>
    public abstract class FzrainControllerBase : AbpController
    {
        protected FzrainControllerBase()
        {
            LocalizationSourceName = FzrainConsts.LocalizationSourceName;
        }
    }
}