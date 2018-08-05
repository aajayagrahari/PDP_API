using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// POCO class for Sequence Load List and Sequence Load Summary in Solution Summary page
    /// </summary>
    public class LoadSummaryLoadsDto
    {
        public LoadSummaryLoadsDto()
        {
            SequenceList = new List<PickListDto>();
            ListOfLoadSummaryLoadList = new List<LoadSummaryLoadListDto>();
        }

        public int ProjectID { get; set; }

        public int SolutionID { get; set; }

        public IEnumerable<PickListDto> SequenceList { get; set; }

        public List<LoadSummaryLoadListDto> ListOfLoadSummaryLoadList { get; set; }

        public SolutionSummaryLoadSummaryDto SolutionSummaryLoadSummary { get; set; }
    }

    /// <summary>
    /// POCO class for Load Sequence along with list of loads in the sequence
    /// </summary>
    public class LoadSummaryLoadListDto
    {
        public LoadSummaryLoadListDto()
        {
            LoadSequenceList = new List<LoadSequenceDto>();
        }

        public int SequenceID { get; set; }

        public int SequenceTypeID { get; set; }

        public string SequenceDescription { get; set; }


        public decimal LoadFactor { get; set; }

        //public string SolutionFDipHertz { get; set; }

        //public string SolutionVDipPercent { get; set; }

        public List<LoadSequenceDto> LoadSequenceList { get; set; }

        public LoadSequenceSummaryDto LoadSequenceSummary { get; set; }
    }

    public class LoadSequenceDto : BaseLoadListDto
    {
        public int LoadID { get; set; }

        //public string LoadName { get; set; }

        //public string Description { get; set; }

        public string KVAPFDescription { get; set; }

        public string HarmonicsDescription { get; set; }

        public int LoadFamilyID { get; set; }

        public int SolutionLoadID { get; set; }

        public int SequenceID { get; set; }

        public string LimitsVdip { get; set; }

        public string LimitsFdip { get; set; }

        public int LoadVoltageSpecific { get; set; }

        public decimal VoltageDip { get; set; }

        public int VoltageDipUnitsID { get; set; }

        public decimal FrequencyDip { get; set; }

        public int FrequencyDipUnitsID { get; set; }

        public bool UPSRevertToBattery { get; set; }

        public decimal HD3 { get; set; }

        public decimal HD5 { get; set; }

        public decimal HD7 { get; set; }

        public decimal HD9 { get; set; }

        public decimal HD11 { get; set; }

        public decimal HD13 { get; set; }

        public decimal HD15 { get; set; }

        public decimal HD17 { get; set; }

        public decimal HD19 { get; set; }

        public string LanguageKey { get; set; }
    }

    public class LoadSequenceSummaryDto : BaseLoadListDto
    {
        public string Sequence { get; set; }
        public string SequenceDescription { get; set; }

        public string LoadFactorDescription { get; set; }

        public string SequencePeakText { get; set; }

        public string ApplicationPeakText { get; set; }

        public decimal VDipPerc { get; set; }

        public decimal VDipVolts { get; set; }

        public decimal FDipPerc { get; set; }

        public decimal FDipHertz { get; set; }

        public int LoadCount { get; set; }

        public bool Shed { get; set; }

        public decimal PeakLoadValue { get; set; }

        public decimal ApplicationPeak { get; set; }

        public decimal KVABaseErrorChecked { get; set; }

        public LargestContinousWithLoadFactor LargestContinuousWithLoadFactor { get; set; }

        public PeakHarmonicWithLoadFactor PeakHarmonicWithLoadFactor { get; set; }

        public decimal ExpectedVoltageDip { get; set; }

        public decimal AlternatorExpectedVoltageDip { get; set; }

        public decimal? VoltsHertzMultiplier { get; set; }

        public decimal EngineExpectedVoltageDip { get; set; }

        public decimal PercAllowableVoltageDip { get; set; }

        public decimal ProjectAllowableVoltageDip { get; set; }

        public decimal ExpectedFrequencyDip { get; set; }

        public decimal PercAllowableFrequencyDip { get; set; }

        public decimal ProjectAllowableFrequencyDip { get; set; }
    }
}
