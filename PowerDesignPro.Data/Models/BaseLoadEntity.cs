
using System;

namespace PowerDesignPro.Data.Models
{
    /// <summary>
    /// Base Load entity for all Load defaults
    /// </summary>
    public class BaseLoadEntity : IEntity
    {
        public int ID { get;  set; }

        public int LoadID { get; set; }

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

        #region Boolean Editable Values

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

        public bool UPSRevertToBattery { get; set; }

        #endregion

        public int? StartingMethodID { get; set; }

        public int? WelderTypeID { get; set; }

        public decimal? Cooling { get; set; }

        public int? CoolingUnitsID { get; set; }

        public int? CompressorsID { get; set; }

        public int? CoolingLoadID { get; set; }

        public int? ReheatLoadID { get; set; }

        public int? LightingTypeID { get; set; }

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

        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public string ModifiedBy { get; set; }

        public bool Active { get; set; }

        #region Navigation Properties

        public virtual Load Load { get; set; }

        public virtual Sequence Sequence { get; set; }

        public virtual SizeUnits SizeRunningUnits { get; set; }

        public virtual SizeUnits SizeStartingUnits { get; set; }

        public virtual HarmonicDeviceType HarmonicDeviceType { get; set; }

        public virtual PF StartingPF { get; set; }

        public virtual PF RunningPF { get; set; }

        public virtual VoltagePhase VoltagePhase { get; set; }

        public virtual VoltageNominal VoltageNominal { get; set; }

        public virtual VoltageSpecific VoltageSpecific { get; set; }

        public virtual VoltageDip VoltageDip { get; set; }

        public virtual VoltageDipUnits VoltageDipUnits { get; set; }

        public virtual FrequencyDip FrequencyDip { get; set; }

        public virtual FrequencyDipUnits FrequencyDipUnits { get; set; }

        public virtual StartingMethod StartingMethod { get; set; }

        public virtual WelderType WelderType { get; set; }

        public virtual SizeUnits CoolingUnits { get; set; }

        public virtual Compressors Compressors { get; set; }

        public virtual CoolingLoad CoolingLoad { get; set; }

        public virtual ReheatLoad ReheatLoad { get; set; }

        public virtual LightingType LightingType { get; set; }

        public virtual Phase Phase { get; set; }

        public virtual Efficiency Efficiency { get; set; }

        public virtual ChargeRate ChargeRate { get; set; }

        public virtual PowerFactor PowerFactor { get; set; }

        public virtual LoadLevel LoadLevel { get; set; }
        
        public virtual UPSType UPSType { get; set; }

        public virtual SizeUnits SizeKVAUnits { get; set; }

        public virtual MotorLoadLevel MotorLoadLevel { get; set; }

        public virtual MotorLoadType MotorLoadType { get; set; }

        public virtual MotorType MotorType { get; set; }

        public virtual StartingCode StartingCode { get; set; }

        public virtual ConfigurationInput ConfigurationInput { get; set; }

        #endregion
    }
}
