using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        private const string GoogleOpenID = "https://www.google.com/accounts/o8/id";

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

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
                        var email = fetches.Attributes[WellKnownAttributes.Contact.Email].Values[0];
                        Session["email"] = email;
                        FormsAuthentication.SetAuthCookie(email, false);
                        return RedirectToAction("UserProfile");
                        break;

                    case AuthenticationStatus.Canceled:
                        Session["GoogleLoginResult"] = "Cancelled.";
                        break;
                    case AuthenticationStatus.Failed:
                        Session["GoogleLoginResult"] = "Login Failed.";
                        break;

                }

            }
            return RedirectToAction("Login");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }
        
        [Authorize]
        public ActionResult UserProfile()
        {
            return View();
        }

    }
}
