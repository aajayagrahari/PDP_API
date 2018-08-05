namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Response object contains all the Voltage Specific data for selected VoltageNominal
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Dtos.PickListDto" />
    public class VoltageSpecificDto : PickListDto
    {
        public bool IsDefaultSelection { get; set; }

        /// <summary>
        /// Gets or sets the voltage nominal identifier.
        /// </summary>
        /// <value>
        /// The voltage nominal identifier.
        /// </value>
        public int VoltageNominalID { get; set; }
    }
}
