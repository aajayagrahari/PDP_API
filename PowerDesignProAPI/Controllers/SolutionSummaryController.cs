using Microsoft.AspNet.Identity.Owin;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Dtos.Project;
using PowerDesignPro.BusinessProcessors.Interface;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="PowerDesignProAPI.Controllers.BaseController" />
    [RoutePrefix("Project/SolutionSummary")]
    public class SolutionSummaryController : BaseController
    {
        /// <summary>
        /// The solution summary
        /// </summary>
        private readonly ISolutionSummary _solutionSummary;

        private readonly IPickList _pickList;
        private ApplicationUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolutionSummaryController"/> class.
        /// </summary>
        /// <param name="solutionSummary">The solution summary.</param>
        /// <param name="pickList"></param>
        public SolutionSummaryController(
            ISolutionSummary solutionSummary,
            IPickList pickList
            )
        {
            _solutionSummary = solutionSummary;
            _pickList = pickList;
        }

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

        //protected int BrandID
        //{
        //    get
        //    {
        //        var user = UserManager.FindByIdAsync(UserID);
        //        return user.Result.BrandID;
        //    }
        //}

        /// <summary>
        /// Gets the load summary loads.
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>       
        /// <returns></returns>
        [HttpGet]
        [Route("GetLoadSummaryLoads")]
        public HttpResponseMessage GetLoadSummaryLoads(int projectID, int solutionID)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetLoadSummaryLoads(projectID, solutionID, UserID, UserName);
                result.SequenceList = _pickList.GetSequence();
                return Request.CreateResponse(result);
            });
        }

        /// <summary>
        /// Gets the Solution Limits
        /// </summary>
        /// <param name="projectID"></param>        
        /// <param name="solutionID"></param>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSolutionSummary")]
        public HttpResponseMessage GetSolutionSummary(int projectID, int solutionID, string brand)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetSolutionSummary(projectID, solutionID, UserID, brand, UserName);
                result.LoadSummaryLoads.SequenceList = _pickList.GetSequence();

                //result.SolutionSummaryRecommendedProductDetails.FamilySelectionMethodList = _pickList.GetFamilySelectionMethod();
                //result.SolutionSummaryRecommendedProductDetails.SizingMethodList = _pickList.GetSizingMethod();
                //result.SolutionSummaryRecommendedProductDetails.GeneratorDocuments = _pickList.GetGeneratorDocuments(result.SolutionSummaryRecommendedProductDetails.GeneratorID);

                return Request.CreateResponse(result);
            });
        }

        /// <summary>
        /// Gets inputs to calculate Harmonic Analysis data
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHarmonicAnalysisInputs")]
        public HttpResponseMessage GetHarmonicAnalysisInputs(int projectID, int solutionID, string brand)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetHarmonicAnalysisInputs(projectID, solutionID, UserID, brand, UserName);
                return Request.CreateResponse(result);
            });
        }

        /// <summary>
        /// Gets the Transient Analysis
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransientAnalysis")]
        public HttpResponseMessage GetTransientAnalysis(int projectID, int solutionID, string brand)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetTransientAnalysis(projectID, solutionID, UserID, brand, UserName);
                return Request.CreateResponse(result);
            });
        }

        [HttpGet]
        [Route("GetGasPipeReport")]
        public HttpResponseMessage GetGasPipeReport(int projectID, int solutionID, string brand)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetGasPipingReport(projectID, solutionID, UserID, brand, UserName);
                return Request.CreateResponse(result);
            });
        }

        [HttpGet]
        [Route("GetExhaustPipingReport")]
        public HttpResponseMessage GetExhaustPipingReport(int projectID, int solutionID, string brand)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetExhaustPipingReport(projectID, solutionID, UserID, brand, UserName);
                return Request.CreateResponse(result);
            });
        }

        /// <summary>
        /// Gets the updated product recommendation details
        /// </summary>
        /// <param name="recommendedProductRequestDto"></param>
        /// <param name="brand"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateRecommendedProductDetails")]
        public HttpResponseMessage UpdateRecommendedProductDetails(RecommendedProductRequestDto recommendedProductRequestDto)
        {
            return CreateHttpResponse(() =>
            {
                var solutionSummary = _solutionSummary.UpdateRecommendedProductDetails(recommendedProductRequestDto, UserID, UserName);
                return Request.CreateResponse(solutionSummary);
            });
        }

        [HttpPost]
        [Route("UpdateFuelTypeForSolution")]
        public HttpResponseMessage UpdateFuelTypeForSolution(UpdateFuelTypeForSolutionRequestDto requestDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionSummary.UpdateFuelTypeForSolution(requestDto, UserID, UserName));
            });
        }

        /// <summary>
        /// Updates the load sequence.
        /// </summary>
        /// <param name="requestDto">The request dto.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateLoadSequence")]
        public HttpResponseMessage UpdateLoadSequence(LoadSequenceRequestDto requestDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionSummary.UpdateLoadSequence(requestDto, UserID, UserName));
            });
        }

        /// <summary>
        /// Updates the load sequence shed detail.
        /// </summary>
        /// <param name="requestDto">The request dto.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateLoadSequenceShedDetail")]
        public HttpResponseMessage UpdateLoadSequenceShedDetail(LoadSequenceShedRequestDto requestDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionSummary.UpdateLoadSequenceShedDetail(requestDto, UserID));
            });
        }

        /// <summary>
        /// Gets the gas piping details.
        /// </summary>
        /// <param name="projectID">The project identifier.</param>
        /// <param name="solutionID">The solution identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGasPipingDetails")]
        public HttpResponseMessage GetGasPipingDetails(int projectID, int solutionID)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetGasPipingDetails(projectID, solutionID);
                return Request.CreateResponse(result);
            });
        }

        /// <summary>
        /// Gets the exhaust piping details
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetExhaustPipingDetails")]
        public HttpResponseMessage GetExhaustPipingDetails(int projectID, int solutionID)
        {
            return CreateHttpResponse(() =>
            {
                var result = _solutionSummary.GetExhaustPipingDetails(projectID, solutionID);
                return Request.CreateResponse(result);
            });
        }

        /// <summary>
        /// Saves gas piping details
        /// </summary>
        /// <param name="gasPipingRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostGasPipingSolutionDetail")]
        public HttpResponseMessage PostGasPipingSolutionDetail(GasPipingRequestDto gasPipingRequest)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionSummary.SaveGasPipingSolution(gasPipingRequest, UserName));
            });
        }

        /// <summary>
        /// Saves Exhaust Piping details
        /// </summary>
        /// <param name="gasPipingRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PostExhaustPipingSolutionDetail")]
        public HttpResponseMessage PostExhaustPipingSolutionDetail(ExhaustPipingRequestDto exhaustPipingRequest)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionSummary.SaveExhaustPipingSolution(exhaustPipingRequest, UserName));
            });
        }

        [HttpPost]
        [Route("RequestForQuote")]
        public async Task<HttpResponseMessage> RequestForQuote(RequestForQuoteDto requestForQuoteDto)
        {
            var result = await _solutionSummary.RequestForQuote(requestForQuoteDto, UserID, UserName);

            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(result);
            });
        }
    }
}
