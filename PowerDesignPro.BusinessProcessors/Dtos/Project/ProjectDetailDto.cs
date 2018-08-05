namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Contains all the Project detail
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Dtos.DashboardProjectDetailDto" />
    public class ProjectDetailDto : DashboardProjectDetailDto
    {
        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>
        /// The name of the contact.
        /// </value>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the contact email.
        /// </summary>
        /// <value>
        /// The contact email.
        /// </value>
        public string ContactEmail { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the access level for the user to the project.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public bool IsReadOnlyAccess { get; set; }

    }
}
