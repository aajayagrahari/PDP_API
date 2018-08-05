using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tLoadPF")]
    public class PF : BasePickListEntity, IEntity
    {
        public PF()
        {
            LoadDefaults1 = new HashSet<LoadDefaults>();
            LoadDefaults2 = new HashSet<LoadDefaults>();

            BasicLoad1 = new HashSet<BasicLoad>();
            BasicLoad2 = new HashSet<BasicLoad>();

            LightingTypes = new HashSet<LightingType>();
        }

        public virtual ICollection<LoadDefaults> LoadDefaults1 { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults2 { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad1 { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad2 { get; set; }

        public virtual ICollection<LightingType> LightingTypes { get; set; }
    }
}
