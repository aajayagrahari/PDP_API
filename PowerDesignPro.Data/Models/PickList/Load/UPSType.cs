using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tUPSType")]
    public class UPSType : BasePickListEntity, IEntity
    {
        public UPSType()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            UPSLoad = new HashSet<UPSLoad>();
        }

        public int HarmonicDeviceTypeID { get; set; }

        public int HarmonicContentID { get; set; }

        public int PhaseID { get; set; }

        public int EfficiencyID { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<UPSLoad> UPSLoad { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }

        public virtual Phase Phase { get; set; }

        public virtual Efficiency Efficiency { get; set; }
    }
}
