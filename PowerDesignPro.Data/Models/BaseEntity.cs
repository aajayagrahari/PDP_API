namespace PowerDesignPro.Data.Models
{
    /// <summary>
    /// Base Entity for all the Entities
    /// </summary>
    /// <seealso cref="PowerDesignPro.Data.Models.IEntity" />
    public class BaseEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID { get; set; }
    }
}
