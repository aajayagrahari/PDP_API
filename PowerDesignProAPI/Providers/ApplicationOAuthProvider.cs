using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Security.Principal;
using PowerDesignPro.Common;
using PowerDesignPro.Data.Models;
using PowerDesignPro.BusinessProcessors.Processors;
using System;
using PowerDesignPro.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace PowerDesignProAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _clientID = ConfigurationManager.AppSettings["ClientID"];
        private readonly string _clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;
            if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                if (clientId == _clientID && clientSecret == _clientSecret)
                {
                    context.Validated();
                }

                var traceMessage = new TraceMessage
                {
                    EventSource = ConfigurationManager.AppSettings["EventSource"].ToString(),
                    TraceLevel = (int)TraceLevel.Info,
                    MessageDateTime = DateTime.UtcNow,
                    Topic = "ValidateClientAuthentication",
                    Context = "ValidateClientAuthentication",
                    MessageText = $"ClientID: {clientId}, ClientSecret: {clientSecret}",
                    Title = "Context Validated."
                };

                var dbContext = new ApplicationDbContext();
                dbContext.TraceMessages.Add(traceMessage);
                dbContext.SaveChanges();
            }
            else
            {
                var traceMessage = new TraceMessage
                {
                    EventSource = ConfigurationManager.AppSettings["EventSource"].ToString(),
                    TraceLevel = (int)TraceLevel.Error,
                    MessageDateTime = DateTime.UtcNow,
                    Topic = "ValidateClientAuthentication",
                    Context = "ValidateClientAuthentication",
                    MessageText = $"ClientID: {clientId}, ClientSecret: {clientSecret}",
                    Title = "Context Rejected. Invalid client credentials."
                };

                var dbContext = new ApplicationDbContext();
                dbContext.TraceMessages.Add(traceMessage);
                dbContext.SaveChanges();

                context.Rejected();
                context.SetError("Access Denied", "Invalid Client Credentials");
            }

            return Task.FromResult(0);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            //enforce client binding of refresh token
            if (context.Ticket == null || context.Ticket.Identity == null || !context.Ticket.Identity.IsAuthenticated)
            {
                context.SetError("Refresh token is not valid", "Refresh token is not valid");
            }
            else
            {
                var userIdentity = context.Ticket.Identity;
                var authenticationTicket = new AuthenticationTicket(userIdentity, context.Ticket.Properties);

                //Additional claim is needed to separate access token updating from authentication 
                //requests in RefreshTokenProvider.CreateAsync() method
                authenticationTicket.Identity.AddClaim(new Claim("refreshToken", "refreshToken"));

                context.Validated(authenticationTicket);
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                ApplicationUserManager userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

                ApplicationUser user;

                user = await userManager.FindByNameAsync(context.UserName.ToUpper());
                if (user != null)
                {

                    if (!await userManager.IsEmailConfirmedAsync(user.Id))
                    {
                        context.SetError("invalid_grant", "The username or password is incorrect.");
                        context.Response.Headers.Add("AuthorizationResponse", new[] { "Failed" });

                        var traceMessage = new TraceMessage
                        {
                            EventSource = System.Configuration.ConfigurationManager.AppSettings["EventSource"].ToString(),
                            TraceLevel = (int)TraceLevel.Error,
                            MessageDateTime = DateTime.UtcNow,
                            Topic = "GrantResourceOwnerCredentials",
                            Context = "GrantResourceOwnerCredentials",
                            MessageText = "Email not confirmed",
                            Title = "Login Failed for " + user.UserName
                        };

                        var dbContext = new ApplicationDbContext();
                        dbContext.TraceMessages.Add(traceMessage);
                        dbContext.SaveChanges();

                        return;
                    }

                    var userDetail = await userManager.FindAsync(context.UserName.ToUpper(), context.Password);
                    var isValidPdpUser = userDetail != null;

                    if (userDetail == null)
                    {
                        ValidateOldPdpUser(userManager, user, context, out isValidPdpUser);
                    }
                    else
                    {
                        user = userDetail;
                    }

                    if (isValidPdpUser)
                    {
                        ClaimsIdentity oAuthIdentity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ExternalBearer);
                        AuthenticationProperties properties = CreateProperties(user);

                        AuthenticationTicket ticket = new AuthenticationTicket(oAuthIdentity, properties);
                        context.Validated(ticket);

                        user.LastLoginDateTime = DateTime.UtcNow;
                        IdentityResult result = await userManager.UpdateAsync(user);

                        var traceMessage = new TraceMessage
                        {
                            EventSource = System.Configuration.ConfigurationManager.AppSettings["EventSource"].ToString(),
                            TraceLevel = (int)TraceLevel.Info,
                            MessageDateTime = DateTime.UtcNow,
                            Topic = "GrantResourceOwnerCredentials",
                            Context = "GrantResourceOwnerCredentials",
                            Title = "Login Succes",
                            MessageText = "Login Succes for " + user.UserName
                        };

                        var dbContext = new ApplicationDbContext();
                        dbContext.TraceMessages.Add(traceMessage);
                        dbContext.SaveChanges();
                        //traceMessageProcessor.WriteTrace(traceMessage);
                    }
                    else
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        context.Response.Headers.Add("AuthorizationResponse", new[] { "Failed" });
                        return;
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    context.Response.Headers.Add("AuthorizationResponse", new[] { "Failed" });
                    return;
                }
            }
            catch (Exception e)
            {
                // Could not retrieve the user due to error.
                context.SetError("server_error");
                context.Response.Headers.Add("AuthorizationResponse", new[] { e.InnerException.Message });

                var traceMessage = new TraceMessage
                {
                    EventSource = System.Configuration.ConfigurationManager.AppSettings["EventSource"].ToString(),
                    TraceLevel = (int)TraceLevel.Error,
                    MessageDateTime = DateTime.UtcNow,
                    Topic = "GrantResourceOwnerCredentials",
                    Context = "GrantResourceOwnerCredentials",
                    Title = "Login Failed",
                    MessageText = e.InnerException.ToString()
                };

                //traceMessageProcessor.WriteTrace(traceMessage);

                var dbContext = new ApplicationDbContext();
                dbContext.TraceMessages.Add(traceMessage);
                dbContext.SaveChanges();

                return;
            }
        }

        private void ValidateOldPdpUser(ApplicationUserManager userManager, ApplicationUser user, OAuthGrantResourceOwnerCredentialsContext context, out bool isValidPdpUser)
        {
            isValidPdpUser = false;
            if (EncryptPassword(context.Password).Equals(user.PasswordHash))
            {
                user.PasswordHash = userManager.PasswordHasher.HashPassword(context.Password);
                userManager.Update(user);
                user = userManager.FindByEmail(context.UserName.ToUpper());
                isValidPdpUser = true;
            }
        }


        private string EncryptPassword(string password)
        {
           using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i <= data.Length - 1; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        public static AuthenticationProperties CreateProperties(ApplicationUser user)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", user.Email }
                //{ "brand", user.Brand.Value }
            };
            return new AuthenticationProperties(data);
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            var identity = new ClaimsIdentity(new GenericIdentity(context.ClientId, OAuthDefaults.AuthenticationType), context.Scope.Select(x => new Claim("urn:oauth:scope", x)));
            context.Validated(identity);
            return Task.FromResult(0);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}