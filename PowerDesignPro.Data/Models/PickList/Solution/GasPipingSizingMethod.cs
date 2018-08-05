using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tGasPipingSizingMethod")]
    public class GasPipingSizingMethod : BasePickListEntity
    {
        public GasPipingSizingMethod()
        {
            GasPipingSolutions = new HashSet<GasPipingSolution>();
        }

        public virtual ICollection<GasPipingSolution> GasPipingSolutions { get; set; }
    }
}
