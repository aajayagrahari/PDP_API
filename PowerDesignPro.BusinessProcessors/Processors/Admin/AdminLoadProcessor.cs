using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    public class AdminLoadProcessor : IAdminLoad
    {
        private readonly IEntityBaseRepository<Load> _loadRepository;

        private readonly IEntityBaseRepository<LoadFamily> _loadFamilyRepository;

        private readonly IEntityBaseRepository<LoadDefaults> _loadDefaultsRepository;

        private readonly IEntityBaseRepository<Project> _projectRepository;

        private readonly IEntityBaseRepository<BasicLoad> _basicLoadRepository;

        private readonly IEntityBaseRepository<ACLoad> _acLoadRepository;

        private readonly IEntityBaseRepository<LightingLoad> _lightingLoadRepository;

        private readonly IEntityBaseRepository<MotorLoad> _motorLoadRepository;

        private readonly IEntityBaseRepository<MotorCalculation> _motorCalculationRepository;

        private readonly IEntityBaseRepository<WelderLoad> _welderLoadRepository;

        private readonly IEntityBaseRepository<UPSLoad> _upsLoadRepository;

        private readonly IMapper<MotorCalculation, MotorCalculationDto> _motorCalculationEntityToMotorCalculationDtoMapper;
        private readonly IMapper<LoadDefaults, AdminLoadDefaultDto> _generatorEntityToGeneratorDtoMapper;
        private readonly IMapper<AdminLoadDefaultDto, LoadDefaults> _addAdminLoadDefaultDtoToEntityMapper;

        public AdminLoadProcessor(
            IEntityBaseRepository<Load> loadRepository,
            IEntityBaseRepository<LoadFamily> loadFamilyRepository,
            IEntityBaseRepository<LoadDefaults> loadDefaultsRepository,
            IEntityBaseRepository<Project> projectRepository,
            IEntityBaseRepository<BasicLoad> basicLoadRepository,
            IEntityBaseRepository<ACLoad> acLoadRepository,
            IEntityBaseRepository<LightingLoad> lightingLoadRepository,
            IEntityBaseRepository<MotorLoad> motorLoadRepository,
            IEntityBaseRepository<MotorCalculation> motorCalculationRepository,
            IEntityBaseRepository<WelderLoad> welderLoadRepository,
            IEntityBaseRepository<UPSLoad> upsLoadRepository,
            IMapper<MotorCalculation, MotorCalculationDto> motorCalculationEntityToMotorCalculationDtoMapper,
            IMapper<LoadDefaults, AdminLoadDefaultDto> generatorEntityToGeneratorDtoMapper,
            IMapper<AdminLoadDefaultDto, LoadDefaults> addAdminLoadDefaultDtoToEntityMapper
            )
        {
            _loadFamilyRepository = loadFamilyRepository;
            _loadRepository = loadRepository;
            _loadDefaultsRepository = loadDefaultsRepository;
            _projectRepository = projectRepository;
            _basicLoadRepository = basicLoadRepository;
            _acLoadRepository = acLoadRepository;
            _lightingLoadRepository = lightingLoadRepository;
            _motorLoadRepository = motorLoadRepository;
            _motorCalculationRepository = motorCalculationRepository;
            _welderLoadRepository = welderLoadRepository;
            _upsLoadRepository = upsLoadRepository;
            _motorCalculationEntityToMotorCalculationDtoMapper = motorCalculationEntityToMotorCalculationDtoMapper;
            _generatorEntityToGeneratorDtoMapper = generatorEntityToGeneratorDtoMapper;
            _addAdminLoadDefaultDtoToEntityMapper = addAdminLoadDefaultDtoToEntityMapper;
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
                    LoadFamily = x.LoadFamily.Value,
                    Active = x.Active
                });
        }

        public AdminLoadDefaultDto GetDefaultLoadDetails(SearchBaseLoadRequestDto searchBasicLoadDto)
        {
            if (searchBasicLoadDto.LoadID > 0)
            {
                var solutionLoadDefaultDetail = _loadDefaultsRepository.GetSingle(x => x.LoadID == searchBasicLoadDto.LoadID);

                if (solutionLoadDefaultDetail == null)
                {
                    throw new PowerDesignProException("LoadNotFound", Message.SolutionLoad);
                }

                var result = _generatorEntityToGeneratorDtoMapper.AddMap(solutionLoadDefaultDetail);

                return result;
            }
            else
            {
                return new AdminLoadDefaultDto
                {

                };
            }

        }

        public AdminResponseDto SaveLoadDetail(AdminLoadDefaultDto adminLoadDefaultDto, string userID)
        {
            if (adminLoadDefaultDto.LoadID == 0)
            {
                return AddLoadDefault(adminLoadDefaultDto, userID);
            }
            else
            {
                return UpdateLoadDefault(adminLoadDefaultDto, userID);
            }
        }

        private AdminResponseDto AddLoadDefault(AdminLoadDefaultDto adminLoadDefaultDto, string userID)
        {
            var loaddefault = _addAdminLoadDefaultDtoToEntityMapper.AddMap(adminLoadDefaultDto);

            var maxOrdinal = _loadRepository.GetAll().ToList().OrderByDescending(af => af.Ordinal).
               FirstOrDefault().Ordinal;

            loaddefault.Load = new Load
            {
                Description = adminLoadDefaultDto.LoadName,
                Active = true,
                LoadFamilyID = adminLoadDefaultDto.LoadFamilyID,
                Value = adminLoadDefaultDto.LoadName,
                Ordinal = maxOrdinal + 1
            };

            loaddefault.Active = true;
            loaddefault.CreatedDateTime = DateTime.UtcNow;
            loaddefault.CreatedBy = userID;
            loaddefault.ModifiedDateTime = DateTime.UtcNow;
            loaddefault.ModifiedBy = userID;

            var loadDefaultDetail = _loadDefaultsRepository.Add(loaddefault);
            _loadDefaultsRepository.Commit();

            return new AdminResponseDto
            {
                ID = loadDefaultDetail.ID
            };
        }

        private AdminResponseDto UpdateLoadDefault(AdminLoadDefaultDto adminLoadDefaultDto, string userID)
        {
            var loadDefault = _loadDefaultsRepository.GetSingle(x => x.LoadID == adminLoadDefaultDto.LoadID);
            if (loadDefault == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.Admin);
            }
            _addAdminLoadDefaultDtoToEntityMapper.UpdateMap(adminLoadDefaultDto, loadDefault);

            loadDefault.Load.Description = adminLoadDefaultDto.LoadName;
            loadDefault.Load.Value = adminLoadDefaultDto.LoadName;

            loadDefault.Active = true;
            loadDefault.ModifiedDateTime = DateTime.UtcNow;
            loadDefault.ModifiedBy = userID;

            var generatorDetail = _loadDefaultsRepository.Update(loadDefault);
            _loadDefaultsRepository.Commit();

            return new AdminResponseDto
            {
                ID = generatorDetail.ID
            };
        }

        public bool DeleteLoadDefault(int loadID, string userName)
        {
            var loadDefault = _loadDefaultsRepository.GetSingle(x => x.LoadID == loadID);
            if (loadDefault == null)
            {
                throw new PowerDesignProException("LoadNotFound", Message.Admin);
            }

            loadDefault.Load.Active = false;


            loadDefault.Active = false;
            loadDefault.ModifiedBy = userName;
            loadDefault.ModifiedDateTime = DateTime.UtcNow;

            _loadDefaultsRepository.Update(loadDefault);
            _loadDefaultsRepository.Commit();
            return true;
        }

        /// <summary>
        /// Updates the load details.
        /// </summary>
        /// <param name="userEmailList">The user email list.</param>
        /// <returns>Dictionary<"userEmail", Dictionary<"Project : Id, Solution : Id", Dictionary<"Success/Failure ", List<Load : Id>>>></returns>
        public Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> UpdateLoadDetails(List<string> userEmailList)
        {
            var result = new Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>>();
            foreach (var userEmail in userEmailList)
            {
                var userProject = _projectRepository.GetAll(x => userEmail.Equals(x.CreatedBy, StringComparison.InvariantCultureIgnoreCase)).ToList();
                var projectResult = new Dictionary<string, Dictionary<string, List<string>>>();

                foreach (var project in userProject)
                {
                    foreach (var solution in project.Solutions.ToList())
                    {
                        var loadResult = new Dictionary<string, List<string>>();
                        var successLoadList = new List<string>();
                        var errorLoadList = new List<string>();

                        // basic Load
                        foreach (var basicLoad in solution.BasicLoadList.ToList())
                        {
                            try
                            {
                                UpdateBasicLoadDetails(basicLoad, solution);
                                successLoadList.Add($"BasicLoad - LoadID : {basicLoad.ID}");
                            }
                            catch (Exception ex)
                            {
                                errorLoadList.Add($"BasicLoad - LoadID : {basicLoad.ID} Error : {ex.Message}");
                            }
                        }

                        // ac Load
                        foreach (var acLoad in solution.ACLoadList.ToList())
                        {
                            try
                            {
                                UpdateACLoadDetails(acLoad, solution);
                                successLoadList.Add($"ACLoad - LoadID : {acLoad.ID}");
                            }
                            catch (Exception ex)
                            {
                                errorLoadList.Add($"ACLoad - LoadID : {acLoad.ID} Error : {ex.Message}");
                            }
                        }

                        // lighting Load
                        foreach (var lightingLoad in solution.LightingLoadList.ToList())
                        {
                            try
                            {
                                UpdateLightingLoadDetails(lightingLoad, solution);
                                successLoadList.Add($"LightingLoad - LoadID : {lightingLoad.ID}");
                            }
                            catch (Exception ex)
                            {
                                errorLoadList.Add($"LightingLoad - LoadID : {lightingLoad.ID} Error : {ex.Message}");
                            }
                        }

                        // motor Load
                        foreach (var motorLoad in solution.MotorLoadList.ToList())
                        {
                            try
                            {
                                UpdateMotorLoadDetails(motorLoad, solution);
                                successLoadList.Add($"MotorLoad - LoadID : {motorLoad.ID}");
                            }
                            catch (Exception ex)
                            {
                                errorLoadList.Add($"MotorLoad - LoadID : {motorLoad.ID} Error : {ex.Message}");
                            }
                        }

                        // welder Load
                        foreach (var welderLoad in solution.WelderLoadList.ToList())
                        {
                            try
                            {
                                UpdateWelderLoadDetails(welderLoad, solution);
                                successLoadList.Add($"WelderLoad - LoadID : {welderLoad.ID}");
                            }
                            catch (Exception ex)
                            {
                                errorLoadList.Add($"WelderLoad - LoadID : {welderLoad.ID} Error : {ex.Message}");
                            }
                        }

                        // ups Load
                        foreach (var upsLoad in solution.UPSLoadList.ToList())
                        {
                            try
                            {
                                UpdateUPSLoadDetails(upsLoad, solution);
                                successLoadList.Add($"UPSLoad - LoadID : {upsLoad.ID}");
                            }
                            catch (Exception ex)
                            {
                                errorLoadList.Add($"UPSLoad - LoadID : {upsLoad.ID} Error : {ex.Message}");
                            }
                        }

                        // repeat above foreach for other loads...
                            //loadResult.Add("success", successLoadList);
                            loadResult.Add("failure", errorLoadList);

                        if (errorLoadList.Count > 0)
                        {
                            projectResult.Add($"ProjectID:{project.ID} SolutionID:{solution.ID}", loadResult);
                        }
                       
                    }
                }
                result.Add(userEmail, projectResult);
            }

            return result;
        } 

        #region update basic Load

        private void UpdateBasicLoadDetails(BasicLoad basicLoad, Solution solution)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.AllIncluding(x => x.HarmonicDeviceType).ToList()
                                                    .Where(l => l.LoadID == basicLoad.Load.ID).FirstOrDefault();

            basicLoad.StartingLoadKva = GetKVAStartingBasicLoad(basicLoad);
            basicLoad.StartingLoadKw = GetKWStartingBasicLoad(basicLoad);
            basicLoad.RunningLoadKva = GetKVARunningBasicLoad(basicLoad);
            basicLoad.RunningLoadKw = GetKWRunningBasicLoad(basicLoad);
            basicLoad.THIDContinuous = GetTHIDContinuousBasicLoad(basicLoad, solutionLoadDefaultDetail);
            basicLoad.THIDMomentary = GetTHIDMomentaryBasicLoad(basicLoad, solutionLoadDefaultDetail);
            basicLoad.THIDKva = GetTHIDKvaBasicLoad(basicLoad, solutionLoadDefaultDetail);
            CalculateHarmonicDistortions(basicLoad, solutionLoadDefaultDetail);
            _basicLoadRepository.Update(basicLoad);
            _basicLoadRepository.Commit();
        }

        private decimal GetKVAStartingBasicLoad(BasicLoad basicLoad)
        {
            var multiplier = (decimal)1;
            var size = basicLoad.SizeStarting > (decimal)0 ? basicLoad.SizeStarting : basicLoad.SizeRunning;
            var pf = basicLoad.StartingPF != null ? Convert.ToDecimal(basicLoad.StartingPF.Value) > 0 ? basicLoad.StartingPF.Value : basicLoad.RunningPF.Value : basicLoad.RunningPF.Value;

            if (basicLoad.SizeStarting == null || basicLoad.SizeStarting == 0)
            {
                basicLoad.SizeStartingUnits = basicLoad.SizeRunningUnits;
            }

            if (basicLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = basicLoad.Quantity;
            }

            if (basicLoad.SizeStartingUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * (size.Value / Convert.ToDecimal(pf));
            }
            else if (basicLoad.SizeStartingUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * size.Value;
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(basicLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(basicLoad.VoltagePhase.Value) == 1)
                {
                    return multiplier * ((size.Value * voltageSpecific / 1000) * 1);
                }
                else
                {
                    return multiplier * ((size.Value * voltageSpecific / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetKWStartingBasicLoad(BasicLoad basicLoad)
        {
            var multiplier = (decimal)1;
            var size = basicLoad.SizeStarting > (decimal)0 ? basicLoad.SizeStarting : basicLoad.SizeRunning;
            var pf = basicLoad.StartingPF != null ? Convert.ToDecimal(basicLoad.StartingPF.Value) > 0 ? basicLoad.StartingPF.Value : basicLoad.RunningPF.Value : basicLoad.RunningPF.Value;


            if (basicLoad.SizeStarting == null || basicLoad.SizeStarting == 0)
            {
                basicLoad.SizeStartingUnits = basicLoad.SizeRunningUnits;
            }
           
            if (basicLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = basicLoad.Quantity;
            }

            if (basicLoad.SizeStartingUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * size.Value;
            }
            else if (basicLoad.SizeStartingUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * (size.Value * Convert.ToDecimal(pf));
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(basicLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(basicLoad.VoltagePhase.Value) == 1)
                {
                    return multiplier * ((size.Value * voltageSpecific * Convert.ToDecimal(pf) / 1000) * 1);
                }
                else
                {
                    return multiplier * ((size.Value * voltageSpecific * Convert.ToDecimal(pf) / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetKVARunningBasicLoad(BasicLoad basicLoad)
        {
            if (basicLoad.SizeRunningUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return basicLoad.Quantity * (basicLoad.SizeRunning.Value / Convert.ToDecimal(basicLoad.RunningPF.Value));
            }
            else if (basicLoad.SizeRunningUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return basicLoad.Quantity * basicLoad.SizeRunning.Value;
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(basicLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(basicLoad.VoltagePhase.Value) == 1)
                {
                    return basicLoad.Quantity * ((basicLoad.SizeRunning.Value * voltageSpecific / 1000) * 1);
                }
                else
                {
                    return basicLoad.Quantity * ((basicLoad.SizeRunning.Value * voltageSpecific / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetKWRunningBasicLoad(BasicLoad basicLoad)
        {
            if (basicLoad.SizeRunningUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return basicLoad.Quantity * (basicLoad.SizeRunning.Value);
            }
            else if (basicLoad.SizeRunningUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return basicLoad.Quantity * basicLoad.SizeRunning.Value *  Convert.ToDecimal(basicLoad.RunningPF.Value);
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(basicLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(basicLoad.VoltagePhase.Value) == 1)
                {
                    return basicLoad.Quantity * ((basicLoad.SizeRunning.Value * voltageSpecific  * Convert.ToDecimal(basicLoad.RunningPF.Value) / 1000) * 1);
                }
                else
                {
                    return basicLoad.Quantity * ((basicLoad.SizeRunning.Value * voltageSpecific * Convert.ToDecimal(basicLoad.RunningPF.Value) / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetTHIDMomentaryBasicLoad(BasicLoad basicLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != null)
            {
                if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.AcrossTheLine || loadDefaults.StartingMethodID != (int)StartingMethodEnum.ReducedVoltage)
                {
                    thid = Convert.ToDecimal(basicLoad.HarmonicContent.Value);
                }
            }

            return thid;
        }

        private decimal GetTHIDContinuousBasicLoad(BasicLoad basicLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != null)
            {
                if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.SoftStarter)
                {
                    thid = Convert.ToDecimal(basicLoad.HarmonicContent.Value);
                }
            }

            return thid;
        }

        private decimal GetTHIDKvaBasicLoad(BasicLoad basicLoad, LoadDefaults loadDefaults)
        {
            if (GetTHIDMomentaryBasicLoad(basicLoad, loadDefaults) == 0 && GetTHIDContinuousBasicLoad(basicLoad, loadDefaults) == 0)
            {
                return 0;
            }

            return GetKVARunningBasicLoad(basicLoad);
        }

        private void CalculateHarmonicDistortions(BasicLoad basicLoad, LoadDefaults loadDefaults)
        {
            if (loadDefaults.HarmonicDeviceType != null)
            {
                var multiplier = 1;

                var thid = Convert.ToDecimal(Math.Sqrt(
                    Convert.ToDouble(
                    (loadDefaults.HarmonicDeviceType.HD3 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD5 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD7 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD9 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD11 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD13 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD15 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD17 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD19 * 2)
                    )));

                thid = Convert.ToDecimal(Math.Max(thid, (decimal)1));

                basicLoad.HD3 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD3.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD5 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD5.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD7 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD7.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD9 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD9.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD11 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD11.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD13 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD13.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD15 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD15.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD17 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD17.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
                basicLoad.HD19 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD19.Value * Convert.ToDecimal(basicLoad.HarmonicContent.Value)) / thid), 2);
            }
        }

        #endregion

        #region update ac Load

        private void UpdateACLoadDetails(ACLoad acLoad, Solution solution)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.AllIncluding(x => x.HarmonicDeviceType).ToList()
                                                    .Where(l => l.LoadID == acLoad.Load.ID).FirstOrDefault();

            acLoad.StartingLoadKva = GetKVAStartingACLoad(acLoad);
            acLoad.StartingLoadKw = GetKWStartingACLoad(acLoad);
            acLoad.RunningLoadKva = GetKVARunningACLoad(acLoad);
            acLoad.RunningLoadKw = GetKWRunningACLoad(acLoad);
            _acLoadRepository.Update(acLoad);
            _acLoadRepository.Commit();
        }

        private decimal GetKVAStartingACLoad(ACLoad acLoad)
        {
            var multiplier = (decimal)1;

            if (acLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = acLoad.Quantity;
            }

            var Compressors = Convert.ToDecimal(acLoad.Compressors.Value);
            var CoolingLoad = Convert.ToDecimal(acLoad.CoolingLoad.Value);
            var estCompressorTotal = acLoad.CoolingUnits.Value.ToString().ToLower() == "tons" ? acLoad.Cooling : acLoad.Cooling / 12000;
            var estLargestMotorStart = estCompressorTotal * CoolingLoad / (decimal)0.85;
            var sKVA = (estLargestMotorStart / Compressors) * 6;

            return (decimal)(multiplier * sKVA);
        }

        private decimal GetKWStartingACLoad(ACLoad acLoad)
        {
            var multiplier = (decimal)1;

            if (acLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = acLoad.Quantity;
            }

            var Compressors = Convert.ToDecimal(acLoad.Compressors.Value);
            var CoolingLoad = Convert.ToDecimal(acLoad.CoolingLoad.Value);
            var ReheatLoad = Convert.ToDecimal(acLoad.ReheatLoad.Value);
            var estCompressorTotal = acLoad.CoolingUnits.Value.ToString().ToLower() == "tons" ? acLoad.Cooling : acLoad.Cooling / 12000;
            var estLargestMotorStart = estCompressorTotal * CoolingLoad / (decimal)0.85;
            var sKW = (estLargestMotorStart / Compressors) * 6;
            var reheatKW = estCompressorTotal * ReheatLoad;

            return multiplier * Math.Max(sKW.Value * (decimal)0.3, reheatKW.Value);
        }

        private decimal GetKVARunningACLoad(ACLoad acLoad)
        {
            var ReheatLoad = Convert.ToDecimal(acLoad.ReheatLoad.Value);
            var CoolingLoad = Convert.ToDecimal(acLoad.CoolingLoad.Value);
            var estCompressorTotal = acLoad.CoolingUnits.Value.ToString().ToLower() == "tons" ? acLoad.Cooling : acLoad.Cooling / 12000;
            var reheatKW = estCompressorTotal * ReheatLoad;
            var rKVA = (estCompressorTotal * CoolingLoad) / (decimal)0.85;

            rKVA = (rKVA + reheatKW) * acLoad.Quantity;

            return (decimal)rKVA;
        }

        private decimal GetKWRunningACLoad(ACLoad acLoad)
        {
            var ReheatLoad = Convert.ToDecimal(acLoad.ReheatLoad.Value);
            var CoolingLoad = Convert.ToDecimal(acLoad.CoolingLoad.Value);
            var estCompressorTotal = acLoad.CoolingUnits.Value.ToString().ToLower() == "tons" ? acLoad.Cooling : acLoad.Cooling / 12000;
            var reheatKW = estCompressorTotal * ReheatLoad;
            var rKW = (estCompressorTotal * CoolingLoad / (decimal)0.85) * (decimal)0.85;

            rKW = (rKW + reheatKW) * acLoad.Quantity;

            return (decimal)rKW;
        }
        #endregion

        #region update lighting Load

        private void UpdateLightingLoadDetails(LightingLoad lightingLoad, Solution solution)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.AllIncluding(x => x.HarmonicDeviceType).ToList()
                                                    .Where(l => l.LoadID == lightingLoad.Load.ID).FirstOrDefault();

            lightingLoad.StartingLoadKva = GetKVAStartingLightingLoad(lightingLoad);
            lightingLoad.StartingLoadKw = GetKWStartingLightingLoad(lightingLoad);
            lightingLoad.RunningLoadKva = GetKVARunningLightingLoad(lightingLoad);
            lightingLoad.RunningLoadKw = GetKWRunningLightingLoad(lightingLoad);
            lightingLoad.THIDContinuous = GetTHIDContinuousLightingLoad(lightingLoad, solutionLoadDefaultDetail);
            lightingLoad.THIDMomentary = GetTHIDMomentaryLightingLoad(lightingLoad, solutionLoadDefaultDetail);
            lightingLoad.THIDKva = GetTHIDKvaLightingLoad(lightingLoad, solutionLoadDefaultDetail);
            CalculateHarmonicDistortions(lightingLoad, solutionLoadDefaultDetail);
            _lightingLoadRepository.Update(lightingLoad);
            _lightingLoadRepository.Commit();
        }

        private decimal GetKVAStartingLightingLoad(LightingLoad lightingLoad)
        {
            var multiplier = (decimal)1;
            var size = lightingLoad.SizeRunning;
            var pf = lightingLoad.RunningPF.Value;
          
            if (lightingLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = lightingLoad.Quantity;
            }

            if (lightingLoad.SizeRunningUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * (size.Value / Convert.ToDecimal(pf));
            }
            else if (lightingLoad.SizeRunningUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * size.Value;
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(lightingLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(lightingLoad.VoltagePhase.Value) == 1)
                {
                    return multiplier * ((size.Value * voltageSpecific / 1000) * 1);
                }
                else
                {
                    return multiplier * ((size.Value * voltageSpecific / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetKWStartingLightingLoad(LightingLoad lightingLoad)
        {
            var multiplier = (decimal)1;
            var size = lightingLoad.SizeRunning;
            var pf = lightingLoad.RunningPF;

            if (lightingLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = lightingLoad.Quantity;
            }

            if (lightingLoad.SizeRunningUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * size.Value;
            }
            else if (lightingLoad.SizeRunningUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return multiplier * (size.Value * Convert.ToDecimal(pf.Value));
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(lightingLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(lightingLoad.VoltagePhase.Value) == 1)
                {
                    return multiplier * ((size.Value * voltageSpecific * Convert.ToDecimal(pf.Value) / 1000) * 1);
                }
                else
                {
                    return multiplier * ((size.Value * voltageSpecific * Convert.ToDecimal(pf.Value) / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetKVARunningLightingLoad(LightingLoad lightingLoad)
        {
            if (lightingLoad.SizeRunningUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return lightingLoad.Quantity * (lightingLoad.SizeRunning.Value / Convert.ToDecimal(lightingLoad.RunningPF.Value));
            }
            else if (lightingLoad.SizeRunningUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return lightingLoad.Quantity * lightingLoad.SizeRunning.Value;
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(lightingLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(lightingLoad.VoltagePhase.Value) == 1)
                {
                    return lightingLoad.Quantity * ((lightingLoad.SizeRunning.Value * voltageSpecific / 1000) * 1);
                }
                else
                {
                    return lightingLoad.Quantity * ((lightingLoad.SizeRunning.Value * voltageSpecific / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetKWRunningLightingLoad(LightingLoad lightingLoad)
        {
            if (lightingLoad.SizeRunningUnits.Description.Equals("kW", StringComparison.InvariantCultureIgnoreCase))
            {
                return lightingLoad.Quantity * lightingLoad.SizeRunning.Value;
            }
            else if (lightingLoad.SizeRunningUnits.Description.Equals("kVA", StringComparison.InvariantCultureIgnoreCase))
            {
                return lightingLoad.Quantity * (lightingLoad.SizeRunning.Value * Convert.ToDecimal(lightingLoad.RunningPF.Value));
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(lightingLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(lightingLoad.VoltagePhase.Value) == 1)
                {
                    return lightingLoad.Quantity * ((lightingLoad.SizeRunning.Value * voltageSpecific * Convert.ToDecimal(lightingLoad.RunningPF.Value) / 1000) * 1);
                }
                else
                {
                    return lightingLoad.Quantity * ((lightingLoad.SizeRunning.Value * voltageSpecific * Convert.ToDecimal(lightingLoad.RunningPF.Value) / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetTHIDMomentaryLightingLoad(LightingLoad lightingLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != null)
            {
                if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.AcrossTheLine || loadDefaults.StartingMethodID != (int)StartingMethodEnum.ReducedVoltage)
                {
                    thid = Convert.ToDecimal(lightingLoad.HarmonicContent.Value);
                }
            }

            return thid;
        }

        private decimal GetTHIDContinuousLightingLoad(LightingLoad lightingLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if(loadDefaults.StartingMethodID !=null)
            {
                if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.SoftStarter)
                {
                    thid = Convert.ToDecimal(lightingLoad.HarmonicContent.Value);
                }
            }

            return thid;
        }

        private decimal GetTHIDKvaLightingLoad(LightingLoad lightingLoad, LoadDefaults loadDefaults)
        {
            if (GetTHIDMomentaryLightingLoad(lightingLoad, loadDefaults) == 0 && GetTHIDContinuousLightingLoad(lightingLoad, loadDefaults) == 0)
            {
                return 0;
            }

            return GetKVARunningLightingLoad(lightingLoad);
        }

        private void CalculateHarmonicDistortions(LightingLoad lightingLoad, LoadDefaults loadDefaults)
        {
            if (loadDefaults.HarmonicDeviceType != null)
            {
                var multiplier = 1;

                var thid = Convert.ToDecimal(Math.Sqrt(
                    Convert.ToDouble(
                    (loadDefaults.HarmonicDeviceType.HD3 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD5 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD7 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD9 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD11 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD13 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD15 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD17 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD19 * 2)
                    )));

                thid = Convert.ToDecimal(Math.Max(thid, (decimal)1));

                lightingLoad.HD3 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD3.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD5 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD5.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD7 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD7.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD9 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD9.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD11 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD11.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD13 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD13.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD15 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD15.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD17 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD17.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
                lightingLoad.HD19 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD19.Value * Convert.ToDecimal(lightingLoad.HarmonicContent.Value)) / thid), 2);
            }
        }
        #endregion

        #region update motor load

        private void UpdateMotorLoadDetails(MotorLoad motorLoad, Solution solution)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.AllIncluding(x => x.HarmonicDeviceType).ToList()
                                                    .Where(l => l.LoadID == motorLoad.Load.ID).FirstOrDefault();

            motorLoad.StartingLoadKva = GetKVAStartingMotorLoad(motorLoad);
            motorLoad.StartingLoadKw = GetKWStartingMotorLoad(motorLoad);
            motorLoad.RunningLoadKva = GetKVARunningMotorLoad(motorLoad);
            motorLoad.RunningLoadKw = GetKWRunningMotorLoad(motorLoad);
            motorLoad.THIDContinuous = GetTHIDContinuousMotorLoad(motorLoad, solutionLoadDefaultDetail);
            motorLoad.THIDMomentary = GetTHIDMomentaryMotorLoad(motorLoad, solutionLoadDefaultDetail);
            motorLoad.THIDKva = GetTHIDKvaMotorLoad(motorLoad, solutionLoadDefaultDetail);
            CalculateHarmonicDistortions(motorLoad, solutionLoadDefaultDetail);
            _motorLoadRepository.Update(motorLoad);
            _motorLoadRepository.Commit();
        }

        private decimal GetKWSMultiplierMotorLoad(MotorLoad motorLoad)
        {
            var _KWStartingMultiplier = (decimal)0;
            
            if (motorLoad.MotorLoadType.Description.ToLower().Equals("low torque at start", StringComparison.InvariantCultureIgnoreCase))
            {
                _KWStartingMultiplier = (decimal)0.85;
            }
            else if (motorLoad.MotorLoadType.Description.ToLower().Equals("unloaded at start", StringComparison.InvariantCultureIgnoreCase))
            {
                _KWStartingMultiplier = (decimal)0.7;
            }
            else if (motorLoad.MotorLoadType.Description.ToLower().Equals("rated torque at start", StringComparison.InvariantCultureIgnoreCase))
            {
                _KWStartingMultiplier = 1;
            }

            return _KWStartingMultiplier;
        }

        private decimal GetKVASMultiplierMotorLoad(MotorLoad motorLoad)
        {
            var _KVAStartingMultiplier = (decimal)0;
            var ehormonicflag = (decimal)3;
            var LowTorqueAtStart = (decimal)2;
            var StartingConfiguration = motorLoad.ConfigurationInput.ID;
            var sKVAMultiplierOverride= motorLoad.ConfigurationInput != null ? motorLoad.ConfigurationInput.sKVAMultiplierOverride : 0;
            var HarmonicKVAMultiplier = motorLoad.HarmonicDeviceType != null ? motorLoad.HarmonicDeviceType.KVAMultiplier.Value : 0;
            var HarmonicLoadLimit = motorLoad.HarmonicDeviceType != null ? motorLoad.HarmonicDeviceType.LoadLimit : 0;

            if (motorLoad.StartingMethod.Description.ToLower().Equals("across the line", StringComparison.InvariantCultureIgnoreCase))
            {
                _KVAStartingMultiplier = 1;
            }
            else if (motorLoad.StartingMethod.Description.ToLower().Equals("reduced voltage", StringComparison.InvariantCultureIgnoreCase))
            {
                if (motorLoad.HarmonicDeviceType.Description.ToLower().Equals("wye / delta (open)", StringComparison.InvariantCultureIgnoreCase))
                {
                    ehormonicflag = 1;
                }
                else if (motorLoad.HarmonicDeviceType.Description.ToLower().Equals("wye / delta (close)", StringComparison.InvariantCultureIgnoreCase))
                {
                    ehormonicflag = 2;
                }
                else
                    ehormonicflag = 3;


                if (ehormonicflag == 1 && motorLoad.MotorLoadType.Description.ToLower().Equals("low torque at start", StringComparison.InvariantCultureIgnoreCase))
                {
                    _KVAStartingMultiplier = (decimal)0.625;
                }
                else if (ehormonicflag == 1 && motorLoad.MotorLoadType.Description.ToLower().Equals("unloaded at start", StringComparison.InvariantCultureIgnoreCase))
                {
                    _KVAStartingMultiplier = HarmonicKVAMultiplier; 
                }
                else if (ehormonicflag == 1 && motorLoad.MotorLoadType.Description.ToLower().Equals("rated torque at start", StringComparison.InvariantCultureIgnoreCase))
                {
                    _KVAStartingMultiplier = 1;
                }
                else if (ehormonicflag == 2 && motorLoad.MotorLoadType.Description.ToLower().Equals("low torque at start", StringComparison.InvariantCultureIgnoreCase))
                {
                    _KVAStartingMultiplier = (decimal)0.5416;
                }
                else if (ehormonicflag == 2 && motorLoad.MotorLoadType.Description.ToLower().Equals("unloaded at start", StringComparison.InvariantCultureIgnoreCase))
                {
                    _KVAStartingMultiplier = HarmonicKVAMultiplier;
                }
                else if (ehormonicflag == 2 && motorLoad.MotorLoadType.Description.ToLower().Equals("rated torque at start", StringComparison.InvariantCultureIgnoreCase))
                {
                    _KVAStartingMultiplier = 1;
                }
                else
                    if (HarmonicLoadLimit >= LowTorqueAtStart)
                    {
                        _KVAStartingMultiplier = HarmonicKVAMultiplier;
                    }
                    else {
                        _KVAStartingMultiplier = 1;
                    }
            }
            else if (motorLoad.StartingMethod.Description.ToLower().Equals("soft starter", StringComparison.InvariantCultureIgnoreCase))
            {
                if (StartingConfiguration == 1)
                {
                    _KVAStartingMultiplier = HarmonicKVAMultiplier;
                }
                else
                {
                    _KVAStartingMultiplier = (decimal)0.2;
                }

            }
            else if (motorLoad.StartingMethod.Description.ToLower().Equals("vfd", StringComparison.InvariantCultureIgnoreCase))
            {
                _KVAStartingMultiplier = sKVAMultiplierOverride;
            }

            return _KVAStartingMultiplier;
        }

        private decimal GetKVAStartingMotorLoad(MotorLoad motorLoad)
        {
            var multiplier = (decimal)1;
            var rKVAInput = (decimal)0;
            var UserDefinedInputs = (decimal)1;
            var startingCodeMultiplier = (decimal)1;
            
            var StartingCode = motorLoad.StartingCode != null ? motorLoad.StartingCode.KVAHPStarting.Value : 0;
            var Size = motorLoad.SizeRunning.Value;
           
            Size = ConvertMotorPowerUnits(motorLoad,motorLoad.SizeRunningUnits.Value, "HP", Size, motorLoad.VoltagePhase.ID, Convert.ToDecimal(motorLoad.VoltageNominal.Value));

            if (motorLoad.Sequence.SequenceType.Description.ToLower().Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = motorLoad.Quantity;
            }

            if (motorLoad.StartingMethod.Description.ToLower().Equals("soft starter", StringComparison.InvariantCultureIgnoreCase) || motorLoad.StartingMethod.Description.ToLower().Equals("vfd", StringComparison.InvariantCultureIgnoreCase))
            {
                if (UserDefinedInputs > 0)
                {
                    if (motorLoad.StartingMethod.Description.ToLower().Equals("vfd", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return rKVAInput = multiplier * (Size * startingCodeMultiplier * GetKVASMultiplierMotorLoad(motorLoad));
                    }
                    else
                    {
                        return rKVAInput = multiplier * (Size * StartingCode * GetKVASMultiplierMotorLoad(motorLoad));
                    }
                }
                else
                {
                    return rKVAInput = multiplier * (Size * UserDefinedInputs); 
                }
            }
            else
                return rKVAInput = multiplier * (Size * StartingCode * GetKVASMultiplierMotorLoad(motorLoad));
        }

        private decimal GetKWStartingMotorLoad(MotorLoad motorLoad)
        {
            var sKWInput = (decimal)0;
            var pf = (decimal)0;
            var Size = motorLoad.SizeRunning.Value;
            var PFStarting = (decimal)0;
            var PFRunning = (decimal)0;
            var calcRow = _motorCalculationRepository.GetSingle(fd => fd.HP == Size);
            if (calcRow != null)
            {
                PFStarting = calcRow != null ? calcRow.PFStarting : 0;
                PFRunning = calcRow != null ? calcRow.PFRunning : 0;
            }
            else
            {
                var closestRow = FindClosetRow(Size, "hp");
                PFStarting = closestRow.PFStarting;
                PFRunning = closestRow.PFRunning;
            }

            if (PFStarting > 0) { pf = PFStarting; }
            else { pf = PFRunning; }
            var UserDefinedInputs = (decimal)1;
            Size = this.ConvertMotorPowerUnits(motorLoad, motorLoad.SizeRunningUnits.Value, "HP", Size, motorLoad.VoltagePhase.ID, Convert.ToDecimal(motorLoad.VoltageNominal.Value));
            if (motorLoad.MotorLoadType.Description.ToLower().Equals("low torque at start", StringComparison.InvariantCultureIgnoreCase))
            {
                if (pf < (decimal)0.35)
                {
                    pf = (decimal)0.35;
                }
            }
            if (motorLoad.StartingMethod.Description.ToLower().Equals("soft starter", StringComparison.InvariantCultureIgnoreCase))
            {
                if (UserDefinedInputs > 0)
                {
                    sKWInput = (GetKVAStartingMotorLoad(motorLoad) * pf * GetKVASMultiplierMotorLoad(motorLoad));
                }
                else
                {
                    sKWInput = (Size * UserDefinedInputs);
                }
            }
            else if (motorLoad.StartingMethod.Description.ToLower().Equals("vfd", StringComparison.InvariantCultureIgnoreCase))
            {
                if (UserDefinedInputs > 0)
                {
                    sKWInput = (GetKVAStartingMotorLoad(motorLoad) * pf);
                }
                else
                {
                    sKWInput = (Size * UserDefinedInputs);
                }
            }
            else if (motorLoad.StartingMethod.Description.ToLower().Equals("reduced voltage", StringComparison.InvariantCultureIgnoreCase))
            {
                sKWInput = (GetKVAStartingMotorLoad(motorLoad) * pf);
            }
            else
            {
                sKWInput = (GetKVAStartingMotorLoad(motorLoad) * pf * GetKVASMultiplierMotorLoad(motorLoad));
            }

            return sKWInput;
        }

        private decimal GetKVARunningMotorLoad(MotorLoad motorLoad)
        {
            var rPF = (decimal)0;
            var rKW = (decimal)0;
            var Size = motorLoad.SizeRunning.Value;
            var MotorKVARunning = (decimal)0;
            var PFStarting = (decimal)0;
            var PFRunning = (decimal)0;
            var calcRow = _motorCalculationRepository.GetSingle(fd => fd.HP == Size);
            if (calcRow != null)
            {
                PFStarting = calcRow != null ? calcRow.PFStarting : 0;
                PFRunning = calcRow != null ? calcRow.PFRunning : 0;
                MotorKVARunning = calcRow != null ? calcRow.KVARunning : 0;
            }
            else
            {
                var closestRow = FindClosetRow(Size, "hp");
                PFStarting = closestRow.PFStarting;
                PFRunning = closestRow.PFRunning;
                MotorKVARunning = closestRow.KVARunning;
                MotorKVARunning = (Size * closestRow.KVARunning) / closestRow.HP;
            }
            var UserDefinedInputs = (decimal)1;      // need to write code for value
            var SizeRunning = motorLoad.SizeRunning.Value;

            if (motorLoad.StartingMethod.Description.ToLower().Equals("soft starter", StringComparison.InvariantCultureIgnoreCase))
            {
                if (UserDefinedInputs > 0)
                {
                    rKW = (MotorKVARunning / (decimal)0.98) * PFRunning;
                }
                else
                {
                    rKW = UserDefinedInputs * SizeRunning;
                }

                rKW = rKW * Convert.ToDecimal(motorLoad.MotorLoadLevel.Value);
                if (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value) > (decimal)0.99)
                {
                    rPF = PFRunning;
                }
                else
                {
                    rPF = ((PFRunning - (decimal)0.2) / 1 * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value)) + (decimal)0.2);
                }
                return motorLoad.Quantity * (rKW / rPF);
            }
            else if (motorLoad.StartingMethod.Description.ToLower().Equals("vfd", StringComparison.InvariantCultureIgnoreCase))
            {
                if (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value) > (decimal)0.99)
                {
                    rPF = PFRunning;
                }
                else
                {
                    rPF = ((PFRunning - (decimal)0.2) / 1 * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value)) + (decimal)0.2);
                }
                if (UserDefinedInputs > 0)
                {

                    rKW = (MotorKVARunning * PFRunning) * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value));
                    if (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value) > (decimal)0.99)
                    {
                        rPF = PFRunning;
                    }
                    else
                    {
                        rPF = ((PFRunning - (decimal)0.2) / 1 * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value)) + (decimal)0.2);
                    }
                    return ((motorLoad.Quantity * (rKW / rPF)) / (decimal)0.95);
                }
                else
                {
                    return (motorLoad.Quantity * (UserDefinedInputs * SizeRunning) / rPF);
                }
            }
            else
            {
                rKW = (MotorKVARunning * PFRunning) * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value));
                if (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value) > (decimal)0.99)
                {
                    rPF = PFRunning;
                }
                else
                {
                    rPF = ((PFRunning - (decimal)0.2) / 1 * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value)) + (decimal)0.2);
                }
            }
                

            return (motorLoad.Quantity * (rKW / rPF));
        }

        private decimal GetKWRunningMotorLoad(MotorLoad motorLoad)
        {
            var rKW = (decimal)0;
            var rPF = (decimal)0;
            var Size = motorLoad.SizeRunning.Value;
            var MotorKVARunning = (decimal)0;
            var PFStarting = (decimal)0;
            var PFRunning = (decimal)0;
            var calcRow = _motorCalculationRepository.GetSingle(fd => fd.HP == Size);
            if (calcRow != null)
            {
                PFStarting = calcRow != null ? calcRow.PFStarting : 0;
                PFRunning = calcRow != null ? calcRow.PFRunning : 0;
                MotorKVARunning = calcRow != null ? calcRow.KVARunning : 0;
            }
            else
            {
                var closestRow = FindClosetRow(Size, "hp");
                PFStarting = closestRow.PFStarting;
                PFRunning = closestRow.PFRunning;
                MotorKVARunning = closestRow.KVARunning;
                MotorKVARunning = (Size * closestRow.KVARunning) / closestRow.HP;
            }
            var UserDefinedInputs = (decimal)1;      
            var SizeRunning = motorLoad.SizeRunning.Value;

            if (motorLoad.StartingMethod.Description.ToLower().Equals("soft starter", StringComparison.InvariantCultureIgnoreCase))
            {
                if (UserDefinedInputs > 0)
                {
                    rKW = (MotorKVARunning / (decimal)0.98) * PFRunning;
                }
                else
                {
                    rKW = UserDefinedInputs * SizeRunning;  //UserRKWMultiplier
                }

                return motorLoad.Quantity * (rKW * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value)));
            }
            else if (motorLoad.StartingMethod.Description.ToLower().Equals("vfd", StringComparison.InvariantCultureIgnoreCase))
            {
                if (UserDefinedInputs > 0)
                {
                    rKW = (MotorKVARunning * PFRunning);
                    return motorLoad.Quantity * (rKW * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value))) / (decimal)0.95;
                }
                else
                {
                    return UserDefinedInputs * SizeRunning;  //UserRKWMultiplier
                }
            }
            else
            {
                rKW = (MotorKVARunning * PFRunning);
                return motorLoad.Quantity * (rKW * (Convert.ToDecimal(motorLoad.MotorLoadLevel.Value)));
            }
        }

        private decimal GetTHIDContinuousMotorLoad(MotorLoad motorLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.AcrossTheLine || loadDefaults.StartingMethodID != (int)StartingMethodEnum.ReducedVoltage)
            {
                thid = Convert.ToDecimal(motorLoad.HarmonicContent.Value);
            }

            return thid;
        }

        private decimal GetTHIDMomentaryMotorLoad(MotorLoad motorLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.SoftStarter)
            {
                thid = Convert.ToDecimal(motorLoad.HarmonicContent.Value);
            }

            return thid;
        }

        private decimal GetTHIDKvaMotorLoad(MotorLoad motorLoad, LoadDefaults loadDefaults)
        {
            if (GetTHIDMomentaryMotorLoad(motorLoad, loadDefaults) == 0 && GetTHIDContinuousMotorLoad(motorLoad, loadDefaults) == 0)
            {
                return 0;
            }

            return GetKVARunningMotorLoad(motorLoad);
        }

        private void CalculateHarmonicDistortions(MotorLoad motorLoad, LoadDefaults loadDefaults)
        {
            if (loadDefaults.HarmonicDeviceType != null)
            {
                var multiplier = 1;

                var thid = Convert.ToDecimal(Math.Sqrt(
                    Convert.ToDouble(
                    (loadDefaults.HarmonicDeviceType.HD3 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD5 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD7 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD9 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD11 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD13 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD15 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD17 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD19 * 2)
                    )));

                thid = Convert.ToDecimal(Math.Max(thid, (decimal)1));

                motorLoad.HD3 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD3.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD5 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD5.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD7 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD7.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD9 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD9.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD11 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD11.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD13 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD13.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD15 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD15.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD17 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD17.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
                motorLoad.HD19 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD19.Value * Convert.ToDecimal(motorLoad.HarmonicContent.Value)) / thid), 2);
            }
        }        

        private MotorCalculationDto FindClosetRow(decimal inSize,string units)
        {
            var closetRow = new MotorCalculation();
            var defaultMotorLoadDto = _motorCalculationRepository.GetAll().OrderBy(mc => mc.HP).ToList();
            var minRow = defaultMotorLoadDto.FirstOrDefault();
            var maxRow = defaultMotorLoadDto.LastOrDefault();

            if(units.ToLower() == "kwm")
            {
                closetRow = defaultMotorLoadDto.Where(x => x.kWm <= inSize).OrderBy(x => x.kWm).FirstOrDefault();
            }
            else if (units.ToLower() == "hp")
            {
                closetRow = defaultMotorLoadDto.Where(x => x.HP <= inSize).OrderBy(x => x.HP).FirstOrDefault();
            }
            else if (units.ToLower() == "kvarunning")
            {
                closetRow = defaultMotorLoadDto.Where(x => x.KVARunning <= inSize).OrderBy(x => x.KVARunning).FirstOrDefault();
            }
            if(closetRow == null && inSize < minRow.HP)
            {
                closetRow = minRow;
            }
            else if(closetRow == null && inSize >= minRow.HP)
            {
                closetRow = maxRow;
            }
            var motorCalculationDto = _motorCalculationEntityToMotorCalculationDtoMapper.AddMap(closetRow, userName: "");
            return motorCalculationDto;
        }

        private decimal GetAmpsFromKVARunning(decimal voltage,decimal phase,decimal KVARunning)
        {
            var phaseMultiplier = (decimal)1;
            if (phase == 2)
            {
                return phaseMultiplier = (decimal)1.732;
            }
            else
            {
                return KVARunning / (voltage* phaseMultiplier) * 1000;
            }
        }

        private decimal GetKVARunningFromAmps(decimal voltage,decimal phase,decimal amps)
        {
            var phaseMultiplier = (decimal)1;
            if (phase == 2) {
                return phaseMultiplier = (decimal)1.732;
            }
            else {
                return (amps / 1000) * voltage * phaseMultiplier;
            }
        }

        private decimal ConvertMotorPowerUnits(MotorLoad motorLoad,string currUnits,string targetUnits,decimal inSize,decimal phase,decimal voltage)
        {
            if (currUnits.ToLower() == targetUnits.ToLower())
            {
                return inSize;
            }

            var KVARunning = (decimal)0;
            if (targetUnits.ToLower() == "amps")
            {
                var roundedAmps = (decimal)0;
                var tempAmps = (decimal)0;
                if (currUnits.ToLower() == "hp") {
                    var calcRow = _motorCalculationRepository.GetSingle(x => x.HP == inSize);
                    var closestRow = FindClosetRow(inSize, "hp");
                    if (calcRow == null) {
                        if (closestRow == null) {
                            tempAmps = 1;
                        }
                        else {
                            tempAmps = (inSize / closestRow.HP) * GetAmpsFromKVARunning(voltage, phase, closestRow.KVARunning);
                        }
                    }
                    else {
                        KVARunning = calcRow != null? calcRow.KVARunning : 0;
                        tempAmps = GetAmpsFromKVARunning(voltage, phase, KVARunning);
                    }
                }
                else if (currUnits.ToLower() == "kwm") {
                    var calcRow = _motorCalculationRepository.GetSingle(fd => fd.kWm == inSize);
                    var closestRow = FindClosetRow(inSize, "kwm");
                    if (calcRow == null) {
                        if (closestRow == null) {
                            tempAmps = 1;
                        }
                        else {
                            tempAmps = (inSize / closestRow.kWm) * GetAmpsFromKVARunning(voltage, phase, closestRow.KVARunning);
                        }
                    }
                    else {
                        KVARunning = calcRow != null? calcRow.KVARunning : 0;
                        tempAmps = GetAmpsFromKVARunning(voltage, phase, KVARunning);
                    }
                }

                roundedAmps = ((tempAmps) * 10) / 10;
                return decimal.Round(roundedAmps, 2, MidpointRounding.AwayFromZero);
               
            }
            else if (targetUnits.ToLower() == "hp")
            {
                var returnHP = (decimal)0;
                if (currUnits.ToLower() == "amps")
                {
                    KVARunning = GetKVARunningFromAmps(voltage, phase, inSize);
                    var kvaRunningRounded = (KVARunning * 10);
                    KVARunning = kvaRunningRounded / 10;
                    var calcRow = _motorCalculationRepository.GetSingle(fd => fd.KVARunning == inSize);
                    var closestRow = FindClosetRow(inSize, "kvarunning");
                    if (calcRow == null)
                    {
                        if (closestRow == null)
                        {
                            returnHP = 1;
                        }
                        else
                        {
                            returnHP = inSize / GetAmpsFromKVARunning(voltage, phase, closestRow.KVARunning) * closestRow.HP;
                        }
                    }
                    else
                    {
                        returnHP = calcRow != null ? calcRow.HP : 0;
                    }

                }
                else if (currUnits.ToLower() == "kwm")
                {
                    var calcRow = _motorCalculationRepository.GetSingle(fd => fd.kWm == inSize);
                    var closestRow = FindClosetRow(inSize, "kwm");
                    if (calcRow == null)
                    {
                        if (closestRow == null) {
                            returnHP = 1;
                        }
                        else {
                            returnHP = inSize / (decimal)0.746;
                        }
                    }
                    else {
                        returnHP = calcRow != null? calcRow.HP : 0;
                    }
                }

                var rValue = ((returnHP * 10) / 10);
                return decimal.Round(rValue, 2, MidpointRounding.AwayFromZero);
            }
            else if (targetUnits.ToLower() == "kwm")
            {
                var returnKWM= (decimal)0;
                if (currUnits.ToLower() == "amps")
                {
                    KVARunning = GetKVARunningFromAmps(voltage, phase, inSize);
                    var kvaRunningRounded = (KVARunning * 10);
                    KVARunning = kvaRunningRounded / 10;
                    var calcRow = _motorCalculationRepository.GetSingle(fd => fd.KVARunning == KVARunning);
                    var closestRow = FindClosetRow(inSize, "kvarunning");
                    if (calcRow == null) {
                        if (closestRow == null)
                        {
                            returnKWM = 1;
                        }
                        else
                        {
                            returnKWM = closestRow.kWm / GetAmpsFromKVARunning(voltage, phase, closestRow.KVARunning) * inSize;
                        }
                    }
                    else {
                            returnKWM = calcRow != null ? calcRow.kWm : 0;
                    }
                }
                else if (currUnits.ToLower() == "hp") {
                    var calcRow = _motorCalculationRepository.GetSingle(fd => fd.HP == inSize);
                    if (calcRow == null) {
                        returnKWM = inSize* (decimal)0.746;
                    }
                    else {
                        returnKWM = calcRow != null? calcRow.kWm : 0;
                    }
                }

                var rValue = ((returnKWM * 10) / 10);
                return decimal.Round(rValue, 2, MidpointRounding.AwayFromZero);
            }
            return 0;
        }
        #endregion

        #region update welder load

        private void UpdateWelderLoadDetails(WelderLoad welderLoad, Solution solution)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.AllIncluding(x => x.HarmonicDeviceType).ToList()
                                                    .Where(l => l.LoadID == welderLoad.Load.ID).FirstOrDefault();

            welderLoad.StartingLoadKva = GetKVAStartingWelderLoad(welderLoad);
            welderLoad.StartingLoadKw = GetKWStartingWelderLoad(welderLoad);
            welderLoad.RunningLoadKva = GetKVARunningWelderLoad(welderLoad);
            welderLoad.RunningLoadKw = GetKWRunningWelderLoad(welderLoad);
            welderLoad.THIDContinuous = GetTHIDContinuousWelderLoad(welderLoad, solutionLoadDefaultDetail);
            welderLoad.THIDMomentary = GetTHIDMomentaryWelderLoad(welderLoad, solutionLoadDefaultDetail);
            welderLoad.THIDKva = GetTHIDKvaWelderLoad(welderLoad, solutionLoadDefaultDetail);
            CalculateHarmonicDistortions(welderLoad, solutionLoadDefaultDetail);
            _welderLoadRepository.Update(welderLoad);
            _welderLoadRepository.Commit();
        }

        private decimal GetKVAStartingWelderLoad(WelderLoad welderLoad)
        {
            var multiplier = (decimal)1;
            var size = welderLoad.SizeRunning;
            var pf = welderLoad.RunningPF.Value;

            if (welderLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = welderLoad.Quantity;
            }

            if (welderLoad.SizeRunningUnits.Value == "kW")
            {
                return multiplier * (size.Value / Convert.ToDecimal(pf));
            }
            else if (welderLoad.SizeRunningUnits.Value == "kVA")
            {
                return multiplier * size.Value;
            }
            else
            {
                var voltageSpecific = Convert.ToInt32(welderLoad.VoltageSpecific.Value);
                if (Convert.ToInt32(welderLoad.VoltagePhase.Value) == 1)
                {
                    return multiplier * ((size.Value * voltageSpecific / 1000) * 1);
                }
                else
                {
                    return multiplier * ((size.Value * voltageSpecific / 1000) * (decimal)1.732);
                }
            }
        }

        private decimal GetKWStartingWelderLoad(WelderLoad welderLoad)
        {
            var multiplier = (decimal)1;

            if (welderLoad.Sequence.SequenceType.Description.Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = welderLoad.Quantity;
            }

            return multiplier * this.GetKVAStartingWelderLoad(welderLoad) * (decimal)0.4;
        }

        private decimal GetKVARunningWelderLoad(WelderLoad welderLoad)
        {
            var voltageSpecific = Convert.ToInt32(welderLoad.VoltageSpecific.Value);
            if (Convert.ToInt32(welderLoad.VoltageSpecific.Value) == 1)
            {
                return welderLoad.Quantity * ((welderLoad.SizeRunning.Value * voltageSpecific / 1000) * 1);
            }
            else
            {
                return welderLoad.Quantity * ((welderLoad.SizeRunning.Value * voltageSpecific / 1000) * (decimal)1.732);
            }
        }

        private decimal GetKWRunningWelderLoad(WelderLoad welderLoad)
        {
            var voltageSpecific = Convert.ToInt32(welderLoad.VoltageSpecific.Value);
            if (Convert.ToInt32(welderLoad.VoltageSpecific.Value) == 1)
            {
                return welderLoad.Quantity * ((welderLoad.SizeRunning.Value * voltageSpecific * Convert.ToDecimal(welderLoad.RunningPF.Value) / 1000) * 1);
            }
            else
            {
                return welderLoad.Quantity * ((welderLoad.SizeRunning.Value * voltageSpecific * Convert.ToDecimal(welderLoad.RunningPF.Value) / 1000) * (decimal)1.732);
            }
        }

        private decimal GetTHIDContinuousWelderLoad(WelderLoad welderLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.AcrossTheLine || loadDefaults.StartingMethodID != (int)StartingMethodEnum.ReducedVoltage)
            {
                thid = Convert.ToDecimal(welderLoad.HarmonicContent.Value);
            }

            return thid;
        }

        private decimal GetTHIDMomentaryWelderLoad(WelderLoad welderLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.SoftStarter)
            {
                thid = Convert.ToDecimal(welderLoad.HarmonicContent.Value);
            }

            return thid;
        }

        private decimal GetTHIDKvaWelderLoad(WelderLoad welderLoad, LoadDefaults loadDefaults)
        {
            if (GetTHIDMomentaryWelderLoad(welderLoad, loadDefaults) == 0 && GetTHIDContinuousWelderLoad(welderLoad, loadDefaults) == 0)
            {
                return 0;
            }

            return GetKVARunningWelderLoad(welderLoad);
        }

        private void CalculateHarmonicDistortions(WelderLoad welderLoad, LoadDefaults loadDefaults)
        {
            if (loadDefaults.HarmonicDeviceType != null)
            {
                var multiplier = 1;

                var thid = Convert.ToDecimal(Math.Sqrt(
                    Convert.ToDouble(
                    (loadDefaults.HarmonicDeviceType.HD3 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD5 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD7 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD9 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD11 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD13 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD15 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD17 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD19 * 2)
                    )));

                thid = Convert.ToDecimal(Math.Max(thid, (decimal)1));

                welderLoad.HD3 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD3.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD5 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD5.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD7 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD7.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD9 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD9.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD11 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD11.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD13 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD13.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD15 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD15.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD17 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD17.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
                welderLoad.HD19 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD19.Value * Convert.ToDecimal(welderLoad.HarmonicContent.Value)) / thid), 2);
            }
        }

        #endregion

        #region upate ups load

        private void UpdateUPSLoadDetails(UPSLoad upsLoad, Solution solution)
        {
            var solutionLoadDefaultDetail = _loadDefaultsRepository.AllIncluding(x => x.HarmonicDeviceType).ToList()
                                                    .Where(l => l.LoadID == upsLoad.Load.ID).FirstOrDefault();

            upsLoad.StartingLoadKva = GetKVAStartingUPSLoad(upsLoad);
            upsLoad.StartingLoadKw = GetKWStartingUPSLoad(upsLoad);
            upsLoad.RunningLoadKva = GetKVARunningUPSLoad(upsLoad);
            upsLoad.RunningLoadKw = GetKWRunningUPSLoad(upsLoad);
            upsLoad.THIDContinuous = GetTHIDContinuousUPSLoad(upsLoad, solutionLoadDefaultDetail);
            upsLoad.THIDMomentary = GetTHIDMomentaryUPSLoad(upsLoad, solutionLoadDefaultDetail);
            upsLoad.THIDKva = GetTHIDKvaUPSLoad(upsLoad, solutionLoadDefaultDetail);
            CalculateHarmonicDistortions(upsLoad, solutionLoadDefaultDetail);
            _upsLoadRepository.Update(upsLoad);
            _upsLoadRepository.Commit();
        }

        private decimal GetKVAStartingUPSLoad(UPSLoad upsLoad)
        {
            var multiplier = (decimal)1;
            if (upsLoad.Sequence.SequenceType.Description.ToLower().Equals("concurrent", StringComparison.InvariantCultureIgnoreCase))
            {
                multiplier = upsLoad.Quantity;
            }

            var rKVAInput = (decimal)0;
            if (upsLoad.SizeKVAUnits.Value.ToLower() == "input")
            {
                rKVAInput = upsLoad.SizeKVA.Value;
            }
            else
            {
                rKVAInput = upsLoad.SizeKVA.Value * Convert.ToDecimal(upsLoad.LoadLevel.Value) / Convert.ToDecimal(upsLoad.Efficiency.Value) + upsLoad.SizeKVA.Value * Convert.ToDecimal(upsLoad.ChargeRate.Value);
            }

            if (upsLoad.UPSType.Value.ToLower() == "passiveptandby")
            {
                return rKVAInput = (rKVAInput * 1);
            }
            else if (upsLoad.UPSType.Value.ToLower() == "lineinteractive")
            {
                return rKVAInput = (rKVAInput * 1);
            }
            else if (upsLoad.UPSType.Value.ToLower() == "ferroresonant")
            {
                return rKVAInput = (rKVAInput * 1);
            }
            else if (upsLoad.UPSType.Value.ToLower() == "deltaconversion")
            {
                return rKVAInput = (rKVAInput * (decimal)0.2);
            }
            else if (upsLoad.UPSType.Value.ToLower() == "doubleconversion")
            {
                return rKVAInput = (rKVAInput * (decimal)0.2);
            }
            else
                return multiplier * rKVAInput;
        }

        private decimal GetKWStartingUPSLoad(UPSLoad upsLoad)
        {
            var multiplier = (decimal)1;
            var pf = upsLoad.PowerFactor.Value;
            return multiplier * (GetKVAStartingUPSLoad(upsLoad) * Convert.ToDecimal(pf));
        }

        private decimal GetKVARunningUPSLoad(UPSLoad upsLoad)
        {
            var rKVAInput= (decimal)0;
            if (upsLoad.SizeKVAUnits.Value.ToLower() == "input")
            {
                rKVAInput = upsLoad.SizeKVA.Value;
            }
            else
            {
                rKVAInput = upsLoad.SizeKVA.Value * Convert.ToDecimal(upsLoad.LoadLevel.Value) / Convert.ToDecimal(upsLoad.Efficiency.Value) + upsLoad.SizeKVA.Value * Convert.ToDecimal(upsLoad.ChargeRate.Value);
            }

            return upsLoad.Quantity * rKVAInput;
        }

        private decimal GetKWRunningUPSLoad(UPSLoad upsLoad)
        {
            var pf = upsLoad.PowerFactor;
            return (GetKVARunningUPSLoad(upsLoad) * Convert.ToDecimal(pf.Value));
        }

        private decimal GetTHIDContinuousUPSLoad(UPSLoad upsLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.AcrossTheLine || loadDefaults.StartingMethodID != (int)StartingMethodEnum.ReducedVoltage)
            {
                thid = Convert.ToDecimal(upsLoad.HarmonicContent.Value);
            }

            return thid;
        }

        private decimal GetTHIDMomentaryUPSLoad(UPSLoad upsLoad, LoadDefaults loadDefaults)
        {
            var thid = (decimal)0;
            if (loadDefaults.StartingMethodID != (int)StartingMethodEnum.SoftStarter)
            {
                thid = Convert.ToDecimal(upsLoad.HarmonicContent.Value);
            }

            return thid;
        }

        private decimal GetTHIDKvaUPSLoad(UPSLoad upsLoad, LoadDefaults loadDefaults)
        {
            if (GetTHIDMomentaryUPSLoad(upsLoad, loadDefaults) == 0 && GetTHIDContinuousUPSLoad(upsLoad, loadDefaults) == 0)
            {
                return 0;
            }

            return GetKVARunningUPSLoad(upsLoad);
        }

        private void CalculateHarmonicDistortions(UPSLoad upsLoad, LoadDefaults loadDefaults)
        {
            if (loadDefaults.HarmonicDeviceType != null)
            {
                var multiplier = 1;

                var thid = Convert.ToDecimal(Math.Sqrt(
                    Convert.ToDouble(
                    (loadDefaults.HarmonicDeviceType.HD3 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD5 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD7 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD9 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD11 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD13 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD15 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD17 * 2) +
                    (loadDefaults.HarmonicDeviceType.HD19 * 2)
                    )));

                thid = Convert.ToDecimal(Math.Max(thid, (decimal)1));

                upsLoad.HD3 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD3.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD5 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD5.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD7 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD7.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD9 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD9.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD11 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD11.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD13 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD13.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD15 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD15.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD17 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD17.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
                upsLoad.HD19 = Math.Round(multiplier * ((loadDefaults.HarmonicDeviceType.HD19.Value * Convert.ToDecimal(upsLoad.HarmonicContent.Value)) / thid), 2);
            }
        }

        #endregion
    }
}

