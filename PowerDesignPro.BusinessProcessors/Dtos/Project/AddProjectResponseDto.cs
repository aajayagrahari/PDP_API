namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    ///  response object returned for new Project added.
    /// </summary>
    public class AddProjectResponseDto
    {
        /// <summary>
        /// Gets or sets the project identifier.
        /// </summary>
        /// <value>
        /// The project identifier.
        /// </value>
        public int ProjectID { get; set; }
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        /// <value>
        /// The name of the project.
        /// </value>
        public string ProjectName { get; set; }
    }
}
