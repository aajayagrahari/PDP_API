using PowerDesignPro.BusinessProcessors.Interface;
using System.Collections.Generic;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Framework.Interface;
using System.Linq;
using PowerDesignPro.Data.Models;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Models.User;
using PowerDesignPro.BusinessProcessors.Dtos.PickList;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    /// <summary>
    /// Processor class to fetch all the dropdown data from database.
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Interface.IPickList" />
    public class PickListProcessor : IPickList
    {
        /// <summary>
        /// The ambient temperature repository
        /// </summary>
        private readonly IEntityBaseRepository<AmbientTemperature> _ambientTemperatureRepository;
        /// <summary>
        /// The continuous allowable voltage distortion repository
        /// </summary>
        private readonly IEntityBaseRepository<ContinuousAllowableVoltageDistortion> _continuousAllowableVoltageDistortionRepository;
        /// <summary>
        /// The desired run time repository
        /// </summary>
        private readonly IEntityBaseRepository<DesiredRunTime> _desiredRunTimeRepository;
        /// <summary>
        /// The desired sound repository
        /// </summary>
        private readonly IEntityBaseRepository<DesiredSound> _desiredSoundRepository;
        /// <summary>
        /// The elevation repository
        /// </summary>
        private readonly IEntityBaseRepository<Elevation> _elevationRepository;
        /// <summary>
        /// The enclosure type repository
        /// </summary>
        private readonly IEntityBaseRepository<EnclosureType> _enclosureTypeRepository;
        /// <summary>
        /// The engine duty repository
        /// </summary>
        private readonly IEntityBaseRepository<EngineDuty> _engineDutyRepository;
        /// <summary>
        /// The frequency repository
        /// </summary>
        private readonly IEntityBaseRepository<Frequency> _frequencyRepository;
        /// <summary>
        /// The frequency dip repository
        /// </summary>
        private readonly IEntityBaseRepository<FrequencyDip> _frequencyDipRepository;
        /// <summary>
        /// The frequency dip units repository
        /// </summary>
        private readonly IEntityBaseRepository<FrequencyDipUnits> _frequencyDipUnitsRepository;
        /// <summary>
        /// The fuel tank repository
        /// </summary>
        private readonly IEntityBaseRepository<FuelTank> _fuelTankRepository;
        /// <summary>
        /// The fuel type repository
        /// </summary>
        private readonly IEntityBaseRepository<FuelType> _fuelTypeRepository;
        /// <summary>
        /// The load sequence cyclic repository
        /// </summary>
        private readonly IEntityBaseRepository<LoadSequenceCyclic> _loadSequenceCyclicRepository;
        /// <summary>
        /// The maximum running load repository
        /// </summary>
        private readonly IEntityBaseRepository<MaxRunningLoad> _maxRunningLoadRepository;
        /// <summary>
        /// The momentary allowable voltage distortion repository
        /// </summary>
        private readonly IEntityBaseRepository<MomentaryAllowableVoltageDistortion> _momentaryAllowableVoltageDistortionRepository;
        /// <summary>
        /// The solution application repository
        /// </summary>
        private readonly IEntityBaseRepository<SolutionApplication> _solutionApplicationRepository;
        /// <summary>
        /// The regulatory filter repository
        /// </summary>
        private readonly IEntityBaseRepository<RegulatoryFilter> _regulatoryFilterRepository;
        /// <summary>
        /// The units repository
        /// </summary>
        private readonly IEntityBaseRepository<Units> _unitsRepository;
        /// <summary>
        /// The voltage dip repository
        /// </summary>
        private readonly IEntityBaseRepository<VoltageDip> _voltageDipRepository;
        /// <summary>
        /// The voltage dip units repository
        /// </summary>
        private readonly IEntityBaseRepository<VoltageDipUnits> _voltageDipUnitsRepository;
        /// <summary>
        /// The voltage nominal repository
        /// </summary>
        private readonly IEntityBaseRepository<VoltageNominal> _voltageNominalRepository;
        /// <summary>
        /// The voltage phase repository
        /// </summary>
        private readonly IEntityBaseRepository<VoltagePhase> _voltagePhaseRepository;
        /// <summary>
        /// The voltage specific repository
        /// </summary>
        private readonly IEntityBaseRepository<VoltageSpecific> _voltageSpecificRepository;

        private readonly IEntityBaseRepository<AlternatorFamily> _alternatorFamilyRepository;

        private readonly IEntityBaseRepository<Load> _loadRepository;

        /// <summary>
        /// The load family repository
        /// </summary>
        private readonly IEntityBaseRepository<LoadFamily> _loadFamilyRepository;

        private readonly IEntityBaseRepository<State> _stateRepository;

        private readonly IEntityBaseRepository<Country> _countryRepository;

        private readonly IEntityBaseRepository<Brand> _brandRepository;

        private readonly IEntityBaseRepository<UserCategory> _userCategoryRepository;

        private readonly IEntityBaseRepository<MotorCalculation> _motorCalculationRepository;

        private readonly IEntityBaseRepository<HarmonicProfile> _harmonicProfileRepository;

        #region Solution Summary

        private readonly IEntityBaseRepository<FamilySelectionMethod> _familySelectionMethodRepository;
        private readonly IEntityBaseRepository<ProductFamily> _productFamilyRepository;
        //private readonly IEntityBaseRepository<ModuleSize> _moduleSizeRepository;
        private readonly IEntityBaseRepository<SizingMethod> _sizingMethodRepository;
        private readonly IEntityBaseRepository<Generator> _generatorRepository;
        private readonly IEntityBaseRepository<Alternator> _alternatorRepository;
        private readonly IEntityBaseRepository<Documentation> _documentationRepository;
        private readonly IEntityBaseRepository<ParallelQuantity> _parallelQuantityRepository;

        #endregion

        #region Solution Load Pick List

        private readonly IEntityBaseRepository<Sequence> _sequenceRepository;

        private readonly IEntityBaseRepository<PF> _pfRepository;

        private readonly IEntityBaseRepository<SizeUnits> _sizeUnitsRepository;

        private readonly IEntityBaseRepository<HarmonicDeviceType> _harmonicDeviceTypeRepository;

        private readonly IEntityBaseRepository<HarmonicContent> _harmonicContentRepository;
        #endregion

        #region AC Load

        private readonly IEntityBaseRepository<Compressors> _compressorsRepository;

        private readonly IEntityBaseRepository<CoolingLoad> _coolingLoadRepository;

        private readonly IEntityBaseRepository<ReheatLoad> _reheatLoadRepository;

        #endregion

        #region Lighting Load

        private readonly IEntityBaseRepository<LightingType> _lightingTypeRepository;

        #endregion

        #region Welder Load

        private readonly IEntityBaseRepository<WelderType> _welderTypeRepository;

        #endregion

        #region UPS Load
        private readonly IEntityBaseRepository<Phase> _phaseRepository;

        private readonly IEntityBaseRepository<Efficiency> _efficiencyRepository;

        private readonly IEntityBaseRepository<ChargeRate> _chargeRateRepository;

        private readonly IEntityBaseRepository<PowerFactor> _powerFactorRepository;

        private readonly IEntityBaseRepository<UPSType> _upsTypeRepository;

        private readonly IEntityBaseRepository<LoadLevel> _loadLevelRepository;

        #endregion

        #region Motor Load
        private readonly IEntityBaseRepository<MotorLoadLevel> _motorLoadLevelRepository;

        private readonly IEntityBaseRepository<MotorLoadType> _motorLoadTypeRepository;

        private readonly IEntityBaseRepository<MotorType> _motorTypeRepository;

        private readonly IEntityBaseRepository<StartingCode> _startingCodeRepository;

        private readonly IEntityBaseRepository<StartingMethod> _startingMethodRepository;

        private readonly IEntityBaseRepository<ConfigurationInput> _configurationInputRepository;
        #endregion

        #region Exhaust Piping

        //private readonly IEntityBaseRepository<ExhaustPipingPipeSize> _exhaustPipingPipeSizeRepository;

        private readonly IEntityBaseRepository<ExhaustSystemConfiguration> _exhaustSystemConfigurationRepository;

        #endregion

        #region Search Filter
        private readonly IEntityBaseRepository<SearchFilter> _searchFilterRepository;
        #endregion

        /// <summary>
        /// The pick list entity to dto mapper
        /// </summary>
        private readonly IMapper<BasePickListEntity, PickListDto> _pickListEntityToDtoMapper;

        private readonly IMapper<HarmonicDeviceType, HarmonicDeviceTypeDto> _harmonicDeviceTypeEntityToDtoMapper;
        /// <summary>
        /// The voltage nominal entity to dto mapper
        /// </summary>
        private readonly IMapper<VoltageNominal, VoltageNominalDto> _voltageNominalEntityToDtoMapper;
        /// <summary>
        /// The voltage specific entity to dto mapper
        /// </summary>
        private readonly IMapper<VoltageSpecific, VoltageSpecificDto> _voltageSpecificEntityToDtoMapper;

        private readonly IMapper<State, StatePickListDto> _statePickListEntityToDtoMapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="PickListProcessor" /> class.
        /// </summary>
        /// <param name="ambientTemperatureRepository">The ambient temperature repository.</param>
        /// <param name="continuousAllowableVoltageDistortionRepository">The continuous allowable voltage distortion repository.</param>
        /// <param name="desiredRunTimeRepository">The desired run time repository.</param>
        /// <param name="desiredSoundRepository">The desired sound repository.</param>
        /// <param name="elevationRepository">The elevation repository.</param>
        /// <param name="enclosureTypeRepository">The enclosure type repository.</param>
        /// <param name="engineDutyRepository">The engine duty repository.</param>
        /// <param name="frequencyRepository">The frequency repository.</param>
        /// <param name="frequencyDipRepository">The frequency dip repository.</param>
        /// <param name="frequencyDipUnitsRepository">The frequency dip units repository.</param>
        /// <param name="fuelTankRepository">The fuel tank repository.</param>
        /// <param name="fuelTypeRepository">The fuel type repository.</param>
        /// <param name="loadSequenceCyclicRepository">The load sequence cyclic repository.</param>
        /// <param name="maxRunningLoadRepository">The maximum running load repository.</param>
        /// <param name="momentaryAllowableVoltageDistortionRepository">The momentary allowable voltage distortion repository.</param>
        /// <param name="solutionApplicationRepository">The solution application repository.</param>
        /// <param name="regulatoryFilterRepository">The regulatory filter repository.</param>
        /// <param name="unitsRepository">The units repository.</param>
        /// <param name="voltageDipRepository">The voltage dip repository.</param>
        /// <param name="voltageDipUnitsRepository">The voltage dip units repository.</param>
        /// <param name="voltageNominalRepository">The voltage nominal repository.</param>
        /// <param name="voltagePhaseRepository">The voltage phase repository.</param>
        /// <param name="voltageSpecificRepository">The voltage specific repository.</param>
        /// <param name="loadFamilyRepository">The load family repository.</param>
        /// <param name="motorTypeRepository">The motor type repository.</param>
        /// <param name="pickListEntityToDtoMapper">The pick list entity to dto mapper.</param>
        /// <param name="voltageNominalEntityToDtoMapper">The voltage nominal entity to dto mapper.</param>
        /// <param name="voltageSpecificEntityToDtoMapper">The voltage specific entity to dto mapper.</param>
        public PickListProcessor(
            IEntityBaseRepository<AmbientTemperature> ambientTemperatureRepository,
            IEntityBaseRepository<ContinuousAllowableVoltageDistortion> continuousAllowableVoltageDistortionRepository,
            IEntityBaseRepository<DesiredRunTime> desiredRunTimeRepository,
            IEntityBaseRepository<DesiredSound> desiredSoundRepository,
            IEntityBaseRepository<Elevation> elevationRepository,
            IEntityBaseRepository<EnclosureType> enclosureTypeRepository,
            IEntityBaseRepository<EngineDuty> engineDutyRepository,
            IEntityBaseRepository<Frequency> frequencyRepository,
            IEntityBaseRepository<FrequencyDip> frequencyDipRepository,
            IEntityBaseRepository<FrequencyDipUnits> frequencyDipUnitsRepository,
            IEntityBaseRepository<FuelTank> fuelTankRepository,
            IEntityBaseRepository<FuelType> fuelTypeRepository,
            IEntityBaseRepository<LoadSequenceCyclic> loadSequenceCyclicRepository,
            IEntityBaseRepository<MaxRunningLoad> maxRunningLoadRepository,
            IEntityBaseRepository<MomentaryAllowableVoltageDistortion> momentaryAllowableVoltageDistortionRepository,
            IEntityBaseRepository<SolutionApplication> solutionApplicationRepository,
            IEntityBaseRepository<RegulatoryFilter> regulatoryFilterRepository,
            IEntityBaseRepository<Units> unitsRepository,
            IEntityBaseRepository<VoltageDip> voltageDipRepository,
            IEntityBaseRepository<VoltageDipUnits> voltageDipUnitsRepository,
            IEntityBaseRepository<VoltageNominal> voltageNominalRepository,
            IEntityBaseRepository<VoltagePhase> voltagePhaseRepository,
            IEntityBaseRepository<VoltageSpecific> voltageSpecificRepository,
            IEntityBaseRepository<LoadFamily> loadFamilyRepository,
            IEntityBaseRepository<State> stateRepository,
            IEntityBaseRepository<Country> countryRepository,
            IEntityBaseRepository<Brand> brandRepository,
            IEntityBaseRepository<UserCategory> userCategoryRepository,
            IEntityBaseRepository<MotorCalculation> motorCalculationRepository,
            IEntityBaseRepository<Load> loadRepository,
            IEntityBaseRepository<Sequence> sequenceRepository,
            IEntityBaseRepository<PF> pfRepository,
            IEntityBaseRepository<SizeUnits> sizeUnitsRepository,
            IEntityBaseRepository<HarmonicDeviceType> harmonicDeviceTypeRepository,
            IEntityBaseRepository<HarmonicContent> harmonicContentRepository,
            IEntityBaseRepository<Compressors> compressorsRepository,
            IEntityBaseRepository<CoolingLoad> coolingLoadRepository,
            IEntityBaseRepository<ReheatLoad> reheatLoadRepository,
            IEntityBaseRepository<LightingType> lightingTypeRepository,
            IEntityBaseRepository<Phase> phaseRepository,
            IEntityBaseRepository<Efficiency> efficiencyRepository,
            IEntityBaseRepository<ChargeRate> chargeRateRepository,
            IEntityBaseRepository<PowerFactor> powerFactorRepository,
            IEntityBaseRepository<UPSType> upsTypeRepository,
            IEntityBaseRepository<LoadLevel> loadLevelRepository,
            IEntityBaseRepository<WelderType> welderTypeRepository,
            IEntityBaseRepository<FamilySelectionMethod> familySelectionMethodRepository,
            IEntityBaseRepository<ProductFamily> productFamilyRepository,
            IEntityBaseRepository<SizingMethod> sizingMethodRepository,
            IEntityBaseRepository<Generator> generatorRepository,
            IEntityBaseRepository<ParallelQuantity> parallelQuantityRepository,
            IEntityBaseRepository<Alternator> alternatorRepository,
            IEntityBaseRepository<Documentation> documentationRepository,
            IEntityBaseRepository<AlternatorFamily> alternatorFamilyRepository,
            IMapper<BasePickListEntity, PickListDto> pickListEntityToDtoMapper,
            IMapper<VoltageNominal, VoltageNominalDto> voltageNominalEntityToDtoMapper,
            IMapper<VoltageSpecific, VoltageSpecificDto> voltageSpecificEntityToDtoMapper,
            IMapper<HarmonicDeviceType, HarmonicDeviceTypeDto> harmonicDeviceTypeEntityToDtoMapper,
            IMapper<State, StatePickListDto> statePickListEntityToDtoMapper,
            IEntityBaseRepository<MotorLoadLevel> motorLoadLevelRepository,
            IEntityBaseRepository<MotorLoadType> motorLoadTypeRepository,
            IEntityBaseRepository<MotorType> motorTypeRepository,
            IEntityBaseRepository<StartingCode> startingCodeRepository,
            IEntityBaseRepository<StartingMethod> startingMethodRepository,
            IEntityBaseRepository<ConfigurationInput> configurationInputRepository,
            //IEntityBaseRepository<ExhaustPipingPipeSize> exhaustPipingPipeSizeRepository
            IEntityBaseRepository<ExhaustSystemConfiguration> exhaustSystemConfigurationRepository,
            IEntityBaseRepository<HarmonicProfile> harmonicProfileRepository,
            IEntityBaseRepository<SearchFilter> searchFilterRepository
            )
        {
            _ambientTemperatureRepository = ambientTemperatureRepository;
            _continuousAllowableVoltageDistortionRepository = continuousAllowableVoltageDistortionRepository;
            _desiredRunTimeRepository = desiredRunTimeRepository;
            _desiredSoundRepository = desiredSoundRepository;
            _elevationRepository = elevationRepository;
            _enclosureTypeRepository = enclosureTypeRepository;
            _engineDutyRepository = engineDutyRepository;
            _frequencyRepository = frequencyRepository;
            _frequencyDipRepository = frequencyDipRepository;
            _frequencyDipUnitsRepository = frequencyDipUnitsRepository;
            _fuelTankRepository = fuelTankRepository;
            _fuelTypeRepository = fuelTypeRepository;
            _loadSequenceCyclicRepository = loadSequenceCyclicRepository;
            _maxRunningLoadRepository = maxRunningLoadRepository;
            _momentaryAllowableVoltageDistortionRepository = momentaryAllowableVoltageDistortionRepository;
            _solutionApplicationRepository = solutionApplicationRepository;
            _regulatoryFilterRepository = regulatoryFilterRepository;
            _unitsRepository = unitsRepository;
            _voltageDipRepository = voltageDipRepository;
            _voltageDipUnitsRepository = voltageDipUnitsRepository;
            _voltageNominalRepository = voltageNominalRepository;
            _voltagePhaseRepository = voltagePhaseRepository;
            _voltageSpecificRepository = voltageSpecificRepository;
            _loadFamilyRepository = loadFamilyRepository;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _brandRepository = brandRepository;
            _userCategoryRepository = userCategoryRepository;
            _motorCalculationRepository = motorCalculationRepository;
            _loadRepository = loadRepository;
            _sequenceRepository = sequenceRepository;
            _pfRepository = pfRepository;
            _sizeUnitsRepository = sizeUnitsRepository;
            _harmonicDeviceTypeRepository = harmonicDeviceTypeRepository;
            _harmonicContentRepository = harmonicContentRepository;
            _compressorsRepository = compressorsRepository;
            _coolingLoadRepository = coolingLoadRepository;
            _reheatLoadRepository = reheatLoadRepository;
            _lightingTypeRepository = lightingTypeRepository;
            _phaseRepository = phaseRepository;
            _efficiencyRepository = efficiencyRepository;
            _chargeRateRepository = chargeRateRepository;
            _powerFactorRepository = powerFactorRepository;
            _upsTypeRepository = upsTypeRepository;
            _loadLevelRepository = loadLevelRepository;
            _welderTypeRepository = welderTypeRepository;
            _familySelectionMethodRepository = familySelectionMethodRepository;
            _productFamilyRepository = productFamilyRepository;
            _parallelQuantityRepository = parallelQuantityRepository;
            _sizingMethodRepository = sizingMethodRepository;
            _generatorRepository = generatorRepository;
            _alternatorRepository = alternatorRepository;
            _documentationRepository = documentationRepository;
            _alternatorFamilyRepository = alternatorFamilyRepository;
            _pickListEntityToDtoMapper = pickListEntityToDtoMapper;
            _voltageSpecificEntityToDtoMapper = voltageSpecificEntityToDtoMapper;
            _voltageNominalEntityToDtoMapper = voltageNominalEntityToDtoMapper;
            _harmonicDeviceTypeEntityToDtoMapper = harmonicDeviceTypeEntityToDtoMapper;
            _statePickListEntityToDtoMapper = statePickListEntityToDtoMapper;
            _motorLoadLevelRepository = motorLoadLevelRepository;
            _motorLoadTypeRepository = motorLoadTypeRepository;
            _motorTypeRepository = motorTypeRepository;
            _startingCodeRepository = startingCodeRepository;
            _startingMethodRepository = startingMethodRepository;
            _configurationInputRepository = configurationInputRepository;
            //_exhaustPipingPipeSizeRepository = exhaustPipingPipeSizeRepository;
            _exhaustSystemConfigurationRepository = exhaustSystemConfigurationRepository;
            _harmonicProfileRepository = harmonicProfileRepository;
            _searchFilterRepository = searchFilterRepository;
        }

        public IEnumerable<PickListDto> GetSearchFilter()
        {
            return _searchFilterRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        #region Solution Setup PickList
        /// <summary>
        /// Gets the ambient temperature.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetAmbientTemperature()
        {
            return _ambientTemperatureRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the continuous allowable voltage distortion.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetContinuousAllowableVoltageDistortion()
        {
            return _continuousAllowableVoltageDistortionRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the desired run time.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DesiredRunTimeDto> GetDesiredRunTime()
        {
            return _desiredRunTimeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new DesiredRunTimeDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    FuelTypeID = x.FuelTypeID,
                    LanguageKey = x.LanguageKey,
                    IsDefaultSelection = x.IsDefaultSelection
                }).ToList();
        }

        /// <summary>
        /// Gets the desired sound.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetDesiredSound()
        {
            return _desiredSoundRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the elevation.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetElevation()
        {
            return _elevationRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the type of the enclosure.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetEnclosureType()
        {
            return _enclosureTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the engine duty.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetEngineDuty()
        {
            return _engineDutyRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the frequency.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetFrequency()
        {
            return _frequencyRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the frequency dip.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetFrequencyDip()
        {
            return _frequencyDipRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the frequency dip units.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetFrequencyDipUnits()
        {
            return _frequencyDipUnitsRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the fuel tank.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FuelTankDto> GetFuelTank()
        {
            return _fuelTankRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new FuelTankDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    FuelTypeID = x.FuelTypeID,
                    LanguageKey=x.LanguageKey,
                    IsDefaultSelection = x.IsDefaultSelection
                }).ToList();
        }

        /// <summary>
        /// Gets the type of the fuel.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetFuelType()
        {
            return _fuelTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        public IEnumerable<FuelTypeDto> GetFuelTypeCode()
        {
            return _fuelTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
               Select(x => new FuelTypeDto
               {
                   ID = x.ID,
                   Description = x.Description,
                   Value = x.Value,
                   LanguageKey=x.LanguageKey,
                   Code = x.Code
               }).ToList();
        }

        /// <summary>
        /// Gets the load sequence cyclic.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetLoadSequenceCyclic()
        {
            return _loadSequenceCyclicRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the maximum running load.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetMaxRunningLoad()
        {
            return _maxRunningLoadRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the momentary allowable voltage distortion.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetMomentaryAllowableVoltageDistortion()
        {
            return _momentaryAllowableVoltageDistortionRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the solution application.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetSolutionApplication()
        {
            return _solutionApplicationRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the regulatory filtor.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RegulatoryFilterDto> GetRegulatoryFilter()
        {
            return _regulatoryFilterRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new RegulatoryFilterDto
                {
                    Id = x.ID,
                    ItemName = x.Description,
                    LanguageKey=x.LanguageKey
                }).ToList();
        }

        /// <summary>
        /// Gets the units.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetUnits()
        {
            return _unitsRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the voltage dip.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetVoltageDip()
        {
            return _voltageDipRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the voltage dip units.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetVoltageDipUnits()
        {
            return _voltageDipUnitsRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the voltage nominal.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VoltageNominalDto> GetVoltageNominal(bool IsForLoads)
        {
            return _voltageNominalRepository.GetAll(vn => vn.IsForLoads == IsForLoads).OrderBy(x => x.Ordinal).ToList().
                Select(x => _voltageNominalEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the voltage phase.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetVoltagePhase()
        {
            return _voltagePhaseRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        /// <summary>
        /// Gets the voltage specific.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<VoltageSpecificDto> GetVoltageSpecific()
        {
            return _voltageSpecificRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _voltageSpecificEntityToDtoMapper.AddMap(x)).ToList();
        }


        public IEnumerable<StatePickListDto> GetState()
        {
            return _stateRepository.GetAll().OrderBy(x => x.Description).ToList().
                Select(x => _statePickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        public IEnumerable<CountryPickListDto> GetCountry()
        {
            return _countryRepository.GetAll().OrderBy(x => x.Description).ToList().
                Select(x => new CountryPickListDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    CountryCode = x.CountryCode,
                    LanguageKey=x.LanguageKey
                })
                .ToList();
        }

        public IEnumerable<PickListDto> GetBrand()
        {
            return _brandRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        public IEnumerable<PickListDto> GetUserCategory()
        {
            return _userCategoryRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        #endregion

        #region Load PickList

        /// <summary>
        /// Gets the load family.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PickListDto> GetLoadFamily()
        {
            return _loadFamilyRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new PickListDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    LanguageKey=x.LanguageKey
                });
        }

        public IEnumerable<LoadsDto> GetLoads()
        {
            return _loadRepository.AllIncluding(l => l.LoadFamily).OrderBy(x => x.Ordinal).ToList().
                Select(x => new LoadsDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    LoadFamilyID = x.LoadFamilyID,
                    IsDefaultSelection = x.IsDefaultSelection,
                    LanguageKey=x.LanguageKey,
                    LoadFamily = x.LoadFamily.Value
                });
        }

        public IEnumerable<SequenceDto> GetSequence()
        {
            return _sequenceRepository.AllIncluding(s => s.SequenceType).OrderBy(x => x.Ordinal).ToList().
                Select(x => new SequenceDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    LanguageKey=x.LanguageKey,
                    SequenceType = x.SequenceType.Value
                });
        }

        public IEnumerable<PickListDto> GetPF()
        {
            return _pfRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        public IEnumerable<MotorCalculationDto> GetMotorCalculation()
        {
            return _motorCalculationRepository.GetAll().OrderBy(x => x.HP).ToList().
                Select(x => new MotorCalculationDto
                {
                    HP=x.HP,
                    kWm=x.kWm,
                    KVARunning=x.KVARunning,
                    PFRunning=x.PFRunning,
                    PFStarting=x.PFStarting,
                    kVAHPStartingNema=x.kVAHPStartingNema,
                    KVAHPStartingIEC=x.KVAHPStartingIEC,
                    StartingCodeIDNema=x.StartingCodeIDNema,
                    StartingCodeIDIEC=x.StartingCodeIDIEC,
                    CalcReferenceIEC=x.CalcReferenceIEC
                });
        }

        public IEnumerable<SizeUnitsDto> GetSizeUnits()
        {
            return _sizeUnitsRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new SizeUnitsDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    LanguageKey=x.LanguageKey,
                    LoadFamilyID = x.LoadFamilyID
                });
        }

        public IEnumerable<HarmonicDeviceTypeDto> GetHarmonicDeviceType()
        {
            return _harmonicDeviceTypeRepository.GetAll(x=> x.Active).OrderBy(x => x.Ordinal).ToList().
                Select(x => _harmonicDeviceTypeEntityToDtoMapper.AddMap(x)).ToList();
        }

        public IEnumerable<PickListDto> GetHarmonicContent()
        {
            return _harmonicContentRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x)).ToList();
        }

        public IEnumerable<HarmonicContentDto> GetHarmonicContentStartingMethod()
        {
            return _harmonicContentRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new HarmonicContentDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    StartingMethodID = x.StartingMethodID,
                    LanguageKey=x.LanguageKey
                });
        }

        #endregion

        #region AC Load PickList

        public IEnumerable<PickListDto> GetCompressors()
        {
            return _compressorsRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetCoolingLoad()
        {
            return _coolingLoadRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetReheatLoad()
        {
            return _reheatLoadRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }
        #endregion

        #region Lighting Load PickList
        public IEnumerable<LightingTypeDto> GetLightingType()
        {
            return _lightingTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new LightingTypeDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    HarmonicDeviceTypeID = x.HarmonicDeviceTypeID,
                    RunningPFID = x.RunningPFID,
                    LanguageKey=x.LanguageKey,
                    HarmonicContentID = x.HarmonicContentID
                });
        }
        #endregion

        #region UPS Load PickList
        public IEnumerable<PickListDto> GetPhase()
        {
            return _phaseRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetEfficiency()
        {
            return _efficiencyRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetChargeRate()
        {
            return _chargeRateRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetPowerFactor()
        {
            return _powerFactorRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<UPSTypeDto> GetUPSType()
        {
            return _upsTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new UPSTypeDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    HarmonicDeviceTypeID = x.HarmonicDeviceTypeID,
                    PhaseID = x.PhaseID,
                    EfficiencyID = x.EfficiencyID,
                    LanguageKey=x.LanguageKey,
                    HarmonicContentID = x.HarmonicContentID
                });
        }

        public IEnumerable<PickListDto> GetLoadLevel()
        {
            return _loadLevelRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        #endregion

        #region Welder Load PickList
        public IEnumerable<WelderTypeDto> GetWelderType()
        {
            return _welderTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new WelderTypeDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    LanguageKey=x.LanguageKey,
                    HarmonicDeviceTypeID = x.HarmonicDeviceTypeID
                });
        }
        #endregion

        #region Motor Load PickList
        public IEnumerable<PickListDto> GetMotorLoadLevel()
        {
            return _motorLoadLevelRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetMotorLoadType()
        {
            return _motorLoadTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<MotorTypeDto> GetMotorType()
        {
            return _motorTypeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                 Select(x => new MotorTypeDto
                 {
                     ID = x.ID,
                     Description = x.Description,
                     Value = x.Value,
                     LanguageKey = x.LanguageKey,
                     StartingCodeID = x.StartingCodeID,
                 });
        }

        public IEnumerable<StartingCodeDto> GetStartingCode()
        {
            return _startingCodeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                 Select(x => new StartingCodeDto
                 {
                     ID = x.ID,
                     Description = x.Description,
                     Value = x.Value,
                     LanguageKey=x.LanguageKey,
                     KVAHPStarting = x.KVAHPStarting,
                     AmpsDescription = x.AmpsDescription,
                     KWMDescription = x.KWMDescription,
                     LanguageKeyKWM=x.LanguageKeyKWM,
                     LanguageKeyHP=x.LanguageKeyHP,
                 });
        }

        public IEnumerable<StartingMethodDto> GetStartingMethod()
        {
            return _startingMethodRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                 Select(x => new StartingMethodDto
                 {
                     ID = x.ID,
                     Description = x.Description,
                     Value = x.Value,
                     DefaultHarmonicTypeID = x.DefaultHarmonicTypeID,
                     VoltageDipID=x.VoltageDipID,
                     FrequencyDipID=x.FrequencyDipID,
                     MotorLoadTypeID=x.MotorLoadTypeID,
                     LanguageKey = x.LanguageKey,
                 });
        }

        public IEnumerable<ConfigurationInputDto> GetConfigurationInput()
        {
            return _configurationInputRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new ConfigurationInputDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    StartingMethodID = x.StartingMethodID,
                    LanguageKey=x.LanguageKey,
                    sKVAMultiplierOverride = x.sKVAMultiplierOverride,
                    sKWMultiplierOverride = x.sKWMultiplierOverride,
                    rKWMultiplierOverride = x.rKWMultiplierOverride,
                    IsDefaultSelection = x.IsDefaultSelection
                });
        }

        public IEnumerable<HarmonicContentDto> GetMotorHarmonicContent()
        {
            return _harmonicContentRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => new HarmonicContentDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    Value = x.Value,
                    LanguageKey=x.LanguageKey,
                    StartingMethodID = x.StartingMethodID
                });
        }
        #endregion

        #region SolutionSummary Recommended Product

        public IEnumerable<PickListDto> GetFamilySelectionMethod()
        {
            return _familySelectionMethodRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetProductFamily()
        {
            return _productFamilyRepository.GetAll(x=> x.Active).OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        //public IEnumerable<PickListDto> GetModuleSize()
        //{
        //    return _moduleSizeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
        //        Select(x => _pickListEntityToDtoMapper.AddMap(x));
        //}

        public IEnumerable<PickListDto> GetParallelQuantity()
        {
            return _parallelQuantityRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetSizingMethod()
        {
            return _sizingMethodRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<GeneratorPickListDto> GetGenerator(int productFamilyId)
        {
            return _generatorRepository.GetAll(x => x.ProductFamilyID == productFamilyId).ToList().
                Select(x => new GeneratorPickListDto
                {
                    ID = x.ID,
                    Description = x.InternalDescription,
                    ProductFamilyID = x.ProductFamilyID,
                    Value = x.InternalDescription
                });
        }

        public IEnumerable<VoltageNominalDto> GetVoltageNominal(int frequencyId)
        {
            return _voltageNominalRepository.GetAll(x => x.FrequencyID == frequencyId && !x.IsForLoads).ToList().
                Select(x => new VoltageNominalDto
                {
                    ID = x.ID,
                    VoltagePhaseID = x.VoltagePhaseID,
                    FrequencyID = x.FrequencyID,
                    Description = x.Description,
                    LanguageKey=x.LanguageKey,
                    Value = x.Value,
                    IsDefaultSelection=x.IsDefaultSelection
                });
        }

        public IEnumerable<AlternatorPickListDto> GetAlternator(int generatorId)
        {
            return _alternatorRepository.GetAll(x => x.GeneratorAvailableAlternators
                                                        .Where(y => y.GeneratorID == generatorId).Any()).ToList().
                Select(x => new AlternatorPickListDto
                {
                    ID = x.ID,
                    Description = x.InternalDescription,
                    GeneratorID = generatorId,
                    Value = x.InternalDescription
                });
        }

        public IEnumerable<SolutionSummaryGeneratorDocumentationDto> GetGeneratorDocuments(int generatorID)
        {
            return _documentationRepository.GetAll().Where(x => x.GeneratorID == generatorID).OrderBy(x => x.Ordinal).ToList()
                .Select(x => new SolutionSummaryGeneratorDocumentationDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    DocumentURL = x.DocumentURL,
                    LanguageKey=x.LanguageKey,
                    GeneratorID = x.GeneratorID
                });
        }

        #endregion

        //public IEnumerable<PickListDto> GetExhaustPipingPipeSize()
        //{
        //    return _exhaustPipingPipeSizeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
        //        Select(x => _pickListEntityToDtoMapper.AddMap(x));
        //}

        public IEnumerable<PickListDto> GetExhaustSystemConfiguration()
        {
            return _exhaustSystemConfigurationRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetAlternatorFamily()
        {
            return _alternatorFamilyRepository.GetAll(x=>x.Active).OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }

        public IEnumerable<PickListDto> GetHarmonicProfile()
        {
            return _harmonicProfileRepository.GetAll().OrderBy(x => x.Ordinal).ToList().
                Select(x => _pickListEntityToDtoMapper.AddMap(x));
        }
    }
}