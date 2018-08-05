using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    [Authorize]
    [RoutePrefix("PDF")]
    public class PDFController : BaseController
    {
        private IPDF _pdfProcesor;

        private ITraceMessage _traceMessage;

        public PDFController(IPDF pdfProcessor, ITraceMessage traceMessage)
        {
            _pdfProcesor = pdfProcessor;
            _traceMessage = traceMessage;
        }

        [HttpPost]
        [Route("GetHarmonicAnalysisPDF")]
        public HttpResponseMessage GetHarmonicAnalysisPDF(HarmonicAnalysisDto harmonicAnalysis)
        {
            return CreateHttpResponse(() =>
            {            
                return Request.CreateResponse(_pdfProcesor.WritePDF(harmonicAnalysis));
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPDF")]
        public HttpResponseMessage GetPDF(ReportModel reportModel)
        {
            return CreateHttpResponse(() =>
            {
                if (reportModel != null)
                {
                    var user = GetUserDetailsByUserName(UserName);
                    if (user != null)
                    {
                        reportModel.Company = user.CompanyName;
                        reportModel.Email = user.Email;
                        reportModel.UserID = UserID;
                        reportModel.UserName = UserName;
                        reportModel.Phone = user.Phone;
                        reportModel.brand = user.CompanyName;
                        var pdfStream = _pdfProcesor.GeneratePDF(reportModel);
                        if (pdfStream != null)
                        {
                            var memoryStream = new MemoryStream(pdfStream.ToArray());
                            if (memoryStream != null)
                            {
                                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                                result.Content = new StreamContent(memoryStream);
                                _pdfProcesor.DeleteImage();
                                return result;
                            }
                            else
                            {
                                _traceMessage.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + reportModel.SolutionId.ToString(), "GetPDF", "PDF Memory stream null.", "PDF Memory stream null.");
                                //return Request.CreateResponse(HttpStatusCode.NoContent, "PDF not found");
                                return null;
                            }
                        }
                        else
                        {
                            _traceMessage.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + reportModel.SolutionId.ToString(), "GetPDF", "PDF Memory stream null.", "PDF Memory stream null.");
                            //return Request.CreateResponse(HttpStatusCode.NoContent, "PDF not found");
                            return null;
                        }
                    }
                    else
                    {
                        _traceMessage.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + reportModel.SolutionId.ToString(), "GetPDF", "User not available.", "User not available.");
                        return null;
                    }
                }
                else
                {
                    _traceMessage.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + reportModel.SolutionId.ToString(), "GetPDF", "Report model variable is null.", "Report model variable is null.");
                    //return Request.CreateResponse(HttpStatusCode.BadRequest, "Report Parameter is NULL");
                    return null;
                }
            });            

        }
    }
}
