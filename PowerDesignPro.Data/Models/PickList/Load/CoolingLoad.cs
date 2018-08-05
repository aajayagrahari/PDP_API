using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tCoolingLoad")]
    public class CoolingLoad : BasePickListEntity, IEntity
    {
        public CoolingLoad()
        {
            LoadDefaults = new HashSet<LoadDefaults>();
            ACLoad = new HashSet<ACLoad>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<ACLoad> ACLoad { get; set; }
    }
}
