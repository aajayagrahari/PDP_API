using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tParallelQuantity")]
    public class ParallelQuantity : BasePickListEntity, IEntity
    {
        public ParallelQuantity()
        {
            RecommendedProduct = new HashSet<RecommendedProduct>();
        }

        public virtual ICollection<RecommendedProduct> RecommendedProduct { get; set; }
    }
}
