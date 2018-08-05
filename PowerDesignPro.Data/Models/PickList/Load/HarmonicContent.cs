using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tHarmonicContent")]
    public class HarmonicContent : BasePickListEntity
    {
        public HarmonicContent()
        {
            HarmonicTypes = new HashSet<HarmonicDeviceType>();
            BasicLoad = new HashSet<BasicLoad>();
            LightingTypes = new HashSet<LightingType>();
            UPSTypes = new HashSet<UPSType>();
        }

        public int StartingMethodID { get; set; }

        public virtual ICollection<HarmonicDeviceType> HarmonicTypes { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad { get; set; }

        public virtual StartingMethod StartingMethod { get; set; }

        public virtual ICollection<LightingType> LightingTypes { get; set; }

        public virtual ICollection<UPSType> UPSTypes { get; set; }
    }
}
