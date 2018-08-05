using PowerDesignPro.Data.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tVoltagePhase")]
    public class VoltagePhase : BasePickListEntity, IEntity
    {
        public VoltagePhase()
        {
            VoltageNominals = new HashSet<VoltageNominal>();
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
            LoadDefaults = new HashSet<LoadDefaults>();
            BasicLoad = new HashSet<BasicLoad>();
            Alternators = new HashSet<Alternator>();
        }

        public virtual ICollection<VoltageNominal> VoltageNominals { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad { get; set; }

        public virtual ICollection<Alternator> Alternators { get; set; }
    }
}
