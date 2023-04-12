using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using RKC.Models;

namespace RKC
{
    public partial class Startup
    {
        public static string OidcAuthority = "https://localhost:5443";
        public static string OidcRedirectUrl = "https://localhost:44345";
        public static string OidcClientId = "client_id_swagger";
        public static string OidcClientSecret = "client_secret_swagger";
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
        {
            context.HandleResponse();
            context.Response.Redirect("/Home/Error?message=" + context.Exception.Message);
            return Task.FromResult(0);
        }
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
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            jwtSecurityTokenHandler.InboundClaimTypeMap.Clear();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            var redirectUri = new Uri(OidcRedirectUrl);
            var openIdConnectOptions = new OpenIdConnectAuthenticationOptions
            {
                Authority = OidcAuthority,
                ClientId = OidcClientId,
                ClientSecret = OidcClientSecret,
                PostLogoutRedirectUri = OidcRedirectUrl,
                ResponseType = OpenIdConnectResponseType.Code, // authorization code flow
                ResponseMode = null, // leave undefined, defaults to query
                Scope = "api1.read", // enables openid connect
                RedirectUri = OidcRedirectUrl,
                SignInAsAuthenticationType = "Cookies",
                RedeemCode = true, // authorization code flow
                SecurityTokenValidator = new MyValid(),
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    //
                    // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                    //
                    AuthenticationFailed = OnAuthenticationFailed,

                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        //// we want to keep first name, last name, subject and roles
                        //var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                        //var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                        //var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        //var roles = id.FindAll(Constants.ClaimTypes.Role);

                        //// create new identity and set name and role claim type
                        var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            ClaimTypes.Name,
                            ClaimTypes.Role);

                        nid.AddClaims(id.Claims);
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));
                        nid.AddClaim(new Claim("access_Token", n.ProtocolMessage.AccessToken));

                        ////nid.AddClaim(givenName);
                        ////nid.AddClaim(familyName);
                        ////nid.AddClaim(sub);
                        ////nid.AddClaims(roles);

                        ////// add some other app specific claim
                        // Connect to you ASP.NET database for example
                        ////nid.AddClaim(new Claim("app_specific", "some data"));

                        //// keep the id_token for logout
                        //nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    }
                }
            };

            // the following is a workaround for https://github.com/aspnet/AspNetKatana/issues/386
            // make sure to only enable when running on localhost without https
            if (!"https".Equals(redirectUri.Scheme) && redirectUri.IsLoopback)
            {
                openIdConnectOptions.ProtocolValidator = new OpenIdConnectProtocolValidator
                {
                    RequireStateValidation = false,
                    RequireNonce = false,
                };
            }

            app.UseOpenIdConnectAuthentication(openIdConnectOptions);


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
        }
    }
    public class MyValid : ISecurityTokenValidator
    {
        public bool CanValidateToken => throw new NotImplementedException();

        public int MaximumTokenSizeInBytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CanReadToken(string securityToken)
        {
            return true;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            throw new NotImplementedException();
        }
    }
}