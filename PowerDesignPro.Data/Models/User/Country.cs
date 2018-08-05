using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models.User
{
    [Table("tCountry")]
    public class Country : IEntity
    {
        public Country()
        {
            ApplicationUser = new HashSet<ApplicationUser>();
            States = new HashSet<State>();
        }

        public int ID { get; set; }

        [Required, MaxLength(255)]
        public string Description { get; set; }

        [MaxLength(10)]
        public string CountryCode { get; set; }

        public string LanguageKey { get; set; }

        public virtual ICollection<ApplicationUser> ApplicationUser { get; set; }

        public virtual ICollection<State> States { get; set; }
    }
}
