using System.Net;
using System.Threading.Tasks;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Dtos.Project;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface ISolutionSummary
    {
        LoadSummaryLoadsDto GetLoadSummaryLoads(int projectID, int solutionID, string userID, string userName = "");

        TransientAnalysisDto GetTransientAnalysis(int projectID, int solutionID, string userID, string brand, string userName = "");

        HarmonicAnalysisInputsDto GetHarmonicAnalysisInputs(int projectID, int solutionID, string userID, string brand, string userName = "");

        GasPipingReportDto GetGasPipingReport(int projectID, int solutionID, string userID, string brand, string userName = "");

        ExhaustPipingReportDto GetExhaustPipingReport(int projectID, int solutionID, string userID, string brand, string userName = "");

        SolutionLimitsDto GetSolutionLimits(int projectID, int solutionID, string userID, string userName = "");

        //SolutionSummaryRecommendedProductDto GetRecommendedProduct(int projectID, int solutionID, string userID);

        SolutionSummaryDto GetSolutionSummary(int projectID, int solutionID, string userID, string brand, string userName = "");

        bool UpdateLoadSequence(LoadSequenceRequestDto requestDto, string userID, string userName = "");

        bool UpdateLoadSequenceShedDetail(LoadSequenceShedRequestDto requestDto, string userID);

        SolutionSummaryRecommendedProductDto UpdateRecommendedProductDetails(RecommendedProductRequestDto recommendedProductRequestDto, string userID, string userName);

        bool UpdateFuelTypeForSolution(UpdateFuelTypeForSolutionRequestDto requestDto, string userID, string userName);


        /// <summary>
        /// Method to get Gas Piping Details
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <returns></returns>
        GasPipingDto GetGasPipingDetails(int projectID, int solutionID);

        ExhaustPipingDto GetExhaustPipingDetails(int projectID, int solutionID);

        GasPipingDto SaveGasPipingSolution(GasPipingRequestDto gasPipingRequest, string userName);

        int SaveExhaustPipingSolution(ExhaustPipingRequestDto exhaustPiping, string username);

        Task<int> RequestForQuote(RequestForQuoteDto requestForQuoteDto, string userID, string userName);
    }
}
