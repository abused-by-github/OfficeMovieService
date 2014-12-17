using System;
using System.Configuration;
using System.Dynamic;
using System.Net;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using MovieService.Core.Entities;
using MovieService.DomainApi;
using MovieService.DomainApi.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MovieService.MvcControllers
{
    public class AccountController : BaseController
    {
        private const string GoogleOpenID = "https://www.google.com/accounts/o8/id";

        private readonly IUserFacade userFacade;

        public AccountController(IUserFacade userFacade, PresentationContext presentationContext)
            : base(presentationContext)
        {
            this.userFacade = userFacade;
        }

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult LoginGoogle()
        {
            OpenIdRelyingParty openID = new OpenIdRelyingParty();
            var callbackUrl = GetBaseUrl(Url.Action("LoginCallback", "Account"));
            var request = openID.CreateRequest(GoogleOpenID, GetBaseUrl(), new Uri(callbackUrl));
            FetchRequest fetch = new FetchRequest();
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email, true));
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.First, true));
            fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.Last, true));
            request.AddExtension(fetch);
            return request.RedirectingResponse.AsActionResult();
        }

        [HttpGet]
        public ActionResult LoginFacebook(string token)
        {
            var appId = ConfigurationManager.AppSettings["FBAppId"];
            var appSecret = ConfigurationManager.AppSettings["FBAppSecret"];
            var verifyUrl = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}|{2}",
                token, appId, appSecret);

            using (var client = new WebClient())
            {
                var verifyResponse = client.DownloadString(verifyUrl);
                dynamic verifyResponseObj = JsonConvert.DeserializeObject<ExpandoObject>(verifyResponse, new ExpandoObjectConverter());
                if (verifyResponseObj.data.is_valid)
                {
                    var getEmailUrl = string.Format("https://graph.facebook.com/v2.2/me?access_token={0}", token);
                    var getEmailResponse = client.DownloadString(getEmailUrl);
                    dynamic getEmailResponseObj = JsonConvert.DeserializeObject<ExpandoObject>(getEmailResponse, new ExpandoObjectConverter());
                    var email = getEmailResponseObj.email;
                    var user = new User
                    {
                        Name = email
                    };
                    userFacade.Save(user);
                    FormsAuthentication.SetAuthCookie(email, false);
                }
                else
                {
                    throw new SecurityException("Access denied.");
                }
            }

            return Redirect(GetBaseUrl());
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

        public ActionResult LoginCallback()
        {
            OpenIdRelyingParty rp = new OpenIdRelyingParty();
            var response = rp.GetResponse();
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

        public void SaveUser(FetchResponse data)
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

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToLandingAction();
        }
        
        [Authorize]
        public ActionResult UserProfile()
        {
            return View();
        }

        private RedirectToRouteResult RedirectToLandingAction()
        {
            return RedirectToAction("List", "Movie");
        }

        private bool IsEmailDomainValid(string email, string allowedDomain)
        {
            return string.IsNullOrEmpty(allowedDomain) || email.EndsWith(allowedDomain);
        }

    }
}
