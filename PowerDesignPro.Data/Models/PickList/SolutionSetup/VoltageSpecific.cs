using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    //[Table("tVoltageSpecific")]
    public class VoltageSpecific : BasePickListEntity
    {
        public VoltageSpecific()
        {
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
            LoadDefaults = new HashSet<LoadDefaults>();
            BasicLoad = new HashSet<BasicLoad>();
        }

        public int VoltageNominalID { get; set; }

        public bool IsDefaultSelection { get; set; }

        public virtual VoltageNominal VoltageNominal { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad { get; set; }
    }
}
