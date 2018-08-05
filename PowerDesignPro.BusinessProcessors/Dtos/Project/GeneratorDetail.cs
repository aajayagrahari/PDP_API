using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class GeneratorDetail
    {
        public Generator Generator { get; set; }

        public int GeneratorID { get; internal set; }

        public int Quantity { get; internal set; } = 1;

        public string Description { get; internal set; }

        public int KwStandby { get; internal set; }

        public int KWPrime { get; internal set; }

        public int KWPeak { get; set; }

        public int KWNominalPrime { get; internal set; }

        public int KWNominalPeak { get; internal set; }

        public int KWNominalRated { get; set; }

        public int KWRated50 { get; internal set; }

        public decimal Derate { get; internal set; }

        public int KWApplicationPeak { get; internal set; } // Also PeakKW

        public int KWApplicationRunning { get; internal set; } // Also RunningKW

        public decimal TransientKWFDIP_1 { get; internal set; }

        public decimal TransientKWFDIP_2 { get; internal set; }

        public decimal TransientKWFDIP_3 { get; internal set; }

        public decimal TransientKWFDIP_4 { get; internal set; }

        public decimal TransientKWFDIP_5 { get; internal set; }

        public decimal TransientKWFDIP_6 { get; internal set; }

        public decimal TransientKWFDIP_7 { get; internal set; }

        public decimal TransientKWFDIP_8 { get; internal set; }

        public decimal TransientKWFDIP_9 { get; internal set; }

        public decimal TransientKWFDIP_10 { get; internal set; }

        public decimal TransientKWFDIP_11 { get; internal set; }

        public decimal TransientKWFDIP_12 { get; internal set; }

        public decimal TransientKWFDIP_13 { get; internal set; }

        public decimal TransientKWFDIP_14 { get; internal set; }

        public decimal TransientKWFDIP_15 { get; internal set; }

        public AlternatorDetail AlternatorDetail { get; internal set; }

        public decimal SKWFdip { get; internal set; }

        //public decimal FDipFromKW { get; set; }
    }
}
