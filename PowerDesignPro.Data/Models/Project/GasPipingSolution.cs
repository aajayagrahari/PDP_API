using PowerDesignPro.Data.Framework.Annotations;

namespace PowerDesignPro.Data.Models
{
    public class GasPipingSolution : BaseEntity, IEntity
    {
        public int SolutionID { get; set; }

        public decimal PressureDrop { get; set; }

        public decimal AvailablePressure { get; set; }

        [Precision(18,4)]
        public decimal AllowablePercentage { get; set; }

        public decimal SupplyGasPressure { get; set; }

        public decimal LengthOfRun { get; set; }

        public decimal GeneratorMinPressure { get; set; }

        public decimal NumberOf90Elbows { get; set; }

        public decimal NumberOf45Elbows { get; set; }

        public decimal NumberOfTees { get; set; }

        public int SizingMethodID { get; set; }

        public int PipeSizeID { get; set; }

        public int UnitID { get; set; }

        public bool SingleUnit { get; set; }

        public decimal FuelConsumption { get; set; }

        public virtual GasPipingSizingMethod GasPipingSizingMethod { get; set; }

        public virtual GasPipingPipeSize GasPipingPipeSize { get; set; }

        public virtual Units Units { get; set; }

        public virtual Solution Solution { get; set; }
    }
}
