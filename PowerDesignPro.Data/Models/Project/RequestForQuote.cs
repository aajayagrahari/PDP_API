using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tRequestForQuote")]
    public class RequestForQuote : BaseEntity, IEntity
    {
        public int SolutionID { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public string Comments { get; set; }

        public bool EmailSent { get; set; }

        public virtual Solution Solution { get; set; }
    }
}
