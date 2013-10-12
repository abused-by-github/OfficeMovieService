using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using Svitla.MovieService.Core.Entities;
using Svitla.MovieService.Core.Logging;
using Svitla.MovieService.DomainApi;
using Svitla.MovieService.DomainApi.Exceptions;

namespace Svitla.MovieService.MvcControllers
{
    public class AccountController : Controller
    {
        private const string GoogleOpenID = "https://www.google.com/accounts/o8/id";

        private readonly IUserFacade userFacade;

        public AccountController(IUserFacade userFacade)
        {
            this.userFacade = userFacade;
        }

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        [return:Log(Verbosity.Full)]
        public virtual ActionResult LoginGoogle()
        {
            OpenIdRelyingParty openID = new OpenIdRelyingParty();
            var callbackUrl = GetBaseUrl(Url.Action("LoginCallback", "Account"));
            Logger.LogInfo("Google openID request: {0}", new { callbackUrl, realm = GetBaseUrl() });
            var realm = GetBaseUrl();
            if (realm.Last() == '/')
                realm = realm.Substring(0, realm.Length - 1);
            var request = openID.CreateRequest(GoogleOpenID, realm, new Uri(callbackUrl));
            FetchRequest fetch = new FetchRequest();
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, true));
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, true));
            request.AddExtension(fetch);
            LogAutRequest(request);
            return request.RedirectingResponse.AsActionResult();
        }

        private static void LogAutRequest(IAuthenticationRequest request)
        {
            var headers =
                request.RedirectingResponse.Headers.AllKeys.Select(
                    k => new KeyValuePair<string, string>(k, request.RedirectingResponse.Headers[k])).ToList();
            Logger.LogInfo("Google openID request details: {0}", new { headers, request.RedirectingResponse.Body });
        }

        private string GetBaseUrl(string path = null)
        {
            HttpRequestBase request = ControllerContext.RequestContext.HttpContext.Request;
            if (request == null || request.Url == null)
                throw new Exception("Request is null");
            path = path ?? HttpRuntime.AppDomainAppVirtualPath;
            string baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, path);
            return baseUrl;
        }

        [return: Log(Verbosity.Full)]
        public virtual ActionResult LoginCallback()
        {
            OpenIdRelyingParty rp = new OpenIdRelyingParty();
            var response = rp.GetResponse();
            Logger.LogInfo("Google OAuth response: {0}", response);
            if (response != null)
            {
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        var fetches = response.GetExtension<FetchResponse>();

                        try
                        {
                            SaveUser(fetches);
                        }
                        catch (UserDomainDeniedException e)
                        {
                            Session["ViewError"] = string.Format("Sorry, but only emails from {0} domain are allowed for registration", e.AllowedDomain);
                            return RedirectToLandingAction();
                        }

                        var email = fetches.Attributes[WellKnownAttributes.Contact.Email].Values[0];
                        FormsAuthentication.SetAuthCookie(email, false);
                        return RedirectToLandingAction();
                        break;

                    case AuthenticationStatus.Canceled:
                        Session["GoogleLoginResult"] = "Cancelled.";
                        break;
                    case AuthenticationStatus.Failed:
                        Session["GoogleLoginResult"] = "Login Failed.";
                        break;

                }

            }
            return RedirectToLandingAction();
        }

        public virtual void SaveUser(FetchResponse data)
        {
            var email = data.Attributes[WellKnownAttributes.Contact.Email].Values[0];
            var firstname = data.Attributes[WellKnownAttributes.Name.First].Values[0];
            var lastname = data.Attributes[WellKnownAttributes.Name.Last].Values[0];
            var user = new User
            {
                Name = email
            };
            userFacade.Save(user);
        }

        [return: Log(Verbosity.Full)]
        public virtual ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToLandingAction();
        }

        [Authorize]
        [return: Log(Verbosity.Full)]
        public virtual ActionResult UserProfile()
        {
            return View();
        }

        private RedirectToRouteResult RedirectToLandingAction()
        {
            return RedirectToAction("List", "Movie");
        }
    }
}
