namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Dto class to show all the dropdown picklist data
    /// </summary>
    public class PickListDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        public string LanguageKey { get; set; }
        //public bool Selected { get; set; }
    }
}
