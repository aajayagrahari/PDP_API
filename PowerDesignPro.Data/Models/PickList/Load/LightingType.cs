using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tLightingType")]
    public class LightingType : BasePickListEntity, IEntity
    {
        public LightingType()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            LightingLoad = new HashSet<LightingLoad>();
        }

        public int RunningPFID { get; set; }

        public int HarmonicDeviceTypeID { get; set; }

        public int HarmonicContentID { get; set; }

        public virtual PF RunningPF { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual HarmonicContent HarmonicContent { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<LightingLoad> LightingLoad { get; set; }
    }
}
