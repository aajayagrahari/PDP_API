using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tFuelType")]
    public class FuelType : BasePickListEntity
    {
        public FuelType()
        {
            SolutionSetups = new HashSet<SolutionSetup>();
            FuelTanks = new HashSet<FuelTank>();
            DesiredRunTimes = new HashSet<DesiredRunTime>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
        }

        [Required, MaxLength(10)]
        public string Code { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<FuelTank> FuelTanks { get; set; }

        public virtual ICollection<DesiredRunTime> DesiredRunTimes { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }
    }
}
