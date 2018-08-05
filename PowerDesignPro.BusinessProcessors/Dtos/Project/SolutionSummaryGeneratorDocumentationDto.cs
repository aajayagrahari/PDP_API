using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SolutionSummaryGeneratorDocumentationDto : PickListDto
    {
        public int GeneratorID { get; set; }

        public string DocumentURL { get; set; }
    }
}
