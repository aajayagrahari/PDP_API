namespace PowerDesignPro.Data.Models
{
    /// <summary>
    /// BaseSolutionSetupEntity is a base class for UserDefaultSettings/SolutionSetupSettings Entities.
    /// Used to gets/sets all the dropdown selected/default values...
    /// </summary>
    /// <seealso cref="PowerDesignPro.Data.Models.IEntity" />
    public class BaseSolutionSetupEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the ambient temperature identifier.
        /// </summary>
        /// <value>
        /// The ambient temperature identifier.
        /// </value>
        public int AmbientTemperatureID { get; set; }

        /// <summary>
        /// Gets or sets the elevation identifier.
        /// </summary>
        /// <value>
        /// The elevation identifier.
        /// </value>
        public int ElevationID { get; set; }

        /// <summary>
        /// Gets or sets the voltage phase identifier.
        /// </summary>
        /// <value>
        /// The voltage phase identifier.
        /// </value>
        public int VoltagePhaseID { get; set; }

        /// <summary>
        /// Gets or sets the frequency identifier.
        /// </summary>
        /// <value>
        /// The frequency identifier.
        /// </value>
        public int FrequencyID { get; set; }

        /// <summary>
        /// Gets or sets the voltage nominal identifier.
        /// </summary>
        /// <value>
        /// The voltage nominal identifier.
        /// </value>
        public int VoltageNominalID { get; set; }

        /// <summary>
        /// Gets or sets the voltage specific identifier.
        /// </summary>
        /// <value>
        /// The voltage specific identifier.
        /// </value>
        public int VoltageSpecificID { get; set; }

        /// <summary>
        /// Gets or sets the units identifier.
        /// </summary>
        /// <value>
        /// The units identifier.
        /// </value>
        public int UnitsID { get; set; }

        /// <summary>
        /// Gets or sets the maximum running load identifier.
        /// </summary>
        /// <value>
        /// The maximum running load identifier.
        /// </value>
        public int MaxRunningLoadID { get; set; }

        /// <summary>
        /// Gets or sets the voltage dip identifier.
        /// </summary>
        /// <value>
        /// The voltage dip identifier.
        /// </value>
        public int VoltageDipID { get; set; }

        /// <summary>
        /// Gets or sets the voltage dip units identifier.
        /// </summary>
        /// <value>
        /// The voltage dip units identifier.
        /// </value>
        public int VoltageDipUnitsID { get; set; }

        /// <summary>
        /// Gets or sets the frequency dip identifier.
        /// </summary>
        /// <value>
        /// The frequency dip identifier.
        /// </value>
        public int FrequencyDipID { get; set; }

        /// <summary>
        /// Gets or sets the frequency dip units identifier.
        /// </summary>
        /// <value>
        /// The frequency dip units identifier.
        /// </value>
        public int FrequencyDipUnitsID { get; set; }

        /// <summary>
        /// Gets or sets the continuous allowable voltage distortion identifier.
        /// </summary>
        /// <value>
        /// The continuous allowable voltage distortion identifier.
        /// </value>
        public int ContinuousAllowableVoltageDistortionID { get; set; }

        /// <summary>
        /// Gets or sets the momentary allowable voltage distortion identifier.
        /// </summary>
        /// <value>
        /// The momentary allowable voltage distortion identifier.
        /// </value>
        public int MomentaryAllowableVoltageDistortionID { get; set; }

        /// <summary>
        /// Gets or sets the engine duty identifier.
        /// </summary>
        /// <value>
        /// The engine duty identifier.
        /// </value>
        public int EngineDutyID { get; set; }

        /// <summary>
        /// Gets or sets the fuel type identifier.
        /// </summary>
        /// <value>
        /// The fuel type identifier.
        /// </value>
        public int FuelTypeID { get; set; }

        /// <summary>
        /// Gets or sets the regulatory filter.
        /// </summary>
        /// <value>
        /// The regulatory filter.
        /// </value>
        public string RegulatoryFilter { get; set; }
        /// <summary>
        /// Gets or sets the solution application identifier.
        /// </summary>
        /// <value>
        /// The solution application identifier.
        /// </value>
        public int SolutionApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the enclosure type identifier.
        /// </summary>
        /// <value>
        /// The enclosure type identifier.
        /// </value>
        public int EnclosureTypeID { get; set; }

        /// <summary>
        /// Gets or sets the desired sound identifier.
        /// </summary>
        /// <value>
        /// The desired sound identifier.
        /// </value>
        public int DesiredSoundID { get; set; }

        /// <summary>
        /// Gets or sets the fuel tank identifier.
        /// </summary>
        /// <value>
        /// The fuel tank identifier.
        /// </value>
        public int FuelTankID { get; set; }

        /// <summary>
        /// Gets or sets the desired run time identifier.
        /// </summary>
        /// <value>
        /// The desired run time identifier.
        /// </value>
        public int DesiredRunTimeID { get; set; }

        /// <summary>
        /// Gets or sets the load sequence cyclic1 identifier.
        /// </summary>
        /// <value>
        /// The load sequence cyclic1 identifier.
        /// </value>
        public int LoadSequenceCyclic1ID { get; set; }

        /// <summary>
        /// Gets or sets the load sequence cyclic2 identifier.
        /// </summary>
        /// <value>
        /// The load sequence cyclic2 identifier.
        /// </value>
        public int LoadSequenceCyclic2ID { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Gets or sets the ambient temperature.
        /// </summary>
        /// <value>
        /// The ambient temperature.
        /// </value>
        public virtual AmbientTemperature AmbientTemperature { get; set; }

        /// <summary>
        /// Gets or sets the elevation.
        /// </summary>
        /// <value>
        /// The elevation.
        /// </value>
        public virtual Elevation Elevation { get; set; }

        /// <summary>
        /// Gets or sets the voltage phase.
        /// </summary>
        /// <value>
        /// The voltage phase.
        /// </value>
        public virtual VoltagePhase VoltagePhase { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public virtual Frequency Frequency { get; set; }

        /// <summary>
        /// Gets or sets the voltage nominal.
        /// </summary>
        /// <value>
        /// The voltage nominal.
        /// </value>
        public virtual VoltageNominal VoltageNominal { get; set; }

        /// <summary>
        /// Gets or sets the voltage specific.
        /// </summary>
        /// <value>
        /// The voltage specific.
        /// </value>
        public virtual VoltageSpecific VoltageSpecific { get; set; }

        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>
        /// The units.
        /// </value>
        public virtual Units Units { get; set; }

        /// <summary>
        /// Gets or sets the maximum running load.
        /// </summary>
        /// <value>
        /// The maximum running load.
        /// </value>
        public virtual MaxRunningLoad MaxRunningLoad { get; set; }

        /// <summary>
        /// Gets or sets the voltage dip.
        /// </summary>
        /// <value>
        /// The voltage dip.
        /// </value>
        public virtual VoltageDip VoltageDip { get; set; }

        /// <summary>
        /// Gets or sets the voltage dip units.
        /// </summary>
        /// <value>
        /// The voltage dip units.
        /// </value>
        public virtual VoltageDipUnits VoltageDipUnits { get; set; }

        /// <summary>
        /// Gets or sets the frequency dip.
        /// </summary>
        /// <value>
        /// The frequency dip.
        /// </value>
        public virtual FrequencyDip FrequencyDip { get; set; }

        /// <summary>
        /// Gets or sets the frequency dip units.
        /// </summary>
        /// <value>
        /// The frequency dip units.
        /// </value>
        public virtual FrequencyDipUnits FrequencyDipUnits { get; set; }

        /// <summary>
        /// Gets or sets the continuous allowable voltage distortion.
        /// </summary>
        /// <value>
        /// The continuous allowable voltage distortion.
        /// </value>
        public virtual ContinuousAllowableVoltageDistortion ContinuousAllowableVoltageDistortion { get; set; }

        /// <summary>
        /// Gets or sets the momentary allowable voltage distortion.
        /// </summary>
        /// <value>
        /// The momentary allowable voltage distortion.
        /// </value>
        public virtual MomentaryAllowableVoltageDistortion MomentaryAllowableVoltageDistortion { get; set; }

        /// <summary>
        /// Gets or sets the engine duty.
        /// </summary>
        /// <value>
        /// The engine duty.
        /// </value>
        public virtual EngineDuty EngineDuty { get; set; }

        /// <summary>
        /// Gets or sets the type of the fuel.
        /// </summary>
        /// <value>
        /// The type of the fuel.
        /// </value>
        public virtual FuelType FuelType { get; set; }

        /// <summary>
        /// Gets or sets the solution application.
        /// </summary>
        /// <value>
        /// The solution application.
        /// </value>
        public virtual SolutionApplication SolutionApplication { get; set; }

        /// <summary>
        /// Gets or sets the type of the enclosure.
        /// </summary>
        /// <value>
        /// The type of the enclosure.
        /// </value>
        public virtual EnclosureType EnclosureType { get; set; }

        /// <summary>
        /// Gets or sets the desired sound.
        /// </summary>
        /// <value>
        /// The desired sound.
        /// </value>
        public virtual DesiredSound DesiredSound { get; set; }

        /// <summary>
        /// Gets or sets the fuel tank.
        /// </summary>
        /// <value>
        /// The fuel tank.
        /// </value>
        public virtual FuelTank FuelTank { get; set; }

        /// <summary>
        /// Gets or sets the desired run time.
        /// </summary>
        /// <value>
        /// The desired run time.
        /// </value>
        public virtual DesiredRunTime DesiredRunTime { get; set; }

        /// <summary>
        /// Gets or sets the load sequence cyclic1.
        /// </summary>
        /// <value>
        /// The load sequence cyclic1.
        /// </value>
        public virtual LoadSequenceCyclic LoadSequenceCyclic1 { get; set; }

        /// <summary>
        /// Gets or sets the load sequence cyclic2.
        /// </summary>
        /// <value>
        /// The load sequence cyclic2.
        /// </value>
        public virtual LoadSequenceCyclic LoadSequenceCyclic2 { get; set; }

        #endregion
    }
}
