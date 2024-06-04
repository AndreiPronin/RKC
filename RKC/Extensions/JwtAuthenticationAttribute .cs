using BL.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;
using BL.Security;
using BL.Helper;
using System.Net;
using System.Web.Http;

namespace RKC.Extensions
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private TokenCreator _tokenCreator { get; set; }
        public NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public JwtAuthenticationAttribute()
        {
            _tokenCreator = new TokenCreator();
            _tokenCreator.Key = new GetConfigurationManager().GetAppSettings(KeyConfigurationManager.GeneralServiceKey).GetString();
        }
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (!_tokenCreator.IsAuthorize(authorization.Parameter))
            {
                Logger.Error($"Ошибка авторизации по JWT. Адресс удаленной машины: {context.Request.RequestUri}");
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}