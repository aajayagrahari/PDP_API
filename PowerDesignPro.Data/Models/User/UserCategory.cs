using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models.User
{
    [Table("tUserCategory")]
    public class UserCategory : BasePickListEntity
    {
        public UserCategory()
        {
            ApplicationUser = new HashSet<ApplicationUser>();
        }

        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }

    }
}
