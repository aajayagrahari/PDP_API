using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.MessageConfig;
using PowerDesignPro.Data.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    public class BaseController : ApiController
    {
        private ApplicationUserManager _userManager;

        protected string UserID
        {
            get
            {
                return User.Identity?.GetUserId();
            }
        }

        protected string UserName
        {
            get
            {
                return User.Identity?.GetUserName();
            }
        }

        public ApplicationUserManager UserManagerNew
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

        public ApplicationUser GetUserDetailsByUserName(string userName)
        {
            return UserManagerNew.FindByName(userName);
        }

        protected HttpResponseMessage CreateHttpResponse(Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;
            try
            {
                response = function.Invoke();
                if(response != null && response.StatusCode != HttpStatusCode.OK)
                {
                    return GenerateHttpErrorMessage(response.Content.ToString(), HttpStatusCode.OK);
                }
                if (response == null)
                {
                    return GenerateHttpErrorMessage(null, HttpStatusCode.OK);
                }
                return response;
            }
            catch (PowerDesignProException ex)
            {
                if (!string.IsNullOrEmpty(ex.ErrorMessage))
                    return GenerateHttpErrorMessage(ex.ErrorMessage, HttpStatusCode.OK);

                var errorMessage = MessageCollection.Instance.GetMessage(ex.Code, ex.Category);
                return GenerateHttpErrorMessage(errorMessage, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                //return GenerateHttpErrorMessage("Error occured while in Operation. ", HttpStatusCode.OK);
                return GenerateHttpErrorMessage(ex.Message, HttpStatusCode.OK);
            }

        }

        protected HttpResponseMessage GenerateHttpErrorMessage(string message, HttpStatusCode statCode)
        {
            var response = new
            {
                ErrorCode = -1,
                ErrorDescription = message
            };

            return Request.CreateResponse(statCode, response);
        }
    }
}
