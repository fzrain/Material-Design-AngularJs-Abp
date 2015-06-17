using Abp.Web.Mvc.Views;

namespace Fzrain.Web.Views
{
    public abstract class FzrainWebViewPageBase : FzrainWebViewPageBase<dynamic>
    {

    }

    public abstract class FzrainWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected FzrainWebViewPageBase()
        {
            LocalizationSourceName = FzrainConsts.LocalizationSourceName;
        }
    }
}