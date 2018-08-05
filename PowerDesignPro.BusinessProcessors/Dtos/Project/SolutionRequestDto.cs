namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Solution Request data to add/update solution details
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Dtos.BaseSolutionSetupMappingValuesDto" />
    public class SolutionRequestDto : BaseSolutionSetupMappingValuesDto
    {
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


        public string Notes { get; set; }

        public string Language { get; set; }

        //public BaseSolutionSetupMappingValuesDto SolutionSetupRequestDto { get; set; }
    }
}
