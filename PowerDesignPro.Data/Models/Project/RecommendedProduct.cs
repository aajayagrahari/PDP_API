using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tRecommendedProduct")]
    public class RecommendedProduct : IEntity
    {
        public int ID { get; set; }

        public int SolutionID { get; set; }

        public int FamilySelectionMethodID { get; set; }

        public int? ProductFamilyID { get; set; }

        public int SizingMethodID { get; set; }

        public int? GeneratorID { get; set; }

        public int? ParallelQuantityID { get; set; }

        public int? AlternatorID { get; set; }

        public virtual Solution Solution { get; set; }

        public virtual FamilySelectionMethod FamilySelectionMethod { get; set; }

        public virtual ProductFamily ProductFamily { get; set; }

        public virtual ParallelQuantity ParallelQuantity { get; set; }

        public virtual SizingMethod SizingMethod { get; set; }

        public virtual Generator Generator { get; set; }

        public virtual Alternator Alternator { get; set; }
    }
}
