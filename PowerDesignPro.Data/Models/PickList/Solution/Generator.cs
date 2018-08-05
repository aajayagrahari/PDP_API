using PowerDesignPro.Data.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tGenerator")]
    public class Generator : IEntity
    {
        public Generator()
        {
            Documents = new HashSet<Documentation>();
            RecommendedProducts = new HashSet<RecommendedProduct>();
            GeneratorAvailableVoltages = new HashSet<GeneratorAvailableVoltage>();
            GeneratorAvailableAlternators = new HashSet<GeneratorAvailableAlternator>();
            GeneratorRegulatoryFilters = new HashSet<GeneratorRegulatoryFilter>();
        }

        public int ID { get; set; }

        public int ProductFamilyID { get; set; }

        [StringLength(100)]
        public string ModelDescription { get; set; }

        [Precision(4,1)]
        public decimal Liters { get; set; }

        public int KwStandby { get; set; }

        public int KWPrime { get; set; }

        public int KWPeak { get; set; }

        public bool PrimePowerAvailable { get; set; }

        public int FrequencyID { get; set; }

        [Precision(4,1)]
        public decimal FDip50 { get; set; }

        [Precision(4, 1)]
        public decimal FDip100 { get; set; }

        public int? AltitudeDeratePoint { get; set; }

        [Precision(4, 3)]
        public decimal AltitudeDeratePercent { get; set; }

        public int AmbientDeratePoint { get; set; }

        [Precision(4, 3)]
        public decimal AmbientDeratePercent { get; set; }

        [Required]
        [StringLength(20)]
        public string AvailableFuelCode { get; set; }

        public bool PMGConfigurable { get; set; }

        [Precision(5, 2)]
        public decimal SoundOpen { get; set; }

        [Precision(5, 2)]
        public decimal SoundWeather { get; set; }

        [Precision(5, 2)]
        public decimal SoundLevel1 { get; set; }

        [Precision(5, 2)]
        public decimal SoundLevel2 { get; set; }

        [Precision(5, 2)]
        public decimal SoundLevel3 { get; set; }

        public int? NG_CF_HR { get; set; }

        public int? NG_h20 { get; set; }

        public int ExhaustCFM { get; set; }

        public int ExhaustTempF { get; set; }

        [Precision(4, 2)]
        public decimal ExhaustHg { get; set; }

        [Precision(4, 1)]
        public decimal FrameFootprintLengthIn { get; set; }

        [Precision(4, 1)]
        public decimal FrameFootprintWidthIn { get; set; }

        [Precision(4, 1)]
        public decimal WeatherTotalLengthIn { get; set; }

        [Precision(4, 1)]
        public decimal SoundL2TotalLengthIn { get; set; }

        public bool LPFuelCheck { get; set; }

        [Precision(4, 1)]
        public decimal SoundL1TotalLengthIn { get; set; }

        public bool IsGemini { get; set; }

        public bool IsParallelable { get; set; }

        [StringLength(100)]
        public string InternalDescription { get; set; }

        public int WeatherHoods { get; set; }

        public int SoundL1Hoods { get; set; }

        public int SoundL2Hoods { get; set; }

        public int PadDepth { get; set; }

        [Precision(3, 1)]
        public decimal ExhaustFlex { get; set; }

        public bool ExhaustDual { get; set; }

        public decimal? VoltsHertzMultiplier { get; set; }

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

        public virtual ProductFamily ProductFamily { get; set; }

        public virtual Frequency Frequency { get; set; }

        public virtual ICollection<Documentation> Documents { get; set; }

        public virtual ICollection<RecommendedProduct> RecommendedProducts { get; set; }

        public virtual ICollection<GeneratorAvailableVoltage> GeneratorAvailableVoltages { get; set; }

        public virtual ICollection<GeneratorAvailableAlternator> GeneratorAvailableAlternators { get; set; }

        public virtual ICollection<GeneratorRegulatoryFilter> GeneratorRegulatoryFilters { get; set; }
    }
}
