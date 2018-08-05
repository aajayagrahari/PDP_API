using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tSizingMethod")]
    public class SizingMethod : BasePickListEntity, IEntity
    {
        public SizingMethod()
        {
            RecommendedProduct = new HashSet<RecommendedProduct>();
            ExhaustPipingSolutions = new HashSet<ExhaustPipingSolution>();
        }

        public virtual ICollection<RecommendedProduct> RecommendedProduct { get; set; }

        public virtual ICollection<ExhaustPipingSolution> ExhaustPipingSolutions { get; set; }
    }
}
