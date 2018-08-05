using Newtonsoft.Json;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common;
using PowerDesignProAPI.BusinessProcessors.Dtos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    /// <summary>
    /// Controller for Address Validation
    /// </summary>  
    [AllowAnonymous]
    [RoutePrefix("AddressValidation")]
    public class AddressValidationController : BaseController
    {
        private string _addressValidationAPIURL;
        private ITraceMessage _traceMessageProcessor;
        /// <summary>
        /// Constructor
        /// </summary>
        public AddressValidationController(ITraceMessage traceMessageProcessor)
        {
            _traceMessageProcessor = traceMessageProcessor;

            _addressValidationAPIURL = ConfigurationManager.AppSettings["AddressValidationAPIURL"].ToString();

            if (String.IsNullOrEmpty(_addressValidationAPIURL))
                _addressValidationAPIURL = "http://wkwebsoatest02a/AddressValidationAPI/AddressValidation/Validate";
        }

        /// <summary>
        /// Method to validate the Address request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Validate")]
        public HttpResponseMessage Validate([FromBody]AddressValidationRequestDto request)
        {
            string requestString = "";
            try
            {
                requestString = JsonConvert.SerializeObject(request);
                var requestContentData = new StringContent(requestString, System.Text.Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.PostAsync(_addressValidationAPIURL, requestContentData).Result;
                    if (!response.IsSuccessStatusCode || response.StatusCode != HttpStatusCode.OK)
                    {
                        var errorResponse = new
                        {
                            ErrorCode = -1,
                            ErrorDescription = "Address validation call failed for " + requestString + "." + " API URL = " + _addressValidationAPIURL
                        };
                        _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "Address Validation", "Validate", "Address Validation Failed for " + requestString, "");
                        return Request.CreateResponse( HttpStatusCode.OK, errorResponse);
                    }
                    var successResponse = new
                    {
                        ErrorCode = 0,
                        Message = "Address validation call succeeded for " + requestString
                    };
                    return Request.CreateResponse(JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result));
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = "Address validation call failed for " + requestString + "." + " API URL = " + _addressValidationAPIURL
                };
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "Address Validation", "Validate", "Address Validation Failed for " + requestString, ex.Message);
                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
        }
    }
}
