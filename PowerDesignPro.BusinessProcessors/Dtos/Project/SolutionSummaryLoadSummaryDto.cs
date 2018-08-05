using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// POCO class for Load Summary in Solution Summary
    /// </summary>
    public class SolutionSummaryLoadSummaryDto
    {
        public decimal RunningKW { get; set; }

        public decimal RunningKVA { get; set; }

        public decimal RunningPF { get; set; }

        public decimal StepKW { get; set; }

        public decimal PeakKW { get; set; }

        public decimal StepKVA { get; set; }

        public decimal HarmonicsKVA { get; set; }

        public decimal THIDContinuous { get; set; }

        public decimal THIDPeak { get; set; }
    }
}
