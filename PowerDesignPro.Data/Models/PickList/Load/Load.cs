using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tLoads")]
    public class Load : BasePickListEntity
    {
        public int LoadFamilyID { get; set; }

        public bool IsDefaultSelection { get; set; }

        public bool Active { get; set; }

        public virtual LoadFamily LoadFamily { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<BasicLoad> BasicLoads { get; set; }
    }
}
