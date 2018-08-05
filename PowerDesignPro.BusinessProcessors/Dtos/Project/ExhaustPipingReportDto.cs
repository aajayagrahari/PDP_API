using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class ExhaustPipingReportDto
    {
        public decimal PressureDrop { get; set; }

        public int SolutionID { get; set; }

        public int SizingMethodID { get; set; }

        public int PipeSizeID { get; set; }

        public int  ExhaustSystemConfigurationID { get; set; }

        public decimal LengthOfRun { get; set; }

        public decimal NumberOfStandardElbows { get; set; }

        public decimal NumberOfLongElbows { get; set; }

        public decimal NumberOf45Elbows { get; set; }

        public decimal PipeSize { get; set; }

        public string SizingMethod { get; set; }

        public string ExhaustEngineConfiguration { get; set; }

        public string ExhaustSystemConfiguration { get; set; }

        public string ModelDescription { get; set; }

        public string ProductFamilyDesc { get; set; }

        public decimal TotalExhaustFlow { get; set; }

        public decimal MaximumBackPressure { get; set; }

        public string PressureUnitText { get; set; }

        public string LengthOfRunUnitText { get; set; }

        public string PipeSizeUnitText { get; set; }
    }
}
