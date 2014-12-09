using System.Web.Mvc;
using MovieService.Core.Entities;
using MovieService.MvcControllers;

namespace MovieService.WebUI.Infrastructure
{
    public abstract class BaseView<TModel> : WebViewPage<TModel>
    {
        public User DomainUser
        {
            get
            {
                return ((BaseController) ViewContext.Controller).PresentationContext.CurrentUser;
            }
        }
    }
}