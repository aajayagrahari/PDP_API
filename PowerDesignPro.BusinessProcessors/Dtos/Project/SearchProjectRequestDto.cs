namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Search request dto, To search User Project
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Dtos.BaseDto" />
    public class SearchProjectRequestDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserID { get; set; }
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; set; }
        /// <summary>
        /// Gets or sets the days old.
        /// </summary>
        /// <value>
        /// The days old.
        /// </value>
        public int DaysOld { get; set; }
        /// <summary>
        /// Gets or sets the solution name.
        /// </summary>
        /// <value>
        /// The name of solution.
        /// </value>
        public string SolutionName { get; set; }
        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>
        /// create date of a project.
        /// </value>
        public string CreateDate { get; set; }
        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        /// <value>
        /// modified date of a project.
        /// </value>
        public string ModifyDate { get; set; }
    }
}
