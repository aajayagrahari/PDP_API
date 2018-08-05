using System.ComponentModel.DataAnnotations.Schema;

namespace PowerDesignPro.Data.Models
{
    //[Table("tExhaustPipingSolution")]
    public class ExhaustPipingSolution : BaseEntity, IEntity
    {
        public int SolutionID { get; set; }

        public int SizingMethodID { get; set; }

        public int PipeSizeID { get; set; }

        public int ExhaustSystemConfigurationID { get; set; }

        public decimal LengthOfRun { get; set; }

        public decimal NumberOfStandardElbows { get; set; }

        public decimal NumberOfLongElbows { get; set; }

        public decimal NumberOf45Elbows { get; set; }

        public decimal PressureDrop { get; set; }

        public int UnitID { get; set; }

        public decimal MaximumBackPressure { get; set; }

        public decimal TotalExhaustFlow { get; set; }

        public virtual Units Units { get; set; }

        public virtual Solution Solution { get; set; }

        public virtual SizingMethod SizingMethod { get; set; }

        public virtual ExhaustPipingPipeSize ExhaustPipingPipeSize { get; set; }

        public virtual ExhaustSystemConfiguration ExhaustSystemConfiguration { get; set; }
    }
}
