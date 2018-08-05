using PowerDesignPro.Data.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tVoltageDipUnits")]
    public class VoltageDipUnits : BasePickListEntity, IEntity
    {
        public VoltageDipUnits()
        {
            VoltageDips = new HashSet<VoltageDip>();
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
            LoadDefaults = new HashSet<LoadDefaults>();
            BasicLoad = new HashSet<BasicLoad>();
        }

        public virtual ICollection<VoltageDip> VoltageDips { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad { get; set; }
    }
}
