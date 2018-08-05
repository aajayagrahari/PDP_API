using PowerDesignPro.Data.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tProductFamily")]
    public class ProductFamily : BasePickListEntity, IEntity
    {
        public ProductFamily()
        {
            Generators = new HashSet<Generator>();
            RecommendedProduct = new HashSet<RecommendedProduct>();
        }

        public bool IsDomestic { get; set; }

        //public bool IsMPS { get; set; }

        public bool IsGemini { get; set; }

        public int Priority { get; set; }

        public int BrandID { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool Active { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual ICollection<Generator> Generators { get; set; }

        public virtual ICollection<RecommendedProduct> RecommendedProduct { get; set; }
    }
}
