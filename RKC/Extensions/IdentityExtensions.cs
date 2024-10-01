using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace RKC.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetFIO(this IIdentity identity)
        {
            string[] fio = ((ClaimsIdentity)identity).FindFirst("FIO")?.Value.Split(' ');
            if (fio is null) return string.Empty;
             var claim = fio[1];
            // Test for null to avoid issues during local testing
            return claim;
        }
        public static string GetFIOFull(this IIdentity identity)
        {
            string fio = ((ClaimsIdentity)identity).FindFirst("FIO")?.Value;
            if (fio is null) return string.Empty;
            return fio;
        }
    }
}