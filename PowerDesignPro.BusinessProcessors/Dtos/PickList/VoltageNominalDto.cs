namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Response object contains all the Voltage Nominal data for selected VoltagePhase
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Dtos.PickListDto" />
    public class VoltageNominalDto : PickListDto
    {
        public bool IsDefaultSelection { get; set; }

        /// <summary>
        /// Gets or sets the voltage phase identifier.
        /// </summary>
        /// <value>
        /// The voltage phase identifier.
        /// </value>
        public int VoltagePhaseID { get; set; }

        /// <summary>
        /// Gets or sets the frequency identifier.
        /// </summary>
        /// <value>
        /// The frequency identifier.
        /// </value>
        public int FrequencyID { get; set; }
    }
}
