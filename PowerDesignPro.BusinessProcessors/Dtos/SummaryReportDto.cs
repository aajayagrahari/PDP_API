using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SummaryReportDto
    {
        public class PDPView
        {
           
            public Project Project { get; set; }
            public string ProjectName { get; set; }
            public Solution Solution { get; set; }
            public GensetLoadSummary GensetLoadSummary { get; set; }
            public TransientAnalysis TransientAnalysis { get; set; }
            public List<HarmonicAnalysis> HarmonicAnalysisList { get; set; }
            public GasPiping GasPiping { get; set; }
            public ExhaustPiping ExhaustPiping { get; set; }
        }
        public enum LoadItemType : int { Group = 1, Step = 2 };
        public class Project
        {
            public string ProjectSummaryLabel { get; set; }
            public string ContactInformationLabel { get; set; }
            public string ProjectName { get; set; }
            public string ProjectNameLabel { get; set; }
            public string ContactLabel { get; set; }
            public string Contact { get; set; }
            public string SpecRefLabel { get; set; }
            public string SpecRef { get; set; }
            public string EmailLabel { get; set; }
            public string Email { get; set; }
            public string PreparedByLabel { get; set; }
            public string PreparedByNameLabel { get; set; }
            public string PreparedByName { get; set; }
            public string PreparedByCompanyLabel { get; set; }
            public string PreparedByCompany { get; set; }
            public string PreparedByPhoneLabel { get; set; }
            public string PreparedByPhone { get; set; }
            public string PreparedByEmailLabel { get; set; }
            public string PreparedByEmail { get; set; }
        }

        #region ## Solution Sections ##
        public class Solution
        {
            public string SetupLabel { get; set; }
            public string NameLabel { get; set; }
            public string Name { get; set; }
            public string SpecRefLabel { get; set; }
            public string SpecRef { get; set; }
            public string DescriptionLabel { get; set; }
            public string Description { get; set; }

            public Environment Environment { get; set; }
            public LoadSeqConf LoadSeqConf { get; set; }
            public ElectricalConf ElectricalConf { get; set; }
            public Units Units { get; set; }
            public MaxAllowableTransients MaxAllowableTransients { get; set; }
            public MaxAllowableVoltageDistortion MaxAllowableVoltageDistortion { get; set; }
            public Engine Engine { get; set; }
            public RegulatoryInformation RegulatoryInformation { get; set; }
            public GeneratorConfiguration GeneratorConfiguration { get; set; }
        }
        public class Environment
        {
            public string TitleLabel { get; set; }
            public string AmbientTempLabel { get; set; }
            public string AmbientTemp { get; set; }
            public string ElevationLabel { get; set; }
            public string Elevation { get; set; }
        }

        public class LoadSeqConf
        {
            public string TitleLabel { get; set; }
            public string Cyclic1Label { get; set; }
            public string Cyclic1 { get; set; }
            public string Cyclic2Label { get; set; }
            public string Cyclic2 { get; set; }
        }

        public class ElectricalConf
        {
            public string TitleLabel { get; set; }
            public string PhaseLabel { get; set; }
            public string Phase { get; set; }
            public string FrequencyLabel { get; set; }
            public string Frequency { get; set; }
            public string VoltageNorminalLabel { get; set; }
            public string VoltageNorminal { get; set; }
            public string VoltageSpecificLabel { get; set; }
            public string VoltageSpecific { get; set; }
        }

        public class Units
        {
            public string TitleLabel { get; set; }
            public string UnitsLabel { get; set; }
            public string UnitsValue { get; set; }
        }
        public class MaxAllowableTransients
        {
            public string TitleLabel { get; set; }
            public string MaximumRunningLoadLabel { get; set; }
            public string MaximumRunningLoad { get; set; }
            public string VoltageDipLabel { get; set; }
            public string VoltageDip { get; set; }
            public string FrequencyDipLabel { get; set; }
            public string FrequencyDip { get; set; }
        }
        public class MaxAllowableVoltageDistortion
        {
            public string TitleLabel { get; set; }
            public string ContinuousLabel { get; set; }
            public string Continuous { get; set; }
            public string MomentaryLabel { get; set; }
            public string Momentary { get; set; }
        }

        public class Engine
        {
            public string TitleLabel { get; set; }
            public string DutyLabel { get; set; }
            public string Duty { get; set; }
            public string FuelLabel { get; set; }
            public string Fuel { get; set; }
        }
        public class RegulatoryInformation
        {
            public string TitleLabel { get; set; }
            public string RegulatoryFiltersLabel { get; set; }
            public string RegulatoryFilters { get; set; }
            public string ApplicationLabel { get; set; }
            public string Application { get; set; }
        }
        public class GeneratorConfiguration
        {
            public string TitleLabel { get; set; }
            public string SoundLabel { get; set; }
            public string Sound { get; set; }
            public string FuelTankLabel { get; set; }
            public string FuelTank { get; set; }
            public string RunTimeLabel { get; set; }
            public string RunTime { get; set; }
        }
        #endregion

        #region ## GensetLoadSummary ##
        public class GensetLoadSummary
        {
            public string TitleLabel { get; set; }
            public SelectedGeneratorAlternator SelectedGeneratorAlternator { get; set; }
            public LoadSummary LoadSummary { get; set; }
            public GensetAlternator GensetAlternator { get; set; }
            public LoadList LoadList { get; set; }

        }

        public class SelectedGeneratorAlternator
        {
            public string TitleLabel { get; set; }
            public string ProductFamilyMethodLabel { get; set; }
            public string ProductFamilyMethod { get; set; }
            public string ProductFamilyLabel { get; set; }
            public string ProductFamily { get; set; }
            public string ModuleSizeLabel { get; set; }
            public string ModuleSize { get; set; }
            public string SizingMethodLabel { get; set; }
            public string SizingMethod { get; set; }
            public string GeneratorLabel { get; set; }
            public string Generator { get; set; }
            public string AlternatorLabel { get; set; }
            public string Alternator { get; set; }
        }

        public class LoadSummary
        {
            public string TitleLabel { get; set; }
            public string RunningLabel { get; set; }
            public string TransientsLabel { get; set; }
            public string HarmonicsLabel { get; set; }

            public string RunningKWLabel { get; set; }
            public double RunningKW { get; set; }
            public string RunningkVALabel { get; set; }
            public double RunningkVA { get; set; }
            public string RunningPFLabel { get; set; }
            public double RunningPF { get; set; }

            public string TransientsLkWStepLabel { get; set; }
            public double TransientsLkWStep { get; set; }
            public string TransientsLkWPeakLabel { get; set; }
            public double TransientsLkWPeak { get; set; }
            public string TransientsLkVAStepLabel { get; set; }
            public double TransientsLkVAStep { get; set; }

            public string HarmonicskVALabel { get; set; }
            public double HarmonicskVA { get; set; }
            public string HarmonicsTHIDContLabel { get; set; }
            public double HarmonicsTHIDCont { get; set; }
            public string HarmonicsTHIDPeakLabel { get; set; }
            public double HarmonicsTHIDPeak { get; set; }

        }

        public class GensetAlternator
        {
            public string TitleLabel1 { get; set; }
            public string TitleLabel2 { get; set; }
            public string LoadLevelLabel { get; set; }
            public string TransientsLabel { get; set; }
            public string HarmonicsLabel { get; set; }
            public string LoadLevelRunningLabel { get; set; }
            public double LoadLevelRunning { get; set; }
            public string LoadLevelPeakLabel { get; set; }
            public double LoadLevelPeak { get; set; }

            public string TransientsFdipLabel { get; set; }
            public double TransientsFdip { get; set; }
            public string TransientsVdipLabel { get; set; }
            public double TransientsVdip { get; set; }

            public string HarmonicsTHVDContLabel { get; set; }
            public string HarmonicsTHVDCont { get; set; }
            public string HarmonicsTHVDPeakLabel { get; set; }
            public string HarmonicsTHVDPeak { get; set; }
            public string LimitProjectLabel { get; set; }
            public string LimitMaxLoadingLabel { get; set; }
            public double LimitMaxLoading { get; set; }
            public string LimitFdipLabel { get; set; }
            public double LimitFdip { get; set; }
            public string LimitVdipLabel { get; set; }
            public double LimitVdip { get; set; }
            public string LimitTHVDContLabel { get; set; }
            public string LimitTHVDCont { get; set; }
            public string LimitTHVDPeakLabel { get; set; }
            public string LimitTHVDPeak { get; set; }

        }

        public class LoadList
        {
            // header column labels
            public string TitleLabel { get; set; }
            public string SequenceHeaderColumnLabel { get; set; }
            public string DescriptionHeaderColumnLabel { get; set; }
            public string StartingHeaderColumnLabel { get; set; }
            public string StartingkWHeaderColumnLabel { get; set; }
            public string StartingkVAHeaderColumnLabel { get; set; }
            public string RunningHeaderColumnLabel { get; set; }
            public string RunningHeaderkWColumnLabel { get; set; }
            public string RunningHeaderkVAColumnLabel { get; set; }
            public string HarmonicCurrentDistortionHeaderColumnLabel { get; set; }
            public string HarmonicCurrentDistortionPeakHeaderColumnLabel { get; set; }
            public string HarmonicCurrentDistortionContHeaderColumnLabel { get; set; }
            public string HarmonicCurrentDistortionkVAHeaderColumnLabel { get; set; }
            public string LimitsHeaderColumnLabel { get; set; }
            public string LimitsVdipHeaderColumnLabel { get; set; }
            public string LimitsFdipHeaderColumnLabel { get; set; }

            //summary row labels
            public string SummaryLabel { get; set; } // Summary 
            public string SummaryAllLoadsLabel { get; set; } // All loads on (sequence starting) in summary row
            public string SequencePeakLabel { get; set; } // kW Sequence Peak in summary row
            public string ApplicationPeakLabel { get; set; } // kW Application Peak in summary row

            // load items
            public List<LoadItem> LoadItemList { get; set; }


        }

        public class LoadItem
        {
            //public enum LoadItemType : int { Group = 1, Step = 2 };
            public string ItemType { get; set; }
            public string GroupStepLabel { get; set; }
            public string Sequence { get; set; }
            public int ItemID { get; set; } // unique load item iD
            public string ItemDesc { get; set; }
            public string ItemKVA { get; set; }
            public string ItemPF { get; set; }
            public string ItemHarmonics { get; set; }
            public double StartingkW { get; set; }
            public double StartingkVA { get; set; }
            public double RunningkW { get; set; }
            public double RunningkVA { get; set; }
            public double HoarmonicCurrentDistortionPeak { get; set; }
            public double HoarmonicCurrentDistortionCont { get; set; }
            public double HoarmonicCurrentDistortionkVA { get; set; }
            public string LimitsVdip { get; set; }
            public string LimitsFdip { get; set; }

            // summary row
            public int isSummaryRow { get; set; }
            public string SequencePeak { get; set; }
            public double? ApplicationPeak { get; set; }

            public double? SequencePeakLimitVdip { get; set; }
            public double? SequencePeakLimitFdip { get; set; }
            public double? ApplicationPeakLimitVdip { get; set; }
            public double? ApplicationPeakLimitFdip { get; set; }

        }

        #endregion

        #region ## Transient Analysis ## 
        public class TransientAnalysis
        {
            public string TitleLabel { get; set; }
            public AlternatorTransientReq AlternatorTransientReq { get; set; }
            public EngineTransientReq EngineTransientReq { get; set; }
            public string Note { get; set; }
            public AnalysisList AlternatorTransientAnalysis { get; set; }
            public AnalysisList EnginTransientAnalysis { get; set; }

        }
        public class AlternatorTransientReq
        {
            public string TitleLabel { get; set; }
            public string SequenceLabel { get; set; }
            public string Sequence { get; set; }
            public string LoadLabel { get; set; }
            public string Load { get; set; }
            public string StartingKVALabel { get; set; }
            public double StartingKVA { get; set; }
            public string VdipToleranceLabel { get; set; }
            public string VdipTolerance { get; set; }
            public string VdipExpectedLabel { get; set; }
            public string VdipExpected { get; set; }
        }
        public class EngineTransientReq
        {
            public string TitleLabel { get; set; }
            public string SequenceLabel { get; set; }
            public string Sequence { get; set; }
            public string LoadLabel { get; set; }
            public string Load { get; set; }
            public string StartingKVALabel { get; set; }
            public double StartingKVA { get; set; }
            public string VdipToleranceLabel { get; set; }
            public double VdipTolerance { get; set; }
            public string VdipExpectedLabel { get; set; }
            public double VdipExpected { get; set; }
        }

        public class AnalysisList
        {
            public string TitleLabel { get; set; }
            public string SequenceColumnLabel { get; set; }
            public string AllowableVdipColumnLabel { get; set; }
            public string ExpectedVdipColumnLabel { get; set; }
            public string SequenceStartingKVAColumnLabel { get; set; }
            public string LargestTransientLoadColumnLabel { get; set; }
            public List<AnalysisItem> AnalysisItemList { get; set; }
        }

        public class AnalysisItem
        {
            public string ItemType { get; set; }
            public string GroupStepLabel { get; set; }
            public string Sequence { get; set; }
            public int ItemID { get; set; } // unique load item iD
            public string AllowableVdip { get; set; }
            public string ExpectedVdip { get; set; }
            public double SequenceVdip { get; set; }
            public string LargestTransientLoad { get; set; }
        }
        #endregion

        #region ## Harmonic Analysis ## 
        public class HarmonicAnalysis
        {
            public string TitleLabel { get; set; }
            public string HarmonicProfileLabel { get; set; }
            public string HarmonicProfile { get; set; }
            public string kVANonlinearLoadLabel { get; set; }
            public string kVANonlinearLoad { get; set; }
            public string kVABaseLabel { get; set; }
            public string kVANBase { get; set; }
            public string SequenceLabel { get; set; }
            public string Sequence { get; set; }
            public string THIDLabel { get; set; }
            public string THID { get; set; }
            public string THVDLabel { get; set; }
            public string THVD { get; set; }
            public string SelectedSeqHarmonicAlternatorLoadingLabel { get; set; }
            public string SelectedSeqHarmonicAlternatorLoading { get; set; }
            public HarmonicCurrentAdnVoltageProfiles HarmonicCurrentAdnVoltageProfiles { get; set; }

        }
        public class HarmonicCurrentAdnVoltageProfiles
        {
            public string TitleLabel { get; set; }
            public string ProfileLabel { get; set; }
            public string CurrentLabel { get; set; }
            public string VoltageLabel { get; set; }

            public double Current3 { get; set; }
            public double Current5 { get; set; }
            public double Current7 { get; set; }
            public double Current9 { get; set; }
            public double Current11 { get; set; }
            public double Current13 { get; set; }
            public double Current15 { get; set; }
            public double Current17 { get; set; }
            public double Current19 { get; set; }
            public double Voltage3 { get; set; }
            public double Voltage5 { get; set; }
            public double Voltage7 { get; set; }
            public double Voltage9 { get; set; }
            public double Voltage11 { get; set; }
            public double Voltage13 { get; set; }
            public double Voltage15 { get; set; }
            public double Voltage17 { get; set; }
            public double Voltage19 { get; set; }

            public string GraphImagePath { get; set; }
        }
        #endregion

        #region ## Gas Piping ##
        public class GasPiping
        {
            public string TitleLabel { get; set; }
            public string Note { get; set; }
            public GeneratorSummary GeneratorSummary { get; set; }
            public GasPipingSolution GasPipingSolution { get; set; }
            public GasPipingInput GasPipingInput { get; set; }
        }

        public class GeneratorSummary
        {
            public string TitleLabel { get; set; }
            public string SigingMethodLabel { get; set; }
            public string SigingMethod { get; set; }
            public string PipeSizeLabel { get; set; }
            public double PipeSize { get; set; }
            public string ProductFamilyLabel { get; set; }
            public string ProductFamily { get; set; }
            public string GeneratorLabel { get; set; }
            public string Generator { get; set; }
            public string FuelTypeLabel { get; set; }
            public string FuelType { get; set; }
            public string FuelConsumptionLabel { get; set; }
            public double FuelConsumption { get; set; }
            public string MinimumPressureLabel { get; set; }
            public double MinimumPressure { get; set; }
            public string SummaryNote { get; set; }
        }
        public class GasPipingSolution
        {
            public string TitleLabel { get; set; }
            public string PressureDropLabel { get; set; }
            public double PressureDrop { get; set; }
            public string PercentageAllowableLabel { get; set; }
            public double PercentageAllowable { get; set; }
            public string AvailablePressureLabel { get; set; }
            public double AvailablePressure { get; set; }
        }

        public class GasPipingInput
        {
            public string TitleLabel { get; set; }
            public string SigingMethodLabel { get; set; }
            public string SigingMethod { get; set; }
            public string PipeSizeLabel { get; set; }
            public double PipeSize { get; set; }
            public string SupplyGasPressureLabel { get; set; }
            public double SupplyGasPressure { get; set; }
            public string LenghOfRunLabel { get; set; }
            public double LenghOfRun { get; set; }
            public string GeneratorMinPressureLabel { get; set; }
            public double GeneratorMinPressure { get; set; }
            public string NumberOf90ElbowsLabel { get; set; }
            public double NumberOf90Elbows { get; set; }
            public string Numberof45ElblowsLabel { get; set; }
            public double Numberof45Elblows { get; set; }
            public string NumberOfTeesLabel { get; set; }
            public double NumberOfTees { get; set; }
        }
        #endregion

        #region ## Exhaust Piping ##
        public class ExhaustPiping
        {
            public string TitleLabel { get; set; }
            public string Note { get; set; }
            public ExhaustGeneratorSummary ExhaustGeneratorSummary { get; set; }
            public ExhaustSolution ExhaustSolution { get; set; }
            public ExhausPipingInput ExhausPipingInput { get; set; }
        }

        public class ExhaustGeneratorSummary
        {
            public string SigingMethodLabel { get; set; }
            public string SigingMethod { get; set; }
            public string PipeSizeLabel { get; set; }
            public string PipeSize { get; set; }
            public string TitleLabel { get; set; }
            public string ProductFamilyLabel { get; set; }
            public string ProductFamily { get; set; }
            public string GeneratorLabel { get; set; }
            public string Generator { get; set; }
            public string TotalExahustFlowLabel { get; set; }
            public double TotalExahustFlow { get; set; }
            public string MaxBackPressureLabel { get; set; }
            public double MaxBackPressure { get; set; }
        }
        public class ExhaustSolution
        {
            public string TitleLabel { get; set; }
            public string PressureDropLabel { get; set; }
            public double PressureDrop { get; set; }
        }

        public class ExhausPipingInput
        {
            public string TitleLabel { get; set; }
            public string SigingMethodLabel { get; set; }
            public string SigingMethod { get; set; }
            public string PipeSizeLabel { get; set; }
            public double PipeSize { get; set; }
            public string ExhaustSystemConfigurationLabel { get; set; }
            public string ExhaustSystemConfiguration { get; set; }
            public string LenghOfRunLabel { get; set; }
            public double LenghOfRun { get; set; }
            public string NumberOfStandardElbowsLabel { get; set; }
            public double NumberOfStandardElbows { get; set; }
            public string NumberOfLongLabel { get; set; }
            public double NumberOfLong { get; set; }
            public string Numberof45ElblowsLabel { get; set; }
            public double Numberof45Elblows { get; set; }

        }
        #endregion

    }
}
