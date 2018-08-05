using PowerDesignPro.BusinessProcessors.Dtos.PickList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos.Project
{
    public class ExhaustPipingDto
    {
        public ExhaustPipingDto()
        {
            SizingMethodList = new List<PickListDto>();
            PipeSizeList = new List<ExhaustPipingPipeSizeDto>();
            ExhaustSystemConfigurationList = new List<PickListDto>();
        }

        public ExhaustPipingGeneratorSummaryDto ExhaustPipingGeneratorSummary { get; set; }

        public ExhaustPipingInputDto ExhaustPipingInput { get; set; }

        public ExhaustPipingSolutionDto ExhaustPipingSolution { get; set; }

        public int ID { get; set; }

        public int SizingMethodID { get; set; }

        public int PipeSizeID { get; set; }

        public int ExhaustSystemConfigurationID { get; set; }

        public IEnumerable<PickListDto> SizingMethodList { get; set; }

        public IEnumerable<PickListDto> PipeSizeList { get; set; }

        public IEnumerable<PickListDto> ExhaustSystemConfigurationList { get; set; }

        public UnitMeasureDto UnitMeasure { get; set; }

        public int UnitSelected { get; set; }
    }

    public class ExhaustPipingGeneratorSummaryDto
    {
        public string ProductFamily { get; set; }

        public string Generator { get; set; }

        public decimal TotalExhaustFlow { get; set; }

        public decimal MaximumBackPressure { get; set; }

        public decimal KWStandby { get; set; }

        public int? NG_CF_HR { get; set; }

        public int KWPrime { get; set; }

        public int EngineDuty { get; set; }

        public int Quantity { get; set; }

        public int ExhaustTempF { get; set; }

        public decimal ExhaustFlex { get; set; }
    }

    public class ExhaustPipingInputDto
    {
        public decimal LengthOfRun { get; set; } = 1;

        public decimal NumberOfStandardElbows { get; set; } = 0;

        public decimal NumberOfLongElbows { get; set; } = 0;

        public decimal NumberOf45Elbows { get; set; } = 0;
    }

    public class ExhaustPipingSolutionDto
    {
        public decimal PressureDrop { get; set; }
    }
}
