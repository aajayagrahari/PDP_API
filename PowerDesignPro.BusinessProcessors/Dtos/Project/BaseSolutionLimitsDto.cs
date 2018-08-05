using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Base POCO class for Solution Limits in Solution Summary screen
    /// </summary>
    public class BaseSolutionLimitsDto
    {
        public decimal FDip { get; set; }

        public decimal VDip { get; set; }

        public string THVDContinuous { get; set; }

        public string THVDPeak { get; set; }
    }
}
