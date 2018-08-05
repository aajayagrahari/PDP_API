using PowerDesignPro.Data.Framework.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using PowerDesignPro.Data.Framework.Annotations;

namespace PowerDesignPro.Data.Models
{
    [Table("tMotorLoad")]
    public class MotorLoad : BaseSolutionLoadEntity
    {
        [Precision(10, 2)]
        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        public int? MotorLoadLevelID { get; set; }

        public int? MotorLoadTypeID { get; set; }

        public int? MotorTypeID { get; set; }

        public int? StartingCodeID { get; set; }

        public int? StartingMethodID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? ConfigurationInputID { get; set; }

        public int? HarmonicContentID { get; set; }

        public virtual SizeUnits SizeRunningUnits { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }

        public virtual MotorLoadLevel MotorLoadLevel { get; set; }

        public virtual MotorLoadType MotorLoadType { get; set; }

        public virtual MotorType MotorType { get; set; }

        public virtual StartingCode StartingCode { get; set; }

        public virtual StartingMethod StartingMethod { get; set; }

        public virtual ConfigurationInput ConfigurationInput { get; set; }
    }
}
