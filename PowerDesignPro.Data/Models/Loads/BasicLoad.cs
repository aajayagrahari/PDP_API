using PowerDesignPro.Data.Framework.Annotations;

namespace PowerDesignPro.Data.Models
{
    public class BasicLoad : BaseSolutionLoadEntity
    {
        [Precision(10, 2)]
        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        [Precision(10, 2)]
        public decimal? SizeStarting { get; set; }

        public int? SizeStartingUnitsID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? HarmonicContentID { get; set; }

        public int? RunningPFID { get; set; }

        public int? StartingPFID { get; set; }

        public bool? LockVoltageDip { get; set; }

        #region NavigationProperties

        public virtual SizeUnits SizeRunningUnits { get; set; }

        public virtual SizeUnits SizeStartingUnits { get; set; }

        public virtual PF RunningPF { get; set; }

        public virtual PF StartingPF { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }

        #endregion
    }
}
