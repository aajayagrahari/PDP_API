using PowerDesignPro.Data.Framework.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using PowerDesignPro.Data.Framework.Annotations;

namespace PowerDesignPro.Data.Models
{
    [Table("tUPSLoad")]
    public class UPSLoad : BaseSolutionLoadEntity
    {
        public int? PhaseID { get; set; }

        public int? EfficiencyID { get; set; }

        public int? ChargeRateID { get; set; }

        public int? PowerFactorID { get; set; }

        public bool UPSRevertToBattery { get; set; }

        public int? LoadLevelID { get; set; }

        [Precision(10, 2)]
        public decimal? SizeKVA { get; set; }

        public int? SizeKVAUnitsID { get; set; }

        public int? UPSTypeID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? HarmonicContentID { get; set; }

        public virtual Phase Phase { get; set; }

        public virtual Efficiency Efficiency { get; set; }

        public virtual ChargeRate ChargeRate { get; set; }

        public virtual PowerFactor PowerFactor { get; set; }

        public virtual LoadLevel LoadLevel { get; set; }

        public virtual UPSType UPSType { get; set; }

        public virtual SizeUnits SizeKVAUnits { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }
    }
}
