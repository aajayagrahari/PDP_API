using PowerDesignPro.Data.Framework.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tHarmonicDeviceType")]
    public class HarmonicDeviceType : BasePickListEntity, IEntity
    {
        public HarmonicDeviceType()
        {
            BasicLoad = new HashSet<BasicLoad>();
            LoadDefaults = new HashSet<LoadDefaults>();
            LightingTypes = new HashSet<LightingType>();
            WelderTypes = new HashSet<WelderType>();
            UPSTypes = new HashSet<UPSType>();
        }
        public int HarmonicContentID { get; set; }

        public int StartingMethodID { get; set; }
        
        [Precision(4, 2)]
        public decimal? HD3 { get; set; }

        [Precision(4, 2)]
        public decimal? HD5 { get; set; }

        [Precision(4, 2)]
        public decimal? HD7 { get; set; }

        [Precision(4, 2)]
        public decimal? HD9 { get; set; }

        [Precision(4, 2)]
        public decimal? HD11 { get; set; }

        [Precision(4, 2)]
        public decimal? HD13 { get; set; }

        [Precision(4, 2)]
        public decimal? HD15 { get; set; }

        [Precision(4, 2)]
        public decimal? HD17 { get; set; }

        [Precision(4, 2)]
        public decimal? HD19 { get; set; }

        [Precision(5, 2)]
        public decimal? KVAMultiplier { get; set; }

        [Precision(5, 2)]
        public decimal? KWMultiplier { get; set; }

        public int LoadLimit { get; set; }

        public bool IsDefaultSelection { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool Active { get; set; }

        public int MotorLoadTypeID { get; set; }

        public virtual MotorLoadType MotorLoadType { get; set; }

        public virtual StartingMethod StartingMethod { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<LightingType> LightingTypes { get; set; }

        public virtual ICollection<WelderType> WelderTypes { get; set; }

        public virtual ICollection<UPSType> UPSTypes { get; set; }

    }
}
