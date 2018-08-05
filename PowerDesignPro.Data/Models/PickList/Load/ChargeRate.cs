using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tChargeRate")]
    public class ChargeRate : BasePickListEntity, IEntity
    {
        public ChargeRate()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            UPSLoad = new HashSet<UPSLoad>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<UPSLoad> UPSLoad { get; set; }
    }
}
