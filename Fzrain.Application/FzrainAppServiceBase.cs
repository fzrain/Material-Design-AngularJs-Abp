using Abp.Application.Services;

namespace Fzrain
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class FzrainAppServiceBase : ApplicationService
    {
        protected FzrainAppServiceBase()
        {
            LocalizationSourceName = FzrainConsts.LocalizationSourceName;
        }
    }
}