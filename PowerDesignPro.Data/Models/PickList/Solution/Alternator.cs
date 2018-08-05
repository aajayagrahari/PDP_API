using PowerDesignPro.Data.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tAlternator")]
    public class Alternator : IEntity
    {
        public Alternator()
        {
            RecommendedProducts = new HashSet<RecommendedProduct>();
            GeneratorAvailableAlternators = new HashSet<GeneratorAvailableAlternator>();
        }

        public int ID { get; set; }

        public int AlternatorFamilyID { get; set; }

        public int FrequencyID { get; set; }

        public int VoltagePhaseID { get; set; }

        public int VoltageNominalID { get; set; }

        public string ModelDescription { get; set; }

        public int KWRating { get; set; }

        public int Percent35 { get; set; }

        public int Percent25 { get; set; }

        public int Percent15 { get; set; }

        public int KVABase { get; set; }

        [Precision(5, 3)]
        public decimal SubTransientReactance { get; set; }

        [Precision(4, 2)]
        public decimal? TransientReactance { get; set; }

        [Precision(4, 2)]
        public decimal? SynchronousReactance { get; set; }

        [Precision(4, 2)]
        public decimal? X2 { get; set; }

        [Precision(4, 2)]
        public decimal? X0 { get; set; }

        [Precision(4, 2)]
        public decimal? SubtransientTime { get; set; }

        [Precision(4, 2)]
        public decimal? TransientTime { get; set; }

        [Precision(4, 2)]
        public decimal? AmbientTemperature { get; set; }

        public int SSPU { get; set; }

        public string InternalDescription { get; set; }

        public int Ordinal { get; set; }

        /// <summary>
        /// Gets or sets the created by.
        /// </summary>
        /// <value>
        /// The created by.
        /// </value>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        /// <value>
        /// The modified by.
        /// </value>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date time.
        /// </summary>
        /// <value>
        /// The modified date time.
        /// </value>
        public DateTime ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Project"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        public bool Active { get; set; }

        public virtual AlternatorFamily AlternatorFamily { get; set; }

        public virtual VoltagePhase VoltagePhase { get; set; }

        public virtual Frequency Frequency { get; set; }

        public virtual VoltageNominal VoltageNominal { get; set; }

        public virtual ICollection<RecommendedProduct> RecommendedProducts { get; set; }

        public virtual ICollection<GeneratorAvailableAlternator> GeneratorAvailableAlternators { get; set; }
    }
}
