using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tLoadSequenceCyclic")]
    public class LoadSequenceCyclic : BasePickListEntity
    {
        public LoadSequenceCyclic()
        {
            SolutionSetups1 = new HashSet<SolutionSetup>();
            SolutionSetups2 = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups1 = new HashSet<UserDefaultSolutionSetup>();
            UserDefaultSolutionSetups2 = new HashSet<UserDefaultSolutionSetup>();
        }

        public virtual ICollection<SolutionSetup> SolutionSetups1 { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups2 { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups1 { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups2 { get; set; }
    }
}
