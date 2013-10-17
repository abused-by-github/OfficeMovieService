using System.Web.Mvc;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.MvcControllers;

namespace Svitla.MovieService.WebUI.Infrastructure
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