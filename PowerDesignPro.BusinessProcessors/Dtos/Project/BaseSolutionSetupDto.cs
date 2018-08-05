namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Base class for all the solutionSetup Dto's (UserDefaultsSolutionSetup/ProjectSolutionSetup)
    /// </summary>
    public class BaseSolutionSetupDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSolutionSetupDto"/> class.
        /// </summary>
        public BaseSolutionSetupDto()
        {
            ProjectSolutionPickListDto = new ProjectSolutionPickListDto();            
        }

        //public int ID { get; set; }

        /// <summary>
        /// Gets or sets the solution setup mapping values dto.
        /// </summary>
        /// <value>
        /// The solution setup mapping values dto.
        /// </value>
        public BaseSolutionSetupMappingValuesDto SolutionSetupMappingValuesDto { get; set; }

        /// <summary>
        /// Gets or sets the project solution pick list dto.
        /// </summary>
        /// <value>
        /// The project solution pick list dto.
        /// </value>
        public ProjectSolutionPickListDto ProjectSolutionPickListDto { get; set; }
    }
}
