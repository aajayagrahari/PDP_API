using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    [Table("tVoltageDip")]
    public class VoltageDip : BasePickListEntity
    {
        public VoltageDip()
        {
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
            LoadDefaults = new HashSet<LoadDefaults>();
            BasicLoad = new HashSet<BasicLoad>();
            StartingMethod = new HashSet<StartingMethod>();
        }

        public int VoltageDipUnitsID { get; set; }

        public VoltageDipUnits VoltageDipUnits { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad { get; set; }

        public virtual ICollection<StartingMethod> StartingMethod { get; set; }
    }
}
