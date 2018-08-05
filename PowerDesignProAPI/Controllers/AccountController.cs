using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PowerDesignPro.Common.MessageConfig;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Data.Models;
using Microsoft.AspNet.Identity.Owin;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using Microsoft.AspNet.Identity.EntityFramework;
using PowerDesignPro.Data;
using System.Linq;
using PowerDesignPro.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PowerDesignProAPI.Controllers
{
    [Authorize]
    [RoutePrefix("Account")]
    public class AccountController : BaseController
    {
        //private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        private ITraceMessage _traceMessageProcessor;
        private IPickList _pickListRegister;
        private UserRegisterDto _userRegisterDto;

        //public AccountController(
        //    IAuthenticationManager authenticationManager)
        //{
        //    _authenticationManager = authenticationManager;
        //}

        public AccountController(
            ITraceMessage traceMessageProcessor,
            IPickList pickListRegister,
            UserRegisterDto userRegisterDto)
        {
            _pickListRegister = pickListRegister;
            _userRegisterDto = userRegisterDto;
            _traceMessageProcessor = traceMessageProcessor;
        }

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _AppRoleManager = null;
        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        /// <summary>
        /// Register as user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        [HttpPost]
        public async Task<HttpResponseMessage> Register(UserRegisterDto model)
        {
            string errorMessage = string.Empty;
            if (model == null || !ModelState.IsValid)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = MessageCollection.Instance.GetMessage("UserRegisteredError", Message.UserAccount)
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }

            try
            {
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Mobile = model.Mobile,
                    CompanyName = model.CompanyName,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    StateID = model.StateID,
                    ZipCode = model.ZipCode,
                    CountryID = model.CountryID,
                    UserCategoryID = model.UserCategoryID,
                    CustomerNumber = model.CustomerNo,
                    //BrandID = model.BrandID,
                    CreatedDateTime = DateTime.UtcNow,
                    ModifiedDateTime = DateTime.UtcNow,
                    LastLoginDateTime = null
                };

                IdentityResult result = UserManager.Create(user, model.Password);

                //Comment the code not sure the role need to submit from there or not. Need to make sure. 

                //if (AppRoleManager.Roles.Count() == 0)
                //{
                //    AppRoleManager.Create(new IdentityRole { Name = "Admin" });
                //    var adminUser = UserManager.FindByName("admin@test.com");

                //    UserManager.AddToRoles(adminUser.Id, new string[] { "Admin" });
                //}

                if (!result.Succeeded)
                {
                    if (result.Errors != null)
                    {
                        foreach (string error in result.Errors)
                        {
                            errorMessage += error + " <br /> <hr /> ";
                        }
                    }

                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = errorMessage
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }


                string callbackURL = await SendEmailConfirmationTokenAsync(user, model.Language);

                var successResponse = new
                {
                    ErrorCode = 0,
                    Message = MessageCollection.Instance.GetMessage("UserRegisteredSuccess", Message.UserAccount)
                };

                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Info, "User Registration", "Register", "Registration Success", "Registration Success for " + model.UserName);

                return Request.CreateResponse(HttpStatusCode.OK, successResponse);
            }

            catch (Exception ex)
            {
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "User Registration", "Register", "Registration Failed for " + model.UserName, ex.Message);

                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "Exception occured in User registration"
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserRegister")]
        public HttpResponseMessage GetUserRegister()
        {
            return CreateHttpResponse(() =>
            {
                LoadPickListDetailForRegister(_userRegisterDto.UserRegisterPickListDto);
                return Request.CreateResponse(_userRegisterDto);
            });
        }

        [HttpGet]
        [Route("GetUserDetails")]
        public async Task<HttpResponseMessage> GetUserDetails()
        {
            try
            {
                var user = await UserManager.FindByEmailAsync(UserName);
                var userRegisterDto = new UserRegisterDto
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    Mobile = user.Mobile,
                    CompanyName = user.CompanyName,
                    Address1 = user.Address1,
                    Address2 = user.Address2,
                    City = user.City,
                    StateID = user.StateID,
                    ZipCode = user.ZipCode,
                    CountryID = user.CountryID,
                    CustomerNo = user.CustomerNumber,
                    UserCategoryID = user.UserCategoryID,
                };
                LoadPickListDetailForRegister(userRegisterDto.UserRegisterPickListDto);
                return Request.CreateResponse(userRegisterDto);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "Exception occured in get account setting"
                };
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "Account Setting", "Account Setting", "Account setting Failed for " + UserName, ex.Message);

                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }


        [Route("AccountUpdate")]
        [HttpPost]
        public async Task<HttpResponseMessage> AccountUpdate(UserRegisterDto model)
        {
            string errorMessage = string.Empty;
            if (model == null || (!ModelState.IsValid && !ModelState.Keys.Contains("model.Password")))
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = MessageCollection.Instance.GetMessage("AccountUpdateError", Message.UserAccount)
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }

            try
            {
                var user = await UserManager.FindByEmailAsync(UserName);
                user.UserName = model.UserName;
                user.Email = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Phone = model.Phone;
                user.Mobile = model.Mobile;
                user.CompanyName = model.CompanyName;
                user.Address1 = model.Address1;
                user.Address2 = model.Address2;
                user.City = model.City;
                user.StateID = model.StateID;
                user.ZipCode = model.ZipCode;
                user.CountryID = model.CountryID;
                user.UserCategoryID = model.UserCategoryID;
                user.CustomerNumber = model.CustomerNo;
                user.ModifiedDateTime = DateTime.UtcNow;
                IdentityResult result = UserManager.Update(user);

                //Comment the code not sure the role need to submit from there or not. Need to make sure. 

                //if (AppRoleManager.Roles.Count() == 0)
                //{
                //    AppRoleManager.Create(new IdentityRole { Name = "Admin" });
                //    var adminUser = UserManager.FindByName("admin@test.com");

                //    UserManager.AddToRoles(adminUser.Id, new string[] { "Admin" });
                //}

                if (!result.Succeeded)
                {
                    if (result.Errors != null)
                    {
                        foreach (string error in result.Errors)
                        {
                            errorMessage += error + " <br /> <hr /> ";
                        }
                    }

                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = errorMessage
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }

                var successResponse = new
                {
                    ErrorCode = 0,
                    Message = MessageCollection.Instance.GetMessage("AccountUpdateSuccess", Message.UserAccount)
                };

                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Info, "Account update", "AccountUpdate", "Account Update Success", "Account Update Success for " + model.UserName);

                return Request.CreateResponse(HttpStatusCode.OK, successResponse);
            }

            catch (Exception ex)
            {
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "Account update", "AccountUpdate", "Account Update Success" + model.UserName, ex.Message);

                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "Exception occured in Account update"
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        /// <summary>
        /// Sends confirmation email on registration
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("ConfirmRegistrationEmail")]
        public async Task<HttpResponseMessage> ConfirmRegistrationEmail(string userId, string code)
        {
            try
            {
                if (userId == null || code == null)
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "An error occurred while processing your request."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }
                var result = await UserManager.ConfirmEmailAsync(userId, code);

                if (result.Succeeded)
                {
                    var successResponse = new
                    {
                        ErrorCode = 0,
                        Message = "Email Verified Successfully"
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
                else
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "An error occurred while processing your request."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = ex.Message
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        /// <summary>
        /// Sends confirmation email for Forgot Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<HttpResponseMessage> ForgotPassword(ForgotPasswordDto model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "Model State is invalid."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }

                var user = await UserManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "forgotPassword.ForgotPasswordEmailNotFoundError"
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }
                else if(!(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "forgotPassword.ForgotPasswordTechnicalError"
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                //var clientURL = Request.GetOwinContext().Request.Uri.GetLeftPart(UriPartial.Authority);
                var clientURL = RequestContext.Url.Request.Headers.GetValues("Origin").FirstOrDefault();
                var callbackURL = $"{clientURL}/resetPassword?userId={user.Id}&code={code}";

                var urlBrand = "generac";
                if (clientURL.ToLower().Contains("pramac"))
                {
                    urlBrand = "pramac";
                }

                if (String.IsNullOrEmpty(model.Language) || model.Language != "en" || model.Language != "en")
                    model.Language = "en";

                var emaildata = new SendGridEmailData
                {
                    template_id = SectionHandler.GetSectionValue($"{model.Language.ToLower()}/emailTemplates/resetPassword", "TemplateID", ""),
                    personalizations = new List<Personalization>()
                    {
                        new Personalization
                        {
                            to = new List<To>()
                            {
                                new To
                                {
                                    email = user.Email,
                                    name = user.FirstName + " " + user.LastName
                                }
                            },
                            substitutions = new Dictionary<string, string>()
                            {
                                { "%FirstName%", user.FirstName},
                                { "%LastName%", user.LastName},
                                { "%LoginID%", user.Email},
                                { "%PasswordResetLink%", callbackURL },
                                { "%CompanyName%", EmailHelper.CompanyName(model.Language, urlBrand) },
                                { "%CompanyAddress%", EmailHelper.CompanyAddress(model.Language, urlBrand) }
                            }
                        }
                    }
                };

                await EmailHelper.SendGridAsyncWithTemplate(emaildata);

                var successResponse = new
                {
                    ErrorCode = 0,
                    Message = "Password Reset Email sent successfully."
                };
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Info, "Forgot Password", "Forgot Password", "Forgot Password", "Forgot Password Success for " + model.EmailAddress);

                return Request.CreateResponse(HttpStatusCode.OK, successResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "Exception occured in User registration"
                };
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "Forgot Password", "Forgot Password", "Forgot Password Failed for " + model.EmailAddress, ex.Message);

                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        /// <summary>
        /// Resets Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<HttpResponseMessage> ResetPassword(ResetPasswordDto model)
        {

            var errorMessage = "";
            if (model == null || !ModelState.IsValid)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "Model State is invalid."
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
            try
            {
                var user = await UserManager.FindByEmailAsync(model.EmailAddress);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "An error occurred while processing your request."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }

                var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    user.LastPasswordChangedDateTime = DateTime.UtcNow;
                    var identityUpdateResult = await UserManager.UpdateAsync(user);
                    var successResponse = new
                    {
                        ErrorCode = 0,
                        Message = "Password Reset successfully."
                    };
                    _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Info, "Reset Password", "Reset Password", "Reset Password", "Reset Password Success for " + model.EmailAddress);

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }

                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        errorMessage += error + " <br /> <hr /> ";
                    }
                }

                var response = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "An error occurred while processing your request."
                };
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "Reset Password", "Reset Password", "Reset Password Failed for " + model.EmailAddress, errorMessage);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "An error occurred while processing your request."
                };
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "Forgot Password", "Reset Password", "Reset Password Failed for " + model.EmailAddress, ex.Message);

                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        /// <summary>
        /// Admin method to resend Confirmation Email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Admin/ResendConfirmationEmail")]
        public async Task<HttpResponseMessage> ResendConfirmationEmail(ResendConfirmationEmailDto model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "Model State is invalid. Missing required fields."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }

                var user = await UserManager.FindByEmailAsync(model.EmailAddress);
                if (user == null)
                {
                    var errorResponse = new
                    {
                        ErrorCode = -1,
                        ErrorDescription = "User not found."
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
                }

                string callbackURL = await SendEmailConfirmationTokenAsync(user, model.Language);

                var successResponse = new
                {
                    ErrorCode = 0,
                    Message = "Confirmation Email sent successfully."
                };

                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Info, "ResendConfirmationEmail", "ResendConfirmationEmail", "ResendConfirmationEmail Success", "ResendConfirmationEmail Success for " + model.EmailAddress);

                return Request.CreateResponse(HttpStatusCode.OK, successResponse);
            }
            catch (Exception ex)
            {
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "ResendConfirmationEmail", "ResendConfirmationEmail", "ResendConfirmationEmail Failed for " + model.EmailAddress, ex.Message);

                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = ex.Message
                };
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }

        private async Task<string> SendEmailConfirmationTokenAsync(ApplicationUser user, string language)
        {
            var code = UserManager.GenerateEmailConfirmationToken(user.Id);
            var clientURL = RequestContext.Url.Request.Headers.GetValues("Origin").FirstOrDefault();
            var callbackURL = $"{clientURL}/confirmEmail?userId={user.Id}&code={code}";

            var urlBrand = "generac";
            if (clientURL.ToLower().Contains("pramac"))
            {
                urlBrand = "pramac";
            }

            if (String.IsNullOrEmpty(language) || language != "en" || language != "en")
                language = "en";

            var emaildata = new SendGridEmailData
            {
                template_id = SectionHandler.GetSectionValue($"{language.ToLower()}/emailTemplates/confirmEmail", "TemplateID", ""),
                personalizations = new List<Personalization>()
                {
                    new Personalization
                    {
                        to = new List<To>()
                        {
                            new To
                            {
                                email = user.Email,
                                name = user.FirstName + " " + user.LastName
                            }
                        },
                        substitutions = new Dictionary<string, string>()
                        {
                            { "%FirstName%", user.FirstName},
                            { "%LastName%", user.LastName},
                            { "%EmailConfirmationLink%", callbackURL },
                            { "%CompanyName%", EmailHelper.CompanyName(language, urlBrand) },
                            { "%CompanyAddress%", EmailHelper.CompanyAddress(language, urlBrand) }
                        }
                    }
                }
            };

            var responseCode = await EmailHelper.SendGridAsyncWithTemplate(emaildata);

            return callbackURL;
        }

        private void LoadPickListDetailForRegister(UserRegisterPickListDto userRegisterPickListDto)
        {
            userRegisterPickListDto.StateList = _pickListRegister.GetState();
            userRegisterPickListDto.CountryList = _pickListRegister.GetCountry();
            userRegisterPickListDto.UserCategoryList = _pickListRegister.GetUserCategory();
            //userRegisterPickListDto.BrandList = _pickListRegister.GetBrand();
        }


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && _userManager != null)
        //    {
        //        _userManager.Dispose();
        //        _userManager = null;
        //    }

        //    base.Dispose(disposing);
        //}

        //#region Helpers

        //private IHttpActionResult GetErrorResult(IdentityResult result)
        //{
        //    if (result == null)
        //    {
        //        return InternalServerError();
        //    }

        //    if (!result.Succeeded)
        //    {
        //        if (result.Errors != null)
        //        {
        //            foreach (string error in result.Errors)
        //            {
        //                ModelState.AddModelError("", error);
        //            }
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            // No ModelState errors are available to send, so just return an empty BadRequest.
        //            return BadRequest();
        //        }

        //        return BadRequest(ModelState);
        //    }

        //    return null;
        //}

        //private class ExternalLoginData
        //{
        //    public string LoginProvider { get; set; }
        //    public string ProviderKey { get; set; }
        //    public string UserName { get; set; }

        //    public IList<Claim> GetClaims()
        //    {
        //        IList<Claim> claims = new List<Claim>();
        //        claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

        //        if (UserName != null)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
        //        }

        //        return claims;
        //    }

        //    public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
        //    {
        //        if (identity == null)
        //        {
        //            return null;
        //        }

        //        Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

        //        if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
        //            || String.IsNullOrEmpty(providerKeyClaim.Value))
        //        {
        //            return null;
        //        }

        //        if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
        //        {
        //            return null;
        //        }

        //        return new ExternalLoginData
        //        {
        //            LoginProvider = providerKeyClaim.Issuer,
        //            ProviderKey = providerKeyClaim.Value,
        //            UserName = identity.FindFirstValue(ClaimTypes.Name)
        //        };
        //    }
        //}

        //private static class RandomOAuthStateGenerator
        //{
        //    private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

        //    public static string Generate(int strengthInBits)
        //    {
        //        const int bitsPerByte = 8;

        //        if (strengthInBits % bitsPerByte != 0)
        //        {
        //            throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
        //        }

        //        int strengthInBytes = strengthInBits / bitsPerByte;

        //        byte[] data = new byte[strengthInBytes];
        //        _random.GetBytes(data);
        //        return HttpServerUtility.UrlTokenEncode(data);
        //    }
        //}

        //#endregion
    }
}
