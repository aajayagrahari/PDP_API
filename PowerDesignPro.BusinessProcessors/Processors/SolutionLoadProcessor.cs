using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using System;
using System.Dynamic;
using System.Linq;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    public class SolutionLoadProcessor : ISolutionLoad
    {
        private IEntityBaseRepository<LoadDefaults> _loadDefaultsRepository;
        private IEntityBaseRepository<BasicLoad> _basicLoadRepository;
        private IEntityBaseRepository<SolutionSetup> _solutionSetupRepository;
        private IEntityBaseRepository<Solution> _solutionRepository;
        private IMapper<BasicLoad, BasicLoadDto> _basicLoadEntityToBasicLoadDtoMapper;
        private IMapper<BasicLoad, LoadDefaultDto> _basicLoadEntityToLoadDefaultDtoMapper;
        private IMapper<LoadDefaults, BasicLoadDto> _loadDefaultsEntityToBasicLoadDtoMapper;
        private IMapper<LoadDefaults, LoadDefaultDto> _loadDefaultsEntityToLoadDefaultDtoMapper;
        private readonly IMapper<BasicLoadDto, BasicLoad> _addBasicLoadDtoToEntityMapper;

        private IEntityBaseRepository<ACLoad> _acLoadRepository;
        private IMapper<ACLoad, ACLoadDto> _acLoadEntityToacLoadDtoMapper;
        private IMapper<LoadDefaults, ACLoadDto> _loadDefaultsEntityToACLoadDtoMapper;
        private readonly IMapper<ACLoadDto, ACLoad> _addAcLoadDtoToEntityMapper;

        private IEntityBaseRepository<LightingLoad> _lightingLoadRepository;
        private IMapper<LightingLoad, LightingLoadDto> _lightingLoadEntityToLightingLoadDtoMapper;
        private IMapper<LoadDefaults, LightingLoadDto> _loadDefaultsEntityToLightingLoadDtoMapper;
        private IMapper<LightingLoadDto, LightingLoad> _addLightingLoadDtoToEntityMapper;

        private IEntityBaseRepository<UPSLoad> _upsLoadRepository;
        private IMapper<UPSLoad, UPSLoadDto> _upsLoadEntityToUpsLoadDtoMapper;
        private IMapper<LoadDefaults, UPSLoadDto> _loadDefaultsEntityToUpsLoadDtoMapper;
        private IMapper<UPSLoadDto, UPSLoad> _addUpsLoadDtoToEntityMapper;

        private IEntityBaseRepository<WelderLoad> _welderLoadRepository;
        private IMapper<WelderLoad, WelderLoadDto> _welderLoadEntityToWelderLoadDtoMapper;
        private IMapper<LoadDefaults, WelderLoadDto> _loadDefaultsEntityToWelderLoadDtoMapper;
        private IMapper<WelderLoadDto, WelderLoad> _addWelderLoadDtoToEntityMapper;

        private IEntityBaseRepository<MotorLoad> _motorLoadRepository;
        private IMapper<MotorLoad, MotorLoadDto> _motorLoadEntityToMotorLoadDtoMapper;
        private IMapper<LoadDefaults, MotorLoadDto> _loadDefaultsEntityToMotorLoadDtoMapper;
        private IMapper<MotorLoadDto, MotorLoad> _addMotorLoadDtoToEntityMapper;

        private readonly IPickList _pickList;

        public SolutionLoadProcessor(
            IEntityBaseRepository<LoadDefaults> loadDefaultsRepository,
            IEntityBaseRepository<BasicLoad> basicLoadRepository,
            IEntityBaseRepository<SolutionSetup> solutionSetupRepository,
            IEntityBaseRepository<Solution> solutionRepository,
            IMapper<BasicLoad, BasicLoadDto> basicLoadEntityToBasicLoadDtoMapper,
            IMapper<LoadDefaults, BasicLoadDto> loadDefaultsEntityToBasicLoadDtoMapper,
            IMapper<LoadDefaults, LoadDefaultDto> loadDefaultsEntityToLoadDefaultDtoMapper,
            IMapper<BasicLoadDto, BasicLoad> addBasicLoadDtoToEntityMapper,
            IEntityBaseRepository<ACLoad> acLoadRepository,
            IMapper<ACLoad, ACLoadDto> acLoadEntityToacLoadDtoMapper,
            IMapper<LoadDefaults, ACLoadDto> loadDefaultsEntityToACLoadDtoMapper,
            IMapper<ACLoadDto, ACLoad> addAcLoadDtoToEntityMapper,
            IMapper<BasicLoad, LoadDefaultDto> basicLoadEntityToLoadDefaultDtoMapper,
            IEntityBaseRepository<LightingLoad> lightingLoadRepository,
            IMapper<LightingLoad, LightingLoadDto> lightingLoadEntityToLightingLoadDtoMapper,
            IMapper<LoadDefaults, LightingLoadDto> loadDefaultsEntityToLightingLoadDtoMapper,
            IMapper<LightingLoadDto, LightingLoad> addLightingLoadDtoToEntityMapper,
            IEntityBaseRepository<UPSLoad> upsLoadRepository,
            IMapper<UPSLoad, UPSLoadDto> upsLoadEntityToUpsLoadDtoMapper,
            IMapper<LoadDefaults, UPSLoadDto> loadDefaultsEntityToUpsLoadDtoMapper,
            IMapper<UPSLoadDto, UPSLoad> addUpsLoadDtoToEntityMapper,
            IEntityBaseRepository<WelderLoad> welderLoadRepository,
            IMapper<WelderLoad, WelderLoadDto> welderLoadEntityToWelderLoadDtoMapper,
            IMapper<LoadDefaults, WelderLoadDto> loadDefaultsEntityToWelderLoadDtoMapper,
            IMapper<WelderLoadDto, WelderLoad> addWelderLoadDtoToEntityMapper,
            IEntityBaseRepository<MotorLoad> motorLoadRepository,
            IMapper<MotorLoad, MotorLoadDto> motorLoadEntityToMotorLoadDtoMapper,
            IMapper<LoadDefaults, MotorLoadDto> loadDefaultsEntityToMotorLoadDtoMapper,
            IMapper<MotorLoadDto, MotorLoad> addMotorLoadDtoToEntityMapper,
            IPickList pickList)
        {
            _loadDefaultsRepository = loadDefaultsRepository;
            _basicLoadRepository = basicLoadRepository;
            _solutionSetupRepository = solutionSetupRepository;
            _solutionRepository = solutionRepository;
            _basicLoadEntityToBasicLoadDtoMapper = basicLoadEntityToBasicLoadDtoMapper;
            _basicLoadEntityToLoadDefaultDtoMapper = basicLoadEntityToLoadDefaultDtoMapper;
            _loadDefaultsEntityToBasicLoadDtoMapper = loadDefaultsEntityToBasicLoadDtoMapper;
            _loadDefaultsEntityToLoadDefaultDtoMapper = loadDefaultsEntityToLoadDefaultDtoMapper;
            _addBasicLoadDtoToEntityMapper = addBasicLoadDtoToEntityMapper;
            _acLoadRepository = acLoadRepository;
            _acLoadEntityToacLoadDtoMapper = acLoadEntityToacLoadDtoMapper;
            _loadDefaultsEntityToACLoadDtoMapper = loadDefaultsEntityToACLoadDtoMapper;
            _addAcLoadDtoToEntityMapper = addAcLoadDtoToEntityMapper;
            _lightingLoadRepository = lightingLoadRepository;
            _lightingLoadEntityToLightingLoadDtoMapper = lightingLoadEntityToLightingLoadDtoMapper;
            _loadDefaultsEntityToLightingLoadDtoMapper = loadDefaultsEntityToLightingLoadDtoMapper;
            _addLightingLoadDtoToEntityMapper = addLightingLoadDtoToEntityMapper;
            _upsLoadRepository = upsLoadRepository;
            _upsLoadEntityToUpsLoadDtoMapper = upsLoadEntityToUpsLoadDtoMapper;
            _loadDefaultsEntityToUpsLoadDtoMapper = loadDefaultsEntityToUpsLoadDtoMapper;
            _addUpsLoadDtoToEntityMapper = addUpsLoadDtoToEntityMapper;
            _welderLoadRepository = welderLoadRepository;
            _welderLoadEntityToWelderLoadDtoMapper = welderLoadEntityToWelderLoadDtoMapper;
            _loadDefaultsEntityToWelderLoadDtoMapper = loadDefaultsEntityToWelderLoadDtoMapper;
            _addWelderLoadDtoToEntityMapper = addWelderLoadDtoToEntityMapper;
            _motorLoadRepository = motorLoadRepository;
            _motorLoadEntityToMotorLoadDtoMapper = motorLoadEntityToMotorLoadDtoMapper;
            _loadDefaultsEntityToMotorLoadDtoMapper = loadDefaultsEntityToMotorLoadDtoMapper;
            _addMotorLoadDtoToEntityMapper = addMotorLoadDtoToEntityMapper;
            _pickList = pickList;
        }

        public bool CheckLoadExistForSolution(int solutionId)
        {
            var solutionDetail = _solutionRepository.GetAll(x => x.ID == solutionId
                         & (x.BasicLoadList.Any()
                               || x.MotorLoadList.Any()
                               || x.WelderLoadList.Any()
                               || x.LightingLoadList.Any()
                               || x.ACLoadList.Any()
                               || x.UPSLoadList.Any())
                );

            return solutionDetail.Any();
        }

        #region Basic Load
        public BasicLoadDto GetLoadDetailsForBasicLoad(SearchBaseLoadRequestDto searchBasicLoadDto, string userName)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.AllIncluding(x => x.HarmonicDeviceType)
                                                    .Where(l => l.LoadID == searchBasicLoadDto.LoadID).FirstOrDefault();

            var voltageFrequencyDetail = _solutionSetupRepository.GetAll(x => x.SolutionID == searchBasicLoadDto.SolutionID)
                                                    .Select(r =>
                                                    new
                                                    {
                                                        r.VoltagePhaseID,
                                                        r.VoltageNominalID,
                                                        r.VoltageSpecificID,
                                                        r.FrequencyID,
                                                        r.Frequency.Value
                                                    }).FirstOrDefault();
            if (solutionLoadDefaultDetail == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.SolutionLoad);
            }
            solutionLoadDefaultDetail.ID = 0;
            if (searchBasicLoadDto.ID != 0)
            {
                var basicLoadDetail = _basicLoadRepository.GetSingle(x => x.ID == searchBasicLoadDto.ID);

                if (basicLoadDetail == null)
                {
                    throw new PowerDesignProException("BasicLoadNotFound", Message.SolutionLoad);
                }


                var result = _basicLoadEntityToBasicLoadDtoMapper.AddMap(basicLoadDetail, userName: userName);

                result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);
                //result.HarmonicContentID = solutionLoadDefaultDetail.HarmonicDeviceType.HarmonicContentID;

                result.RunningPFEditable = solutionLoadDefaultDetail.RunningPFEditable;
                result.SizeStartingEditable = solutionLoadDefaultDetail.SizeStartingEditable;
                result.SizeRunningEditable = solutionLoadDefaultDetail.SizeRunningEditable;
                result.StartingPFEditable = solutionLoadDefaultDetail.StartingPFEditable;
                result.HarmonicTypeEditable = solutionLoadDefaultDetail.HarmonicTypeEditable;
                result.StartingMethodID = solutionLoadDefaultDetail.StartingMethodID;

                result.PFStarting = basicLoadDetail.StartingPF != null ? Convert.ToDecimal(basicLoadDetail.StartingPF.Value) : 0;
                result.PFRunning = basicLoadDetail.RunningPF != null ? Convert.ToDecimal(basicLoadDetail.RunningPF.Value) : 0;
                result.SizeRunningUnits = basicLoadDetail.SizeRunningUnits?.Value;
                result.SizeStartingUnits = basicLoadDetail.SizeRunningUnits?.Value;
                result.VoltagePhase = basicLoadDetail.VoltagePhase != null ? Convert.ToInt32(basicLoadDetail.VoltagePhase.Value) : 0;
                result.VoltageSpecific = basicLoadDetail.VoltageSpecific != null ? Convert.ToInt32(basicLoadDetail.VoltageSpecific.Value) : 0;
                result.LoadSequenceType = basicLoadDetail.Sequence?.SequenceType.Value;

                return result;
            }
            else
            {
                var result = _loadDefaultsEntityToBasicLoadDtoMapper.AddMap(solutionLoadDefaultDetail);
                if (result != null)
                {
                    var countLoad = _basicLoadRepository.GetAll(x => x.LoadID == solutionLoadDefaultDetail.LoadID && x.SolutionID == searchBasicLoadDto.SolutionID).Count();
                    result.Description = string.Concat(solutionLoadDefaultDetail.Load.Description, " #", countLoad+1);

                    var voltageNominalSpecificLoads = GetVoltageNominalSpecificForLoads(voltageFrequencyDetail.VoltageNominalID, voltageFrequencyDetail.VoltageSpecificID, voltageFrequencyDetail.VoltagePhaseID,
                                                                                        voltageFrequencyDetail.FrequencyID);
                    result.VoltagePhaseID = voltageFrequencyDetail.VoltagePhaseID;
                    result.VoltageNominalID = voltageNominalSpecificLoads.VoltageNominalID;
                    result.VoltageSpecificID = voltageNominalSpecificLoads.VoltageSpecificID;
                    result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                    result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);
                    result.HarmonicContentID = solutionLoadDefaultDetail.HarmonicDeviceType.HarmonicContentID;
                    result.StartingMethodID = solutionLoadDefaultDetail.StartingMethodID;

                    return result;
                }
            }

            return new BasicLoadDto();
        }

        private dynamic GetVoltageNominalSpecificForLoads(int solutionVoltageNominalID, int solutionVoltageSpecificID, int solutionVoltagePhaseID, int solutionFrequencyID)
        {
            int voltageNominalIDLoads = 0;
            dynamic voltageNominalSpecific = new ExpandoObject();

            var voltageNominalValueSolutionSetup = int.Parse(_pickList.GetVoltageNominal(false).Where(x => x.ID == solutionVoltageNominalID && x.VoltagePhaseID == solutionVoltagePhaseID
                                                                                                      && x.FrequencyID == solutionFrequencyID).FirstOrDefault().Value);

            var voltagePhaseSolutionSetup = _pickList.GetVoltagePhase().Where(x => x.ID == solutionVoltagePhaseID).FirstOrDefault().Value;

            if (int.Parse(voltagePhaseSolutionSetup) == 1)
                voltageNominalValueSolutionSetup *= 2;

            var voltageNominalLoads = _pickList.GetVoltageNominal(true).Where(x => x.Value == voltageNominalValueSolutionSetup.ToString() && x.VoltagePhaseID == solutionVoltagePhaseID
                                                                                && x.FrequencyID == solutionFrequencyID).FirstOrDefault();
            if(voltageNominalLoads == null)
            {
                voltageNominalSpecific.VoltageNominalID = null;
                voltageNominalSpecific.VoltageSpecificID = null;

                return voltageNominalSpecific;
            }
            voltageNominalIDLoads = voltageNominalLoads.ID;
            voltageNominalSpecific.VoltageNominalID = voltageNominalIDLoads;
            voltageNominalSpecific.VoltageSpecificID = GetVoltageSpecificForLoads(solutionVoltageNominalID, solutionVoltageSpecificID, voltageNominalIDLoads);

            if(voltageNominalSpecific.VoltageSpecificID == 0)
            {
                voltageNominalSpecific.VoltageNominalID = null;
                voltageNominalSpecific.VoltageSpecificID = null;

                return voltageNominalSpecific;
            }

            return voltageNominalSpecific;
        }

        private int GetVoltageSpecificForLoads(int solutionVoltageNominalID, int solutionVoltageSpecificID, int loadsVoltageNominalID)
        {
            var voltageSpecificValueSolutionSetup = _pickList.GetVoltageSpecific().Where(x => x.ID == solutionVoltageSpecificID && x.VoltageNominalID == solutionVoltageNominalID).FirstOrDefault().Value;

            var voltageSpecificLoads = _pickList.GetVoltageSpecific().Where(x => x.VoltageNominalID == loadsVoltageNominalID && x.Value == voltageSpecificValueSolutionSetup).FirstOrDefault();

            if (voltageSpecificLoads == null)
                return 0;

            return voltageSpecificLoads.ID;
        }

        public BasicLoadDto SaveSolutionBasicLoad(BasicLoadDto basicLoadDto, string userID, string userName)
        {
            if (basicLoadDto.ID == 0)
            {
                return AddSolutionLoad(basicLoadDto, userID, userName);
            }
            else
            {
                return UpdateSolutionLoad(basicLoadDto, userID, userName);
            }
        }
        #endregion

        public dynamic GetSolutionDetail(int solutionId)
        {
            dynamic solution = new ExpandoObject();
            //solution = _solutionRepository.GetAll(x => x.ID == solutionId).Select(r => new { r.SolutionName }).FirstOrDefault();
            solution.SolutionName = _solutionRepository.GetAll(x => x.ID == solutionId).FirstOrDefault().SolutionName;
            return solution;
        }

        #region Delete All Load
        public bool DeleteSolutionLoad(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            switch (loadFamilyID)
            {
                case (int)SolutionLoadFamilyEnum.Basic:
                    var solutionLoad = _basicLoadRepository.GetSingle(x => x.ID == solutionLoadID && x.Solution.ID == solutionID);
                    if (solutionLoad == null)
                    {
                        throw new PowerDesignProException("BasicLoadNotFound", Message.SolutionLoad);
                    }
                    _basicLoadRepository.Delete(solutionLoad);
                    _basicLoadRepository.Commit();

                    return true;
                case (int)SolutionLoadFamilyEnum.AC:
                    var solutionACLoad = _acLoadRepository.GetSingle(x => x.ID == solutionLoadID && x.Solution.ID == solutionID);
                    if (solutionACLoad == null)
                    {
                        throw new PowerDesignProException("ACLoadNotFound", Message.SolutionLoad);
                    }
                    _acLoadRepository.Delete(solutionACLoad);
                    _acLoadRepository.Commit();

                    return true;
                case (int)SolutionLoadFamilyEnum.Lighting:
                    var solutionLightingLoad = _lightingLoadRepository.GetSingle(x => x.ID == solutionLoadID && x.Solution.ID == solutionID);
                    if (solutionLightingLoad == null)
                    {
                        throw new PowerDesignProException("LightingLoadNotFound", Message.SolutionLoad);
                    }
                    _lightingLoadRepository.Delete(solutionLightingLoad);
                    _lightingLoadRepository.Commit();

                    return true;
                case (int)SolutionLoadFamilyEnum.UPS:
                    var solutionUPSLoad = _upsLoadRepository.GetSingle(x => x.ID == solutionLoadID && x.Solution.ID == solutionID);
                    if (solutionUPSLoad == null)
                    {
                        throw new PowerDesignProException("UpsLoadNotFound", Message.SolutionLoad);
                    }
                    _upsLoadRepository.Delete(solutionUPSLoad);
                    _upsLoadRepository.Commit();

                    return true;
                case (int)SolutionLoadFamilyEnum.Welder:
                    var solutionWelderLoad = _welderLoadRepository.GetSingle(x => x.ID == solutionLoadID && x.Solution.ID == solutionID);
                    if (solutionWelderLoad == null)
                    {
                        throw new PowerDesignProException("WelderLoadNotFound", Message.SolutionLoad);
                    }
                    _welderLoadRepository.Delete(solutionWelderLoad);
                    _welderLoadRepository.Commit();

                    return true;
                case (int)SolutionLoadFamilyEnum.Motor:
                    var solutionMotorLoad = _motorLoadRepository.GetSingle(x => x.ID == solutionLoadID && x.Solution.ID == solutionID);
                    if (solutionMotorLoad == null)
                    {
                        throw new PowerDesignProException("MotorLoadNotFound", Message.SolutionLoad);
                    }
                    _motorLoadRepository.Delete(solutionMotorLoad);
                    _motorLoadRepository.Commit();

                    return true;
                default:
                    throw new PowerDesignProException("LoadFamilyNotFound", Message.SolutionLoad);
            }
        }
        #endregion

        #region AC Load
        public ACLoadDto GetLoadDetailsForACLoad(SearchBaseLoadRequestDto searchACLoadDto, string userName)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.GetSingle(l => l.LoadID == searchACLoadDto.LoadID);

            if (solutionLoadDefaultDetail == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.SolutionLoad);
            }

            var voltageFrequencyDetail = _solutionSetupRepository.GetAll(x => x.SolutionID == searchACLoadDto.SolutionID)
                                        .Select(r =>
                                        new
                                        {
                                            VoltageSpecific = r.VoltageSpecific.Value,
                                            Frequency = r.Frequency.Value
                                        }).FirstOrDefault();

            var voltageSpecific = voltageFrequencyDetail.VoltageSpecific;
            var frequency = voltageFrequencyDetail.Frequency;

            solutionLoadDefaultDetail.ID = 0;
            if (searchACLoadDto.ID != 0)
            {
                var acLoadDetail = _acLoadRepository.GetSingle(x => x.ID == searchACLoadDto.ID);

                if (acLoadDetail == null)
                {
                    throw new PowerDesignProException("ACLoadNotFound", Message.SolutionLoad);
                }

                var result = _acLoadEntityToacLoadDtoMapper.AddMap(acLoadDetail, userName: userName);
                result.VoltageSpecific = int.Parse(voltageSpecific);
                result.Frequency = int.Parse(frequency);

                return result;
            }
            else
            {
                var result = _loadDefaultsEntityToACLoadDtoMapper.AddMap(solutionLoadDefaultDetail);
                if (result != null)
                {
                    var countLoad = _acLoadRepository.GetAll(x => x.LoadID == solutionLoadDefaultDetail.LoadID && x.SolutionID == searchACLoadDto.SolutionID).Count();
                    result.Description = string.Concat(solutionLoadDefaultDetail.Load.Description, " #", countLoad + 1);

                    result.VoltageSpecific = int.Parse(voltageSpecific);
                    result.Frequency = int.Parse(frequency);

                    return result;
                }
            }

            return new ACLoadDto();
        }

        public ACLoadDto SaveSolutionACLoad(ACLoadDto acLoadDto, string userID, string userName)
        {
            if (acLoadDto.ID == 0)
            {
                return AddSolutionACLoad(acLoadDto, userID, userName);
            }
            else
            {
                return UpdateSolutionACLoad(acLoadDto, userID, userName);
            }
        }
        #endregion

        #region Lighting Load
        public LightingLoadDto GetLoadDetailsForLightingLoad(SearchBaseLoadRequestDto searchLightingLoadDto, string userName)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.GetSingle(l => l.LoadID == searchLightingLoadDto.LoadID);

            if (solutionLoadDefaultDetail == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.SolutionLoad);
            }

            var voltageFrequencyDetail = _solutionSetupRepository.GetAll(x => x.SolutionID == searchLightingLoadDto.SolutionID)
                                        .Select(r =>
                                        new
                                        {
                                            r.VoltagePhaseID,
                                            r.VoltageNominalID,
                                            r.VoltageSpecificID,
                                            r.FrequencyID,
                                            r.Frequency.Value
                                        }).FirstOrDefault();

            solutionLoadDefaultDetail.ID = 0;
            if (searchLightingLoadDto.ID != 0)
            {
                var lightingLoadDetail = _lightingLoadRepository.GetSingle(x => x.ID == searchLightingLoadDto.ID);

                if (lightingLoadDetail == null)
                {
                    throw new PowerDesignProException("LightingLoadNotFound", Message.SolutionLoad);
                }

                var result = _lightingLoadEntityToLightingLoadDtoMapper.AddMap(lightingLoadDetail, userName: userName);
                result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);
                //result.HarmonicContentID = solutionLoadDefaultDetail.HarmonicDeviceType.HarmonicContentID;

                result.RunningPFEditable = solutionLoadDefaultDetail.RunningPFEditable;
                result.SizeRunningEditable = solutionLoadDefaultDetail.SizeRunningEditable;
                result.HarmonicTypeEditable = solutionLoadDefaultDetail.HarmonicTypeEditable;
                result.StartingMethodID = solutionLoadDefaultDetail.StartingMethodID;

                return result;
            }
            else
            {
                var result = _loadDefaultsEntityToLightingLoadDtoMapper.AddMap(solutionLoadDefaultDetail);
                if (result != null)
                {
                    var countLoad = _lightingLoadRepository.GetAll(x => x.LoadID == solutionLoadDefaultDetail.LoadID && x.SolutionID == searchLightingLoadDto.SolutionID).Count();
                    result.Description = string.Concat(solutionLoadDefaultDetail.Load.Description, " #", countLoad + 1);

                    var voltageNominalSpecificLoads = GetVoltageNominalSpecificForLoads(voltageFrequencyDetail.VoltageNominalID, voltageFrequencyDetail.VoltageSpecificID, voltageFrequencyDetail.VoltagePhaseID,
                                                                                        voltageFrequencyDetail.FrequencyID);

                    result.VoltagePhaseID = voltageFrequencyDetail.VoltagePhaseID;
                    result.VoltageNominalID = voltageNominalSpecificLoads.VoltageNominalID;
                    result.VoltageSpecificID = voltageNominalSpecificLoads.VoltageSpecificID;
                    result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                    result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);
                    result.HarmonicContentID = solutionLoadDefaultDetail.HarmonicDeviceType.HarmonicContentID;
                    result.StartingMethodID = solutionLoadDefaultDetail.StartingMethodID;

                    return result;
                }
            }

            return new LightingLoadDto();
        }

        public LightingLoadDto SaveSolutionLightingLoad(LightingLoadDto lightingLoadDto, string userID, string userName)
        {
            if (lightingLoadDto.ID == 0)
            {
                return AddSolutionLightingLoad(lightingLoadDto, userID, userName);
            }
            else
            {
                return UpdateSolutionLightingLoad(lightingLoadDto, userID, userName);
            }
        }
        #endregion

        #region UPS Load
        public UPSLoadDto GetLoadDetailsForUpsLoad(SearchBaseLoadRequestDto searchUpsLoadDto, string userName)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.GetSingle(l => l.LoadID == searchUpsLoadDto.LoadID);

            if (solutionLoadDefaultDetail == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.SolutionLoad);
            }

            var voltageFrequencyDetail = _solutionSetupRepository.GetAll(x => x.SolutionID == searchUpsLoadDto.SolutionID)
                                        .Select(r =>
                                        new
                                        {
                                            VoltageSpecific = r.VoltageSpecific.Value,
                                            Frequency = r.Frequency.Value
                                        }).FirstOrDefault();
            
            solutionLoadDefaultDetail.ID = 0;
            if (searchUpsLoadDto.ID != 0)
            {
                var upsLoadDetail = _upsLoadRepository.GetSingle(x => x.ID == searchUpsLoadDto.ID);

                if (upsLoadDetail == null)
                {
                    throw new PowerDesignProException("UpsLoadNotFound", Message.SolutionLoad);
                }

                var result = _upsLoadEntityToUpsLoadDtoMapper.AddMap(upsLoadDetail, userName: userName);
                result.VoltageSpecific = int.Parse(voltageFrequencyDetail.VoltageSpecific);
                result.Frequency = int.Parse(voltageFrequencyDetail.Frequency);

                return result;
            }
            else
            {
                var result = _loadDefaultsEntityToUpsLoadDtoMapper.AddMap(solutionLoadDefaultDetail);
                if (result != null)
                {
                    var countLoad = _upsLoadRepository.GetAll(x => x.LoadID == solutionLoadDefaultDetail.LoadID && x.SolutionID == searchUpsLoadDto.SolutionID).Count();
                    result.Description = string.Concat(solutionLoadDefaultDetail.Load.Description, " #", countLoad + 1);

                    result.VoltageSpecific = int.Parse(voltageFrequencyDetail.VoltageSpecific);
                    result.Frequency = int.Parse(voltageFrequencyDetail.Frequency);
                    result.HarmonicContentID = solutionLoadDefaultDetail.HarmonicDeviceType.HarmonicContentID;

                    return result;
                }
            }

            return new UPSLoadDto();
        }

        public UPSLoadDto SaveSolutionUpsLoad(UPSLoadDto upsLoadDto, string userID, string userName)
        {
            if (upsLoadDto.ID == 0)
            {
                return AddSolutionUpsLoad(upsLoadDto, userID, userName);
            }
            else
            {
                return UpdateSolutionUpsLoad(upsLoadDto, userID, userName);
            }
        }
        #endregion

        #region Welder Load
        public WelderLoadDto GetLoadDetailsForWelderLoad(SearchBaseLoadRequestDto searchWelderLoadDto, string userName)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.GetSingle(l => l.LoadID == searchWelderLoadDto.LoadID);

            if (solutionLoadDefaultDetail == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.SolutionLoad);
            }

            var voltageFrequencyDetail = _solutionSetupRepository.GetAll(x => x.SolutionID == searchWelderLoadDto.SolutionID)
                                        .Select(r =>
                                        new
                                        {
                                            r.VoltagePhaseID,
                                            r.VoltageNominalID,
                                            r.VoltageSpecificID,
                                            r.FrequencyID,
                                            r.Frequency.Value
                                        }).FirstOrDefault();

            solutionLoadDefaultDetail.ID = 0;
            if (searchWelderLoadDto.ID != 0)
            {
                var welderLoadDetail = _welderLoadRepository.GetSingle(x => x.ID == searchWelderLoadDto.ID);

                if (welderLoadDetail == null)
                {
                    throw new PowerDesignProException("WelderLoadNotFound", Message.SolutionLoad);
                }

                var result = _welderLoadEntityToWelderLoadDtoMapper.AddMap(welderLoadDetail, userName: userName);
                result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);

                result.RunningPFEditable = solutionLoadDefaultDetail.RunningPFEditable;
                result.SizeRunningEditable = solutionLoadDefaultDetail.SizeRunningEditable;                
                result.HarmonicTypeEditable = solutionLoadDefaultDetail.HarmonicTypeEditable;
                result.StartingMethodID = solutionLoadDefaultDetail.StartingMethodID;

                return result;
            }
            else
            {
                var result = _loadDefaultsEntityToWelderLoadDtoMapper.AddMap(solutionLoadDefaultDetail);
                if (result != null)
                {
                    var countLoad = _welderLoadRepository.GetAll(x => x.LoadID == solutionLoadDefaultDetail.LoadID && x.SolutionID == searchWelderLoadDto.SolutionID).Count();
                    result.Description = string.Concat(solutionLoadDefaultDetail.Load.Description, " #", countLoad + 1);

                    var voltageNominalSpecificLoads = GetVoltageNominalSpecificForLoads(voltageFrequencyDetail.VoltageNominalID, voltageFrequencyDetail.VoltageSpecificID, voltageFrequencyDetail.VoltagePhaseID,
                                                                                        voltageFrequencyDetail.FrequencyID);

                    result.VoltagePhaseID = voltageFrequencyDetail.VoltagePhaseID;
                    result.VoltageNominalID = voltageNominalSpecificLoads.VoltageNominalID;
                    result.VoltageSpecificID = voltageNominalSpecificLoads.VoltageSpecificID;
                    result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                    result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);
                    result.HarmonicContentID = solutionLoadDefaultDetail.HarmonicDeviceType.HarmonicContentID;
                    result.StartingMethodID = solutionLoadDefaultDetail.StartingMethodID;

                    return result;
                }
            }

            return new WelderLoadDto();
        }

        public WelderLoadDto SaveSolutionWelderLoad(WelderLoadDto welderLoadDto, string userID, string userName)
        {
            if (welderLoadDto.ID == 0)
            {
                return AddSolutionWelderLoad(welderLoadDto, userID, userName);
            }
            else
            {
                return UpdateSolutionWelderLoad(welderLoadDto, userID, userName);
            }
        }
        #endregion

        #region Motor Load
        public MotorLoadDto GetLoadDetailsForMotorLoad(SearchBaseLoadRequestDto searchMotorLoadDto, string userName)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.GetSingle(l => l.LoadID == searchMotorLoadDto.LoadID);

            if (solutionLoadDefaultDetail == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.SolutionLoad);
            }

            var voltageFrequencyDetail = _solutionSetupRepository.GetAll(x => x.SolutionID == searchMotorLoadDto.SolutionID)
                                        .Select(r =>
                                        new
                                        {
                                            r.VoltagePhaseID,
                                            r.VoltageNominalID,
                                            r.VoltageSpecificID,
                                            r.FrequencyID,
                                            r.Frequency.Value
                                        }).FirstOrDefault();

            solutionLoadDefaultDetail.ID = 0;
            if (searchMotorLoadDto.ID != 0)
            {
                var motorLoadDetail = _motorLoadRepository.GetSingle(x => x.ID == searchMotorLoadDto.ID);

                if (motorLoadDetail == null)
                {
                    throw new PowerDesignProException("MotorLoadNotFound", Message.SolutionLoad);
                }

                var result = _motorLoadEntityToMotorLoadDtoMapper.AddMap(motorLoadDetail, userName: userName);
                result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);
                result.StartingMethodEditable = solutionLoadDefaultDetail.StartingMethodEditable;
                result.ConfigurationInputEditable = solutionLoadDefaultDetail.ConfigurationInputEditable;
                result.MotorLoadLevelEditable = solutionLoadDefaultDetail.MotorLoadLevelEditable;
                result.MotorLoadTypeEditable = solutionLoadDefaultDetail.MotorLoadTypeEditable;
                result.MotorTypeEditable = solutionLoadDefaultDetail.MotorTypeEditable;
                result.StartingCodeEditable = solutionLoadDefaultDetail.StartingCodeEditable;
                result.SizeRunningEditable = solutionLoadDefaultDetail.SizeRunningEditable;
                result.HarmonicTypeEditable = solutionLoadDefaultDetail.HarmonicTypeEditable;

                result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                return result;
            }
            else
            {
                var result = _loadDefaultsEntityToMotorLoadDtoMapper.AddMap(solutionLoadDefaultDetail);
                if (result != null)
                {
                    var countLoad = _motorLoadRepository.GetAll(x => x.LoadID == solutionLoadDefaultDetail.LoadID && x.SolutionID == searchMotorLoadDto.SolutionID).Count();
                    result.Description = string.Concat(solutionLoadDefaultDetail.Load.Description, " #", countLoad + 1);

                    var voltageNominalSpecificLoads = GetVoltageNominalSpecificForLoads(voltageFrequencyDetail.VoltageNominalID, voltageFrequencyDetail.VoltageSpecificID, voltageFrequencyDetail.VoltagePhaseID,
                                                                                        voltageFrequencyDetail.FrequencyID);

                    result.VoltagePhaseID = voltageFrequencyDetail.VoltagePhaseID;
                    result.VoltageNominalID = voltageNominalSpecificLoads.VoltageNominalID;
                    result.VoltageSpecificID = voltageNominalSpecificLoads.VoltageSpecificID;
                    result.Frequency = Convert.ToInt32(voltageFrequencyDetail.Value);
                    result.FrequencyID = voltageFrequencyDetail.FrequencyID;
                    result.HarmonicContentID = solutionLoadDefaultDetail.HarmonicDeviceType.HarmonicContentID;
                    result.StartingMethodID = solutionLoadDefaultDetail.StartingMethodID;

                    return result;
                }
            }

            return new MotorLoadDto();
        }

        public MotorLoadDto SaveSolutionMotorLoad(MotorLoadDto motorLoadDto, string userID, string userName)
        {
            if (motorLoadDto.ID == 0)
            {
                return AddSolutionMotorLoad(motorLoadDto, userID, userName);
            }
            else
            {
                return UpdateSolutionMotorLoad(motorLoadDto, userID, userName);
            }
        }
        #endregion

        #region Private Method
        private BasicLoadDto AddSolutionLoad(BasicLoadDto basicLoadDto, string userID, string userName)
        {
            var solutionLoad = _addBasicLoadDtoToEntityMapper.AddMap(basicLoadDto, userID, userName);
            solutionLoad.CreatedDateTime = DateTime.UtcNow;
            solutionLoad.CreatedBy = userName;
            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _basicLoadRepository.Add(solutionLoad);
            _basicLoadRepository.Commit();

            return new BasicLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private BasicLoadDto UpdateSolutionLoad(BasicLoadDto basicLoadDto, string userID, string userName)
        {
            var solutionLoad = _basicLoadRepository.Find(basicLoadDto.ID);

            _addBasicLoadDtoToEntityMapper.UpdateMap(basicLoadDto, solutionLoad, userID, userName);

            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _basicLoadRepository.Update(solutionLoad);
            _basicLoadRepository.Commit();

            return new BasicLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private ACLoadDto AddSolutionACLoad(ACLoadDto acLoadDto, string userID, string userName)
        {
            var solutionLoad = _addAcLoadDtoToEntityMapper.AddMap(acLoadDto, userID, userName);
            solutionLoad.CreatedDateTime = DateTime.UtcNow;
            solutionLoad.CreatedBy = userName;
            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _acLoadRepository.Add(solutionLoad);
            _acLoadRepository.Commit();

            return new ACLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private ACLoadDto UpdateSolutionACLoad(ACLoadDto acLoadDto, string userID, string userName)
        {
            var solutionLoad = _acLoadRepository.Find(acLoadDto.ID);

            _addAcLoadDtoToEntityMapper.UpdateMap(acLoadDto, solutionLoad, userID, userName);

            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _acLoadRepository.Update(solutionLoad);
            _acLoadRepository.Commit();

            return new ACLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private LightingLoadDto AddSolutionLightingLoad(LightingLoadDto lightingLoadDto, string userID, string userName)
        {
            var solutionLoad = _addLightingLoadDtoToEntityMapper.AddMap(lightingLoadDto, userID, userName);
            solutionLoad.CreatedDateTime = DateTime.UtcNow;
            solutionLoad.CreatedBy = userName;
            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _lightingLoadRepository.Add(solutionLoad);
            _lightingLoadRepository.Commit();

            return new LightingLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private LightingLoadDto UpdateSolutionLightingLoad(LightingLoadDto lightingLoadDto, string userID, string userName)
        {
            var solutionLoad = _lightingLoadRepository.Find(lightingLoadDto.ID);

            _addLightingLoadDtoToEntityMapper.UpdateMap(lightingLoadDto, solutionLoad, userID, userName);

            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _lightingLoadRepository.Update(solutionLoad);
            _lightingLoadRepository.Commit();

            return new LightingLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private UPSLoadDto AddSolutionUpsLoad(UPSLoadDto upsLoadDto, string userID, string userName)
        {
            var solutionLoad = _addUpsLoadDtoToEntityMapper.AddMap(upsLoadDto, userID, userName);
            solutionLoad.CreatedDateTime = DateTime.UtcNow;
            solutionLoad.CreatedBy = userName;
            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _upsLoadRepository.Add(solutionLoad);
            _upsLoadRepository.Commit();

            return new UPSLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private UPSLoadDto UpdateSolutionUpsLoad(UPSLoadDto upsLoadDto, string userID, string userName)
        {
            var solutionLoad = _upsLoadRepository.Find(upsLoadDto.ID);

            _addUpsLoadDtoToEntityMapper.UpdateMap(upsLoadDto, solutionLoad, userID, userName);

            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _upsLoadRepository.Update(solutionLoad);
            _upsLoadRepository.Commit();

            return new UPSLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private WelderLoadDto AddSolutionWelderLoad(WelderLoadDto welderLoadDto, string userID, string userName)
        {
            var solutionLoad = _addWelderLoadDtoToEntityMapper.AddMap(welderLoadDto, userID, userName);
            solutionLoad.CreatedDateTime = DateTime.UtcNow;
            solutionLoad.CreatedBy = userName;
            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _welderLoadRepository.Add(solutionLoad);
            _welderLoadRepository.Commit();

            return new WelderLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private WelderLoadDto UpdateSolutionWelderLoad(WelderLoadDto welderLoadDto, string userID, string userName)
        {
            var solutionLoad = _welderLoadRepository.Find(welderLoadDto.ID);

            _addWelderLoadDtoToEntityMapper.UpdateMap(welderLoadDto, solutionLoad, userID, userName);

            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _welderLoadRepository.Update(solutionLoad);
            _welderLoadRepository.Commit();

            return new WelderLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private MotorLoadDto AddSolutionMotorLoad(MotorLoadDto motorLoadDto, string userID, string userName)
        {
            var solutionLoad = _addMotorLoadDtoToEntityMapper.AddMap(motorLoadDto, userID, userName);
            solutionLoad.CreatedDateTime = DateTime.UtcNow;
            solutionLoad.CreatedBy = userName;
            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _motorLoadRepository.Add(solutionLoad);
            _motorLoadRepository.Commit();

            return new MotorLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }

        private MotorLoadDto UpdateSolutionMotorLoad(MotorLoadDto motorLoadDto, string userID, string userName)
        {
            var solutionLoad = _motorLoadRepository.Find(motorLoadDto.ID);

            _addMotorLoadDtoToEntityMapper.UpdateMap(motorLoadDto, solutionLoad, userID, userName);

            solutionLoad.ModifiedDateTime = DateTime.UtcNow;
            solutionLoad.ModifiedBy = userName;

            var solutionLoadDetail = _motorLoadRepository.Update(solutionLoad);
            _motorLoadRepository.Commit();

            return new MotorLoadDto
            {
                ID = solutionLoadDetail.ID,
                Description = solutionLoadDetail.Description
            };
        }
        #endregion
    }
}
