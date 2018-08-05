using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tSharedProjects")]
    public class SharedProject : IEntity
    {
        public SharedProject()
        {
            SharedProjectSolution = new HashSet<SharedProjectSolution>();
        }
        public int ID { get; set; }

        public int ProjectID { get; set; }

        public string RecipientEmail { get; set; }

        public string Notes { get; set; }

        public DateTime SharedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool Active { get; set; }

        public virtual Project Project { get; set; }

        public virtual ICollection<SharedProjectSolution> SharedProjectSolution { get; set; }
    }
}
