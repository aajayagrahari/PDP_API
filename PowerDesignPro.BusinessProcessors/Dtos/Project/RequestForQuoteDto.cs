using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class RequestForQuoteDto
    {
        public string Brand { get; set; }

        public int ProjectID { get; set; }

        public int SolutionID { get; set; }

        public string Language { get; set; }

        public string GeneratorDescription { get; set; }

        public string AlternatorDescription { get; set; }

        public string Comments { get; set; }

        public bool? EmailSent { get; set; }
    }
}
