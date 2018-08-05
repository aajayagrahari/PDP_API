using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Data.Models
{
    public class VoltageNominal : BaseVoltageEntity
    {
        public VoltageNominal()
        {
            VoltageSpecifics = new HashSet<VoltageSpecific>();
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
            LoadDefaults = new HashSet<LoadDefaults>();
            BasicLoad = new HashSet<BasicLoad>();
            Alternators = new HashSet<Alternator>();
            GeneratorAvailableVoltages = new HashSet<GeneratorAvailableVoltage>();
        }

        public virtual ICollection<VoltageSpecific> VoltageSpecifics { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual ICollection<LoadDefaults> LoadDefaults { get; set; }

        public virtual ICollection<BasicLoad> BasicLoad { get; set; }

        public virtual ICollection<Alternator> Alternators { get; set; }

        public virtual ICollection<GeneratorAvailableVoltage> GeneratorAvailableVoltages { get; set; }
    }
}
