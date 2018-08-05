using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tSharedProjectSolution")]
    public class SharedProjectSolution : IEntity
    {
        public int ID { get; set; }

        public int SharedProjectID { get; set; }

        public int SolutionID { get; set; }

        public DateTime SharedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public bool Active { get; set; }

        public virtual SharedProject SharedProject { get; set; }

        public virtual Solution Solution { get; set; }
    }
}
