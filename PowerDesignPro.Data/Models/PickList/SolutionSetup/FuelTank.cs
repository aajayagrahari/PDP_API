using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    [Table("tFuelTank")]
    public class FuelTank : BasePickListEntity
    {
        public FuelTank()
        {
            SolutionSetups = new HashSet<SolutionSetup>();
            UserDefaultSolutionSetups = new HashSet<UserDefaultSolutionSetup>();
        }

        public bool IsDefaultSelection { get; set; }

        public int? FuelTypeID { get; set; }

        public virtual FuelType FuelType { get; set; }

        public virtual ICollection<SolutionSetup> SolutionSetups { get; set; }

        public virtual ICollection<UserDefaultSolutionSetup> UserDefaultSolutionSetups { get; set; }
    }
}
