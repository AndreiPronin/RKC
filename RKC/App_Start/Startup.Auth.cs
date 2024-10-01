using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using IdentityModel.Client;
using IdentityServer3.Core.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using RKC.Models;
using SameSiteMode = System.Web.SameSiteMode;

namespace RKC
{
    public partial class Startup
    {
        // Дополнительные сведения о настройке аутентификации см. на странице https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            IdentityModelEventSource.ShowPII = true;
            // Настройка контекста базы данных, диспетчера пользователей и диспетчера входа для использования одного экземпляра на запрос
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Включение использования файла cookie, в котором приложение может хранить информацию для пользователя, выполнившего вход,
            // и использование файла cookie для временного хранения информации о входах пользователя с помощью стороннего поставщика входа
            // Настройка файла cookie для входа
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Позволяет приложению проверять метку безопасности при входе пользователя.
                    // Эта функция безопасности используется, когда вы меняете пароль или добавляете внешнее имя входа в свою учетную запись.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
           
            // Позволяет приложению временно хранить информацию о пользователе, пока проверяется второй фактор двухфакторной проверки подлинности.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Позволяет приложению запомнить второй фактор проверки имени входа. Например, это может быть телефон или почта.
            // Если выбрать этот параметр, то на устройстве, с помощью которого вы входите, будет сохранен второй шаг проверки при входе.
            // Точно так же действует параметр RememberMe при входе.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);


            // OpenId Connect
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            // the following is a workaround for https://github.com/aspnet/AspNetKatana/issues/386
            // make sure to only enable when running on localhost without https
            var openIdConnectOptions = new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = "Единая точка входа",
                RequireHttpsMetadata = false,
                Authority = ConfigurationManager.AppSettings["App:OpenId"],
                ClientId = "mvc4Simple",
                ClientSecret = "secret",
                ResponseType = "code id_token token",
                Scope = "openid profile api1.read",//Include that scope here
                UseTokenLifetime = false,
                
                RedirectUri = ConfigurationManager.AppSettings["App:Host"],
                PostLogoutRedirectUri = ConfigurationManager.AppSettings["App:Host"],
                SignInAsAuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                SaveTokens = true,
                RedeemCode = true,
                CookieManager = new SystemWebCookieManager(),
                //ProtocolValidator = new OpenIdConnectProtocolValidator
                //{
                //    //RequireStateValidation = false,
                //    RequireNonce = false,
                //},
                //TokenValidationParameters = new TokenValidationParameters()
                //{
                //    RoleClaimType = "role",
                //    NameClaimType = "name",
                //},
                
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = context =>
                    {
                        context.AuthenticationTicket.Identity.AddClaim(new
                        Claim(ClaimTypes.NameIdentifier, context.ProtocolMessage.IdToken));
                        context.AuthenticationTicket.Identity.AddClaim(new Claim("access_token",
                 context.ProtocolMessage.AccessToken));//Set access token in access_token claim
                        return Task.FromResult(0);
                    },
                    //AuthorizationCodeReceived = (context) =>
                    //{
                    //    var code = context.Code;
                    //    ClientCredential credential = new ClientCredential("mvc4Simple", "secret");
                    //    string tenantID = context.AuthenticationTicket.Identity.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
                    //    string signedInUserID = context.AuthenticationTicket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                    //    AuthenticationContext authContext = new AuthenticationContext(string.Format("https://login.windows.net/{0}", tenantID), new EFADALTokenCache(signedInUserID));
                    //    AuthenticationResult result = authContext.AcquireTokenByAuthorizationCodeAsync(
                    //                code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, graphResourceID);

                    //    return Task.FromResult(0);
                    //},
                    AuthenticationFailed = context =>
                    {
                        if (context.Exception.Message.Contains("IDX21323"))
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Home/ResultEmpty/?Message=Недостаточно прав для просмотра" );
                            return Task.FromResult(1);
                        }
                   
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType ==
              Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectRequestType.Logout)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");
                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }

                        }
                        //if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.Authentication)
                        //{
                        //    var nonceKey = HttpContext.Current.Response.Cookies.AllKeys.Where(x => x.Contains("nonce")).FirstOrDefault();
                        //    if (nonceKey != null)
                        //    {
                        //        var nonce = HttpContext.Current.Response.Cookies.Get(nonceKey);
                        //        nonce.SameSite = SameSiteMode.None;
                        //    }
                        //}
                        return Task.FromResult(0);
                    }
                }
            };
            //var redirectUri = new Uri(ConfigurationManager.AppSettings["App:Host"]);
            //if (!"https".Equals(redirectUri.Scheme) && redirectUri.IsLoopback)
            //{
            //    openIdConnectOptions.ProtocolValidator = new OpenIdConnectProtocolValidator
            //    {

            //        RequireState = false,
                 
            //        RequireNonce = true,
            //    };
            //}

            //app.UseOpenIdConnectAuthentication(openIdConnectOptions);


            // Раскомментируйте приведенные далее строки, чтобы включить вход с помощью сторонних поставщиков входа
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "client_id_swagger",
            //   appSecret: "client_id_swagger");


            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap =
        new Dictionary<string, string>();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //CookieHttpOnly = true,
                 AuthenticationMode = AuthenticationMode.Active
            });
            app.UseOpenIdConnectAuthentication(openIdConnectOptions);
        }
    }
    
}