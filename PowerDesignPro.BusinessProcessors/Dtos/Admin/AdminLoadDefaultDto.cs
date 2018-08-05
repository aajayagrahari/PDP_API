using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class AdminLoadDefaultDto
    {
        public int ID { get; set; }

        public string LoadName { get; set; }

        public int LoadFamilyID { get; set; }

        public int SequenceID { get; set; }

        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        public decimal? SizeStarting { get; set; }

        public int? SizeStartingUnitsID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? StartingPFID { get; set; }

        public int? RunningPFID { get; set; }

        public int? VoltagePhaseID { get; set; }

        public int? VoltageNominalID { get; set; }

        public int? VoltageSpecificID { get; set; }

        public int VoltageDipID { get; set; }

        public int VoltageDipUnitsID { get; set; }

        public int FrequencyDipID { get; set; }

        public int FrequencyDipUnitsID { get; set; }

        public bool? SizeRunningEditable { get; set; }

        public bool? SizeStartingEditable { get; set; }

        public bool? StartingPFEditable { get; set; }

        public bool? RunningPFEditable { get; set; }

        public bool? HarmonicTypeEditable { get; set; }

        public bool? LockVoltageDip { get; set; }

        public int LoadID { get; set; }

        public string UserName { get; set; }

        public int? StartingMethodID { get; set; }

        public bool? CoolingLoadEditable { get; set; }

        public bool? ReheatLoadEditable { get; set; }

        public bool? MotorLoadLevelEditable { get; set; }

        public bool? MotorLoadTypeEditable { get; set; }

        public bool? MotorTypeEditable { get; set; }

        public bool? StartingCodeEditable { get; set; }

        public bool? StartingMethodEditable { get; set; }

        public bool? ConfigurationInputEditable { get; set; }

        public bool? LightingTypeEditable { get; set; }

        public bool? UPSTypeEditable { get; set; }

        public bool? LoadLevelEditable { get; set; }

        public bool? PhaseEditable { get; set; }

        public bool? EfficiencyEditable { get; set; }

        public bool? ChargeRateEditable { get; set; }

        public bool? PowerFactorEditable { get; set; }

        public bool? UPSSystemTransientsEditable { get; set; }

        public int? CoolingLoadID { get; set; }

        public int? CoolingUnitsID { get; set; }

        public int? ReheatLoadID { get; set; }

        public int? CompressorsID { get; set; }

        public int? LightingTypeID { get; set; }

        public int? WelderTypeID { get; set; }

        public decimal? Cooling { get; set; }

        public int? PhaseID { get; set; }

        public int? EfficiencyID { get; set; }

        public int? ChargeRateID { get; set; }

        public int? LoadLevelID { get; set; }

        public int? UPSTypeID { get; set; }

        public decimal? SizeKVA { get; set; }

        public int? SizeKVAUnitsID { get; set; }

        public int? MotorLoadLevelID { get; set; }

        public int? MotorLoadTypeID { get; set; }

        public int? MotorTypeID { get; set; }

        public int? StartingCodeID { get; set; }

        public int? ConfigurationInputID { get; set; }

        public int? PowerFactorID { get; set; }

        public bool UPSRevertToBattery { get; set; }

        public bool Active { get; set; }

        public IEnumerable<PickListDto> SequenceList { get; set; }

        public IEnumerable<PickListDto> VoltageDipList { get; set; }

        public IEnumerable<PickListDto> VoltageDipUnitsList { get; set; }

        public IEnumerable<PickListDto> FrequencyDipList { get; set; }

        public IEnumerable<PickListDto> FrequencyDipUnitsList { get; set; }

        //AC
        public IEnumerable<SizeUnitsDto> CoolingUnitsList { get; set; }

        public IEnumerable<PickListDto> CompressorsList { get; set; }

        public IEnumerable<PickListDto> CoolingLoadList { get; set; }

        public IEnumerable<PickListDto> ReheatLoadList { get; set; }

        //Lighting and Basic and UPS and Welder
        public IEnumerable<PickListDto> PFList { get; set; }


        //Lighting and Basic and Motor and welder
        public IEnumerable<PickListDto> VoltagePhaseList { get; set; }

        public IEnumerable<VoltageNominalDto> VoltageNominalList { get; set; }

        public IEnumerable<VoltageSpecificDto> VoltageSpecificList { get; set; }

        //Lighting 
        public IEnumerable<LightingTypeDto> LightingTypeList { get; set; }

        //Motor
        public IEnumerable<PickListDto> MotorLoadLevelList { get; set; }

        public IEnumerable<PickListDto> MotorLoadTypeList { get; set; }

        public IEnumerable<PickListDto> MotorTypeList { get; set; }

        public IEnumerable<PickListDto> StartingCodeList { get; set; }

        public IEnumerable<PickListDto> StartingMethodList { get; set; }


        public IEnumerable<ConfigurationInputDto> ConfigurationInputList { get; set; }

        //UPS
        public IEnumerable<PickListDto> PhaseList { get; set; }

        public IEnumerable<PickListDto> EfficiencyList { get; set; }

        public IEnumerable<PickListDto> ChargeRateList { get; set; }

        public IEnumerable<PickListDto> PowerFactorList { get; set; }

        public IEnumerable<PickListDto> LoadLevelList { get; set; }

        public IEnumerable<PickListDto> UPSTypeList { get; set; }

        public IEnumerable<SizeUnitsDto> SizeKVAUnitsList { get; set; }

        //UPS And Lighting and Basic and Motor and welder
        public IEnumerable<SizeUnitsDto> SizeUnitsList { get; set; }

        //Welder
        public IEnumerable<WelderTypeDto> WelderTypeList { get; set; }

        //Welder and UPS And Lighting and Basic and Motor
        public IEnumerable<PickListDto> HarmonicDeviceTypeList { get; set; }

        public IEnumerable<PickListDto> HarmonicContentList { get; set; }
    }
}
