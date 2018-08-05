using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Contains All the selected dropdown values required for solution setup
    /// </summary>
    public class BaseSolutionSetupMappingValuesDto
    {
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
        /// Gets or sets the solution application list.
        /// </summary>
        /// <value>
        /// The solution application list.
        /// </value>
        public IEnumerable<RegulatoryFilterDto> SelectedRegulatoryFilterList { get; set; }

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
    }
}
