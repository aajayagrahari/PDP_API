using PowerDesignPro.Data.Framework.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tWelderLoad")]
    public class WelderLoad : BaseSolutionLoadEntity
    {
        [Precision(10,2)]
        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        public int? RunningPFID { get; set; }

        public int? WelderTypeID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? HarmonicContentID { get; set; }

        #region NavigationProperties

        public virtual SizeUnits SizeRunningUnits { get; set; }

        public virtual PF RunningPF { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }

        public virtual WelderType WelderType { get; set; }

        #endregion
    }
}
