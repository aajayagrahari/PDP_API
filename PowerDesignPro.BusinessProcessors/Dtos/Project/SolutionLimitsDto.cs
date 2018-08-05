namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SolutionLimitsDto : BaseSolutionLimitsDto
    {
        public decimal MaxLoading { get; set; }

        public decimal MaxRunningLoadValue { get; set; }
    }
}
