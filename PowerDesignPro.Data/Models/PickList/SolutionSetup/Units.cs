using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tUnits")]
    public class Units : BasePickListEntity
    {
        public Units()
        {
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
            GasPipingSolutions = new HashSet<GasPipingSolution>();
            ExhaustPipingSolutions = new HashSet<ExhaustPipingSolution>();
        }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }

        public virtual ICollection<GasPipingSolution> GasPipingSolutions { get; set; }

        public virtual ICollection<ExhaustPipingSolution> ExhaustPipingSolutions { get; set; }

    }
}
