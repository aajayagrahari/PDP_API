namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Project Solution Response to show saved solution details
    /// </summary>
    public class ProjectSolutionDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSolutionDto"/> class.
        /// </summary>
        //public ProjectSolutionResponseDto()
        //{
        //    ProjectSolutionPickListDto = new ProjectSolutionPickListDto();
        //}

        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public int ProjectID { get; set; }

        /// <summary>
        /// Gets or sets the solution identifier.
        /// </summary>
        /// <value>
        /// The solution identifier.
        /// </value>
        public int SolutionID { get; set; }

        /// <summary>
        /// Gets or sets the name of the solution.
        /// </summary>
        /// <value>
        /// The name of the solution.
        /// </value>
        public string SolutionName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the spec reference number.
        /// </summary>
        /// <value>
        /// The spec reference number.
        /// </value>
        public string SpecRefNumber { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public string CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the modified date time.
        /// </summary>
        /// <value>
        /// The modified date time.
        /// </value>
        public string ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the access level for the user to the project.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public bool IsReadOnlyAccess { get; set; }

        public BaseSolutionSetupDto BaseSolutionSetupDto { get; set; }
        ///// <summary>
        ///// Gets or sets the solution setup mapping values dto.
        ///// </summary>
        ///// <value>
        ///// The solution setup mapping values dto.
        ///// </value>
        //public BaseSolutionSetupMappingValuesDto SolutionSetupMappingValuesDto { get; set; }

        ///// <summary>
        ///// Gets or sets the project solution pick list dto.
        ///// </summary>
        ///// <value>
        ///// The project solution pick list dto.
        ///// </value>
        //public ProjectSolutionPickListDto ProjectSolutionPickListDto { get; set; }
    }
}
