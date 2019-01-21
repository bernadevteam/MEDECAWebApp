using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using MEDECAWebApp.Models;
using Microsoft.Owin.Security.OAuth;
using IMPEMASA.Providers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace MEDECAWebApp
{
    public partial class Startup
    {
        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup()
        {
            PublicClientId = "web";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(4),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromDays(1),
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromDays(1),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");
#if DEBUG

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "30889076172-p6sg88hq4ff4upcrvqpqnefrqtc2ird4.apps.googleusercontent.com",
                ClientSecret = "RoG4EvpE4fSsoLR0hV7X52aL",
                UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo",
                BackchannelHttpHandler = new GoogleUserInfoRemapper(new WebRequestHandler())
            });
        }
#else
         app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "282963924364-6d1j9dik8grqifggjulbocr1gvt0oif0.apps.googleusercontent.com",
                ClientSecret = "8NXDQmTDYSTncHDTY-fEMuXk",
                UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo",
                BackchannelHttpHandler = new GoogleUserInfoRemapper(new WebRequestHandler())
            });
        }
#endif
    }

    internal class GoogleUserInfoRemapper : DelegatingHandler
    {
        public GoogleUserInfoRemapper(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!request.RequestUri.AbsoluteUri.Equals("https://www.googleapis.com/oauth2/v2/userinfo"))
            {
                return response;
            }

            response.EnsureSuccessStatusCode();
            var text = await response.Content.ReadAsStringAsync();
            JObject user = JObject.Parse(text);
            JObject legacyFormat = new JObject();

            JToken token;
            if (user.TryGetValue("id", out token))
            {
                legacyFormat["id"] = token;
            }
            if (user.TryGetValue("name", out token))
            {
                legacyFormat["displayName"] = token;
            }
            JToken given, family;
            if (user.TryGetValue("given_name", out given) && user.TryGetValue("family_name", out family))
            {
                var name = new JObject();
                name["givenName"] = given;
                name["familyName"] = family;
                legacyFormat["name"] = name;
            }
            if (user.TryGetValue("link", out token))
            {
                legacyFormat["url"] = token;
            }
            if (user.TryGetValue("email", out token))
            {
                var email = new JObject();
                email["value"] = token;
                legacyFormat["emails"] = new JArray(email);
            }
            if (user.TryGetValue("picture", out token))
            {
                var image = new JObject();
                image["url"] = token;
                legacyFormat["image"] = image;
            }

            text = legacyFormat.ToString();
            response.Content = new StringContent(text);
            return response;
        }
    }
}