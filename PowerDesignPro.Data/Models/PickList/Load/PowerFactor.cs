using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tPowerFactor")]
    public class PowerFactor : BasePickListEntity, IEntity
    {
        public PowerFactor()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            UPSLoad = new HashSet<UPSLoad>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<UPSLoad> UPSLoad { get; set; }
    }
}
