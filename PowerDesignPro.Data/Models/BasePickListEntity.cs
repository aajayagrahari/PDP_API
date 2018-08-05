using System.ComponentModel.DataAnnotations;

namespace PowerDesignPro.Data.Models
{
    /// <summary>
    /// Base Entity for all the dropdown Entities
    /// </summary>
    /// <seealso cref="PowerDesignPro.Data.Models.IEntity" />
    public class BasePickListEntity : IEntity
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
        [Required, MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Required, MaxLength(100)]
        public string Value { get; set; }

        [Required]
        public int Ordinal { get; set; }


        
        public string LanguageKey { get; set; }

        //public bool Selected { get; set; }
    }
}
