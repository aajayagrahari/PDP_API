using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class HarmonicAnalysisInputsDto
    {
        public HarmonicAnalysisInputsDto()
        {
            HarmonicAnalysisSizingSolution = new HarmonicAnalysisSizingSolution();
            HarmonicAnalysisSequenceList = new List<HarmonicAnalysisSequence>();
            HarmonicProfileList = new List<PickListDto>();
        }

        public HarmonicAnalysisSizingSolution HarmonicAnalysisSizingSolution { get; set; }

        public ICollection<HarmonicAnalysisSequence> HarmonicAnalysisSequenceList { get; set; }

        public IEnumerable<PickListDto> HarmonicProfileList { get; set; }
    }

    public class HarmonicAnalysisSequence
    {
        public int SequenceID { get; set; }

        public int SequencePriority { get; set; }

        public string SequenceDescription { get; set; }

        public decimal KVABaseErrorChecked { get; set; }

        public decimal AllContinuousKVABase { get; set; }

        public decimal AllContinuousAndMomentaryKVABase { get; set; }

        public LargestContinousWithLoadFactor LargestContinuousWithLoadFactor { get; set; }

        public PeakHarmonicWithLoadFactor PeakHarmonicWithLoadFactor { get; set; }
    }

    public class HarmonicAnalysisSizingSolution
    {
        public decimal SolutionVoltageSpecific { get; set; }

        public decimal KVABase { get; set; }

        public decimal AlternatorKVABase { get; set; }

        public decimal AlternatorSubtransientReactanceCorrected { get; set; }

        public decimal AlternatorKWDerated { get; set; }

        public int GeneratorQuantity { get; set; }

        public decimal[,] LargestContinuousHarmonicsWithLoadFactor { get; set; }

        public decimal[,] PeakHarmonicsWithLoadFactor { get; set; }
    }
}
