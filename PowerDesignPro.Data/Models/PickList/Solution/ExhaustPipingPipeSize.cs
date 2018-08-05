using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tExhaustPipingPipeSize")]
    public class ExhaustPipingPipeSize : BasePipeSizeEntity
    {
        public ExhaustPipingPipeSize()
        {
            ExhaustPipingSolutions = new HashSet<ExhaustPipingSolution>();
        }

        public string Description { get; set; }

        public virtual ICollection<ExhaustPipingSolution> ExhaustPipingSolutions { get; set; }
    }
}
