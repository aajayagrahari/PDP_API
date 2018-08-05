using PowerDesignPro.BusinessProcessors.Dtos.PickList;
using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Dtos.Project
{
    public class GasPipingDto
    {
        public GasPipingDto()
        {
            SizingMethodList = new List<PickListDto>();
            PipeSizeList = new List<GasPipingPipeSizeDto>();
        }

        public bool IsGasesousSolution { get; set; }

        public GeneratorSummaryDto GeneratorSummary { get; set; }

        public GasPipingInputDto GasPipingInput { get; set; }

        public GasPipingSolutionDto GasPipingSolution { get; set; }

        public int ID { get; set; }

        public int SizingMethodID { get; set; }

        public int PipeSizeID { get; set; }

        public IEnumerable<PickListDto> SizingMethodList { get; set; }

        public IEnumerable<GasPipingPipeSizeDto> PipeSizeList { get; set; }

        public UnitMeasureDto UnitMeasure { get; set; }

        public FuelConfigDto FuelConfig { get; set; }

        public int TempRank { get; set; }

        public int UnitSelected { get; set; }

        public bool SingleUnit { get; set; }

        public string Error { get; set; }
    }

    public class GeneratorSummaryDto
    {
        public string ProductFamily { get; set; }

        public string Generator { get; set; }

        public string FuelType { get; set; }

        public decimal FuelConsumption { get; set; }

        public decimal KWStandby { get; set; }

        public int? NG_CF_HR { get; set; }

        public int KWPrime { get; set; }

        public int EngineDuty { get; set; }

        public int Quantity { get; set; }

        public double GasFuelFlow { get; set; }
    }

    public class GasPipingInputDto
    {
        public decimal SupplyGasPressure { get; set; }

        public decimal LengthOfRun { get; set; }

        public decimal GeneratorMinPressure { get; set; }

        public decimal NumberOf90Elbows { get; set; }

        public decimal NumberOf45Elbows { get; set; }

        public decimal NumberOfTees { get; set; }
    }

    public class GasPipingSolutionDto
    {
        public decimal PressureDrop { get; set; }

        public decimal AvailablePressure { get; set; }

        public decimal AllowablePercentage { get; set; }

        public bool SolutionFound { get; set; }
    }
}
