using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tLoadFamily")]
    public class LoadFamily : BasePickListEntity, IEntity
    {
        public LoadFamily()
        {
            Loads = new HashSet<Load>();
            SizeUnits = new HashSet<SizeUnits>();
        }

        public virtual ICollection<Load> Loads { get; set; }

        public virtual ICollection<SizeUnits> SizeUnits { get; set; }
    }
}
