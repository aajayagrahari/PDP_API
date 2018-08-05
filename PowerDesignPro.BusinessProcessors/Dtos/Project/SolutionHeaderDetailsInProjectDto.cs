namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// POCO for List of solution header details in Project Screen
    /// </summary>
    public class SolutionHeaderDetailsInProjectDto
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
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }


        /// <summary>
        /// Gets or sets the access level for the user to the solution.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public bool IsReadOnlyAccess { get; set; }
    }
}
