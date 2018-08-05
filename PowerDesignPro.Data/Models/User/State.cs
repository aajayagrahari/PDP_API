using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models.User
{
    [Table("tStates")]
    public class State : IEntity
    {
        public State()
        {
            ApplicationUser = new HashSet<ApplicationUser>();
        }

        public int ID { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(10)]
        public string StateCode { get; set; }

        public string LanguageKey { get; set; }

        [MaxLength(10)]
        public string CountryCode { get; set; }

        public int CountryID { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }
    }
}
