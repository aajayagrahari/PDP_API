namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Contains Project basic details to show on user dashboard
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Dtos.BaseDto" />
    public class DashboardProjectDetailDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; set; }

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
    }
}
