using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class GasPipingReportDto
    {
        public decimal PressureDrop { get; set; }

        public decimal AvailablePressure { get; set; }

        public decimal AllowablePercentage { get; set; }

        public int UnitID { get; set; }

        public int SolutionID { get; set; }

        public decimal SupplyGasPressure { get; set; }

        public decimal LengthOfRun { get; set; }

        public decimal GeneratorMinPressure { get; set; } = 15;

        public decimal NumberOf90Elbows { get; set; }

        public decimal NumberOf45Elbows { get; set; }

        public decimal NumberOfTees { get; set; }

        public int SizingMethodID { get; set; }

        public int PipeSizeID { get; set; }

        public bool SingleUnit { get; set; }

        public decimal PipeSize { get; set; }

        public string SizingMethod { get; set; }

        public string FuelType { get; set; }

        public string ModelDescription { get; set; }

        public string ProductFamilyDesc { get; set; }

        public decimal FuelConsumption { get; set; }

        public string PressureUnitText { get; set; }

        public string LengthOfRunUnitText { get; set; }

        public string PipeSizeUnitText { get; set; }
    }
}
