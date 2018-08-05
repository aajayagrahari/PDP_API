using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class TransientAnalysisDto
    {
        public TransientAnalysisDto()
        {
            AlternatorTransientRequirement = new AlternatorTransientRequirement();
            EngineTransientRequirement = new EngineTransientRequirement();
            AlternatorTransientAnalysisList = new List<AlternatorTransientAnalysis>();
            EngineTransientAnalysisList = new List<EngineTransientAnalysis>();
        }

        public AlternatorTransientRequirement AlternatorTransientRequirement { get; set; }

        public EngineTransientRequirement EngineTransientRequirement { get; set; }

        public ICollection<AlternatorTransientAnalysis> AlternatorTransientAnalysisList { get; set; }

        public ICollection<EngineTransientAnalysis> EngineTransientAnalysisList { get; set; }

        public bool IsVdipEngineConfiguration { get; set; }
    }

    public class AlternatorTransientRequirement
    {
        public string Sequence { get; set; }

        public string Load { get; set; }

        public decimal StartingkVA { get; set; }

        public string VdipTolerance { get; set; }

        public string VdipExpected { get; set; }
    }

    public class EngineTransientRequirement
    {
        public string Sequence { get; set; }

        public string Load { get; set; }

        public decimal StartingkW { get; set; }

        public decimal FdipTolerance { get; set; }

        public decimal FdipExpected { get; set; }
    }

    public class AlternatorTransientAnalysis
    {
        public string Sequence { get; set; }

        public string AllowableVdip { get; set; }

        public string VdipExpected { get; set; }

        public decimal SequenceStartingkVA { get; set; }

        public string LargestTransientLoad { get; set; }
    }

    public class EngineTransientAnalysis
    {
        public string Sequence { get; set; }

        public decimal AllowableFdip { get; set; }

        public decimal FdipExpected { get; set; }

        public decimal SequenceStartingkW { get; set; }

        public string LargestTransientLoad { get; set; }
    }
}
