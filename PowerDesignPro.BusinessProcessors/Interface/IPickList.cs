using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Dtos.PickList;
using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    /// <summary>
    /// Defines the signature for required methods to load dropdown data from database.
    /// </summary>
    public interface IPickList
    {
        #region SolutionSetup picklists
        /// <summary>
        /// Gets the frequency dip units.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetFrequencyDipUnits();

        /// <summary>
        /// Gets the voltage dip units.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetVoltageDipUnits();

        /// <summary>
        /// Gets the ambient temperature.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetAmbientTemperature();

        /// <summary>
        /// Gets the elevation.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetElevation();

        /// <summary>
        /// Gets the engine duty.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetEngineDuty();

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetFrequency();

        /// <summary>
        /// Gets the frequency dip.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetFrequencyDip();

        /// <summary>
        /// Gets the type of the fuel.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetFuelType();

        /// <summary>
        /// Gets the type of the fuel.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FuelTypeDto> GetFuelTypeCode();

        /// <summary>
        /// Gets the maximum running load.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetMaxRunningLoad();

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetUnits();

        /// <summary>
        /// Gets the voltage dip.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetVoltageDip();

        /// <summary>
        /// Gets the voltage nominal.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VoltageNominalDto> GetVoltageNominal(bool IsForLoads);

        /// <summary>
        /// Gets the voltage specific.
        /// </summary>
        /// <returns></returns>
        IEnumerable<VoltageSpecificDto> GetVoltageSpecific();

        /// <summary>
        /// Gets the voltage phase.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetVoltagePhase();

        /// <summary>
        /// Gets the load family.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetLoadFamily();

        /// <summary>
        /// Gets the continuous allowable voltage distortion.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetContinuousAllowableVoltageDistortion();

        /// <summary>
        /// Gets the desired run time.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DesiredRunTimeDto> GetDesiredRunTime();

        /// <summary>
        /// Gets the desired sound.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetDesiredSound();

        /// <summary>
        /// Gets the type of the enclosure.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetEnclosureType();

        /// <summary>
        /// Gets the fuel tank.
        /// </summary>
        /// <returns></returns>
        IEnumerable<FuelTankDto> GetFuelTank();

        /// <summary>
        /// Gets the load sequence cyclic.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetLoadSequenceCyclic();

        /// <summary>
        /// Gets the momentary allowable voltage distortion.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetMomentaryAllowableVoltageDistortion();

        /// <summary>
        /// Gets the solution application.
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickListDto> GetSolutionApplication();

        /// <summary>
        /// Gets the regulatory filtor.
        /// </summary>
        /// <returns></returns>
        IEnumerable<RegulatoryFilterDto> GetRegulatoryFilter();

        #endregion

        IEnumerable<PickListDto> GetSearchFilter();

        IEnumerable<StatePickListDto> GetState();

        IEnumerable<CountryPickListDto> GetCountry();

        IEnumerable<PickListDto> GetBrand();

        IEnumerable<PickListDto> GetUserCategory();

        IEnumerable<PickListDto> GetAlternatorFamily();

        IEnumerable<MotorCalculationDto> GetMotorCalculation();

        #region Solution Load PickList

        IEnumerable<LoadsDto> GetLoads();

        IEnumerable<SequenceDto> GetSequence();

        IEnumerable<PickListDto> GetPF();

        IEnumerable<SizeUnitsDto> GetSizeUnits();

        IEnumerable<HarmonicDeviceTypeDto> GetHarmonicDeviceType();

        IEnumerable<PickListDto> GetHarmonicContent();

        IEnumerable<HarmonicContentDto> GetHarmonicContentStartingMethod();
        #endregion

        #region AC Load PickList

        IEnumerable<PickListDto> GetCompressors();

        IEnumerable<PickListDto> GetCoolingLoad();

        IEnumerable<PickListDto> GetReheatLoad();

        #endregion

        #region Lighting Load PickList

        IEnumerable<LightingTypeDto> GetLightingType();

        #endregion

        #region UPS Load
        IEnumerable<PickListDto> GetPhase();

        IEnumerable<PickListDto> GetEfficiency();

        IEnumerable<PickListDto> GetChargeRate();

        IEnumerable<PickListDto> GetPowerFactor();

        IEnumerable<UPSTypeDto> GetUPSType();

        IEnumerable<PickListDto> GetLoadLevel();

        #endregion

        #region Welder Load PickList
        IEnumerable<WelderTypeDto> GetWelderType();
        #endregion

        #region Motor Load
        IEnumerable<PickListDto> GetMotorLoadLevel();

        IEnumerable<PickListDto> GetMotorLoadType();

        IEnumerable<MotorTypeDto> GetMotorType();

        IEnumerable<StartingCodeDto> GetStartingCode();

        IEnumerable<StartingMethodDto> GetStartingMethod();

        IEnumerable<ConfigurationInputDto> GetConfigurationInput();

        IEnumerable<HarmonicContentDto> GetMotorHarmonicContent();

        #endregion

        #region SolutionSummary Recommended Product

        IEnumerable<PickListDto> GetFamilySelectionMethod();

        IEnumerable<PickListDto> GetProductFamily();

        //IEnumerable<PickListDto> GetModuleSize();

        IEnumerable<PickListDto> GetParallelQuantity();

        IEnumerable<PickListDto> GetSizingMethod();

        IEnumerable<GeneratorPickListDto> GetGenerator(int productFamilyId);

        IEnumerable<AlternatorPickListDto> GetAlternator(int generatorId);

        IEnumerable<SolutionSummaryGeneratorDocumentationDto> GetGeneratorDocuments(int generatorID);

        #endregion

        //IEnumerable<PickListDto> GetExhaustPipingPipeSize();

        IEnumerable<PickListDto> GetExhaustSystemConfiguration();

        IEnumerable<VoltageNominalDto> GetVoltageNominal(int frequencyId);

        IEnumerable<PickListDto> GetHarmonicProfile();
    }
}
