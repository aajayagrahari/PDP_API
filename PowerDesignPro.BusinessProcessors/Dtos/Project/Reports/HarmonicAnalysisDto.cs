using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class HarmonicAnalysisDto
    {
        public string HarmonicProfile { get; set; }

        public string Sequence { get; set; }

        public string KVANonLinearLoad { get; set; }

        public string THID { get; set; }

        public string THVD { get; set; }

        public string KVABase { get; set; }

        public string AlternatorLoading { get; set; }

        public HarmonicDistortion CurrentHarmonicDistortion { get; set; }

        public HarmonicDistortion VoltageHarmonicDistortion { get; set; }
    }

    public class HarmonicDistortion
    {
        public decimal HarmonicDistortion3 { get; set; }

        public decimal HarmonicDistortion5 { get; set; }

        public decimal HarmonicDistortion7 { get; set; }

        public decimal HarmonicDistortion9 { get; set; }

        public decimal HarmonicDistortion11 { get; set; }

        public decimal HarmonicDistortion13 { get; set; }

        public decimal HarmonicDistortion15 { get; set; }

        public decimal HarmonicDistortion17 { get; set; }

        public decimal HarmonicDistortion19 { get; set; }
    }
}
