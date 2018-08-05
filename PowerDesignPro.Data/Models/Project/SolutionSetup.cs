namespace PowerDesignPro.Data.Models
{
    /// <summary>
    /// Contains all the selected dropdown value for solution
    /// </summary>
    /// <seealso cref="PowerDesignPro.Data.Models.BaseSolutionSetupEntity" />
    /// <seealso cref="PowerDesignPro.Data.Models.IEntity" />
    public class SolutionSetup : BaseSolutionSetupEntity, IEntity
    {
        /// <summary>
        /// Gets or sets the solution identifier.
        /// </summary>
        /// <value>
        /// The solution identifier.
        /// </value>
        public int SolutionID { get; set; }

        /// <summary>
        /// Gets or sets the solution.
        /// </summary>
        /// <value>
        /// The solution.
        /// </value>
        public virtual Solution Solution { get; set; }        
    }
}
