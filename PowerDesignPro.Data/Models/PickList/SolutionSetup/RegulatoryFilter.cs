using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tRegulatoryFilter")]
    public class RegulatoryFilter : BasePickListEntity
    {
        public RegulatoryFilter()
        {
            GeneratorRegulatoryFilters = new HashSet<GeneratorRegulatoryFilter>();
        }

        public virtual ICollection<GeneratorRegulatoryFilter> GeneratorRegulatoryFilters { get; set; }
    }
}
