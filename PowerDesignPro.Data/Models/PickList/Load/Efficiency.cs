using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tEfficiency")]
    public class Efficiency : BasePickListEntity, IEntity
    {
        public Efficiency()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            UPSLoad = new HashSet<UPSLoad>();
            UPSTypes = new HashSet<UPSType>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<UPSLoad> UPSLoad { get; set; }

        public virtual ICollection<UPSType> UPSTypes { get; set; }
    }
}
