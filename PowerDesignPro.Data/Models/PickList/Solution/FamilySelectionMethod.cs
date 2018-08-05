using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tFamilySelectionMethod")]
    public class FamilySelectionMethod : BasePickListEntity, IEntity
    {
        public FamilySelectionMethod()
        {
            RecommendedProduct = new HashSet<RecommendedProduct>();
        }

        public virtual ICollection<RecommendedProduct> RecommendedProduct { get; set; }
    }
}
