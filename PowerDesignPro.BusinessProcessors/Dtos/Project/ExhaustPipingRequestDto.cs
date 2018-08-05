using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos.Project
{
    public class ExhaustPipingRequestDto
    {
        public int ID { get; set; }

        public int SolutionID { get; set; }

        public int SizingMethodID { get; set; }

        public int PipeSizeID { get; set; }

        public int ExhaustSystemConfigurationID { get; set; }

        public int UnitID { get; set; }

        public ExhaustPipingInputDto ExhaustPipingInput { get; set; }

        public ExhaustPipingSolutionDto ExhaustPipingSolution { get; set; }

        public ExhaustPipingGeneratorSummaryDto ExhaustPipingGeneratorSummary { get; set; }
    }
}
