namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SolutionSummaryDto
    {
        public int SolutionID { get; set; }

        public int ProjectID { get; set; }

        public string SolutionName { get; set; }

        public string FuelType { get; set; }

        public SolutionLimitsDto SolutionLimits { get; set; }

        public LoadSummaryLoadsDto LoadSummaryLoads { get; set; }

        public SolutionSummaryRecommendedProductDto SolutionSummaryRecommendedProductDetails { get; set; }
    }
}
