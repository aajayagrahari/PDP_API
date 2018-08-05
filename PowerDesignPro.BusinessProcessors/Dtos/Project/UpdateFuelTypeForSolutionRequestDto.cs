namespace PowerDesignPro.BusinessProcessors.Dtos.Project
{
    public class UpdateFuelTypeForSolutionRequestDto
    {
        public int ProjectID { get; set; }

        public int SolutionID { get; set; }

        public bool DieselProduct{ get; set; }
    }
}
