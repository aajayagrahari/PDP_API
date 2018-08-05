using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PowerDesignProAPI.Providers;
using PowerDesignPro.Data;
using Microsoft.Owin.Security.Infrastructure;
using System.Collections.Concurrent;
using PowerDesignPro.Common;

namespace PowerDesignProAPI
{
    public partial class Startup
    {  
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            int accessTokenTimeout;
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["AccessTokenTimeOut"]) && int.TryParse(System.Configuration.ConfigurationManager.AppSettings["AccessTokenTimeOut"], out accessTokenTimeout))
            {
                accessTokenTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AccessTokenTimeOut"].ToString());
            }
            else
            {
                accessTokenTimeout = 240;
            }

            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Setup Authorization Server
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString(Paths.AuthorizePath),
                TokenEndpointPath = new PathString(Paths.TokenPath),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(accessTokenTimeout),
                ApplicationCanDisplayErrors = true,
//#if DEBUG
                AllowInsecureHttp = true,
//#endif
                // Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new ApplicationOAuthProvider(),

                // Authorization code provider which creates and receives authorization code
                AuthorizationCodeProvider = new AuthenticationTokenProvider
                {
                    OnCreate = CreateAuthenticationCode,
                    OnReceive = ReceiveAuthenticationCode,
                },

                // Refresh token provider which creates and receives referesh token
                RefreshTokenProvider = new AuthenticationTokenProvider
                {
                    OnCreate = CreateRefreshToken,
                    OnReceive = ReceiveRefreshToken,
                }
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }


        private readonly ConcurrentDictionary<string, string> _authenticationCodes =
            new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        private void CreateAuthenticationCode(AuthenticationTokenCreateContext context)
        {
            context.SetToken(Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"));
            _authenticationCodes[context.Token] = context.SerializeTicket();
        }

        private void ReceiveAuthenticationCode(AuthenticationTokenReceiveContext context)
        {
            string value;
            if (_authenticationCodes.TryRemove(context.Token, out value))
            {
                context.DeserializeTicket(value);
            }
        }

        private void CreateRefreshToken(AuthenticationTokenCreateContext context)
        {
            int refressTokenTimeout;
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["RefreshTokenTimeOut"]) && int.TryParse(System.Configuration.ConfigurationManager.AppSettings["RefreshTokenTimeOut"], out refressTokenTimeout))
            {
                refressTokenTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RefreshTokenTimeOut"].ToString());
            }
            else
            {
                refressTokenTimeout = 240;
            }
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddMinutes(refressTokenTimeout));
            context.SetToken(context.SerializeTicket());
        }

        private void ReceiveRefreshToken(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}
