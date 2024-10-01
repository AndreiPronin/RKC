using AppCache;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RKC.Extensions
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public ICacheApp _cacheApp;
        public AuthAttribute() {
            _cacheApp = new CacheApp();
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // 403
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "home",
                        action = "AccessDenied"
                    })
                );
            }
            else
            {
                // 401
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "Account",
                        action = "Login"
                    })
                );
            }
        }
    }
}