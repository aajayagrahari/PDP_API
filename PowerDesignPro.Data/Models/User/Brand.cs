using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tBrand")]
    public class Brand : BasePickListEntity
    {
        public Brand()
        {
            //ApplicationUser = new HashSet<ApplicationUser>();
            ProductFamilies = new HashSet<ProductFamily>();
        }

        //public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }

        public virtual ICollection<ProductFamily> ProductFamilies { get; set; }
    }
}
