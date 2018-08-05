using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tFrequency")]
    public class Frequency : BasePickListEntity
    {
        public Frequency()
        {
            VoltageNominals = new HashSet<VoltageNominal>();
            //VoltageSpecific = new HashSet<VoltageSpecific>();
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
            Alternators = new HashSet<Alternator>();
        }

        public virtual ICollection<VoltageNominal> VoltageNominals { get; set; }

        //public virtual ICollection<VoltageSpecific> VoltageSpecific { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual ICollection<Alternator> Alternators { get; set; }
    }
}
