using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RKC.Extensions
{
    public class AuthAttribute : AuthorizeAttribute
    {
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
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}