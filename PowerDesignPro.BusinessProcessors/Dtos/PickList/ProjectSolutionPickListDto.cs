using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// Contails all the dropdown list for SolutionSetup/UserDefaultSetup settings
    /// </summary>
    public class ProjectSolutionPickListDto
    {
        /// <summary>
        /// Gets or sets the ambient temperature list.
        /// </summary>
        /// <value>
        /// The ambient temperature list.
        /// </value>
        public IEnumerable<PickListDto> AmbientTemperatureList { get; set; }

        /// <summary>
        /// Gets or sets the continuous allowable voltage distortion list.
        /// </summary>
        /// <value>
        /// The continuous allowable voltage distortion list.
        /// </value>
        public IEnumerable<PickListDto> ContinuousAllowableVoltageDistortionList { get; set; }

        /// <summary>
        /// Gets or sets the desired run time list.
        /// </summary>
        /// <value>
        /// The desired run time list.
        /// </value>
        public IEnumerable<PickListDto> DesiredRunTimeList { get; set; }

        /// <summary>
        /// Gets or sets the desired sound list.
        /// </summary>
        /// <value>
        /// The desired sound list.
        /// </value>
        public IEnumerable<PickListDto> DesiredSoundList { get; set; }

        /// <summary>
        /// Gets or sets the elevation list.
        /// </summary>
        /// <value>
        /// The elevation list.
        /// </value>
        public IEnumerable<PickListDto> ElevationList { get; set; }

        /// <summary>
        /// Gets or sets the enclosure type list.
        /// </summary>
        /// <value>
        /// The enclosure type list.
        /// </value>
        public IEnumerable<PickListDto> EnclosureTypeList { get; set; }

        /// <summary>
        /// Gets or sets the engine duty list.
        /// </summary>
        /// <value>
        /// The engine duty list.
        /// </value>
        public IEnumerable<PickListDto> EngineDutyList { get; set; }

        /// <summary>
        /// Gets or sets the frequency list.
        /// </summary>
        /// <value>
        /// The frequency list.
        /// </value>
        public IEnumerable<PickListDto> FrequencyList { get; set; }

        /// <summary>
        /// Gets or sets the frequency dip list.
        /// </summary>
        /// <value>
        /// The frequency dip list.
        /// </value>
        public IEnumerable<PickListDto> FrequencyDipList { get; set; }

        /// <summary>
        /// Gets or sets the frequency dip unit list.
        /// </summary>
        /// <value>
        /// The frequency dip unit list.
        /// </value>
        public IEnumerable<PickListDto> FrequencyDipUnitList { get; set; }

        /// <summary>
        /// Gets or sets the fuel tank list.
        /// </summary>
        /// <value>
        /// The fuel tank list.
        /// </value>
        public IEnumerable<PickListDto> FuelTankList { get; set; }

        /// <summary>
        /// Gets or sets the fuel type list.
        /// </summary>
        /// <value>
        /// The fuel type list.
        /// </value>
        public IEnumerable<PickListDto> FuelTypeList { get; set; }

        /// <summary>
        /// Gets or sets the load sequence cyclic.
        /// </summary>
        /// <value>
        /// The load sequence cyclic.
        /// </value>
        public IEnumerable<PickListDto> LoadSequenceCyclic { get; set; }

        /// <summary>
        /// Gets or sets the maximum running load list.
        /// </summary>
        /// <value>
        /// The maximum running load list.
        /// </value>
        public IEnumerable<PickListDto> MaxRunningLoadList { get; set; }

        /// <summary>
        /// Gets or sets the momentary allowable voltage distortion list.
        /// </summary>
        /// <value>
        /// The momentary allowable voltage distortion list.
        /// </value>
        public IEnumerable<PickListDto> MomentaryAllowableVoltageDistortionList { get; set; }

        public IEnumerable<PickListDto> SolutionApplicationList { get; set; }

        /// <summary>
        /// Gets or sets the regulatory filter list.
        /// </summary>
        /// <value>
        /// The regulatory filter list.
        /// </value>
        public IEnumerable<RegulatoryFilterDto> RegulatoryFilterList { get; set; }

        /// <summary>
        /// Gets or sets the units list.
        /// </summary>
        /// <value>
        /// The units list.
        /// </value>
        public IEnumerable<PickListDto> UnitsList { get; set; }

        /// <summary>
        /// Gets or sets the voltage dip list.
        /// </summary>
        /// <value>
        /// The voltage dip list.
        /// </value>
        public IEnumerable<PickListDto> VoltageDipList { get; set; }

        /// <summary>
        /// Gets or sets the voltage dip unit list.
        /// </summary>
        /// <value>
        /// The voltage dip unit list.
        /// </value>
        public IEnumerable<PickListDto> VoltageDipUnitList { get; set; }

        /// <summary>
        /// Gets or sets the voltage nominal list.
        /// </summary>
        /// <value>
        /// The voltage nominal list.
        /// </value>
        public IEnumerable<VoltageNominalDto> VoltageNominalList { get; set; }

        /// <summary>
        /// Gets or sets the voltage phase list.
        /// </summary>
        /// <value>
        /// The voltage phase list.
        /// </value>
        public IEnumerable<PickListDto> VoltagePhaseList { get; set; }

        /// <summary>
        /// Gets or sets the voltage specific list.
        /// </summary>
        /// <value>
        /// The voltage specific list.
        /// </value>
        public IEnumerable<VoltageSpecificDto> VoltageSpecificList { get; set; }
    }
}
