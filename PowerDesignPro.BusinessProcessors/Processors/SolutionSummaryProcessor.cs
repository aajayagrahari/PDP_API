using PowerDesignPro.BusinessProcessors.Interface;
using System.Collections.Generic;
using System.Linq;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using System;
using PowerDesignPro.Common;
using System.Dynamic;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Data.Models.Loads;
using PowerDesignPro.BusinessProcessors.Dtos.Project;
using PowerDesignPro.Common.Mapper;
using System.Diagnostics;
using PowerDesignPro.Data.Models.User;
using PowerDesignPro.BusinessProcessors.Dtos.PickList;
using System.Configuration;
using PowerDesignPro.Common.UnitMeasure.Mapper;
using PowerDesignPro.Common.UnitMeasure;
using System.Threading.Tasks;
using System.Net;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    public class SolutionSummaryProcessor : ISolutionSummary
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;

        private readonly IEntityBaseRepository<Solution> _solutionRepository;

        private readonly IEntityBaseRepository<Sequence> _sequenceRepository;

        private readonly IEntityBaseRepository<SolutionSetup> _solutionSetupRepository;

        private readonly IEntityBaseRepository<BasicLoad> _basicLoadRepository;

        private readonly IEntityBaseRepository<ACLoad> _acLoadRepository;

        private readonly IEntityBaseRepository<LightingLoad> _lightingLoadRepository;

        private readonly IEntityBaseRepository<UPSLoad> _upsLoadRepository;

        private readonly IEntityBaseRepository<WelderLoad> _welderLoadRepository;

        private readonly IEntityBaseRepository<MotorLoad> _motorLoadRepository;

        private readonly IEntityBaseRepository<LoadSequence> _loadSequenceRepository;

        private readonly IEntityBaseRepository<FrequencyDip> _frequencyDipRepository;

        private readonly IEntityBaseRepository<VoltageDip> _voltageDipRepository;

        private readonly IEntityBaseRepository<ProductFamily> _productFamilyRepository;

        private readonly IEntityBaseRepository<Generator> _generatorRepository;

        private readonly IEntityBaseRepository<Alternator> _alternatorRepository;

        private readonly IEntityBaseRepository<RecommendedProduct> _recommendedProductRepository;

        private readonly IEntityBaseRepository<ParallelQuantity> _parallelQuantityRepository;

        private readonly IEntityBaseRepository<Brand> _brandRepository;

        private readonly IGeneratorAlternator _generatorAlternator;

        private readonly IPickList _pickListProcessor;

        private ITraceMessage _traceMessageProcessor;

        private readonly IEntityBaseRepository<GasPipingSizingMethod> _gasPipingSizingMethodRepository;

        private readonly IEntityBaseRepository<GasPipingPipeSize> _gasPipingPipeSizeRepository;

        private readonly IEntityBaseRepository<ExhaustPipingPipeSize> _exhaustPipingPipeSizeRepository;

        private readonly IEntityBaseRepository<GasPipingSolution> _gasPipingSolutionRepository;

        private readonly IEntityBaseRepository<ExhaustPipingSolution> _exhaustPipingSolutionRepository;

        private readonly IEntityBaseRepository<HarmonicProfile> _harmonicProfileRepository;

        private readonly IEntityBaseRepository<RequestForQuote> _requestForQuoteRepository;

        /// <summary>
        /// Private variables to calculate the Recommended Product
        /// </summary>

        private LoadSummaryLoadsDto _loadSummaryLoads;
        private LoadSummaryLoadListDto _loadSummaryLoadListDto;
        private IEnumerable<int> _regulatoryFilters;
        private int _solutionFrequency;

        private SolutionSummaryLoadSummaryDto _solutionSummaryLoadSummary;
        private Dictionary<int, decimal> _FDipList = new Dictionary<int, decimal>();
        private Dictionary<int, decimal> _VDipList = new Dictionary<int, decimal>();

        private int _ParallelQuantity = 1;
        private int _maxParallelQuantity;
        private int? _GeneratorID;
        private int? _AlternatorID;
        private decimal[,] _LargestContinuousHarmonicWithLoadFactor;
        private decimal[,] _PeakHarmonicWithLoadFactor;

        private SizingMethodEnum _SizingMethod;
        private FamilySelectionMethodEnum _FamiySelectionMethod;
        private int? _ProductFamilyID;
        private decimal _fDip;
        private decimal _vDip;
        private decimal _metricMultiplecationPipeSizeFactor;
        private int _tempRank;
        private decimal _exhaustFlowMetricConversionFactor;
        private decimal _pressureMetricConversionFactor;
        private decimal _meterToFootConversionFactor;
        private IDictionary<string, decimal> _conversionFactorList = new Dictionary<string, decimal>();
        private UnitMeasureCollection _unitOfMeasureCollection;
        private decimal _applicationPeak = 0;
        private decimal _defaultSupplyGasPressure = 15;
        private decimal _defaultGeneratorMinPressure = 11;

        public SolutionSummaryProcessor(
            IEntityBaseRepository<Project> projectRepository,
            IEntityBaseRepository<Solution> solutionRepository,
            IEntityBaseRepository<Sequence> sequenceRepository,
            IEntityBaseRepository<SolutionSetup> solutionSetupRepository,
            IEntityBaseRepository<BasicLoad> basicLoadRepository,
            IEntityBaseRepository<ACLoad> acLoadRepository,
            IEntityBaseRepository<LightingLoad> lightingLoadRepository,
            IEntityBaseRepository<UPSLoad> upsLoadRepository,
            IEntityBaseRepository<WelderLoad> welderLoadRepository,
            IEntityBaseRepository<MotorLoad> motorLoadRepository,
            IEntityBaseRepository<LoadSequence> loadSequenceRepository,
            IEntityBaseRepository<FrequencyDip> frequencyDipRepository,
            IEntityBaseRepository<VoltageDip> voltageDipRepository,
            IEntityBaseRepository<ProductFamily> productFamilyRepository,
            IEntityBaseRepository<Generator> generatorRepository,
            IEntityBaseRepository<Alternator> alternatorRepository,
            IEntityBaseRepository<RecommendedProduct> recommendedProductRepository,
            IEntityBaseRepository<ParallelQuantity> parallelQuantityRepository,
            IEntityBaseRepository<Brand> brandRepository,
            IGeneratorAlternator generatorAlternator,
            IPickList pickListProcessor,
            ITraceMessage traceMessageProcessor,
            IEntityBaseRepository<GasPipingSizingMethod> gasPipingSizingMethodRepository,
            IEntityBaseRepository<GasPipingPipeSize> gasPipingPipeSizeRepository,
            IEntityBaseRepository<ExhaustPipingPipeSize> exhaustPipingPipeSizeRepository,
            IEntityBaseRepository<GasPipingSolution> gasPipingSolutionRepository,
            IEntityBaseRepository<ExhaustPipingSolution> exhaustPipingSolutionRepository,
            IEntityBaseRepository<HarmonicProfile> harmonicProfileRepository,
            IEntityBaseRepository<RequestForQuote> requestForQuoteRepository)
        {
            _projectRepository = projectRepository;
            _solutionRepository = solutionRepository;
            _sequenceRepository = sequenceRepository;
            _solutionSetupRepository = solutionSetupRepository;
            _basicLoadRepository = basicLoadRepository;
            _acLoadRepository = acLoadRepository;
            _lightingLoadRepository = lightingLoadRepository;
            _upsLoadRepository = upsLoadRepository;
            _welderLoadRepository = welderLoadRepository;
            _motorLoadRepository = motorLoadRepository;
            _loadSequenceRepository = loadSequenceRepository;
            _frequencyDipRepository = frequencyDipRepository;
            _voltageDipRepository = voltageDipRepository;
            _productFamilyRepository = productFamilyRepository;
            _generatorRepository = generatorRepository;
            _alternatorRepository = alternatorRepository;
            _recommendedProductRepository = recommendedProductRepository;
            _parallelQuantityRepository = parallelQuantityRepository;
            _generatorAlternator = generatorAlternator;
            _pickListProcessor = pickListProcessor;
            _traceMessageProcessor = traceMessageProcessor;
            _brandRepository = brandRepository;
            _gasPipingSizingMethodRepository = gasPipingSizingMethodRepository;
            _gasPipingPipeSizeRepository = gasPipingPipeSizeRepository;
            _exhaustPipingPipeSizeRepository = exhaustPipingPipeSizeRepository;
            _gasPipingSolutionRepository = gasPipingSolutionRepository;
            _exhaustPipingSolutionRepository = exhaustPipingSolutionRepository;
            _harmonicProfileRepository = harmonicProfileRepository;
            _requestForQuoteRepository = requestForQuoteRepository;

            _loadSummaryLoads = new LoadSummaryLoadsDto();
            _solutionSummaryLoadSummary = new SolutionSummaryLoadSummaryDto();
            _loadSummaryLoadListDto = new LoadSummaryLoadListDto();
            _regulatoryFilters = new List<int>();
            _metricMultiplecationPipeSizeFactor = Convert.ToDecimal(ConfigurationManager.AppSettings["metricMultiplecationPipeSizeFactor"]);
            _tempRank = Convert.ToInt32(ConfigurationManager.AppSettings["temp_rank"]);
            _exhaustFlowMetricConversionFactor = Convert.ToDecimal(ConfigurationManager.AppSettings["exhaustFlowMetricConversionFactor"]);
            _pressureMetricConversionFactor = Convert.ToDecimal(ConfigurationManager.AppSettings["pressureMetricConversionFactor"]);
            _meterToFootConversionFactor = Convert.ToDecimal(ConfigurationManager.AppSettings["meterToFootConversionFactor"]);
            _conversionFactorList = Conversion.GetConversionFactorList();
            _unitOfMeasureCollection = new UnitMeasureCollection();
        }

        /// <summary>
        /// Method to get Solution Summary
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public SolutionSummaryDto GetSolutionSummary(int projectID, int solutionID, string userID, string brand, string userName = "")
        {
            try
            {
                var isShared = _projectRepository.GetSingle(x => x.ID == projectID && x.UserID != userID)?.SharedProjects.Any(x => x.RecipientEmail.ToLower() == userName.ToLower());
                var solution = _solutionRepository.GetSingle(x => x.ID == solutionID && x.ProjectID == projectID && (x.Project.UserID == userID || isShared == true));

                if (solution == null)
                    throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);

                var solutionSetup = solution.SolutionSetup.FirstOrDefault();
                var solutionLimits = MapSolutionLimits(solutionSetup);
                var loadSummaryLoads = GetLoadSummaryLoads(projectID, solutionID, userID, userName);

                _loadSummaryLoads = loadSummaryLoads;
                _solutionSummaryLoadSummary = loadSummaryLoads.SolutionSummaryLoadSummary;

                var recommendedProductDetail = GetRecommendedProduct(solution.RecommendedProduct.FirstOrDefault(), solutionLimits, brand);

                if (recommendedProductDetail != null)
                    UpdateRecommendedProduct(solutionID, solution, recommendedProductDetail);

                if (recommendedProductDetail.GeneratorID != null && recommendedProductDetail.AlternatorID != null)
                    loadSummaryLoads = FillAnalysisProperties(loadSummaryLoads, recommendedProductDetail, solutionSetup);

                _loadSummaryLoads = loadSummaryLoads;

                return new SolutionSummaryDto
                {
                    SolutionName = solution.SolutionName,
                    SolutionLimits = solutionLimits,
                    LoadSummaryLoads = loadSummaryLoads,
                    SolutionSummaryRecommendedProductDetails = recommendedProductDetail,
                    FuelType = solutionSetup.FuelType.Code
                };
            }
            catch (PowerDesignProException)
            {
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
            }
            catch (Exception ex)
            {
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + solutionID.ToString(), ex.TargetSite.Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Method to get Loads List in the Solution Summary
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public LoadSummaryLoadsDto GetLoadSummaryLoads(int projectID, int solutionID, string userID, string userName = "")
        {
            try
            {
                var isShared = _projectRepository.GetSingle(x => x.ID == projectID && x.UserID != userID)?.SharedProjects.Any(x => x.RecipientEmail.ToLower() == userName.ToLower());
                var solution = _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && (x.Project.UserID == userID || isShared == true)).Count();

                var solutionSetup = _solutionSetupRepository.GetSingle(s => s.SolutionID == solutionID);
                if (solutionSetup != null)
                    _solutionFrequency = int.Parse(solutionSetup.Frequency.Value);

                if (solution == 0)
                    throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);

                var result = new LoadSummaryLoadsDto();

                var solutionDetail = _solutionRepository
                    .AllIncluding(x => x.BasicLoadList,
                    x => x.MotorLoadList,
                    x => x.WelderLoadList,
                    x => x.LightingLoadList,
                    x => x.ACLoadList,
                    x => x.UPSLoadList,
                    x => x.SolutionSetup,
                    x => x.LoadSequence).FirstOrDefault(x => x.ID == solutionID);

                var sequenceDetail = _sequenceRepository.AllIncluding(s => s.SequenceType).ToList();

                if (solutionDetail == null)
                    throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);

                var loadDetail = new List<LoadSequenceDto>();

                var basicLoadDetail = solutionDetail.BasicLoadList.Select(GetLoadSequencData).ToList();
                loadDetail.AddRange(basicLoadDetail);
                var acLoadDetail = solutionDetail.ACLoadList.Select(GetLoadSequencData).ToList();
                loadDetail.AddRange(acLoadDetail);
                var welderLoadDetail = solutionDetail.WelderLoadList.Select(GetLoadSequencData).ToList();
                loadDetail.AddRange(welderLoadDetail);
                var motorLoadDetail = solutionDetail.MotorLoadList.Select(GetLoadSequencData).ToList();
                loadDetail.AddRange(motorLoadDetail);
                var lightingLoadDetail = solutionDetail.LightingLoadList.Select(GetLoadSequencData).ToList();
                loadDetail.AddRange(lightingLoadDetail);
                var upsLoadDetail = solutionDetail.UPSLoadList.Select(GetLoadSequencData).ToList();
                loadDetail.AddRange(upsLoadDetail);

                var solutionSetupDetail = solutionDetail.SolutionSetup.FirstOrDefault();

                foreach (var item in loadDetail.OrderBy(l => l.SequenceID).GroupBy(g => new { g.SequenceID }))
                {
                    //_loadSummaryLoadListDto = new LoadSummaryLoadListDto();
                    var loadSummaryLoadListDto = new LoadSummaryLoadListDto();
                    var sequenceId = item.Key.SequenceID;

                    foreach (var load in loadDetail.Where(x => x.SequenceID == sequenceId))
                    {
                        var description = GetLoadDescription(solutionDetail, load.SolutionLoadID, load.LoadFamilyID, load.LanguageKey);

                        if (description != null)
                        {
                            load.LoadName = description.LoadName;
                            load.Description = description.Description;
                            load.KVAPFDescription = description.KVAPFDescription;
                            load.HarmonicsDescription = description.HarmonicsDescription; 
                        }
                        loadSummaryLoadListDto.LoadSequenceList.Add(load);
                    }

                    var sequenceTypeID = sequenceDetail.Where(s => s.ID == sequenceId).FirstOrDefault().SequenceType.ID;
                    var sequence = sequenceDetail.Where(s => s.ID == sequenceId).FirstOrDefault().Value.ToLower();
                    var sequenceLoadCount = loadDetail.Where(ld => ld.SequenceID == item.Key.SequenceID).Count();
                    var isShed = solutionDetail.LoadSequence.Where(ls => ls.SequenceID == sequenceId).Count() > 0;

                    loadSummaryLoadListDto.SequenceID = sequenceId;
                    loadSummaryLoadListDto.SequenceTypeID = sequenceTypeID;
                    loadSummaryLoadListDto.SequenceDescription = sequenceDetail.Where(s => s.ID == sequenceId).FirstOrDefault().LanguageKey;
                    loadSummaryLoadListDto.LoadFactor = GetLoadFactor(sequence, solutionSetupDetail);
                    loadSummaryLoadListDto.LoadSequenceSummary = new LoadSequenceSummaryDto();
                    loadSummaryLoadListDto.LoadSequenceSummary.LoadCount = sequenceLoadCount;
                    loadSummaryLoadListDto.LoadSequenceSummary.Shed = isShed;
                    _loadSummaryLoadListDto = loadSummaryLoadListDto;
                    loadSummaryLoadListDto.LoadSequenceSummary = GetLoadSequenceSummary(loadSummaryLoadListDto, sequenceTypeID, sequence, solutionSetupDetail);
                    loadSummaryLoadListDto.LoadSequenceSummary.SequenceDescription = loadSummaryLoadListDto.SequenceDescription;
                    loadSummaryLoadListDto.LoadSequenceSummary.SequencePeakText = Conversion.GetRoundedDecimal(loadSummaryLoadListDto.LoadSequenceSummary.PeakLoadValue, 1) + "kW";
                    loadSummaryLoadListDto.LoadSequenceSummary.ApplicationPeakText = Conversion.GetRoundedDecimal(loadSummaryLoadListDto.LoadSequenceSummary.ApplicationPeak, 1) + "kW";

                    result.ListOfLoadSummaryLoadList.Add(loadSummaryLoadListDto);
                }
                _loadSummaryLoads = result;
                result.ListOfLoadSummaryLoadList = result.ListOfLoadSummaryLoadList.OrderBy(o => o.SequenceID).ToList();
                result.SolutionSummaryLoadSummary = GetSolutionSummaryLoadSummary(result);
                result.SolutionID = solutionID;
                result.ProjectID = projectID;
                return result;
            }
            catch (PowerDesignProException)
            {
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
            }
            catch (Exception ex)
            {
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + solutionID.ToString(), ex.TargetSite.Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Method to get Soluton Limits
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public SolutionLimitsDto GetSolutionLimits(int projectID, int solutionID, string userID, string userName = "")
        {
            var isShared = _projectRepository.GetSingle(x => x.ID == projectID && x.UserID != userID)?.SharedProjects.Any(x => x.RecipientEmail.ToLower() == userName.ToLower());
            var solution = _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && (x.Project.UserID == userID || isShared == true)).Count();

            if (solution == 0)
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);

            var solutionSetup = _solutionSetupRepository.GetSingle(ss => ss.SolutionID == solutionID);

            return MapSolutionLimits(solutionSetup);
        }

        public bool UpdateLoadSequence(LoadSequenceRequestDto requestDto, string userID, string userName = "")
        {
            var isShared = _projectRepository.GetSingle(x => x.ID == requestDto.ProjectID && x.UserID != userID)?.SharedProjects.Any(x => x.RecipientEmail.ToLower() == userName.ToLower());
            switch (requestDto.LoadFamilyID)
            {
                case (int)SolutionLoadFamilyEnum.Basic:
                    var solutionBasicLoad = _basicLoadRepository.GetSingle(x => x.ID == requestDto.SolutionLoadID && (x.Solution.Project.UserID == userID || isShared == true));
                    if (solutionBasicLoad == null)
                    {
                        throw new PowerDesignProException("BasicLoadNotFound", Message.SolutionLoad);
                    }
                    solutionBasicLoad.SequenceID = requestDto.LoadSequenceID;
                    _basicLoadRepository.Update(solutionBasicLoad);
                    _basicLoadRepository.Commit();
                    return true;
                case (int)SolutionLoadFamilyEnum.AC:
                    var solutionAcLoad = _acLoadRepository.GetSingle(x => x.ID == requestDto.SolutionLoadID && (x.Solution.Project.UserID == userID || isShared == true));
                    if (solutionAcLoad == null)
                    {
                        throw new PowerDesignProException("ACLoadNotFound", Message.SolutionLoad);
                    }
                    solutionAcLoad.SequenceID = requestDto.LoadSequenceID;
                    _acLoadRepository.Update(solutionAcLoad);
                    _acLoadRepository.Commit();
                    return true;
                case (int)SolutionLoadFamilyEnum.Lighting:
                    var solutionLightingLoad = _lightingLoadRepository.GetSingle(x => x.ID == requestDto.SolutionLoadID && (x.Solution.Project.UserID == userID || isShared == true));
                    if (solutionLightingLoad == null)
                    {
                        throw new PowerDesignProException("LightingLoadNotFound", Message.SolutionLoad);
                    }
                    solutionLightingLoad.SequenceID = requestDto.LoadSequenceID;
                    _lightingLoadRepository.Update(solutionLightingLoad);
                    _lightingLoadRepository.Commit();
                    return true;
                case (int)SolutionLoadFamilyEnum.UPS:
                    var solutionUpsLoad = _upsLoadRepository.GetSingle(x => x.ID == requestDto.SolutionLoadID && (x.Solution.Project.UserID == userID || isShared == true));
                    if (solutionUpsLoad == null)
                    {
                        throw new PowerDesignProException("UpsLoadNotFound", Message.SolutionLoad);
                    }
                    solutionUpsLoad.SequenceID = requestDto.LoadSequenceID;
                    _upsLoadRepository.Update(solutionUpsLoad);
                    _upsLoadRepository.Commit();
                    return true;
                case (int)SolutionLoadFamilyEnum.Welder:
                    var solutionWelderLoad = _welderLoadRepository.GetSingle(x => x.ID == requestDto.SolutionLoadID && (x.Solution.Project.UserID == userID || isShared == true));
                    if (solutionWelderLoad == null)
                    {
                        throw new PowerDesignProException("WelderLoadNotFound", Message.SolutionLoad);
                    }
                    solutionWelderLoad.SequenceID = requestDto.LoadSequenceID;
                    _welderLoadRepository.Update(solutionWelderLoad);
                    _welderLoadRepository.Commit();
                    return true;
                case (int)SolutionLoadFamilyEnum.Motor:
                    var solutionMotorLoad = _motorLoadRepository.GetSingle(x => x.ID == requestDto.SolutionLoadID && (x.Solution.Project.UserID == userID || isShared == true));
                    if (solutionMotorLoad == null)
                    {
                        throw new PowerDesignProException("MotorLoadNotFound", Message.SolutionLoad);
                    }
                    solutionMotorLoad.SequenceID = requestDto.LoadSequenceID;
                    _motorLoadRepository.Update(solutionMotorLoad);
                    _motorLoadRepository.Commit();
                    return true;
                default:
                    throw new PowerDesignProException("LoadFamilyNotFound", Message.SolutionLoad);
            }
        }

        public bool UpdateLoadSequenceShedDetail(LoadSequenceShedRequestDto requestDto, string userID)
        {
            var loadSequence = _loadSequenceRepository.GetSingle(x => x.SolutionID == requestDto.SolutionID && x.SequenceID == requestDto.SequenceID);
            if (requestDto.Shed)
            {
                var loadSequenceEntity = new LoadSequence
                {
                    SequenceID = requestDto.SequenceID,
                    SolutionID = requestDto.SolutionID
                };

                _loadSequenceRepository.Add(loadSequenceEntity);
            }
            else
            {
                if (loadSequence != null)
                {
                    _loadSequenceRepository.Delete(loadSequence);
                }
            }

            _loadSequenceRepository.Commit();

            return true;
        }

        public SolutionSummaryRecommendedProductDto UpdateRecommendedProductDetails(RecommendedProductRequestDto recommendedProductRequestDto, string userID, string userName)
        {
            try
            {
                var isShared = _projectRepository.GetSingle(x => x.ID == recommendedProductRequestDto.ProjectID && x.UserID != userID)?.SharedProjects.Any(x => x.RecipientEmail.ToLower() == userName.ToLower());
                var solution = _solutionRepository.GetSingle(x => x.ID == recommendedProductRequestDto.SolutionID && x.ProjectID == recommendedProductRequestDto.ProjectID && (x.Project.UserID == userID || isShared == true));              

                if (solution == null)
                {
                    throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
                }
                var solutionSetupDetail = _solutionSetupRepository.GetSingle(s => s.SolutionID == recommendedProductRequestDto.SolutionID);
                var recommendedProduct = solution.RecommendedProduct.FirstOrDefault();
                if (recommendedProduct != null)
                {
                    if (recommendedProductRequestDto.FamilySelectionMethodID > 0)
                        recommendedProduct.FamilySelectionMethodID = recommendedProductRequestDto.FamilySelectionMethodID;

                    if (recommendedProductRequestDto.SizingMethodID > 0)
                        recommendedProduct.SizingMethodID = recommendedProductRequestDto.SizingMethodID;

                    recommendedProduct.ProductFamilyID = recommendedProductRequestDto.ProductFamilyID;
                    recommendedProduct.GeneratorID = recommendedProductRequestDto.GeneratorID;

                    if (recommendedProductRequestDto.AlternatorID != null)
                        recommendedProduct.AlternatorID = recommendedProductRequestDto.AlternatorID;
                    else if (recommendedProduct.GeneratorID != null)
                    {
                        var generatorList = _generatorRepository.GetAll(x => x.GeneratorAvailableAlternators.Any(y => y.GeneratorID == recommendedProductRequestDto.GeneratorID))
                                                                .FirstOrDefault();
                        if (generatorList != null)
                        {
                            var alternatorList = generatorList.GeneratorAvailableAlternators.Select(g => g.Alternator).Where(a => a.Active
                                                                                                                       && a.VoltagePhaseID == solutionSetupDetail.VoltagePhaseID
                                                                                                                       && a.FrequencyID == solutionSetupDetail.FrequencyID
                                                                                                                       && a.VoltageNominalID == solutionSetupDetail.VoltageNominalID
                                                                                                                       ).OrderBy(a => a.KWRating).ToList();
                            recommendedProduct.AlternatorID = alternatorList.FirstOrDefault().ID;
                        }
                        else
                        {
                            recommendedProduct.AlternatorID = null;
                        }
                    }
                    else
                        recommendedProduct.AlternatorID = null;

                    recommendedProduct.ParallelQuantityID = recommendedProductRequestDto.ParallelQuantityID;

                    _recommendedProductRepository.Update(recommendedProduct);
                    _recommendedProductRepository.Commit();
                }

                var solutionSetup = solution.SolutionSetup.FirstOrDefault();
                var solutionLimits = MapSolutionLimits(solutionSetup);
                var loadSummaryLoads = GetLoadSummaryLoads(recommendedProductRequestDto.ProjectID, recommendedProductRequestDto.SolutionID, userID, userName);

                _loadSummaryLoads = loadSummaryLoads;
                _solutionSummaryLoadSummary = loadSummaryLoads.SolutionSummaryLoadSummary;

                var recommendedProductDetail = GetRecommendedProduct(solution.RecommendedProduct.FirstOrDefault(), solutionLimits, recommendedProductRequestDto.Brand);

                if (recommendedProductDetail.GeneratorID > 0 && recommendedProductDetail.AlternatorID > 0)
                    UpdateRecommendedProduct(recommendedProductRequestDto.SolutionID, solution, recommendedProductDetail);

                return recommendedProductDetail;
            }
            catch (PowerDesignProException)
            {
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
            }
            catch (Exception ex)
            {
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + recommendedProductRequestDto.SolutionID.ToString(), ex.TargetSite.Name, ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        public GasPipingDto GetGasPipingDetails(int projectID, int solutionID)
        {
            GasPipingDto gasPiping;
            var solutionSetupDetail = _solutionSetupRepository.GetSingle(x => x.SolutionID == solutionID);
            var recommendedProduct = solutionSetupDetail.Solution.RecommendedProduct.FirstOrDefault();

            if (recommendedProduct.GeneratorID == null || recommendedProduct.AlternatorID == null)
            {
                return null;
            }

            if (solutionSetupDetail.FuelTypeID != (int)FuelTypeEnum.NaturalGas 
                && solutionSetupDetail.FuelTypeID != (int)FuelTypeEnum.LPVapor
                && solutionSetupDetail.FuelTypeID != (int)FuelTypeEnum.DualFuelVapor
                && solutionSetupDetail.FuelTypeID != (int)FuelTypeEnum.BiFuel
                )
            {
                gasPiping = new GasPipingDto();
                gasPiping.IsGasesousSolution = false;
                return gasPiping;
            }
            var generatorDetail = _generatorAlternator.GetGeneratorAlternatorDetailByID(recommendedProduct.Generator.ID, solutionSetupDetail, Convert.ToInt32(recommendedProduct.ParallelQuantity.Value));
            gasPiping = GetDefaultGasPipingData(solutionSetupDetail);
            var savedGasPipingDetail = _gasPipingSolutionRepository.GetSingle(x => x.SolutionID == solutionID);
            if (savedGasPipingDetail != null)
            {
                MapGasPipingEntityDetailToDto(savedGasPipingDetail, solutionSetupDetail.UnitsID, ref gasPiping);
            }
            else
            {
                gasPiping.SizingMethodID = gasPiping.SizingMethodList.FirstOrDefault().ID;
                gasPiping.PipeSizeID = gasPiping.PipeSizeList.FirstOrDefault().ID;
                gasPiping.GasPipingInput.SupplyGasPressure = _defaultSupplyGasPressure;
                gasPiping.GasPipingInput.GeneratorMinPressure = Convert.ToDecimal(generatorDetail.Generator.NG_h20);
            }            

            gasPiping.GeneratorSummary = new GeneratorSummaryDto
            {
                Generator = $"{(generatorDetail.Quantity > 1 ? generatorDetail.Quantity.ToString() + " x " : "")}{generatorDetail.Generator.ModelDescription}",
                FuelType = solutionSetupDetail.FuelType.LanguageKey,
                ProductFamily = recommendedProduct.ProductFamily.LanguageKey,
                FuelConsumption = GetFuelConsumption(
                    generatorDetail.Generator,
                    gasPiping.SingleUnit,
                    Convert.ToInt32(recommendedProduct.ParallelQuantity.Value)),
                KWPrime = generatorDetail.Generator.KWPrime,
                KWStandby = generatorDetail.Generator.KwStandby,
                NG_CF_HR = generatorDetail.Generator.NG_CF_HR,
                Quantity = Convert.ToInt32(recommendedProduct.ParallelQuantity.Value),
                EngineDuty = solutionSetupDetail.EngineDutyID,
                GasFuelFlow = CalculateGasFuelFlow(
                    generatorDetail.Generator,
                    solutionSetupDetail.FuelType.Code,
                    solutionSetupDetail.EngineDutyID,
                    gasPiping.SingleUnit,
                    Convert.ToInt32(recommendedProduct.ParallelQuantity.Value))
            };

            gasPiping.UnitMeasure = GetUnitMeasuresForPiping(solutionSetupDetail.UnitsID);
            gasPiping.FuelConfig = GetFuelConfig(solutionSetupDetail.FuelType.ID);
            gasPiping.TempRank = _tempRank;
            gasPiping.UnitSelected = solutionSetupDetail.UnitsID;

            var fromUnit = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == gasPiping.UnitSelected);
            gasPiping.GasPipingSolution.SolutionFound = ValidateGasPiping(gasPiping, fromUnit);
            ConvertGasPipingDataFromEnglishToSelectedUnit(gasPiping.UnitSelected, gasPiping);
            gasPiping.IsGasesousSolution = true;

            return gasPiping;
        }

        public ExhaustPipingDto GetExhaustPipingDetails(int projectID, int solutionID)
        {
            var solutionSetupDetail = _solutionSetupRepository.GetSingle(x => x.SolutionID == solutionID);
            var recommendedProduct = solutionSetupDetail.Solution.RecommendedProduct.FirstOrDefault();

            if (recommendedProduct.GeneratorID == null || recommendedProduct.AlternatorID == null)
                return null;

            var exhaustPiping = GetDefaultExhaustPipingData(solutionSetupDetail);
            var savedExhaustPipingDetail = _exhaustPipingSolutionRepository.GetSingle(x => x.SolutionID == solutionID);
            var units = solutionSetupDetail.UnitsID;

            if (savedExhaustPipingDetail != null)
            {
                MapExhaustPipingEntityDetailToDto(savedExhaustPipingDetail, units, ref exhaustPiping);
            }
            else
            {
                exhaustPiping.PipeSizeID = exhaustPiping.PipeSizeList.FirstOrDefault().ID;
                exhaustPiping.SizingMethodID = exhaustPiping.SizingMethodList.FirstOrDefault().ID;
                exhaustPiping.ExhaustSystemConfigurationID = exhaustPiping.ExhaustSystemConfigurationList.FirstOrDefault().ID;
            }

            var generatorDetail = _generatorAlternator.GetGeneratorAlternatorDetailByID(recommendedProduct.Generator.ID, solutionSetupDetail, Convert.ToInt32(recommendedProduct.ParallelQuantity.Value));

            bool isDual = exhaustPiping.ExhaustSystemConfigurationList.FirstOrDefault(x => x.ID == exhaustPiping.ExhaustSystemConfigurationID)
                                                                      .Value.ToLower() == "dual" ? true : false;

            exhaustPiping.ExhaustPipingGeneratorSummary = new ExhaustPipingGeneratorSummaryDto
            {
                Generator = generatorDetail.Generator.ModelDescription,
                ProductFamily = recommendedProduct.ProductFamily.LanguageKey,
                KWPrime = generatorDetail.Generator.KWPrime,
                KWStandby = generatorDetail.Generator.KwStandby,
                NG_CF_HR = generatorDetail.Generator.NG_CF_HR,
                Quantity = Convert.ToInt32(recommendedProduct.ParallelQuantity.Value),
                EngineDuty = solutionSetupDetail.EngineDutyID,
                ExhaustTempF = generatorDetail.Generator.ExhaustTempF,
                TotalExhaustFlow = Conversion.GetRoundedDecimal(CalculateTotalExhaustFlow(generatorDetail.Generator, solutionSetupDetail.EngineDuty.Value, units, isDual), 2),
                //MaximumBackPressure = GetRoundedDecimal(generatorDetail.Generator.ExhaustHg * 13.595m, 2)
                MaximumBackPressure = Conversion.GetRoundedDecimal(CalculateMaximumBackPressure(generatorDetail.Generator.ExhaustHg, units), 2),
                ExhaustFlex = generatorDetail.Generator.ExhaustFlex
            };

            exhaustPiping.UnitSelected = solutionSetupDetail.UnitsID;
            exhaustPiping.UnitMeasure = GetUnitMeasuresForPiping(solutionSetupDetail.UnitsID);

            return exhaustPiping;
        }

        public GasPipingDto SaveGasPipingSolution(GasPipingRequestDto gasPipingRequest, string userName)
        {
            var solutionSetupDetail = _solutionSetupRepository.GetSingle(x => x.SolutionID == gasPipingRequest.SolutionID);
            var recommendedProduct = solutionSetupDetail.Solution.RecommendedProduct.FirstOrDefault();
            var generatorDetail = _generatorAlternator.GetGeneratorAlternatorDetailByID(recommendedProduct.Generator.ID, solutionSetupDetail, Convert.ToInt32(recommendedProduct.ParallelQuantity.Value));
            gasPipingRequest.GasPiping.GeneratorSummary.GasFuelFlow = CalculateGasFuelFlow(
                    generatorDetail.Generator,
                    solutionSetupDetail.FuelType.Code,
                    solutionSetupDetail.EngineDutyID,
                    gasPipingRequest.GasPiping.SingleUnit,
                    Convert.ToInt32(recommendedProduct.ParallelQuantity.Value));

            gasPipingRequest.GasPiping.GeneratorSummary.FuelConsumption = GetFuelConsumption(
                    generatorDetail.Generator,
                    gasPipingRequest.GasPiping.SingleUnit,
                    Convert.ToInt32(recommendedProduct.ParallelQuantity.Value));

            var solutionFound = UpdateGasPiping(gasPipingRequest.GasPiping, gasPipingRequest.GasPiping.UnitSelected);
            gasPipingRequest.GasPiping.GasPipingSolution.SolutionFound = solutionFound;            

            var result = new GasPipingSolution();
            if (gasPipingRequest.GasPiping.ID == 0)
            {
                var gasPipingSolution = new GasPipingSolution
                {
                    AllowablePercentage = gasPipingRequest.GasPiping.GasPipingSolution.AllowablePercentage,
                    PressureDrop = gasPipingRequest.GasPiping.GasPipingSolution.PressureDrop,
                    AvailablePressure = gasPipingRequest.GasPiping.GasPipingSolution.AvailablePressure,
                    LengthOfRun = gasPipingRequest.GasPiping.GasPipingInput.LengthOfRun,
                    NumberOf45Elbows = gasPipingRequest.GasPiping.GasPipingInput.NumberOf45Elbows,
                    NumberOf90Elbows = gasPipingRequest.GasPiping.GasPipingInput.NumberOf90Elbows,
                    NumberOfTees = gasPipingRequest.GasPiping.GasPipingInput.NumberOfTees,
                    SupplyGasPressure = gasPipingRequest.GasPiping.GasPipingInput.SupplyGasPressure,
                    GeneratorMinPressure = gasPipingRequest.GasPiping.GasPipingInput.GeneratorMinPressure,
                    PipeSizeID = gasPipingRequest.GasPiping.PipeSizeID,
                    SizingMethodID = gasPipingRequest.GasPiping.SizingMethodID,
                    UnitID = gasPipingRequest.GasPiping.UnitSelected,
                    SolutionID = gasPipingRequest.SolutionID,
                    SingleUnit = gasPipingRequest.GasPiping.SingleUnit,
                    FuelConsumption = gasPipingRequest.GasPiping.GeneratorSummary.FuelConsumption
                };

                result = _gasPipingSolutionRepository.Add(gasPipingSolution);
            }
            else
            {
                var gasPipingSolution = _gasPipingSolutionRepository.GetSingle(x => x.ID == gasPipingRequest.GasPiping.ID);
                gasPipingSolution.AllowablePercentage = gasPipingRequest.GasPiping.GasPipingSolution.AllowablePercentage;
                gasPipingSolution.PressureDrop = gasPipingRequest.GasPiping.GasPipingSolution.PressureDrop;
                gasPipingSolution.AvailablePressure = gasPipingRequest.GasPiping.GasPipingSolution.AvailablePressure;
                gasPipingSolution.LengthOfRun = gasPipingRequest.GasPiping.GasPipingInput.LengthOfRun;
                gasPipingSolution.NumberOf45Elbows = gasPipingRequest.GasPiping.GasPipingInput.NumberOf45Elbows;
                gasPipingSolution.NumberOf90Elbows = gasPipingRequest.GasPiping.GasPipingInput.NumberOf90Elbows;
                gasPipingSolution.NumberOfTees = gasPipingRequest.GasPiping.GasPipingInput.NumberOfTees;
                gasPipingSolution.SupplyGasPressure = gasPipingRequest.GasPiping.GasPipingInput.SupplyGasPressure;
                gasPipingSolution.GeneratorMinPressure = gasPipingRequest.GasPiping.GasPipingInput.GeneratorMinPressure;
                gasPipingSolution.PipeSizeID = gasPipingRequest.GasPiping.PipeSizeID;
                gasPipingSolution.SizingMethodID = gasPipingRequest.GasPiping.SizingMethodID;
                gasPipingSolution.UnitID = gasPipingRequest.GasPiping.UnitSelected;
                gasPipingSolution.SolutionID = gasPipingRequest.SolutionID;
                gasPipingSolution.SingleUnit = gasPipingRequest.GasPiping.SingleUnit;
                gasPipingSolution.FuelConsumption = gasPipingRequest.GasPiping.GeneratorSummary.FuelConsumption;

                result = _gasPipingSolutionRepository.Update(gasPipingSolution);
            }

            _gasPipingSolutionRepository.Commit();

            ConvertGasPipingDataFromEnglishToSelectedUnit(gasPipingRequest.GasPiping.UnitSelected, gasPipingRequest.GasPiping);

            return gasPipingRequest.GasPiping;
        }

        public int SaveExhaustPipingSolution(ExhaustPipingRequestDto exhaustPiping, string username)
        {
            var result = new ExhaustPipingSolution();

            if (exhaustPiping.ID == 0)
            {
                var exhaustPipingSolution = new ExhaustPipingSolution
                {
                    ExhaustSystemConfigurationID = exhaustPiping.ExhaustSystemConfigurationID,
                    SizingMethodID = exhaustPiping.SizingMethodID,
                    PipeSizeID = exhaustPiping.PipeSizeID,
                    SolutionID = exhaustPiping.SolutionID,
                    UnitID = exhaustPiping.UnitID,
                    LengthOfRun = exhaustPiping.ExhaustPipingInput.LengthOfRun,
                    NumberOf45Elbows = exhaustPiping.ExhaustPipingInput.NumberOf45Elbows,
                    NumberOfLongElbows = exhaustPiping.ExhaustPipingInput.NumberOfLongElbows,
                    NumberOfStandardElbows = exhaustPiping.ExhaustPipingInput.NumberOfStandardElbows,
                    PressureDrop = exhaustPiping.ExhaustPipingSolution.PressureDrop,
                    MaximumBackPressure = exhaustPiping.ExhaustPipingGeneratorSummary.MaximumBackPressure,
                    TotalExhaustFlow = exhaustPiping.ExhaustPipingGeneratorSummary.TotalExhaustFlow
                };

                result = _exhaustPipingSolutionRepository.Add(exhaustPipingSolution);
            }

            else
            {
                var exhaustPipingSolution = _exhaustPipingSolutionRepository.GetSingle(x => x.ID == exhaustPiping.ID);

                exhaustPipingSolution.ExhaustSystemConfigurationID = exhaustPiping.ExhaustSystemConfigurationID;
                exhaustPipingSolution.SizingMethodID = exhaustPiping.SizingMethodID;
                exhaustPipingSolution.PipeSizeID = exhaustPiping.PipeSizeID;
                exhaustPipingSolution.SolutionID = exhaustPiping.SolutionID;
                exhaustPipingSolution.UnitID = exhaustPiping.UnitID;
                exhaustPipingSolution.LengthOfRun = exhaustPiping.ExhaustPipingInput.LengthOfRun;
                exhaustPipingSolution.NumberOf45Elbows = exhaustPiping.ExhaustPipingInput.NumberOf45Elbows;
                exhaustPipingSolution.NumberOfLongElbows = exhaustPiping.ExhaustPipingInput.NumberOfLongElbows;
                exhaustPipingSolution.NumberOfStandardElbows = exhaustPiping.ExhaustPipingInput.NumberOfStandardElbows;
                exhaustPipingSolution.PressureDrop = exhaustPiping.ExhaustPipingSolution.PressureDrop;
                exhaustPipingSolution.MaximumBackPressure = exhaustPiping.ExhaustPipingGeneratorSummary.MaximumBackPressure;
                exhaustPipingSolution.TotalExhaustFlow = exhaustPiping.ExhaustPipingGeneratorSummary.TotalExhaustFlow;


                result = _exhaustPipingSolutionRepository.Update(exhaustPipingSolution);
            }

            _exhaustPipingSolutionRepository.Commit();

            return result.ID;
        }

        public bool UpdateFuelTypeForSolution(UpdateFuelTypeForSolutionRequestDto requestDto, string userID, string userName = "")
        {
            var isShared = _projectRepository.GetSingle(x => x.ID == requestDto.ProjectID && x.UserID != userID)?.SharedProjects.Any(x => x.RecipientEmail.ToLower() == userName.ToLower());
            var solutionSetup = _solutionSetupRepository.GetSingle(x => x.SolutionID == requestDto.SolutionID && (x.Solution.Project.UserID == userID || isShared == true));
            if (solutionSetup == null)
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);

            solutionSetup.FuelTypeID = requestDto.DieselProduct ? (int)FuelTypeEnum.Diesel : (int)FuelTypeEnum.NaturalGas;
            _solutionSetupRepository.Update(solutionSetup);
            _solutionSetupRepository.Commit();

            return true;
        }

        public TransientAnalysisDto GetTransientAnalysis(int projectID, int solutionID, string userID, string brand, string userName = "")
        {
            var recommendedProduct = _recommendedProductRepository.GetSingle(r => r.SolutionID == solutionID);
            if (recommendedProduct == null)
            {
                return null;
            }
            else if (recommendedProduct != null)
            {
                if (recommendedProduct.GeneratorID == null || recommendedProduct.AlternatorID == null)
                    return null;
            }

            var solutionSummary = GetSolutionSummary(projectID, solutionID, userID, brand, userName);

            var vdipLoadSequence = GetVdipLoadSequence();
            var fdipLoadSequence = GetFdipLoadSequence();

            TransientAnalysisDto transientAnalysis = new TransientAnalysisDto();

            transientAnalysis.IsVdipEngineConfiguration = !(vdipLoadSequence.LoadSequenceSummary.AlternatorExpectedVoltageDip >= vdipLoadSequence.LoadSequenceSummary.EngineExpectedVoltageDip);

            transientAnalysis.AlternatorTransientRequirement.Sequence = vdipLoadSequence.SequenceDescription;
            transientAnalysis.AlternatorTransientRequirement.Load = GetLargestKVAStartingLoad(vdipLoadSequence).Description;
            transientAnalysis.AlternatorTransientRequirement.StartingkVA = vdipLoadSequence.LoadSequenceSummary.StartingKVA;
            transientAnalysis.AlternatorTransientRequirement.VdipTolerance = vdipLoadSequence.LoadSequenceSummary.VDipPerc.ToString() + " %";
            transientAnalysis.AlternatorTransientRequirement.VdipExpected = (!transientAnalysis.IsVdipEngineConfiguration ? "" : "*") + (vdipLoadSequence.LoadSequenceSummary.ExpectedVoltageDip.ToString("P1"));

            transientAnalysis.EngineTransientRequirement.Sequence = fdipLoadSequence.SequenceDescription;
            transientAnalysis.EngineTransientRequirement.Load = GetLargestKWStartingLoad(fdipLoadSequence).Description;
            transientAnalysis.EngineTransientRequirement.StartingkW = fdipLoadSequence.LoadSequenceSummary.StartingKW;
            transientAnalysis.EngineTransientRequirement.FdipTolerance = fdipLoadSequence.LoadSequenceSummary.FDipHertz;
            transientAnalysis.EngineTransientRequirement.FdipExpected = fdipLoadSequence.LoadSequenceSummary.ExpectedFrequencyDip;

            foreach (var sequence in _loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                if(!sequence.LoadSequenceSummary.Shed)
                {
                    var alternatorTrasientAnalysis = new AlternatorTransientAnalysis();

                    alternatorTrasientAnalysis.Sequence = sequence.LoadSequenceSummary.SequenceDescription;
                    alternatorTrasientAnalysis.AllowableVdip = sequence.LoadSequenceSummary.ProjectAllowableVoltageDip.ToString("P1");
                    alternatorTrasientAnalysis.VdipExpected = (sequence.LoadSequenceSummary.AlternatorExpectedVoltageDip >= sequence.LoadSequenceSummary.EngineExpectedVoltageDip ? "" : "*") + (sequence.LoadSequenceSummary.ExpectedVoltageDip.ToString("P2"));
                    alternatorTrasientAnalysis.SequenceStartingkVA = sequence.LoadSequenceSummary.StartingKVA;
                    alternatorTrasientAnalysis.LargestTransientLoad = GetLargestKVAStartingLoad(sequence).Description;

                    transientAnalysis.AlternatorTransientAnalysisList.Add(alternatorTrasientAnalysis);
                }
            }

            foreach (var sequence in _loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                if (!sequence.LoadSequenceSummary.Shed)
                {
                    var engineTrasientAnalysis = new EngineTransientAnalysis();

                    engineTrasientAnalysis.Sequence = sequence.LoadSequenceSummary.SequenceDescription;
                    engineTrasientAnalysis.AllowableFdip = sequence.LoadSequenceSummary.ProjectAllowableFrequencyDip;
                    engineTrasientAnalysis.FdipExpected = sequence.LoadSequenceSummary.ExpectedFrequencyDip;
                    engineTrasientAnalysis.SequenceStartingkW = sequence.LoadSequenceSummary.StartingKW;
                    engineTrasientAnalysis.LargestTransientLoad = GetLargestKWStartingLoad(sequence).Description;

                    transientAnalysis.EngineTransientAnalysisList.Add(engineTrasientAnalysis);
                }                
            }

            return transientAnalysis;
        }

        public HarmonicAnalysisInputsDto GetHarmonicAnalysisInputs(int projectID, int solutionID, string userID, string brand, string userName = "")
        {
            var recommendedProduct = _recommendedProductRepository.GetSingle(r => r.SolutionID == solutionID);
            if (recommendedProduct == null)
            {
                return null;
            }
            else if (recommendedProduct != null)
            {
                if (recommendedProduct.GeneratorID == null || recommendedProduct.AlternatorID == null)
                    return null;
            }
            var solutionSummary = GetSolutionSummary(projectID, solutionID, userID, brand, userName);
            var solutionVoltageSpecific = Convert.ToDecimal(_solutionSetupRepository.GetSingle(s => s.SolutionID == solutionID).VoltageSpecific.Value);

            if (solutionSummary.SolutionSummaryRecommendedProductDetails.GeneratorID == null || solutionSummary.SolutionSummaryRecommendedProductDetails.AlternatorID == null)
                return null;

            HarmonicAnalysisInputsDto harmonicAnalysisInputs = new HarmonicAnalysisInputsDto();

            foreach (var loadSequence in _loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                if(!loadSequence.LoadSequenceSummary.Shed)
                {
                    var harmonicAnalysisSequence = new HarmonicAnalysisSequence();

                    harmonicAnalysisSequence.AllContinuousKVABase = AllContinuousKVABase(loadSequence);
                    harmonicAnalysisSequence.AllContinuousAndMomentaryKVABase = GetAllContinuousAndMomentary_KVABase(loadSequence);
                    harmonicAnalysisSequence.LargestContinuousWithLoadFactor = loadSequence.LoadSequenceSummary.LargestContinuousWithLoadFactor;
                    harmonicAnalysisSequence.PeakHarmonicWithLoadFactor = loadSequence.LoadSequenceSummary.PeakHarmonicWithLoadFactor;
                    harmonicAnalysisSequence.SequenceID = loadSequence.SequenceID;
                    harmonicAnalysisSequence.SequencePriority = _sequenceRepository.GetSingle(s => s.ID == loadSequence.SequenceID).Ordinal;
                    harmonicAnalysisSequence.SequenceDescription = loadSequence.SequenceDescription;
                    harmonicAnalysisSequence.KVABaseErrorChecked = loadSequence.LoadSequenceSummary.KVABaseErrorChecked;

                    harmonicAnalysisInputs.HarmonicAnalysisSequenceList.Add(harmonicAnalysisSequence);
                }
            }

            var alternatorDetail = _generatorAlternator.GetAlternatorDetail(_alternatorRepository.GetSingle(a => a.ID == solutionSummary.SolutionSummaryRecommendedProductDetails.AlternatorID));
            var generatorQuantity = Convert.ToInt16(solutionSummary.SolutionSummaryRecommendedProductDetails.ParallelQuantityList.FirstOrDefault(p => p.ID == solutionSummary.SolutionSummaryRecommendedProductDetails.ParallelQuantityID).Value);

            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.SolutionVoltageSpecific = solutionVoltageSpecific;
            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.LargestContinuousHarmonicsWithLoadFactor = _LargestContinuousHarmonicWithLoadFactor;
            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.PeakHarmonicsWithLoadFactor = _PeakHarmonicWithLoadFactor;
            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.KVABase = solutionSummary.LoadSummaryLoads.SolutionSummaryLoadSummary.HarmonicsKVA;
            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.AlternatorKVABase = alternatorDetail.Alternator.KVABase;
            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.AlternatorSubtransientReactanceCorrected = alternatorDetail.SubtransientReactanceCorrected;
            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.AlternatorKWDerated = alternatorDetail.KWDerated;
            harmonicAnalysisInputs.HarmonicAnalysisSizingSolution.GeneratorQuantity = generatorQuantity;

            harmonicAnalysisInputs.HarmonicProfileList = _pickListProcessor.GetHarmonicProfile();

            return harmonicAnalysisInputs;
        }

        public GasPipingReportDto GetGasPipingReport(int projectID, int solutionID, string userID, string brand, string userName = "")
        {
            var gasPipingSolution = _gasPipingSolutionRepository.GetSingle(p => p.SolutionID == solutionID);

            if (gasPipingSolution == null)
            {
                //throw new PowerDesignProException("GasPipingNotFound", Message.Report);
                return null;
            }

            var fromUnitSelected = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == gasPipingSolution.UnitID);
            var toUnitSelected = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == gasPipingSolution.Solution.SolutionSetup.FirstOrDefault().UnitsID);

            var response = new GasPipingReportDto
            {
                PressureDrop = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, gasPipingSolution.PressureDrop),
                SolutionID = gasPipingSolution.SolutionID,
                AvailablePressure = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, gasPipingSolution.AvailablePressure),
                AllowablePercentage = gasPipingSolution.AllowablePercentage,
                SupplyGasPressure = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, gasPipingSolution.SupplyGasPressure),
                LengthOfRun = UnitMapper.ConvertLengthOfRun(fromUnitSelected, toUnitSelected, gasPipingSolution.LengthOfRun),
                NumberOf90Elbows = gasPipingSolution.NumberOf90Elbows,
                NumberOf45Elbows = gasPipingSolution.NumberOf45Elbows,
                NumberOfTees = gasPipingSolution.NumberOfTees,
                GeneratorMinPressure = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, gasPipingSolution.GeneratorMinPressure),
                PipeSize = UnitMapper.ConvertPipeSize(fromUnitSelected, toUnitSelected, gasPipingSolution.GasPipingPipeSize.PipeSize),
                SizingMethod = gasPipingSolution.GasPipingSizingMethod.Description,
                FuelType = gasPipingSolution.Solution.SolutionSetup.FirstOrDefault().FuelType.Description,
                ModelDescription = gasPipingSolution.Solution.RecommendedProduct.FirstOrDefault().Generator.ModelDescription,
                ProductFamilyDesc = gasPipingSolution.Solution.RecommendedProduct.FirstOrDefault().ProductFamily.Description,
                FuelConsumption = gasPipingSolution.FuelConsumption,
                PressureUnitText = toUnitSelected.PressureUnitText,
                LengthOfRunUnitText = toUnitSelected.LengthOfRunUnitText,
                PipeSizeUnitText = toUnitSelected.PipeSizeUnitText
            };

            return response;
        }

        public ExhaustPipingReportDto GetExhaustPipingReport(int projectID, int solutionID, string userID, string brand, string userName = "")
        {
            var exhaustPipingSolution = _exhaustPipingSolutionRepository.GetSingle(p => p.SolutionID == solutionID);

            if (exhaustPipingSolution == null)
            {
                //throw new PowerDesignProException("ExhaustPipingNotFound", Message.Report);
                return null;
            }

            var fromUnitSelected = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == exhaustPipingSolution.UnitID);
            var toUnitSelected = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == exhaustPipingSolution.Solution.SolutionSetup.FirstOrDefault().UnitsID);
            var response = new ExhaustPipingReportDto
            {
                PressureDrop = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, exhaustPipingSolution.PressureDrop),
                SolutionID = exhaustPipingSolution.SolutionID,
                PipeSize = UnitMapper.ConvertPipeSize(fromUnitSelected, toUnitSelected, exhaustPipingSolution.ExhaustPipingPipeSize.PipeSize),
                SizingMethod = exhaustPipingSolution.SizingMethod.Description,
                LengthOfRun = UnitMapper.ConvertLengthOfRun(fromUnitSelected, toUnitSelected, exhaustPipingSolution.LengthOfRun),
                ExhaustSystemConfiguration = exhaustPipingSolution.ExhaustSystemConfiguration.Description,
                NumberOfStandardElbows = exhaustPipingSolution.NumberOfStandardElbows,
                NumberOfLongElbows = exhaustPipingSolution.NumberOfLongElbows,
                NumberOf45Elbows = exhaustPipingSolution.NumberOf45Elbows,
                ModelDescription = exhaustPipingSolution.Solution.RecommendedProduct.FirstOrDefault().Generator.ModelDescription,
                ProductFamilyDesc = exhaustPipingSolution.Solution.RecommendedProduct.FirstOrDefault().ProductFamily.Description,
                MaximumBackPressure = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, exhaustPipingSolution.MaximumBackPressure),
                TotalExhaustFlow = exhaustPipingSolution.TotalExhaustFlow,
                ExhaustEngineConfiguration = exhaustPipingSolution.Solution.RecommendedProduct.FirstOrDefault().Generator.ExhaustDual == true ? "Dual" : "Single",
                PressureUnitText = toUnitSelected.PressureUnitText,
                LengthOfRunUnitText = toUnitSelected.LengthOfRunUnitText,
                PipeSizeUnitText = toUnitSelected.PipeSizeUnitText

            };

            return response;
        }

        public async Task<int> RequestForQuote(RequestForQuoteDto requestForQuoteDto, string userID, string userName)
        {
            var project = _projectRepository.GetSingle(p => p.ID == requestForQuoteDto.ProjectID);
            ApplicationUser user = project.User;
            Solution solution = project.Solutions.FirstOrDefault(s => s.ProjectID == requestForQuoteDto.ProjectID && s.ID == requestForQuoteDto.SolutionID);

            var requestForQuote = new RequestForQuote
            {
                SolutionID = requestForQuoteDto.SolutionID,
                Comments = requestForQuoteDto.Comments,
                CreatedBy = userName,
                ModifiedBy = userName,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow
            };

            try
            {
                if (String.IsNullOrEmpty(requestForQuoteDto.Language) || requestForQuoteDto.Language != "en" || requestForQuoteDto.Language != "en")
                    requestForQuoteDto.Language = "en";
                var toAddress = SectionHandler.GetSectionValue($"{requestForQuoteDto.Language.ToLower()}/emailTemplates/requestForQuote/{requestForQuoteDto.Brand.ToLower()}", "ToAddress", "PowerDesignPro@generac.com").Split(';');
                var toName = SectionHandler.GetSectionValue($"{requestForQuoteDto.Language.ToLower()}/emailTemplates/requestForQuote/{requestForQuoteDto.Brand.ToLower()}", "ToName", "Power Design Pro Support").Split(';');

                var userAddress = user.Address1 + (String.IsNullOrEmpty(user.Address2) ? ", " : user.Address2 + ", ") + user.City + ", " + user.State.Description + " " + user.ZipCode + ", " + user.Country.Description;

                var emailData = new SendGridEmailData();

                emailData.template_id = SectionHandler.GetSectionValue($"{requestForQuoteDto.Language.ToLower()}/emailTemplates/requestForQuote/{requestForQuoteDto.Brand.ToLower()}", "TemplateID", "");
                emailData.personalizations = new List<Personalization>();
                var personalization = new Personalization();
                personalization.to = new List<To>();

                for (int i = 0; i < toAddress.Length; i++)
                {
                    personalization.to.Add(new To
                    {
                        email = toAddress[i],
                        name = toName[i]
                    });
                }

                personalization.substitutions = new Dictionary<string, string>()
                {
                    { "%ProjectName%", project.ProjectName },
                    { "%SolutionName%", solution.SolutionName },
                    { "%SenderName%", user.FirstName + " " + user.LastName},
                    { "%SenderEmail%", userName },
                    { "%SenderPhone%", user.Phone },
                    { "%SenderAddress%", userAddress },
                    { "%GeneratorDescription%", requestForQuoteDto.GeneratorDescription },
                    { "%AlternatorDescription%", requestForQuoteDto.AlternatorDescription },
                    { "%CustomerName%", project.ContactName },
                    { "%CustomerEmail%", project.ContactEmail },
                    { "%Comments%", requestForQuoteDto.Comments },
                    { "%CompanyName%", EmailHelper.CompanyName(requestForQuoteDto.Language, requestForQuoteDto.Brand) },
                    { "%CompanyAddress%", EmailHelper.CompanyAddress(requestForQuoteDto.Language, requestForQuoteDto.Brand) }
                };

                emailData.personalizations.Add(personalization);
                var emailResponse = await EmailHelper.SendGridAsyncWithTemplate(emailData);

                if ((HttpStatusCode)emailResponse == HttpStatusCode.Accepted)
                    requestForQuote.EmailSent = true;
            }
            catch (Exception)
            {
                requestForQuote.EmailSent = false;
            }

            var addedRequestForQuote = _requestForQuoteRepository.Add(requestForQuote);
            _requestForQuoteRepository.Commit();

            return addedRequestForQuote.ID;
        }

        #region All Private Methods

        private void ConvertGasPipingDataFromEnglishToSelectedUnit(int unitID, GasPipingDto gasPipingDto)
        {
            var unitSelected = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == unitID);
            //gasPipingDto.GasPipingInput.GeneratorMinPressure = UnitMapper.ConvertPressure(new English(), unitSelected, gasPipingDto.GasPipingInput.GeneratorMinPressure);
            //gasPipingDto.GasPipingInput.SupplyGasPressure = UnitMapper.ConvertPressure(new English(), unitSelected, gasPipingDto.GasPipingInput.SupplyGasPressure);
            //gasPipingDto.GasPipingInput.LengthOfRun = UnitMapper.ConvertLengthOfRun(new English(), unitSelected, gasPipingDto.GasPipingInput.LengthOfRun);

            gasPipingDto.GasPipingSolution.PressureDrop = Conversion.GetRoundedDecimal(UnitMapper.ConvertPressure(new English(), unitSelected, gasPipingDto.GasPipingSolution.PressureDrop), 2);
            //gasPipingDto.GasPipingSolution.AllowablePercentage = Conversion.GetRoundedDecimal(UnitMapper.ConvertPressure(new English(), unitSelected, gasPipingDto.GasPipingSolution.AllowablePercentage, 4), 4);
            gasPipingDto.GasPipingSolution.AllowablePercentage = Conversion.GetRoundedDecimal(gasPipingDto.GasPipingSolution.AllowablePercentage, 4);
            gasPipingDto.GasPipingSolution.AvailablePressure = Conversion.GetRoundedDecimal(UnitMapper.ConvertPressure(new English(), unitSelected, gasPipingDto.GasPipingSolution.AvailablePressure), 2);
        }

        private bool UpdateGasPiping(GasPipingDto gasPiping, int selectedUnitId)
        {
            var fromUnit = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == selectedUnitId);
            CalculateTolerences(gasPiping, ref fromUnit);
            var solutionFound = ValidateGasPiping(gasPiping, fromUnit);
            return solutionFound;
        }

        private void CalculateTolerences(GasPipingDto gasPipingDto, ref IUnitMeasure fromUnit)
        {
            var lengthFeet = UnitMapper.ConvertLengthOfRun(fromUnit, new English(), gasPipingDto.GasPipingInput.LengthOfRun);

            gasPipingDto.GasPipingSolution.PressureDrop = CalculateFuelPipePressureDrop(gasPipingDto, lengthFeet);

            gasPipingDto.GasPipingSolution.AllowablePercentage = CalculatePercentAllowable(
                gasPipingDto.GasPipingSolution.PressureDrop,
                gasPipingDto.GasPipingInput.GeneratorMinPressure,
                gasPipingDto.GasPipingInput.SupplyGasPressure,
                fromUnit);

            gasPipingDto.GasPipingSolution.AvailablePressure = ServicePressureEnglish(fromUnit, gasPipingDto.GasPipingInput.SupplyGasPressure) - gasPipingDto.GasPipingSolution.PressureDrop;
        }

        private decimal CalculatePercentAllowable(decimal pressureDrop, decimal genMinPressure, decimal supplyGasPressure, IUnitMeasure fromUnit)
        {
            var baseSupply = ServicePressureEnglish(fromUnit, supplyGasPressure) - GeneratorMinPressureEnglish(fromUnit, genMinPressure);
            if (baseSupply == 0)
            {
                return 0;
            }

            return pressureDrop / baseSupply;
        }

        private decimal GeneratorMinPressureEnglish(IUnitMeasure fromUnit, decimal generatorMinPressure)
        {
            return UnitMapper.ConvertPressure(fromUnit, new English(), generatorMinPressure);
        }

        private decimal ServicePressureEnglish(IUnitMeasure fromUnit, decimal supplyGasPressure)
        {
            return UnitMapper.ConvertPressure(fromUnit, new English(), supplyGasPressure);
        }

        private decimal CalculateFuelPipePressureDrop(GasPipingDto gasPipingDto, decimal lengthFeet)
        {
            var pipe = gasPipingDto.PipeSizeList.FirstOrDefault(x => x.ID == gasPipingDto.PipeSizeID);
            if (pipe != null)
            {
                var corrFactor = 0.00354 * gasPipingDto.FuelConfig.SP_GR * gasPipingDto.TempRank
                * (Math.Pow((gasPipingDto.FuelConfig.Viscosity / gasPipingDto.FuelConfig.SP_GR), 0.152));

                var pipeEqLen = lengthFeet + (gasPipingDto.GasPipingInput.NumberOf90Elbows * +pipe.Factor90)
               + (gasPipingDto.GasPipingInput.NumberOf45Elbows * pipe.Factor45)
               + ((gasPipingDto.GasPipingInput.NumberOfTees * pipe.Tee));

                var gasFlow = gasPipingDto.GeneratorSummary.GasFuelFlow;
                var pressureDrop = (decimal)corrFactor * pipeEqLen * (decimal)Math.Pow((gasFlow / (2313 * Math.Pow((double)pipe.Diameter, 2.623))), 1 / 0.541);

                return pressureDrop;
            }

            return 0;
        }

        private bool ValidateGasPiping(GasPipingDto gasPipingDto, IUnitMeasure fromUnit)
        {
            var solutionFound = false;
            switch (gasPipingDto.SizingMethodID)
            {
                case (int)GasPipingSizingMethodEnum.AutoSelectDefault:
                    solutionFound = FindPipeSizePressureDrop(gasPipingDto, fromUnit);
                    break;
                case (int)GasPipingSizingMethodEnum.AutoSelectLessThen33:
                    solutionFound = FindPipeSizePercent(gasPipingDto, fromUnit, (decimal)0.33);
                    break;
                case (int)GasPipingSizingMethodEnum.AutoSelectLessThen50:
                    solutionFound = FindPipeSizePercent(gasPipingDto, fromUnit, (decimal)0.5);
                    break;
                case (int)GasPipingSizingMethodEnum.Manual:
                    solutionFound = (gasPipingDto.GasPipingSolution.AvailablePressure > 0);
                    break;
                default:
                    break;
            }

            if (!solutionFound)
            {
                gasPipingDto.Error = "No solution found. Try a different sizing method.";
            }
            else
            {
                var generatorMinPressureEnglish = GeneratorMinPressureEnglish(fromUnit, gasPipingDto.GasPipingInput.GeneratorMinPressure);
                if (gasPipingDto.GasPipingSolution.AvailablePressure < generatorMinPressureEnglish && gasPipingDto.SizingMethodID == (int)GasPipingSizingMethodEnum.Manual)
                {
                    gasPipingDto.Error = "Warning Increase Pipe Size,  Minimum Acceptable Pressure is " + generatorMinPressureEnglish;
                }
                else if (gasPipingDto.GasPipingSolution.AvailablePressure < generatorMinPressureEnglish)
                {
                    gasPipingDto.Error = "Warning - Supply Gas Pressure must be greater than Minimum Pressure";
                }
                else if (OverServicePressure(gasPipingDto, fromUnit))
                {
                    gasPipingDto.Error = "Recommend increasing pipe size to allow for service presure variations";
                }
            }
            return solutionFound;
        }

        private bool OverServicePressure(GasPipingDto gasPipingDto, IUnitMeasure fromUnit)
        {
            return gasPipingDto.GasPipingSolution.PressureDrop >
                ((ServicePressureEnglish(fromUnit, gasPipingDto.GasPipingInput.SupplyGasPressure) - GeneratorMinPressureEnglish(fromUnit, gasPipingDto.GasPipingInput.GeneratorMinPressure)) / 2);
        }

        private bool FindPipeSizePercent(GasPipingDto gasPipingDto, IUnitMeasure fromUnit, decimal minAllowable)
        {
            gasPipingDto.PipeSizeID = gasPipingDto.PipeSizeList.FirstOrDefault().ID;
            CalculateTolerences(gasPipingDto, ref fromUnit);
            var outOfSize = false;
            while (gasPipingDto.GasPipingSolution.AllowablePercentage >= minAllowable && outOfSize == false)
            {
                outOfSize = !IncreasePipeSize(gasPipingDto);
                CalculateTolerences(gasPipingDto, ref fromUnit);
            }

            return (gasPipingDto.GasPipingSolution.AllowablePercentage < minAllowable);
        }

        private bool FindPipeSizePressureDrop(GasPipingDto gasPipingDto, IUnitMeasure fromUnit)
        {
            gasPipingDto.PipeSizeID = gasPipingDto.PipeSizeList.FirstOrDefault().ID;
            CalculateTolerences(gasPipingDto, ref fromUnit);
            var outOfSize = false;
            while (gasPipingDto.GasPipingSolution.PressureDrop >= (decimal)0.5 && outOfSize == false)
            {
                outOfSize = !IncreasePipeSize(gasPipingDto);
                CalculateTolerences(gasPipingDto, ref fromUnit);
            }

            return (gasPipingDto.GasPipingSolution.PressureDrop < (decimal)0.5);
        }

        private bool IncreasePipeSize(GasPipingDto gasPipingDto)
        {
            var pipeSizeList = gasPipingDto.PipeSizeList.ToList();
            var selectedPipe = pipeSizeList.FirstOrDefault(x => x.ID == gasPipingDto.PipeSizeID);
            var currentIndex = pipeSizeList.FindIndex(x => x == selectedPipe);
            if (currentIndex == -1 || currentIndex == pipeSizeList.Count - 1)
            {
                return false;
            }

            gasPipingDto.PipeSizeID = pipeSizeList[currentIndex + 1].ID;

            return true;
        }

        private void UpdateRecommendedProduct(int solutionID, Solution solution, SolutionSummaryRecommendedProductDto recommendedProductDetail)
        {
            if (solution.RecommendedProduct.FirstOrDefault() == null)
            {
                var recommendedProduct = new RecommendedProduct
                {
                    AlternatorID = recommendedProductDetail.AlternatorID,
                    FamilySelectionMethodID = recommendedProductDetail.FamilySelectionMethodID,
                    GeneratorID = recommendedProductDetail.GeneratorID,
                    ProductFamilyID = recommendedProductDetail.ProductFamilyID,
                    SizingMethodID = recommendedProductDetail.SizingMethodID,
                    ParallelQuantityID = recommendedProductDetail.ParallelQuantityID,
                    SolutionID = solutionID
                };

                _recommendedProductRepository.Add(recommendedProduct);
            }
            else
            {
                var recommendedProduct = solution.RecommendedProduct.FirstOrDefault();

                if (recommendedProductDetail.FamilySelectionMethodID > 0)
                    recommendedProduct.FamilySelectionMethodID = recommendedProductDetail.FamilySelectionMethodID;

                if (recommendedProductDetail.SizingMethodID > 0)
                    recommendedProduct.SizingMethodID = recommendedProductDetail.SizingMethodID;

                recommendedProduct.ProductFamilyID = recommendedProductDetail.ProductFamilyID;
                recommendedProduct.GeneratorID = recommendedProductDetail.GeneratorID;
                recommendedProduct.AlternatorID = recommendedProductDetail.AlternatorID;
                recommendedProduct.ParallelQuantityID = recommendedProductDetail.ParallelQuantityID;

                _recommendedProductRepository.Update(recommendedProduct);
            }

            _recommendedProductRepository.Commit();
        }

        private void ConvertUnitsDescription(ref string description, ref decimal outPipeSize, decimal inPipeSize, int unitsID)
        {
            switch (unitsID)
            {
                case (int)UnitEnum.English:
                    outPipeSize = inPipeSize;
                    description = $"{inPipeSize}''";
                    break;
                case (int)UnitEnum.Metric:
                    outPipeSize = inPipeSize * _metricMultiplecationPipeSizeFactor;
                    description = $"{outPipeSize} mm";
                    break;
                default:
                    break;
            }
        }

        #region Gas Piping

        private decimal GetFuelConsumption(Generator generator, bool singleUnit, int quantity)
        {
            var fuelConsumption = (decimal)generator.NG_CF_HR / 100;
            if (!singleUnit)
            {
                fuelConsumption = fuelConsumption * quantity;
            }

            return Conversion.GetRoundedDecimal(fuelConsumption, 2);
        }

        private double CalculateGasFuelFlow(Generator generator, string fuelCode, int engineDuty, bool singleUnit, int quantity)
        {
            var ngconsumption = (double)generator.NG_CF_HR;
            var kwProjectSelection = engineDuty == 1 ? generator.KwStandby : generator.KWPrime;
            if (fuelCode == FuelTypeEnum.LPVapor.ToString() || fuelCode == FuelTypeEnum.DualFuelVapor.ToString())
            {
                ngconsumption = ngconsumption * 0.4;
            }

            var baseFlow = ngconsumption * kwProjectSelection / generator.KwStandby;

            if (!singleUnit)
                baseFlow = baseFlow * quantity;

            return baseFlow;
        }

        private GasPipingDto GetDefaultGasPipingData(SolutionSetup solutionSetupDetail)
        {
            var gasPiping = new GasPipingDto();

            gasPiping.SizingMethodList = _gasPipingSizingMethodRepository.GetAll().OrderBy(x => x.Ordinal).Select(x => new PickListDto
            {
                ID = x.ID,
                Description = x.Description,
                Value = x.Value,
                LanguageKey = x.LanguageKey,
            }).ToList();

            gasPiping.PipeSizeList = _gasPipingPipeSizeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().ConvertAll(x => ToGasPipingPipeSizeDto(solutionSetupDetail.UnitsID, x));
            gasPiping.SingleUnit = false;
            gasPiping.GasPipingSolution = new GasPipingSolutionDto();
            gasPiping.GasPipingInput = new GasPipingInputDto();

            return gasPiping;
        }

        private static UnitMeasureDto GetUnitMeasuresForPiping(int unitID)
        {
            var unitMeasure = new UnitMeasureDto();
            switch (unitID)
            {
                case (int)UnitEnum.English:
                    unitMeasure.FeetOrMeters = "ft";
                    unitMeasure.InchesOrCentimeters = "in";
                    unitMeasure.WaterOrPascals = "inches of water";
                    unitMeasure.YardOrMeters = "yd";
                    break;
                case (int)UnitEnum.Metric:
                    unitMeasure.FeetOrMeters = "m";
                    unitMeasure.InchesOrCentimeters = "cm";
                    unitMeasure.WaterOrPascals = "kPa";
                    unitMeasure.YardOrMeters = "m";
                    break;
                default:
                    break;
            }

            return unitMeasure;
        }

        private FuelConfigDto GetFuelConfig(int fuelID)
        {
            string fuelDesc = "";
            switch (fuelID)
            {
                case (int)FuelTypeEnum.NaturalGas:
                    fuelDesc = "NG";
                    break;
                case (int)FuelTypeEnum.LPVapor:
                    fuelDesc = "LP";
                    break;
                case (int)FuelTypeEnum.DualFuelVapor:
                    fuelDesc = "LP";
                    break;
                case (int)FuelTypeEnum.BiFuel:
                    fuelDesc = "NG";
                    break;
                default:
                    break;
            }
            var section = (FuelConfigSection)ConfigurationManager.GetSection("fuelConfig");
            foreach (FuelConfigElement item in section.Instances)
            {
                if (item.Type == fuelDesc)
                {
                    return new FuelConfigDto
                    {
                        Type = item.Type,
                        Viscosity = Convert.ToDouble(item.Viscosity),
                        SP_GR = Convert.ToDouble(item.SP_GR)
                    };
                }
            }

            return null;
        }

        private GasPipingPipeSizeDto ToGasPipingPipeSizeDto(int unitsID, GasPipingPipeSize input)
        {
            var description = string.Empty;
            decimal outPipeSize = 0;

            ConvertUnitsDescription(ref description, ref outPipeSize, input.PipeSize, unitsID);
            //switch (unitsID)
            //{
            //    case (int)UnitEnum.English:
            //        pipeSize = input.PipeSize;
            //        description = $"{input.PipeSize}''";
            //        break;
            //    case (int)UnitEnum.Metric:
            //        pipeSize = input.PipeSize * _metricMultiplecationPipeSizeFactor;
            //        description = $"{pipeSize} mm";
            //        break;
            //    default:
            //        break;
            //}

            return new GasPipingPipeSizeDto
            {
                ID = input.ID,
                Description = description,
                PipeSize = outPipeSize,
                Diameter = input.Diameter,
                Factor45 = input.Factor45,
                Factor90 = input.Factor90,
                Tee = input.Tee
            };
        }

        private void MapGasPipingEntityDetailToDto(GasPipingSolution input, int solutionUnit, ref GasPipingDto gasPiping)
        {
            var fromUnitSelected = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == input.UnitID);
            var toUnitSelected = _unitOfMeasureCollection.UnitMeasures.FirstOrDefault(x => x.UnitID == solutionUnit);

            gasPiping.ID = input.ID;
            gasPiping.SizingMethodID = input.SizingMethodID;
            gasPiping.PipeSizeID = input.PipeSizeID;
            gasPiping.SingleUnit = input.SingleUnit;
            gasPiping.GasPipingInput = new GasPipingInputDto
            {
                GeneratorMinPressure = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, input.GeneratorMinPressure),
                LengthOfRun = UnitMapper.ConvertLengthOfRun(fromUnitSelected, toUnitSelected, input.LengthOfRun),
                NumberOf45Elbows = input.NumberOf45Elbows,
                NumberOf90Elbows = input.NumberOf90Elbows,
                NumberOfTees = input.NumberOfTees,
                SupplyGasPressure = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, input.SupplyGasPressure)
            };
            gasPiping.GasPipingSolution = new GasPipingSolutionDto
            {
                AllowablePercentage = input.AllowablePercentage,
                AvailablePressure = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, input.AvailablePressure),
                PressureDrop = UnitMapper.ConvertPressure(fromUnitSelected, toUnitSelected, input.PressureDrop)
            };
        }

        #endregion

        #region Exhaust Piping

        private ExhaustPipingDto GetDefaultExhaustPipingData(SolutionSetup solutionSetupDetail)
        {
            var exhaustPiping = new ExhaustPipingDto();

            exhaustPiping.SizingMethodList = _pickListProcessor.GetSizingMethod();
            exhaustPiping.PipeSizeList = _exhaustPipingPipeSizeRepository.GetAll().OrderBy(x => x.Ordinal).ToList().ConvertAll(x => ToExhaustPipingPipeSizeDto(solutionSetupDetail.UnitsID, x));
            exhaustPiping.ExhaustSystemConfigurationList = _pickListProcessor.GetExhaustSystemConfiguration();
            exhaustPiping.ExhaustPipingSolution = new ExhaustPipingSolutionDto();
            exhaustPiping.ExhaustPipingInput = new ExhaustPipingInputDto();

            return exhaustPiping;
        }

        private ExhaustPipingPipeSizeDto ToExhaustPipingPipeSizeDto(int unitsID, ExhaustPipingPipeSize input)
        {
            var description = string.Empty;
            decimal outPipeSize = 0;

            ConvertUnitsDescription(ref description, ref outPipeSize, input.PipeSize, unitsID);

            return new ExhaustPipingPipeSizeDto
            {
                ID = input.ID,
                Description = description,
                Value = outPipeSize.ToString()
            };
        }

        private decimal CalculateTotalExhaustFlow(Generator generator, string engineDuty, int units, bool isDual)
        {
            int kwProjectSelection = 0;
            decimal exhaustFlow = generator.ExhaustCFM;

            if (engineDuty.ToLower() == "prime")
                kwProjectSelection = generator.KWPrime;
            else
                kwProjectSelection = generator.KwStandby;

            if (isDual)
                exhaustFlow /= 2;

            if (units == (int)UnitEnum.Metric)
            {
                return (exhaustFlow * kwProjectSelection / generator.KwStandby) * _exhaustFlowMetricConversionFactor;
            }
            else
            {
                return (exhaustFlow * kwProjectSelection / generator.KwStandby);
            }
        }

        private decimal CalculateMaximumBackPressure(decimal exhaustHG, int units)
        {
            if (units == (int)UnitEnum.Metric)
            {
                return (exhaustHG * 13.595m) * _pressureMetricConversionFactor;
            }
            else
            {
                return (exhaustHG * 13.595m);
            }
        }

        private void MapExhaustPipingEntityDetailToDto(ExhaustPipingSolution input, int outUnits, ref ExhaustPipingDto exhaustPiping)
        {
            exhaustPiping.ID = input.ID;
            exhaustPiping.PipeSizeID = input.PipeSizeID;
            exhaustPiping.SizingMethodID = input.SizingMethodID;
            exhaustPiping.ExhaustSystemConfigurationID = input.ExhaustSystemConfigurationID;
            exhaustPiping.ExhaustPipingInput = new ExhaustPipingInputDto
            {
                LengthOfRun = Conversion.GetRoundedDecimal(ConvertUnits(input.LengthOfRun, input.UnitID, outUnits, _meterToFootConversionFactor), 2),
                NumberOf45Elbows = input.NumberOf45Elbows,
                NumberOfLongElbows = input.NumberOfLongElbows,
                NumberOfStandardElbows = input.NumberOfStandardElbows
            };
            exhaustPiping.ExhaustPipingSolution = new ExhaustPipingSolutionDto
            {
                PressureDrop = input.PressureDrop
            };
        }

        private decimal ConvertUnits(decimal input, int inUnits, int outUnits, decimal conversionFactor)
        {
            decimal output = 0;

            if (inUnits == (int)UnitEnum.Metric && outUnits == (int)UnitEnum.English)
            {
                output = input / conversionFactor;
            }
            else if (inUnits == (int)UnitEnum.English && outUnits == (int)UnitEnum.Metric)
            {
                output = input * conversionFactor;
            }
            else
            {
                output = input;
            }

            return output;
        }

        #endregion

        #region Solution Summary Private Methods               

        private SolutionSummaryLoadSummaryDto GetSolutionSummaryLoadSummary(LoadSummaryLoadsDto loadSummaryLoads)
        {
            if (loadSummaryLoads.ListOfLoadSummaryLoadList.Any())
            {
                decimal kwr = 0;
                foreach (var loadSequence in loadSummaryLoads.ListOfLoadSummaryLoadList)
                {
                    if (!loadSequence.LoadSequenceSummary.Shed)
                    {
                        _solutionSummaryLoadSummary.RunningKW += loadSequence.LoadSequenceSummary.RunningKW;
                        _solutionSummaryLoadSummary.RunningKVA += loadSequence.LoadSequenceSummary.RunningKVA;

                        if (loadSequence.LoadSequenceSummary.StartingKW > _solutionSummaryLoadSummary.StepKW)
                        {
                            _solutionSummaryLoadSummary.StepKW = loadSequence.LoadSequenceSummary.StartingKW;
                        }

                        if (loadSequence.LoadSequenceSummary.StartingKVA > _solutionSummaryLoadSummary.StepKVA)
                        {
                            _solutionSummaryLoadSummary.StepKVA = loadSequence.LoadSequenceSummary.StartingKVA;
                        }

                        if (kwr + loadSequence.LoadSequenceSummary.PeakLoadValue > _solutionSummaryLoadSummary.PeakKW)
                        {
                            _solutionSummaryLoadSummary.PeakKW = kwr + loadSequence.LoadSequenceSummary.PeakLoadValue;
                            kwr += loadSequence.LoadSequenceSummary.RunningKW;
                        }

                        if (loadSequence.LoadSequenceSummary.KVABaseErrorChecked > (decimal)0.19)
                        {
                            _solutionSummaryLoadSummary.HarmonicsKVA += loadSequence.LoadSequenceSummary.KVABaseErrorChecked;
                        }
                    }
                }

                _LargestContinuousHarmonicWithLoadFactor = new decimal[loadSummaryLoads.ListOfLoadSummaryLoadList.Count, 10];

                //Added +1 to load the Peak Hamonic Sumamry when filling _PeakHarmonicWithLoadFactor
                _PeakHarmonicWithLoadFactor = new decimal[loadSummaryLoads.ListOfLoadSummaryLoadList.Count + 1, 10];

                FillLargestContinuousHarmonicWithLoadFactor();
                FillPeakHarmonicWithLoadFactor();

                _solutionSummaryLoadSummary.THIDContinuous = Conversion.GetRoundedDecimal(_LargestContinuousHarmonicWithLoadFactor[_LargestContinuousHarmonicWithLoadFactor.GetUpperBound(0), 9], 1);
                _solutionSummaryLoadSummary.THIDPeak = Conversion.GetRoundedDecimal(_PeakHarmonicWithLoadFactor[_PeakHarmonicWithLoadFactor.GetUpperBound(0), 9], 1); //This is also called THID Momentary

                if (_solutionSummaryLoadSummary.RunningKVA == 0)
                    _solutionSummaryLoadSummary.RunningPF = decimal.Round(1, 2, MidpointRounding.AwayFromZero);
                else
                    _solutionSummaryLoadSummary.RunningPF = decimal.Round(_solutionSummaryLoadSummary.RunningKW / _solutionSummaryLoadSummary.RunningKVA, 2, MidpointRounding.AwayFromZero);
            }

            return _solutionSummaryLoadSummary;
        }

        private static SolutionLimitsDto MapSolutionLimits(SolutionSetup solutionSetup)
        {
            return new SolutionLimitsDto
            {
                MaxLoading = decimal.Parse(solutionSetup.MaxRunningLoad.Value) * 100,
                MaxRunningLoadValue = decimal.Parse(solutionSetup.MaxRunningLoad.Value),
                FDip = decimal.Parse(solutionSetup.FrequencyDip.Value),
                VDip = decimal.Parse(solutionSetup.VoltageDip.Value) * 100,
                THVDContinuous = solutionSetup.ContinuousAllowableVoltageDistortion.Description,
                THVDPeak = solutionSetup.MomentaryAllowableVoltageDistortion.Description
            };
        }

        /// <summary>
        /// Method to get Recommended Product details
        /// </summary>
        /// <param name="loadSummaryLoads"></param>
        /// <param name="solutionLimits"></param>
        /// <returns></returns>
        private SolutionSummaryRecommendedProductDto GetRecommendedProduct(RecommendedProduct recommendedProduct, SolutionLimitsDto solutionLimits, string brand)
        {
            var solutionSummaryRecommendedProduct = CalculateSolution(recommendedProduct, solutionLimits, brand);

            solutionSummaryRecommendedProduct.FamilySelectionMethodList = _pickListProcessor.GetFamilySelectionMethod();
            solutionSummaryRecommendedProduct.SizingMethodList = _pickListProcessor.GetSizingMethod();
            //solutionSummaryRecommendedProduct.ParallelQuantityList = _pickListProcessor.GetParallelQuantity();

            if (solutionSummaryRecommendedProduct.GeneratorID != null && solutionSummaryRecommendedProduct.AlternatorID != null)
            {
                solutionSummaryRecommendedProduct.GeneratorDocuments = _pickListProcessor.GetGeneratorDocuments((int)solutionSummaryRecommendedProduct.GeneratorID);
            }

            return solutionSummaryRecommendedProduct;
        }

        private SolutionSummaryRecommendedProductDto CalculateSolution(RecommendedProduct recommendedProduct, SolutionLimitsDto solutionLimits, string brand)
        {
            bool IsParallelable = false;
            int units = 1;
            GeneratorDetail selectedGenerator = null;
            AlternatorDetail selectedAlternator = null;

            _maxParallelQuantity = int.Parse(_parallelQuantityRepository.GetAll().OrderByDescending(p => p.Ordinal).FirstOrDefault().Value);

            InitCalculations(_loadSummaryLoads, solutionLimits);

            if (recommendedProduct != null)
            {
                _SizingMethod = (SizingMethodEnum)recommendedProduct.SizingMethodID;
                _FamiySelectionMethod = (FamilySelectionMethodEnum)recommendedProduct.FamilySelectionMethodID;
                _ProductFamilyID = recommendedProduct.ProductFamilyID;
                _ParallelQuantity = int.Parse(recommendedProduct.ParallelQuantity.Value);
                _GeneratorID = recommendedProduct.GeneratorID;
                _AlternatorID = recommendedProduct.AlternatorID;
                IsParallelable = _ParallelQuantity > 1;
            }
            else
            {
                _SizingMethod = SizingMethodEnum.AutoSelect;
                _FamiySelectionMethod = FamilySelectionMethodEnum.AutoSelect;
            }

            //FillLargestContinuousHarmonicWithLoadFactor();
            //FillPeakHarmonicWithLoadFactor();
            var solutionSetupDetail = _solutionSetupRepository.GetSingle(x => x.SolutionID == _loadSummaryLoads.SolutionID);
            var maxVoltageDistortionContinuous = decimal.Parse(solutionSetupDetail.ContinuousAllowableVoltageDistortion.Value);
            var maxVoltageDistortionMomentary = decimal.Parse(solutionSetupDetail.MomentaryAllowableVoltageDistortion.Value);
            var requiredSubtransientReactanceContinuous = _LargestContinuousHarmonicWithLoadFactor != null ? GetRequiredSubtransientReactanceContinuous(maxVoltageDistortionContinuous) : 0;
            var requiredSubtransientReactanceMomentary = _LargestContinuousHarmonicWithLoadFactor != null ? GetRequiredSubtransientReactanceMomentary(maxVoltageDistortionMomentary) : 0;

            var react = requiredSubtransientReactanceContinuous < requiredSubtransientReactanceMomentary
                                    ? requiredSubtransientReactanceContinuous : requiredSubtransientReactanceMomentary;

            if (react > 0 && _loadSummaryLoads.SolutionSummaryLoadSummary.HarmonicsKVA > 0)
            {
                react = 1000 / _loadSummaryLoads.SolutionSummaryLoadSummary.HarmonicsKVA * react;
            }
            else
            {
                react = 100;
            }

            var selectedFuelType = solutionSetupDetail.FuelType.Code;
            decimal maxKWStartingPerFrequencyDip = GetMaxKWStartingPerFrequencyDip(_loadSummaryLoads, int.Parse(solutionSetupDetail.Frequency.Value));

            var brandID = _brandRepository.GetSingle(x => x.Value.ToLower().Equals(brand)).ID;

            var productFamilyForSelectedFuelTypeList = _generatorRepository.GetAll(x => x.AvailableFuelCode.Contains(selectedFuelType) && x.FrequencyID == solutionSetupDetail.FrequencyID && x.ProductFamily.Active)
                                                            .Select(x => x.ProductFamily).Distinct().OrderBy(x => x.Priority).ToList();

            if (string.Equals(brand.ToLower(), "pramac"))
                productFamilyForSelectedFuelTypeList = productFamilyForSelectedFuelTypeList.FindAll(x => x.BrandID == brandID);

            if (solutionSetupDetail.RegulatoryFilter != "")
            {
                _regulatoryFilters = solutionSetupDetail
                       .RegulatoryFilter.Split(';').Select(x => Convert.ToInt32(x.Split(':')[0]));
            }
            else
            {
                _regulatoryFilters = null;
            }
            /* For FamiySelectionMethod = Auto*/
            if (_FamiySelectionMethod != FamilySelectionMethodEnum.Manual || _SizingMethod != SizingMethodEnum.Manual)
            {
                IEnumerable<ProductFamily> tempProductFamilyList = new List<ProductFamily>();

                if (_FamiySelectionMethod == FamilySelectionMethodEnum.Manual && _SizingMethod == SizingMethodEnum.AutoSelect)
                {
                    //tempProductFamilyList = _productFamilyRepository.GetAll(p => p.ID == _ProductFamilyID).ToList();
                    tempProductFamilyList = productFamilyForSelectedFuelTypeList.FindAll(p => p.ID == _ProductFamilyID).ToList();
                }
                else
                {
                    tempProductFamilyList = productFamilyForSelectedFuelTypeList;
                }

                foreach (var productFamily in tempProductFamilyList)
                {
                    var maxKWGenerator = GetMaxKWGenerator(solutionSetupDetail, productFamily, selectedFuelType, 1);

                    if (maxKWGenerator != null)
                    {
                        selectedGenerator = new GeneratorDetail();
                        selectedGenerator = maxKWGenerator;

                        if (IsMaxKWGensetProvidesSolution(maxKWGenerator, react, solutionLimits, maxKWStartingPerFrequencyDip))
                        {
                            _ProductFamilyID = productFamily.ID;
                            IsParallelable = false;
                            _ParallelQuantity = 1;
                            break;
                        }
                        else
                        {
                            /* If a single unit cannot provide a solution, check for a Parallel unit*/
                            maxKWGenerator = GetMaxKWGenerator(solutionSetupDetail, productFamily, selectedFuelType, _maxParallelQuantity);

                            if (maxKWGenerator != null)
                            {
                                if (IsMaxKWGensetProvidesSolution(maxKWGenerator, react, solutionLimits, maxKWStartingPerFrequencyDip))
                                {
                                    _ProductFamilyID = productFamily.ID;
                                    IsParallelable = true;
                                    _ParallelQuantity = maxKWGenerator.Quantity;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (tempProductFamilyList.Count() == 0)
                {
                    _ProductFamilyID = null;
                    IsParallelable = false;
                    _ParallelQuantity = 1;
                }
            }

            /* For FamiySelectionMethod = Manual*/
            else
            {
                if (_ProductFamilyID != null && productFamilyForSelectedFuelTypeList.Count > 0)
                {
                    _GeneratorID = recommendedProduct.GeneratorID;
                    _AlternatorID = recommendedProduct.AlternatorID;
                    IsParallelable = _ParallelQuantity > 1;
                }
                else
                {
                    _ProductFamilyID = null;
                    _ParallelQuantity = 1;
                    _GeneratorID = null;
                    _AlternatorID = null;
                    IsParallelable = _ParallelQuantity > 1;
                }
            }

            if (_SizingMethod == SizingMethodEnum.AutoSelect)
            {
                _GeneratorID = null;
                _AlternatorID = null;
            }

            var generators = _generatorRepository.GetAll(g => g.Active
                                             && g.ProductFamilyID == _ProductFamilyID
                                             && g.FrequencyID == solutionSetupDetail.FrequencyID
                                             && g.AvailableFuelCode.Contains(selectedFuelType)
                                             && g.GeneratorAvailableVoltages.Any(gv => gv.VoltageNominalID == solutionSetupDetail.VoltageNominalID))
                                      .OrderBy(g => g.KwStandby).ToList();

            if (_regulatoryFilters != null)
            {
                generators = generators.Where(g => _regulatoryFilters.All(x => g.GeneratorRegulatoryFilters.Select(y => y.RegulatoryFilterID).Contains(x))).ToList();
            }

            if (IsParallelable)
                generators = generators.Where(g => g.IsParallelable).ToList();

            if (solutionSetupDetail.EngineDuty.Description.ToLower() == "prime")
                generators = generators.Where(g => g.PrimePowerAvailable).ToList();

            /* Sizing Method Auto */
            if (_SizingMethod != SizingMethodEnum.Manual)
            {
                if (generators.Count > 0)
                {
                    var selectedGenset = findGensetRow(generators, solutionSetupDetail, _loadSummaryLoads, react, maxKWStartingPerFrequencyDip);

                    if (selectedGenset != null)
                    {
                        selectedGenerator = new GeneratorDetail();
                        selectedGenerator = selectedGenset;
                        _GeneratorID = selectedGenset.GeneratorID;
                        _AlternatorID = selectedGenset.AlternatorDetail.AlternatorID;
                        units = selectedGenset.Quantity;
                        _ParallelQuantity = units;

                        _fDip = GetRecommendedFrequencyDip(_loadSummaryLoads.SolutionSummaryLoadSummary.StepKW, selectedGenerator);
                        _vDip = GetRecommendedVoltageDip(_loadSummaryLoads.SolutionSummaryLoadSummary.RunningKW,
                            _loadSummaryLoads.SolutionSummaryLoadSummary.StepKVA, _fDip, selectedGenerator, selectedGenerator.AlternatorDetail, int.Parse(solutionSetupDetail.Frequency.Value));
                    }

                    if (_GeneratorID != null)
                    {
                        units = units < 1 ? 1 : units;

                        var kwPerUnit = selectedGenset.KwStandby / units;

                        var alternators = selectedGenset.Generator.GeneratorAvailableAlternators.Select(g => g.Alternator)
                                                                                                .Where(a => a.Active
                                                                                                       && a.VoltagePhaseID == solutionSetupDetail.VoltagePhaseID
                                                                                                       && a.FrequencyID == solutionSetupDetail.FrequencyID
                                                                                                       && a.VoltageNominalID == solutionSetupDetail.VoltageNominalID)
                                                                                                .OrderBy(a => a.KWRating).ToList();
                        selectedAlternator = new AlternatorDetail();
                        selectedAlternator = findAlternatorRow(selectedGenset, alternators, solutionSetupDetail, _loadSummaryLoads, react, units);

                        if (selectedAlternator != null)
                        {
                            _AlternatorID = selectedAlternator.AlternatorID;
                        }
                        else
                            _AlternatorID = null;
                    }
                }
            }

            /* Sizing Method Manual*/
            else
            {
                if (_ProductFamilyID != null)
                {
                    //_ParallelQuantity = int.Parse(recommendedProduct.ParallelQuantity.Value);
                    if (_GeneratorID != null)
                    {
                        var generator = _generatorRepository.GetSingle(g => g.ID == _GeneratorID && g.Active);

                        if (generator.IsParallelable)
                        {
                            _ParallelQuantity = int.Parse(recommendedProduct.ParallelQuantity.Value);
                        }
                        else
                        {
                            _ParallelQuantity = 1;
                        }

                        selectedGenerator = _generatorAlternator.GetGeneratorAlternatorDetailByID((int)_GeneratorID, solutionSetupDetail, _ParallelQuantity);
                        _fDip = GetRecommendedFrequencyDip(_loadSummaryLoads.SolutionSummaryLoadSummary.StepKW, selectedGenerator);
                    }
                    if (_GeneratorID != null && _AlternatorID != null)
                    {
                        var alternator = _alternatorRepository.GetSingle(a => a.ID == _AlternatorID && a.Active);

                        selectedAlternator = _generatorAlternator.GetAlternatorDetail(alternator);
                        _vDip = GetRecommendedVoltageDip(_loadSummaryLoads.SolutionSummaryLoadSummary.RunningKW,
                            _loadSummaryLoads.SolutionSummaryLoadSummary.StepKVA, _fDip, selectedGenerator, selectedAlternator, int.Parse(solutionSetupDetail.Frequency.Value));
                    }
                }
                else
                {
                    _GeneratorID = null;
                    _AlternatorID = null;
                    _ParallelQuantity = 1;
                }

                IsParallelable = _ParallelQuantity > 1;
            }

            var productFamilyList = productFamilyForSelectedFuelTypeList.Select(x => new PickListDto
            {
                ID = x.ID,
                Description = x.Description,
                Value = x.Value,
                LanguageKey = x.LanguageKey,
            }).ToList();

            var generatorList = generators.OrderBy(g => g.KwStandby)
                            .Select(x => new PickListDto
                            {
                                ID = x.ID,
                                Description = x.ModelDescription,
                                Value = x.InternalDescription
                            }).ToList();

            var parallelQuantityID = _parallelQuantityRepository.GetSingle(p => p.Value == _ParallelQuantity.ToString()).ID;

            var parallelQuantityList = _pickListProcessor.GetParallelQuantity();

            if (_GeneratorID != null && _AlternatorID != null)
            {

                var alternatorList = _generatorRepository.GetAll(x => x.GeneratorAvailableAlternators.Any(y => y.GeneratorID == _GeneratorID))
                                                            .FirstOrDefault().GeneratorAvailableAlternators.Where(a => a.Alternator.Active
                                                                                                                       && a.Alternator.VoltagePhaseID == solutionSetupDetail.VoltagePhaseID
                                                                                                                       && a.Alternator.FrequencyID == solutionSetupDetail.FrequencyID
                                                                                                                       && a.Alternator.VoltageNominalID == solutionSetupDetail.VoltageNominalID)
                                                            .OrderBy(x => x.Alternator.KWRating).Select(x => new PickListDto
                                                            {
                                                                ID = x.Alternator.ID,
                                                                Description = x.Alternator.ModelDescription,
                                                                Value = x.Alternator.InternalDescription
                                                            }).ToList();

                if (!selectedGenerator.Generator.IsParallelable)
                    parallelQuantityList = parallelQuantityList.Where(x => Convert.ToInt16(x.Value) == 1).ToList();

                return new SolutionSummaryRecommendedProductDto
                {
                    FamilySelectionMethodID = (int)_FamiySelectionMethod,
                    ProductFamilyID = _ProductFamilyID,
                    ProductFamilyList = productFamilyList,
                    SizingMethodID = (int)_SizingMethod,
                    GeneratorID = _GeneratorID,
                    GeneratorList = generatorList,
                    ParallelQuantityList = parallelQuantityList,
                    AlternatorID = _AlternatorID,
                    AlternatorList = alternatorList,
                    RunningKW = Conversion.GetRoundedDecimal(GetLoadLevelRunning(_loadSummaryLoads.SolutionSummaryLoadSummary.RunningKW, selectedGenerator.KWApplicationRunning) * 100, 0),
                    PeakKW = Conversion.GetRoundedDecimal(GetLoadLevelPeak(_loadSummaryLoads.SolutionSummaryLoadSummary.PeakKW, selectedGenerator.KWApplicationPeak) * 100, 0),
                    FDip = Conversion.GetRoundedDecimal(_fDip, 1),
                    VDip = Conversion.GetRoundedDecimal(_vDip * 100, 1),
                    THVDContinuous = GetRecommendedTHVDContinuous(_loadSummaryLoads.SolutionSummaryLoadSummary.HarmonicsKVA, selectedGenerator, selectedAlternator).ToString("P1"),
                    THVDPeak = GetRecommendedTHVDPeak(_loadSummaryLoads.SolutionSummaryLoadSummary.HarmonicsKVA, selectedGenerator, selectedAlternator).ToString("P1"),
                    Description = GetRecommendedDescription(solutionSetupDetail.FuelType.LanguageKey, selectedGenerator, selectedAlternator),
                    DescriptionPartwo = GetRecommendedDescriptionPartTwo(solutionSetupDetail.FuelType.LanguageKey, selectedGenerator, selectedAlternator),
                    ParallelQuantityID = parallelQuantityID
                };
            }

            parallelQuantityList = parallelQuantityList.Where(x => Convert.ToInt16(x.Value) == 1).ToList();

            if (_GeneratorID != null)
            {
                return new SolutionSummaryRecommendedProductDto
                {
                    FamilySelectionMethodID = (int)_FamiySelectionMethod,
                    ProductFamilyID = _ProductFamilyID,
                    SizingMethodID = (int)_SizingMethod,
                    ProductFamilyList = productFamilyList,
                    Description = $"warning.NoSolutionAvailable",
                    GeneratorID = _GeneratorID,
                    ParallelQuantityList = parallelQuantityList,
                    AlternatorID = _AlternatorID,
                    FDip = Conversion.GetRoundedDecimal(_fDip, 1),
                    THVDContinuous = 0m.ToString("P1"),
                    THVDPeak = 0m.ToString("P1"),
                    RunningKW = Conversion.GetRoundedDecimal(GetLoadLevelRunning(_loadSummaryLoads.SolutionSummaryLoadSummary.RunningKW, selectedGenerator.KWApplicationRunning) * 100, 0),
                    PeakKW = Conversion.GetRoundedDecimal(GetLoadLevelPeak(_loadSummaryLoads.SolutionSummaryLoadSummary.PeakKW, selectedGenerator.KWApplicationPeak) * 100, 0),
                    GeneratorList = generatorList,
                    ParallelQuantityID = parallelQuantityID
                };
            }

            return new SolutionSummaryRecommendedProductDto
            {
                FamilySelectionMethodID = (int)_FamiySelectionMethod,
                ProductFamilyID = _ProductFamilyID,
                SizingMethodID = (int)_SizingMethod,
                ProductFamilyList = productFamilyList,
                ParallelQuantityList = parallelQuantityList,
                Description = $"warning.NoSolutionAvailable",
                GeneratorID = _GeneratorID,
                AlternatorID = _AlternatorID,
                THVDContinuous = 0m.ToString("P1"),
                THVDPeak = 0m.ToString("P1"),
                ParallelQuantityID = parallelQuantityID
            };

            //catch (Exception ex)
            //{
            //    _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, _loadSummaryLoads.SolutionID.ToString(), ex.TargetSite.Name, "", ex.Message);
            //    throw ex;
            //}

        }

        /// <summary>
        /// Method to verify if the MaxKWGenset in the family can provide a solution
        /// </summary>
        /// <param name="maxKWGenerator"></param>
        /// <param name="react"></param>
        /// <param name="solutionLimits"></param>
        /// <param name="maxKWStartingPerFrequencyDip"></param>
        /// <returns></returns>
        private bool IsMaxKWGensetProvidesSolution(GeneratorDetail maxKWGenerator, decimal react, SolutionLimitsDto solutionLimits, decimal maxKWStartingPerFrequencyDip)
        {
            if (_loadSummaryLoads.SolutionSummaryLoadSummary.PeakKW <= maxKWGenerator.KwStandby * solutionLimits.MaxRunningLoadValue
                            && (maxKWGenerator.TransientKWFDIP_1 >= _FDipList[1]
                                && maxKWGenerator.TransientKWFDIP_2 >= _FDipList[2]
                                && maxKWGenerator.TransientKWFDIP_3 >= _FDipList[3]
                                && maxKWGenerator.TransientKWFDIP_4 >= _FDipList[4]
                                && maxKWGenerator.TransientKWFDIP_5 >= _FDipList[5]
                                && maxKWGenerator.TransientKWFDIP_6 >= _FDipList[6]
                                && maxKWGenerator.TransientKWFDIP_7 >= _FDipList[7]
                                && maxKWGenerator.TransientKWFDIP_8 >= _FDipList[8]
                                && maxKWGenerator.TransientKWFDIP_9 >= _FDipList[9]
                                && maxKWGenerator.TransientKWFDIP_10 >= _FDipList[10]
                                && maxKWGenerator.TransientKWFDIP_11 >= _FDipList[11]
                                && maxKWGenerator.TransientKWFDIP_12 >= _FDipList[12]
                                && maxKWGenerator.TransientKWFDIP_13 >= _FDipList[13]
                                && maxKWGenerator.TransientKWFDIP_14 >= _FDipList[14]
                                && maxKWGenerator.TransientKWFDIP_15 >= _FDipList[15])
                            && maxKWGenerator.SKWFdip >= maxKWStartingPerFrequencyDip
                            && maxKWGenerator.AlternatorDetail.SubtransientReactance1000 <= react * maxKWGenerator.Quantity
                            && (maxKWGenerator.AlternatorDetail.TransientKWVDip_10 >= _VDipList[1] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_125 >= _VDipList[2] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_15 >= _VDipList[3] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_175 >= _VDipList[4] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_20 >= _VDipList[5] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_225 >= _VDipList[6] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_25 >= _VDipList[7] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_275 >= _VDipList[8] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_30 >= _VDipList[9] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_325 >= _VDipList[10] / maxKWGenerator.Quantity
                                && maxKWGenerator.AlternatorDetail.TransientKWVDip_35 >= _VDipList[11] / maxKWGenerator.Quantity)
                )
            {
                return true;
            }
            return false;
        }

        private string GetRecommendedDescription(string fuel, GeneratorDetail selectedGenerator, AlternatorDetail selectedAlternator)
        {
            //added multilingual language key for static and dynamic text
            return $"{(selectedGenerator.Quantity > 1 ? selectedGenerator.Quantity.ToString() + " x " : "")} {selectedGenerator.KwStandby / selectedGenerator.Quantity} kw, |{fuel}|Genset_Site_rated|{selectedGenerator.KWApplicationRunning} kw ";
            }

        private string GetRecommendedDescriptionPartTwo(string fuel, GeneratorDetail selectedGenerator, AlternatorDetail selectedAlternator)
        {
            //added multilingual language key for static and dynamic text
            return  $"{selectedGenerator.Generator.Liters} |L_Engine_with|{(selectedAlternator.Alternator.KWRating > selectedGenerator.KwStandby ? "Upsized|" : "Standard|")} ({selectedAlternator.Alternator.ModelDescription}) |Alternator";
        }

        private decimal GetRecommendedTHVDPeak(decimal kvaHarmonics, GeneratorDetail selectedGenerator, AlternatorDetail selectedAlternator)
        {
            var quantity = 1;
            decimal thvd = 0;
            if (selectedGenerator != null)
            {
                quantity = selectedGenerator.Quantity;
            }

            var i = _PeakHarmonicWithLoadFactor.GetUpperBound(0);
            var react =
                Convert.ToDecimal(
                            Math.Sqrt(
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 0] * 3), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 1] * 5), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 2] * 7), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 3] * 9), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 4] * 11), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 5] * 13), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 6] * 15 / 2), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 7] * 17 / 2), 2) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 8] * 19 / 2), 2)
                                    )
                                );

            if (react > 0)
            {
                if (selectedAlternator != null)
                {
                    thvd = selectedAlternator.SubtransientReactanceCorrected * kvaHarmonics
                                                        / (selectedAlternator.Alternator.KVABase * quantity) * react;
                }
            }

            return thvd / 100;
        }

        private decimal GetRecommendedTHVDContinuous(decimal kvaHarmonics, GeneratorDetail selectedGenerator, AlternatorDetail selectedAlternator)
        {
            var quantity = 1;
            decimal thvd = 0;
            if (selectedGenerator != null)
            {
                quantity = selectedGenerator.Quantity;
            }

            var i = _LargestContinuousHarmonicWithLoadFactor.GetUpperBound(0);
            var react =
                Convert.ToDecimal(
                            Math.Sqrt(
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 0] * 3), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 1] * 5), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 2] * 7), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 3] * 9), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 4] * 11), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 5] * 13), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 6] * 15 / 2), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 7] * 17 / 2), 2) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 8] * 19 / 2), 2)
                                    )
                                );

            if (react > 0)
            {
                if (selectedAlternator != null)
                {
                    thvd = selectedAlternator.SubtransientReactanceCorrected * kvaHarmonics
                                                        / (selectedAlternator.Alternator.KVABase * quantity) * react;
                }
            }

            return thvd / 100;
        }

        private decimal GetRecommendedVoltageDip(decimal kwRunning, decimal kvaStep, decimal fDip, GeneratorDetail selectedGenerator, AlternatorDetail alternatorDetail, int solutionSetupFrequnecy)
        {
            decimal vDip = 0;
            if (alternatorDetail.Alternator != null)
            {
                if (kwRunning == 0)
                {
                    return 0;
                }

                vDip = GetVdipFromKVA(kvaStep / selectedGenerator.Quantity, alternatorDetail);
                if (selectedGenerator.Generator.VoltsHertzMultiplier != null)
                {
                    var voltageHertzMultiplier = selectedGenerator.Generator.VoltsHertzMultiplier;
                    var modifiedFDipPercent = (fDip / solutionSetupFrequnecy) * voltageHertzMultiplier;
                    if (vDip < modifiedFDipPercent)
                    {
                        vDip = (decimal)modifiedFDipPercent;
                    }
                }
                else
                {
                    var modifiedFDipPercent = (fDip / solutionSetupFrequnecy) * 1.75m;
                    if (vDip < modifiedFDipPercent)
                    {
                        vDip = modifiedFDipPercent;
                    }
                }
            }

            return vDip;
        }

        private decimal GetVdipFromKVA(decimal kva, AlternatorDetail alternatorDetail)
        {
            try
            {
                var alternator = alternatorDetail.Alternator;
                var multiplier = alternatorDetail.SKVAMultiplier;
                decimal[] firstSeries = { (decimal)(alternator.Percent35 * multiplier), (decimal)(alternator.Percent25 * multiplier), (decimal)(alternator.Percent15 * multiplier) };
                double[] secondSeries = { 0.35, 0.25, 0.15 };
                var rounded = Convert.ToInt32(Statistics.lRegression(firstSeries, secondSeries, (double)kva) * 1000);
                return (decimal)((double)rounded / 1000);
            }
            catch (Exception)
            {
                return 0;
            }

        }

        private decimal GetFDipFromKW(int kw, GeneratorDetail generatorDetail)
        {
            var generator = generatorDetail.Generator;
            decimal[] firstSeries = { generator.FDip50, generator.FDip100 };
            double[] secondSeries = { generatorDetail.KWRated50, generatorDetail.KWNominalRated };
            int rounded;

            rounded = (int)Statistics.lRegression(firstSeries, secondSeries, kw) * 100;

            if ((rounded / 100) < generator.FDip50)
            {
                rounded = (int)((generator.FDip50 / generatorDetail.KWRated50) * kw * 100);
                return rounded / 100;
            }
            return rounded / 100;
        }

        private decimal GetRecommendedFrequencyDip(decimal kw, GeneratorDetail selectedGenerator)
        {
            if (selectedGenerator != null)
            {
                return GetFDipFromKW(kw, selectedGenerator);
            }
            return 0;
        }

        private decimal GetFDipFromKW(decimal kw, GeneratorDetail generator)
        {
            try
            {
                decimal[] firstSeries = { generator.KWRated50, generator.KWNominalRated };
                double[] secondSeries = { (double)generator.Generator.FDip50, (double)generator.Generator.FDip100 };
                double rounded;

                rounded = (int)(Statistics.lRegression(firstSeries, secondSeries, (double)kw) * 100);

                if ((decimal)(rounded / 100) < generator.Generator.FDip50)
                {
                    rounded = Convert.ToInt32((generator.Generator.FDip50 / generator.KWRated50) * kw * 100);
                    return (decimal)((double)rounded / 100);
                }
                return (decimal)((double)rounded / 100);
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        private decimal GetMaxKWStartingPerFrequencyDip(LoadSummaryLoadsDto loadSummaryLoads, int frequency)
        {
            decimal kw = 0;
            foreach (var loadSeq in loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                var maxKWStartingPerFrequencyDip = GetMaxKWStartingPerFrequencyDip(loadSeq, frequency);

                if (!loadSeq.LoadSequenceSummary.Shed && maxKWStartingPerFrequencyDip > kw)
                {
                    kw = maxKWStartingPerFrequencyDip;
                }
            }

            return kw;
        }

        private decimal GetMaxKWStartingPerFrequencyDip(LoadSummaryLoadListDto loadList, int frequency)
        {
            decimal fDip = 0;
            foreach (var load in loadList.LoadSequenceList)
            {                
                if (load.LoadFamilyID == (int)SolutionLoadFamilyEnum.UPS)
                {
                    var loadKWStartingPerFrequencyDip = GetKWStartingPerFrequencyDipUPSLoad(load.FrequencyDip, load.StartingKW, load.FrequencyDipUnitsID, frequency);
                    if (loadKWStartingPerFrequencyDip > fDip)
                        fDip = loadKWStartingPerFrequencyDip;
                }
            }

            return fDip;
        }

        private decimal GetKWStartingPerFrequencyDipUPSLoad(decimal fDip, decimal startingKW, int frequencyDipUnitsID, int frequency)
        {
            try
            {
                if (fDip > 0)
                {
                    if (frequencyDipUnitsID == (int)FrequencyDipUnitsEnum.Percent)
                        fDip = VDipFDip.ConvertFrequencyDip(frequency, fDip, (int)FrequencyDipUnitsEnum.Hertz);

                    return startingKW / fDip;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private decimal GetLoadLevelPeak(decimal kwPeak, int kWPeakGenerator)
        {
            if (kWPeakGenerator != 0)
            {
                return kwPeak / kWPeakGenerator;
            }
            return 0;
        }

        private decimal GetLoadLevelRunning(decimal kwRunning, decimal kwRunningGenerator)
        {
            if (kwRunningGenerator != 0)
            {
                return kwRunning / kwRunningGenerator;
            }
            return 0;
        }

        private decimal GetRequiredSubtransientReactanceContinuous(decimal maxVoltageDistortionContinuous)
        {
            //var maxVoltageDistortionContinuous = 0.11;
            var i = _LargestContinuousHarmonicWithLoadFactor.GetUpperBound(0);
            var react =
                Convert.ToDecimal(
                            Math.Sqrt(
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 0] * 3), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 1] * 5), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 2] * 7), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 3] * 9), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 4] * 11), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 5] * 13), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 6] * 15 / 2), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 7] * 17 / 2), 2.0) +
                                    Math.Pow(((double)_LargestContinuousHarmonicWithLoadFactor[i, 8] * 19 / 2), 2.0)
                                    )
                                );

            if (react != 0)
            {
                return (decimal)(maxVoltageDistortionContinuous * 100) / react;
            }
            else
            {
                return 1;
            }
        }

        private decimal GetRequiredSubtransientReactanceMomentary(decimal maxVoltageDistortionMomentary)
        {
            //var maxVoltageDistortionMomentary = 0.12;
            var i = _LargestContinuousHarmonicWithLoadFactor.GetUpperBound(0);
            var react =
                Convert.ToDecimal(
                            Math.Sqrt(
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 0] * 3), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 1] * 5), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 2] * 7), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 3] * 9), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 4] * 11), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 5] * 13), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 6] * 15 / 2), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 7] * 17 / 2), 2.0) +
                                    Math.Pow(((double)_PeakHarmonicWithLoadFactor[i, 8] * 19 / 2), 2.0)
                                    )
                                );

            if (react != 0)
            {
                return (decimal)(maxVoltageDistortionMomentary * 100) / react;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Find suitable Generator from the list of Gensets in the family
        /// </summary>
        /// <param name="generators"></param>
        /// <param name="solutionSetup"></param>
        /// <param name="loadSummaryLoads"></param>
        /// <param name="react"></param>
        /// <param name="maxKWStartingPerFrequencyDip"></param>
        /// <returns></returns>
        private GeneratorDetail findGensetRow(List<Generator> generators, SolutionSetup solutionSetup, LoadSummaryLoadsDto loadSummaryLoads, decimal react, decimal maxKWStartingPerFrequencyDip)
        {
            var solutionsummaryLoadSummary = loadSummaryLoads.SolutionSummaryLoadSummary;
            for (int i = 1; i <= _ParallelQuantity; i++)
            {
                foreach (var generator in generators)
                {
                    var generatorAlternatordetail = _generatorAlternator.GetGeneratorAlternatorDetailByID(generator.ID, solutionSetup, i);

                    if (generatorAlternatordetail.AlternatorDetail != null && generatorAlternatordetail.KWApplicationPeak >= solutionsummaryLoadSummary.PeakKW)
                    {
                        if (generatorAlternatordetail.KWApplicationRunning >= (solutionsummaryLoadSummary.RunningKW / decimal.Parse(solutionSetup.MaxRunningLoad.Value)))
                        {
                            if (generatorAlternatordetail.TransientKWFDIP_1 >= _FDipList[1]
                               && generatorAlternatordetail.TransientKWFDIP_2 >= _FDipList[2]
                               && generatorAlternatordetail.TransientKWFDIP_3 >= _FDipList[3]
                               && generatorAlternatordetail.TransientKWFDIP_4 >= _FDipList[4]
                               && generatorAlternatordetail.TransientKWFDIP_5 >= _FDipList[5]
                               && generatorAlternatordetail.TransientKWFDIP_6 >= _FDipList[6]
                               && generatorAlternatordetail.TransientKWFDIP_7 >= _FDipList[7]
                               && generatorAlternatordetail.TransientKWFDIP_8 >= _FDipList[8]
                               && generatorAlternatordetail.TransientKWFDIP_9 >= _FDipList[9]
                               && generatorAlternatordetail.TransientKWFDIP_10 >= _FDipList[10]
                               && generatorAlternatordetail.TransientKWFDIP_11 >= _FDipList[11]
                               && generatorAlternatordetail.TransientKWFDIP_12 >= _FDipList[12]
                               && generatorAlternatordetail.TransientKWFDIP_13 >= _FDipList[13]
                               && generatorAlternatordetail.TransientKWFDIP_14 >= _FDipList[14]
                               && generatorAlternatordetail.TransientKWFDIP_15 >= _FDipList[15])
                            {
                                if (generatorAlternatordetail.SKWFdip >= maxKWStartingPerFrequencyDip)
                                {
                                    if (generatorAlternatordetail.AlternatorDetail.SubtransientReactance1000 <= react * generatorAlternatordetail.Quantity)
                                    {
                                        if (generatorAlternatordetail.AlternatorDetail.TransientKWVDip_10 >= _VDipList[1] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_125 >= _VDipList[2] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_15 >= _VDipList[3] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_175 >= _VDipList[4] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_20 >= _VDipList[5] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_225 >= _VDipList[6] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_25 >= _VDipList[7] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_275 >= _VDipList[8] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_30 >= _VDipList[9] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_325 >= _VDipList[10] / generatorAlternatordetail.Quantity
                                            && generatorAlternatordetail.AlternatorDetail.TransientKWVDip_35 >= _VDipList[11] / generatorAlternatordetail.Quantity)
                                        {
                                            _ParallelQuantity = i;
                                            return generatorAlternatordetail;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Find suitable Alterantor from the list of Altenators associated to the Generator
        /// </summary>
        /// <param name="generatorDetail"></param>
        /// <param name="alternators"></param>
        /// <param name="solutionSetup"></param>
        /// <param name="loadSummaryLoads"></param>
        /// <param name="react"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        private AlternatorDetail findAlternatorRow(GeneratorDetail generatorDetail, List<Alternator> alternators, SolutionSetup solutionSetup, LoadSummaryLoadsDto loadSummaryLoads, decimal react, int units)
        {
            var solutionsummaryLoadSummary = loadSummaryLoads.SolutionSummaryLoadSummary;
            foreach (var alternator in alternators)
            {
                var alternatorDetail = _generatorAlternator.GetAlternatorDetail(alternator);

                _vDip = GetRecommendedVoltageDip(loadSummaryLoads.SolutionSummaryLoadSummary.RunningKW,
                            loadSummaryLoads.SolutionSummaryLoadSummary.StepKVA, _fDip, generatorDetail, alternatorDetail, int.Parse(solutionSetup.Frequency.Value));

                if (alternatorDetail.KWDerated >= (solutionsummaryLoadSummary.RunningKW / decimal.Parse(solutionSetup.MaxRunningLoad.Value)) / units)
                {
                    if (alternatorDetail.Alternator.KWRating >= generatorDetail.KwStandby / units)
                    {
                        if (alternatorDetail.TransientKWVDip_10 >= _VDipList[1] / units
                            && alternatorDetail.TransientKWVDip_125 >= _VDipList[2] / units
                            && alternatorDetail.TransientKWVDip_15 >= _VDipList[3] / units
                            && alternatorDetail.TransientKWVDip_175 >= _VDipList[4] / units
                            && alternatorDetail.TransientKWVDip_20 >= _VDipList[5] / units
                            && alternatorDetail.TransientKWVDip_225 >= _VDipList[6] / units
                            && alternatorDetail.TransientKWVDip_25 >= _VDipList[7] / units
                            && alternatorDetail.TransientKWVDip_275 >= _VDipList[8] / units
                            && alternatorDetail.TransientKWVDip_30 >= _VDipList[9] / units
                            && alternatorDetail.TransientKWVDip_325 >= _VDipList[10] / units
                            && alternatorDetail.TransientKWVDip_35 >= _VDipList[11] / units)
                        {
                            if (alternatorDetail.SubtransientReactance1000 <= react * units && _vDip <= decimal.Parse(solutionSetup.VoltageDip.Value))
                            {
                                return alternatorDetail;
                            }
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets MaxKWGenerator from the family
        /// </summary>
        /// <param name="solutionSetup"></param>
        /// <param name="productFamily"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        private GeneratorDetail GetMaxKWGenerator(SolutionSetup solutionSetup, ProductFamily productFamily, string selectedFuelType, int quantity)
        {
            var generators = productFamily.Generators.Where(x => x.Active
                                            && x.FrequencyID == solutionSetup.FrequencyID
                                            && x.AvailableFuelCode.Contains(selectedFuelType)
                                            && x.GeneratorAvailableVoltages.Any(g => g.VoltageNominalID == solutionSetup.VoltageNominalID))
                                            .OrderByDescending(x => x.KwStandby).ToList();


            if (solutionSetup.EngineDuty.Description.ToLower() == "prime")
                generators = generators.Where(g => g.PrimePowerAvailable).ToList();

            if (_regulatoryFilters != null)
            {
                generators = generators.Where(g => _regulatoryFilters.All(x => g.GeneratorRegulatoryFilters.Select(y => y.RegulatoryFilterID).Contains(x))).ToList();
            }

            if (quantity > 1)
                generators = generators.Where(x => x.IsParallelable).OrderByDescending(x => x.KwStandby).ToList();

            var generatorDetail = new GeneratorDetail();
            foreach (var generator in generators)
            {
                var generatorAlternatorDetail = _generatorAlternator.GetGeneratorAlternatorDetailByID(generator.ID, solutionSetup, quantity);

                if (generatorAlternatorDetail.AlternatorDetail != null)
                    return generatorAlternatorDetail;
            }
            return null;
        }

        //private GeneratorDetail GetMaxKWParallelableGenerator(SolutionSetup solutionSetup, ProductFamily productFamily)
        //{
        //    var parallelableGenerators = productFamily.Generators.Where(x => x.IsParallelable
        //                                    && x.FrequencyID == solutionSetup.FrequencyID
        //                                    && x.GeneratorAvailableVoltages.Any(g => g.VoltageNominalID == solutionSetup.VoltageNominalID)
        //                                    && _regulatoryFilters.All(g => x.GeneratorRegulatoryFilters.Select(y => y.RegulatoryFilterID).Contains(g)))
        //                                    .OrderByDescending(x => x.KwStandby).ToList();

        //    var generatorDetail = new GeneratorDetail();
        //    foreach (var generator in parallelableGenerators)
        //    {
        //        var generatorAlternatorDetail = _generatorAlternator.GetGeneratorAlternatorDetailByID(generator.ID, solutionSetup, _ParallelQuantity);

        //        generatorAlternatorDetail.Quantity = 15;

        //        if (generatorAlternatorDetail.AlternatorDetail != null)
        //            return generatorAlternatorDetail;
        //    }

        //    return null;
        //}

        /// <summary>
        /// Function to Init VDip and FDip List
        /// </summary>
        /// <param name="loadSummaryLoads"></param>
        /// <param name="solutionLimits"></param>
        private void InitCalculations(LoadSummaryLoadsDto loadSummaryLoads, SolutionLimitsDto solutionLimits)
        {
            try
            {
                var loadSequenceList = loadSummaryLoads.ListOfLoadSummaryLoadList;
                var sequenceCount = _sequenceRepository.GetAll().Count();

                //_LargestContinuousHarmonicWithLoadFactor = new decimal[loadSequenceList.Count, 10];
                //_PeakHarmonicWithLoadFactor = new decimal[loadSequenceList.Count, 9];

                _FDipList.Clear();
                for (int i = 1; i <= 15; i++)
                    _FDipList.Add(i, 0);

                _VDipList.Clear();
                for (int j = 1; j <= 15; j++)
                    _VDipList.Add(j, 0);

                var currentFDip = ConvertFrequencyDipHertzToIndex(solutionLimits.FDip);
                var currentVDip = ConvertVoltageDipPercentToIndex(solutionLimits.VDip);

                foreach (var sequence in loadSequenceList)
                {
                    if (!sequence.LoadSequenceSummary.Shed && sequence.LoadSequenceSummary.LoadCount > 0)
                    {
                        var fIndex = ConvertFrequencyDipHertzToIndex(sequence.LoadSequenceSummary.FDipHertz);

                        if (fIndex < currentFDip)
                            currentFDip = fIndex;

                        decimal kw = sequence.LoadSequenceSummary.StartingKW;

                        if (kw > _FDipList[currentFDip])
                            _FDipList[currentFDip] = kw;

                        var vIndex = ConvertVoltageDipPercentToIndex(sequence.LoadSequenceSummary.VDipPerc);

                        if (vIndex < currentVDip)
                            currentVDip = vIndex;

                        decimal kva = sequence.LoadSequenceSummary.StartingKVA;

                        if (kva > _VDipList[currentVDip])
                            _VDipList[currentVDip] = kva;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillLargestContinuousHarmonicWithLoadFactor()
        {
            decimal[] runTotal = new decimal[10];
            decimal _harmonicsKVA = _solutionSummaryLoadSummary.HarmonicsKVA;
            LoadSequenceSummaryDto sequence = new LoadSequenceSummaryDto();

            for (int i = 0; i <= _LargestContinuousHarmonicWithLoadFactor.GetUpperBound(0); i++)
            {
                sequence = _loadSummaryLoads.ListOfLoadSummaryLoadList[i].LoadSequenceSummary;

                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion3, 0, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion5, 1, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion7, 2, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion9, 3, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion11, 4, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion13, 5, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion15, 6, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion17, 7, runTotal, _harmonicsKVA);
                sumContinuousHarmonic(sequence, i, sequence.LargestContinuousWithLoadFactor.HarmonicDistortion19, 8, runTotal, _harmonicsKVA);

                _LargestContinuousHarmonicWithLoadFactor[i, 9] =
                    Convert.ToDecimal(
                                Math.Sqrt(
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 0], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 1], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 2], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 3], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 4], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 5], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 6], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 7], 2.0) +
                                         Math.Pow((double)_LargestContinuousHarmonicWithLoadFactor[i, 8], 2.0)
                                         )
                                    );

                for (int j = 0; j <= 9; j++)
                {
                    if (_LargestContinuousHarmonicWithLoadFactor[i, j] > 0)
                        runTotal[j] = _LargestContinuousHarmonicWithLoadFactor[i, j];

                    if (i == _LargestContinuousHarmonicWithLoadFactor.GetUpperBound(0))
                        _LargestContinuousHarmonicWithLoadFactor[i, j] = runTotal[j];
                }
            }
        }

        /// <summary>
        /// Fill the _PeakHarmonicWithLoadFactor 2D array
        /// </summary>
        private void FillPeakHarmonicWithLoadFactor()
        {
            LoadSequenceSummaryDto sequence = new LoadSequenceSummaryDto();
            decimal _harmonicsKVA = _solutionSummaryLoadSummary.HarmonicsKVA;
            decimal maxTHID = 0;
            int maxIDX = 0;

            for (int i = 0; i <= _PeakHarmonicWithLoadFactor.GetUpperBound(0); i++)
            {
                if (i == 0)
                {
                    sequence = _loadSummaryLoads.ListOfLoadSummaryLoadList[i].LoadSequenceSummary;

                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion3, 0, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion5, 1, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion7, 2, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion9, 3, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion11, 4, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion13, 5, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion15, 6, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion17, 7, _harmonicsKVA);
                    sumPeakHarmonic(sequence, i, sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion19, 8, _harmonicsKVA);
                }
                else if (i == _PeakHarmonicWithLoadFactor.GetUpperBound(0))
                {
                    _PeakHarmonicWithLoadFactor[i, 0] = _PeakHarmonicWithLoadFactor[maxIDX, 0];
                    _PeakHarmonicWithLoadFactor[i, 1] = _PeakHarmonicWithLoadFactor[maxIDX, 1];
                    _PeakHarmonicWithLoadFactor[i, 2] = _PeakHarmonicWithLoadFactor[maxIDX, 2];
                    _PeakHarmonicWithLoadFactor[i, 3] = _PeakHarmonicWithLoadFactor[maxIDX, 3];
                    _PeakHarmonicWithLoadFactor[i, 4] = _PeakHarmonicWithLoadFactor[maxIDX, 4];
                    _PeakHarmonicWithLoadFactor[i, 5] = _PeakHarmonicWithLoadFactor[maxIDX, 5];
                    _PeakHarmonicWithLoadFactor[i, 6] = _PeakHarmonicWithLoadFactor[maxIDX, 6];
                    _PeakHarmonicWithLoadFactor[i, 7] = _PeakHarmonicWithLoadFactor[maxIDX, 7];
                    _PeakHarmonicWithLoadFactor[i, 8] = _PeakHarmonicWithLoadFactor[maxIDX, 8];
                }
                else
                {
                    sequence = _loadSummaryLoads.ListOfLoadSummaryLoadList[i].LoadSequenceSummary;

                    if (!sequence.Shed && (sequence.LoadCount > 0 && _harmonicsKVA > 0))
                    {
                        _PeakHarmonicWithLoadFactor[i, 0] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion3 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 0] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 1] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion5 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 1] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 2] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion7 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 2] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 3] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion9 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 3] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 4] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion11 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 4] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 5] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion13 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 5] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 6] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion15 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 6] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 7] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion17 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 7] * _harmonicsKVA) / _harmonicsKVA;
                        _PeakHarmonicWithLoadFactor[i, 8] = (sequence.PeakHarmonicWithLoadFactor.HarmonicDistortion19 * sequence.KVABaseErrorChecked + _LargestContinuousHarmonicWithLoadFactor[i - 1, 8] * _harmonicsKVA) / _harmonicsKVA;
                    }
                    else
                    {
                        for (int j = 0; j <= 8; j++)
                            _PeakHarmonicWithLoadFactor[i, j] = _LargestContinuousHarmonicWithLoadFactor[i - 1, j];
                    }
                }

                _PeakHarmonicWithLoadFactor[i, 9] =
                    Convert.ToDecimal(
                                Math.Sqrt(
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 0], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 1], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 2], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 3], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 4], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 5], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 6], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 7], 2.0) +
                                         Math.Pow((double)_PeakHarmonicWithLoadFactor[i, 8], 2.0)
                                         )
                                    );

                if (i < _PeakHarmonicWithLoadFactor.GetUpperBound(0) && _PeakHarmonicWithLoadFactor[i, 9] >= maxTHID)
                {
                    maxIDX = i;
                    maxTHID = _PeakHarmonicWithLoadFactor[i, 9];
                }
            }
        }

        private void sumContinuousHarmonic(LoadSequenceSummaryDto sequence, int seqIndex, decimal harmonicDistortion, int harmonicIndex, decimal[] runTotal, decimal harmonicsKVA)
        {
            if (!sequence.Shed && sequence.LoadCount > 0 && harmonicsKVA > 0)
            {
                _LargestContinuousHarmonicWithLoadFactor[seqIndex, harmonicIndex] = ((harmonicDistortion * sequence.KVABaseErrorChecked) / harmonicsKVA);
                _LargestContinuousHarmonicWithLoadFactor[seqIndex, harmonicIndex] += runTotal[harmonicIndex];
            }
            else
            {
                _LargestContinuousHarmonicWithLoadFactor[seqIndex, harmonicIndex] = 0;
            }
        }

        private void sumPeakHarmonic(LoadSequenceSummaryDto sequence, int seqIndex, decimal harmonicDistortion, int harmonicIndex, decimal harmonicsKVA)
        {
            if (!sequence.Shed && sequence.LoadCount > 0 && harmonicsKVA > 0)
                _PeakHarmonicWithLoadFactor[seqIndex, harmonicIndex] = ((harmonicDistortion * sequence.KVABaseErrorChecked) / harmonicsKVA);
            else
                _PeakHarmonicWithLoadFactor[seqIndex, harmonicIndex] = 0;
        }

        /// <summary>
        /// Method to Convert FDipHertz to index
        /// </summary>
        /// <param name="hertzValue"></param>
        /// <returns></returns>
        private int ConvertFrequencyDipHertzToIndex(decimal hertzValue)
        {
            var hertzValueText = hertzValue + " hertz";
            var frequencyDipList = _frequencyDipRepository.GetAll().ToList();

            var index = frequencyDipList.FindIndex(fd => String.Equals(fd.Description.ToLower(), hertzValueText)) + 1;

            if (index == 0)
                return -1;
            else
                return index;
        }

        private int ConvertVoltageDipPercentToIndex(decimal voltagePercent)
        {
            //var voltagePercentText = voltagePercent.ToString() + " %";
            var voltageDip = voltagePercent / 100;
            var voltageDipList = _voltageDipRepository.GetAll().ToList();

            var index = voltageDipList.FindIndex(vd => String.Equals(decimal.Parse(vd.Value).ToString("G29"), voltageDip.ToString("G29"))) + 1;

            if (index == 0)
                return -1;
            else
                return index;
        }

        #endregion


        #region Sequence Summary Private Methods

        private dynamic GetLoadDescription(Solution solutionDetail, int solutionLoadID, int loadFamilyID, string description = "")
        {
            dynamic _description = new ExpandoObject();

            switch (loadFamilyID)
            {
                case (int)SolutionLoadFamilyEnum.Basic:
                    var bLoad = solutionDetail.BasicLoadList.FirstOrDefault(x => x.ID == solutionLoadID);

                    _description.LoadName = description;
                    _description.Description = bLoad.Description;
                    _description.KVAPFDescription = $"{bLoad.Quantity} X {bLoad.SizeRunning}, @ {bLoad.RunningPF.LanguageKey}";
                    _description.HarmonicsDescription = $"Harmonics: THID = {bLoad.HarmonicContent.Description}%";

                    return _description;
                case (int)SolutionLoadFamilyEnum.Welder:
                    var welderLoad = solutionDetail.WelderLoadList.FirstOrDefault(x => x.ID == solutionLoadID);

                    _description.LoadName = description;
                    _description.Description = welderLoad.Description;
                    _description.KVAPFDescription = $"{welderLoad.Quantity} X {welderLoad.SizeRunning}, @ {welderLoad.RunningPF.LanguageKey}";
                    _description.HarmonicsDescription = $"Harmonics: THID = {welderLoad.HarmonicContent.Description}%";

                    return _description;
                case (int)SolutionLoadFamilyEnum.Lighting:
                    var lightingLoad = solutionDetail.LightingLoadList.FirstOrDefault(x => x.ID == solutionLoadID);

                    _description.LoadName = description;
                    _description.Description = lightingLoad.Description;
                    _description.KVAPFDescription = $"{lightingLoad.Quantity} X {lightingLoad.SizeRunning}, @ {lightingLoad.RunningPF.LanguageKey}";
                    _description.HarmonicsDescription = $"Harmonics: THID = {lightingLoad.HarmonicContent.Description}%";

                    return _description;
                case (int)SolutionLoadFamilyEnum.AC:
                    var acLoad = solutionDetail.ACLoadList.FirstOrDefault(x => x.ID == solutionLoadID);

                    _description.LoadName = description;
                    _description.Description = acLoad.Description;
                    _description.KVAPFDescription = $"{acLoad.Quantity} X {acLoad.Cooling},w/ ,{acLoad.Compressors.LanguageKey}";
                    _description.HarmonicsDescription = $"Cooling: {acLoad.CoolingLoad.Description} Reheat: {acLoad.ReheatLoad.Description}";

                    return _description;
                case (int)SolutionLoadFamilyEnum.UPS:
                    var upsLoad = solutionDetail.UPSLoadList.FirstOrDefault(x => x.ID == solutionLoadID);

                    _description.LoadName = description;
                    _description.Description = upsLoad.Description;
                    _description.KVAPFDescription = $"{upsLoad.Quantity} X , {upsLoad.SizeKVA} at {upsLoad.SizeKVAUnits.LanguageKey}|Loaded_at|{upsLoad.LoadLevel.Description}, {upsLoad.ChargeRate.LanguageKey}|Battery_Charging";
                    _description.HarmonicsDescription = $"Harmonics: THID = {upsLoad.HarmonicContent.Description}%";
                    return _description;
                case (int)SolutionLoadFamilyEnum.Motor:
                    var motorLoad = solutionDetail.MotorLoadList.FirstOrDefault(x => x.ID == solutionLoadID);

                    _description.LoadName = description;
                    _description.Description = motorLoad.Description;
                    _description.KVAPFDescription = $"{motorLoad.Quantity} X, {motorLoad.SizeRunning}|{motorLoad.SizeRunningUnits.LanguageKey}, {motorLoad.StartingCode.Description}|{motorLoad.StartingMethod.LanguageKey}, {motorLoad.MotorLoadType.LanguageKey}|running_at|{motorLoad.MotorLoadLevel.LanguageKey}";
                    _description.HarmonicsDescription = $"";
                    return _description;
                default:
                    return null;
            }
        }

        private LoadSequenceDto GetLoadSequencData(BaseSolutionLoadEntity x)
        {
            var upsLoad = _solutionRepository.GetSingle(s => s.ID == x.SolutionID).UPSLoadList.FirstOrDefault(u => u.ID == x.ID);

            var upsRevertToBattery = upsLoad == null ? false : upsLoad.UPSRevertToBattery;

            var loadVoltageSpecific = x.VoltageSpecificID != null ? int.Parse(x.VoltageSpecific.Value) : 0;
            var frequency = _solutionFrequency;

            return new LoadSequenceDto
            {
                LoadID = x.LoadID,
                SequenceID = x.SequenceID,
                SolutionLoadID = x.ID,
                LoadFamilyID = x.Load.LoadFamilyID,
                Quantity = x.Quantity,
                StartingKVA = Conversion.GetRoundedDecimal(x.StartingLoadKva, 2),
                StartingKW = Conversion.GetRoundedDecimal(x.StartingLoadKw, 2),
                RunningKVA = Conversion.GetRoundedDecimal(x.RunningLoadKva, 2),
                RunningKW = Conversion.GetRoundedDecimal(x.RunningLoadKw, 2),
                Description = x.Load.Description,
                LanguageKey = x.Load.LanguageKey,
                LimitsVdip = (Conversion.GetRoundedDecimal(Decimal.Parse(x.VoltageDip.Value) * 100, 2)).ToString(),
                LimitsFdip = x.FrequencyDipUnits.Description.ToLower() == "percent" ? VDipFDip.ConvertFrequencyDip(frequency, decimal.Parse(x.FrequencyDip.Value), (int)VoltageDipUnitsEnum.Percent).ToString("P1") : x.FrequencyDip.Value + " Hertz",
                THIDContinuous = Conversion.GetRoundedDecimal(x.THIDContinuous, 1),
                THIDMomentary = Conversion.GetRoundedDecimal(x.THIDMomentary, 1),
                THIDKva = Conversion.GetRoundedDecimal(x.THIDKva, 1),
                HD3 = x.HD3,
                HD5 = x.HD5,
                HD7 = x.HD7,
                HD9 = x.HD9,
                HD11 = x.HD11,
                HD13 = x.HD13,
                HD15 = x.HD15,
                HD17 = x.HD17,
                HD19 = x.HD19,
                LoadVoltageSpecific = loadVoltageSpecific,
                VoltageDip = x.VoltageDipUnits.Description.ToLower() == "volts" ? VDipFDip.ConvertVoltageDip(loadVoltageSpecific, Decimal.Parse(x.VoltageDip.Value), (int)VoltageDipUnitsEnum.Volts) : Decimal.Parse(x.VoltageDip.Value),
                VoltageDipUnitsID = x.VoltageDipUnitsID,
                FrequencyDip = Decimal.Parse(x.FrequencyDip.Value),
                FrequencyDipUnitsID = x.FrequencyDipUnitsID,
                UPSRevertToBattery = upsRevertToBattery
            };
        }

        private LoadSequenceSummaryDto GetLoadSequenceSummary(LoadSummaryLoadListDto loadSummaryLoadListDto, int sequenceTypeID, string sequence, SolutionSetup solutionSetup)
        {
            var loadFactor = GetLoadFactor(sequence, solutionSetup);

            var kwStarting = GetKWStarting(loadSummaryLoadListDto, sequenceTypeID);
            var kwRunning = GetKWRunning(loadSummaryLoadListDto, sequenceTypeID, loadFactor);
            var kvaRunning = GetKVARunning(loadSummaryLoadListDto);
            var kvaStarting = GetKVAStaring(loadSummaryLoadListDto, sequenceTypeID);
            var kvaBaseErrorChecked = GetKVABaseErrorChecked(kvaRunning);
            var largestContinuousWithLoadFactor = GetLargestContinousWithLoadFactor(loadFactor);
            var peakHarmonicWithLoadFactor = GetPeakHarmonicWithLoadFactor(loadSummaryLoadListDto, loadFactor);
            var peakLoadValue = GetPeakLoadValueExtracted(loadSummaryLoadListDto, sequenceTypeID, kwStarting, kwRunning);
            return new LoadSequenceSummaryDto
            {
                Sequence=sequence.Split(' ')[0].Trim().ToLower(),
                LoadFactorDescription = GetLoadFactorDescription(sequence, sequenceTypeID, solutionSetup),
                StartingKW = Conversion.GetRoundedDecimal(kwStarting, 2),
                RunningKW = Conversion.GetRoundedDecimal(kwRunning, 2),
                StartingKVA = Conversion.GetRoundedDecimal(kvaStarting, 2),
                RunningKVA = Conversion.GetRoundedDecimal(kvaRunning, 2),
                THIDMomentary = Conversion.GetRoundedDecimal(GetTHIDPeak(loadSummaryLoadListDto, peakHarmonicWithLoadFactor), 1),
                THIDContinuous = Conversion.GetRoundedDecimal(GetTHIDRunning(loadSummaryLoadListDto, largestContinuousWithLoadFactor), 1),
                THIDKva = Conversion.GetRoundedDecimal(kvaBaseErrorChecked, 1),
                VDipPerc = GetVoltageDipPerc(loadSummaryLoadListDto, solutionSetup),
                VDipVolts = GetVoltageDipVolts(loadSummaryLoadListDto, solutionSetup),
                FDipPerc = GetFrequencyDipPerc(loadSummaryLoadListDto, solutionSetup),
                FDipHertz = GetFrequencyDipHertz(loadSummaryLoadListDto, solutionSetup),
                LoadCount = loadSummaryLoadListDto.LoadSequenceSummary.LoadCount,
                Shed = loadSummaryLoadListDto.LoadSequenceSummary.Shed,
                PeakLoadValue = peakLoadValue,
                KVABaseErrorChecked = kvaBaseErrorChecked,
                LargestContinuousWithLoadFactor = largestContinuousWithLoadFactor,
                PeakHarmonicWithLoadFactor = peakHarmonicWithLoadFactor,
                ApplicationPeak = GetApplicationPeak(peakLoadValue)
            };
        }

        private static decimal GetLoadFactor(string sequence, SolutionSetup solutionSetup)
        {
            if (sequence == "cyclic 1")
                return Decimal.Parse(solutionSetup.LoadSequenceCyclic1.Value);
            else if (sequence == "cyclic 2")
                return Decimal.Parse(solutionSetup.LoadSequenceCyclic2.Value);
            else
                return 1;
        }

        private static string GetLoadFactorDescription(string sequence, int sequenceTypeID, SolutionSetup solutionSetup)
        {
            if (sequence == "cyclic 1")
                return $"{solutionSetup.LoadSequenceCyclic1.Value}%";
            else if (sequence == "cyclic 2")
                return $"{solutionSetup.LoadSequenceCyclic2.Value}%";
            else
                return $"All_Loads_on_({((sequenceTypeID == (int)SequenceTypeEnum.Concurrent) ? "_concurrent_" : "_sequence_")} starting_)";
        }

        private static decimal GetPeakLoadValueExtracted(LoadSummaryLoadListDto loadSummaryLoadListDto, int sequenceTypeID, decimal kwStarting, decimal kwRunning)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            if (sequenceTypeID != (int)SequenceTypeEnum.Concurrent)
            {
                var largestKWStartingLoad = GetLargestKWStartingLoad(loadSummaryLoadListDto);
                if (largestKWStartingLoad == null)
                {
                    return 0;
                }
                else
                {
                    if ((kwStarting - largestKWStartingLoad.RunningKW) / largestKWStartingLoad.Quantity > 0)
                    {
                        return kwRunning + ((kwStarting - largestKWStartingLoad.RunningKW) / largestKWStartingLoad.Quantity);
                    }
                    else
                    {
                        return kwRunning;
                    }
                }
            }
            else
            {
                return kwStarting < kwRunning ? kwRunning : kwStarting;
            }
        }

        private decimal GetApplicationPeak(decimal peakLoadValue)
        {
            _applicationPeak += peakLoadValue;
            return _applicationPeak;
        }

        private static BaseLoadListDto GetLargestKVAStartingLoad(LoadSummaryLoadListDto loadSummaryLoadListDto)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return null;

            BaseLoadListDto largestKVAStartingLoad = null;
            foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
            {
                if (largestKVAStartingLoad == null)
                {
                    largestKVAStartingLoad = new BaseLoadListDto();
                    largestKVAStartingLoad = load;
                }
                else
                {
                    if (largestKVAStartingLoad.StartingKVA < load.StartingKVA)
                    {
                        largestKVAStartingLoad = load;
                    }
                }
            }

            return largestKVAStartingLoad;
        }

        private static BaseLoadListDto GetLargestKWStartingLoad(LoadSummaryLoadListDto loadSummaryLoadListDto)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return null;

            BaseLoadListDto largestKWStartingLoad = null;
            foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
            {
                if (largestKWStartingLoad == null)
                {
                    largestKWStartingLoad = new BaseLoadListDto();
                    largestKWStartingLoad = load;
                }
                else
                {
                    if (largestKWStartingLoad.StartingKW < load.StartingKW)
                    {
                        largestKWStartingLoad = load;
                    }
                }
            }

            return largestKWStartingLoad;
        }

        private static decimal GetKWStarting(LoadSummaryLoadListDto loadSummaryLoadListDto, int sequenceTypeID)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal kw = 0;
            if (sequenceTypeID == (int)SequenceTypeEnum.NonConcurrent)
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    if (load.StartingKW > kw)
                        kw = load.StartingKW;
                }
            }
            else
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                    kw += load.StartingKW;
            }
            return kw;
        }

        private static decimal GetKWRunning(LoadSummaryLoadListDto loadSummaryLoadListDto, int sequenceTypeID, decimal loadFactor)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal kw = 0;
            decimal kwMax = 0;

            if (sequenceTypeID == (int)SequenceTypeEnum.NonConcurrent)
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    if (load.RunningKW > kwMax)
                    {
                        kwMax = load.RunningKW;
                    }
                    kw += load.RunningKW;
                }
                kw = kwMax + (kw - kwMax) * loadFactor;
            }
            else
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                    kw += load.RunningKW;
            }
            return kw;
        }

        private static decimal GetKVAStaring(LoadSummaryLoadListDto loadSummaryLoadListDto, int sequenceTypeID)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal kva = 0;
            if (sequenceTypeID == (int)SequenceTypeEnum.NonConcurrent)
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    if (load.StartingKW > kva)
                        kva = load.StartingKVA;
                }
            }
            else
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                    kva += load.StartingKVA;
            }
            return kva;
        }

        private static decimal GetKVARunning(LoadSummaryLoadListDto loadSummaryLoadListDto)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal kva = 0;
            decimal kvaMax = 0;

            if (loadSummaryLoadListDto.SequenceTypeID == (int)SequenceTypeEnum.NonConcurrent)
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    if (load.RunningKVA > kvaMax)
                    {
                        kvaMax = load.RunningKVA;
                    }
                    kva += load.RunningKVA;
                }
                kva = kvaMax + (kva - kvaMax) * loadSummaryLoadListDto.LoadFactor;
            }
            else
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                    kva += load.RunningKVA;
            }
            return kva;
        }

        private decimal GetTHIDPeak(LoadSummaryLoadListDto loadSummaryLoadListDto, PeakHarmonicWithLoadFactor peakHarmonicWithLoadFactor)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            return GetPeakHarmonicWithLoadFactorTHID(loadSummaryLoadListDto, peakHarmonicWithLoadFactor);
        }

        private decimal GetTHIDRunning(LoadSummaryLoadListDto loadSummaryLoadListDto, LargestContinousWithLoadFactor largestContinousWithLoadFactor)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            return GetLargestContinousWithLoadFactorTHID(loadSummaryLoadListDto, largestContinousWithLoadFactor);
        }

        private static decimal GetVoltageDipPerc(LoadSummaryLoadListDto loadSummaryLoadListDto, SolutionSetup solutionSetup)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal vDip = Decimal.Parse(solutionSetup.VoltageDip.Value);
            decimal vDipLoad = 0;

            foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
            {
                if (load.VoltageDipUnitsID == (int)VoltageDipUnitsEnum.Percent)
                    vDipLoad = load.VoltageDip;
                else
                {
                    var voltageSpecific = load.LoadVoltageSpecific != 0 ? load.LoadVoltageSpecific : int.Parse(solutionSetup.VoltageSpecific.Value);
                    vDipLoad = VDipFDip.ConvertVoltageDip(voltageSpecific, load.VoltageDip, (int)VoltageDipUnitsEnum.Percent);
                }

                if (!load.UPSRevertToBattery && vDipLoad < vDip)
                    vDip = vDipLoad;
            }

            if (vDip == -1)
                vDip = 0;

            return Conversion.GetRoundedDecimal(vDip * 100, 2);
        }

        private static decimal GetVoltageDipVolts(LoadSummaryLoadListDto loadSummaryLoadListDto, SolutionSetup solutionSetup)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal vDip = -1;
            decimal vDipLoad = 0;

            foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
            {
                if (load.VoltageDipUnitsID == (int)VoltageDipUnitsEnum.Volts)
                    vDipLoad = load.VoltageDip;
                else
                {
                    var voltageSpecific = load.LoadVoltageSpecific != 0 ? load.LoadVoltageSpecific : int.Parse(solutionSetup.VoltageSpecific.Value);
                    vDipLoad = VDipFDip.ConvertVoltageDip(voltageSpecific, load.VoltageDip, (int)VoltageDipUnitsEnum.Volts);
                }

                if (vDip == -1)
                    vDip = vDipLoad;
                else
                {
                    if (vDipLoad < vDip)
                        vDip = vDipLoad;
                }
            }

            if (vDip == -1)
                vDip = 0;

            return vDip;
        }

        private static decimal GetFrequencyDipPerc(LoadSummaryLoadListDto loadSummaryLoadListDto, SolutionSetup solutionSetup)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal fDip = Decimal.Parse(solutionSetup.FrequencyDip.Value);
            decimal fDipLoad = 0;

            if (solutionSetup.FrequencyDipUnitsID == (int)FrequencyDipUnitsEnum.Hertz)
                fDip = VDipFDip.ConvertFrequencyDip(int.Parse(solutionSetup.Frequency.Value), fDip, (int)FrequencyDipUnitsEnum.Percent);

            foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
            {
                if (load.FrequencyDipUnitsID == (int)FrequencyDipUnitsEnum.Percent)
                    fDipLoad = load.FrequencyDip;
                else
                    fDipLoad = VDipFDip.ConvertFrequencyDip(int.Parse(solutionSetup.Frequency.Value), load.FrequencyDip, (int)FrequencyDipUnitsEnum.Percent);

                if (!load.UPSRevertToBattery && fDipLoad < fDip)
                    fDip = fDipLoad;
            }

            if (fDip == -1)
                fDip = 0;

            return decimal.Round(fDip * 100, 1, MidpointRounding.AwayFromZero);
        }

        private static decimal GetFrequencyDipHertz(LoadSummaryLoadListDto loadSummaryLoadListDto, SolutionSetup solutionSetup)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal fDip = Decimal.Parse(solutionSetup.FrequencyDip.Value);
            decimal fDipLoad = 0;

            foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
            {
                if (load.FrequencyDipUnitsID == (int)FrequencyDipUnitsEnum.Hertz)
                    fDipLoad = load.FrequencyDip;
                else
                    fDipLoad = VDipFDip.ConvertFrequencyDip(int.Parse(solutionSetup.Frequency.Value), load.FrequencyDip, (int)FrequencyDipUnitsEnum.Hertz);

                if (!load.UPSRevertToBattery && fDipLoad < fDip)
                    fDip = fDipLoad;
            }

            if (fDip == -1)
                fDip = 0;

            return fDip;
        }

        private decimal GetPeakHarmonicWithLoadFactorTHID(LoadSummaryLoadListDto loadSummaryLoadListDto, PeakHarmonicWithLoadFactor peakHarmonicWithLoadFactor)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;
            else
            {
                //var peakHarmonicWithLoadFactor = GetPeakHarmonicWithLoadFactor(loadSummaryLoadListDto, loadFactor);

                return Convert.ToDecimal(Math.Sqrt(Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion3, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion5, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion7, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion9, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion11, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion13, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion15, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion17, 2.0) +
                                 Math.Pow((double)peakHarmonicWithLoadFactor.HarmonicDistortion19, 2.0)));
            }
        }

        #region PeakHarmonicWithLoadFactor

        private PeakHarmonicWithLoadFactor GetPeakHarmonicWithLoadFactor(LoadSummaryLoadListDto loadSummaryLoadListDto, decimal loadFactor)
        {
            var largestContinousWithLoadFactor = GetLargestContinousWithLoadFactor(loadFactor);
            var largestMomentaryTHID = GetLargestMomentaryTHID(loadSummaryLoadListDto);
            decimal kvaRunning = GetKVARunning(loadSummaryLoadListDto);
            decimal kvaBaseErrorChecked = GetKVABaseErrorChecked(kvaRunning);
            var largestMomentaryTHIDKVABase = GetLargestMomentaryTHID_KVABase(_loadSummaryLoadListDto);

            return new PeakHarmonicWithLoadFactor
            {
                HarmonicDistortion3 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion3, largestMomentaryTHID.HarmonicDistortion3,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion5 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion5, largestMomentaryTHID.HarmonicDistortion5,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion7 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion7, largestMomentaryTHID.HarmonicDistortion7,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion9 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion9, largestMomentaryTHID.HarmonicDistortion9,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion11 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion11, largestMomentaryTHID.HarmonicDistortion11,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion13 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion13, largestMomentaryTHID.HarmonicDistortion13,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion15 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion15, largestMomentaryTHID.HarmonicDistortion15,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion17 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion17, largestMomentaryTHID.HarmonicDistortion17,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),
                HarmonicDistortion19 = CalculatePeakHarmonicWithLoadFactor(largestContinousWithLoadFactor.HarmonicDistortion19, largestMomentaryTHID.HarmonicDistortion19,
                                                                          largestMomentaryTHIDKVABase, kvaBaseErrorChecked),

            };
        }

        private decimal CalculatePeakHarmonicWithLoadFactor(decimal largestContinousWithLoadFactorHD, decimal largestMomentaryTHIDHD, decimal largestMomentaryTHIDKVABase, decimal kvaBaseErrorChecked)
        {
            return largestContinousWithLoadFactorHD + largestMomentaryTHIDHD * largestMomentaryTHIDKVABase / kvaBaseErrorChecked;
        }

        #endregion


        #region LargestContinousWithLoadFactor

        private LargestContinousWithLoadFactor GetLargestContinousWithLoadFactor(decimal loadFactor)
        {
            var largestContinuousTHIDLoad = GetLargestContinuousTHIDLoad();
            var allContinuousHarmonicDistortion = GetAllContinuousHarmonicDistortion();
            decimal allContinuousKVABase = AllContinuousKVABase(_loadSummaryLoadListDto);
            decimal kvaRunning = GetKVARunning(_loadSummaryLoadListDto);
            decimal kvaBaseErrorChecked = GetKVABaseErrorChecked(kvaRunning);

            if (largestContinuousTHIDLoad != null)
            {
                return new LargestContinousWithLoadFactor
                {
                    HarmonicDistortion3 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD3, allContinuousHarmonicDistortion.HarmonicDistortion3,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion5 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD5, allContinuousHarmonicDistortion.HarmonicDistortion5,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion7 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD7, allContinuousHarmonicDistortion.HarmonicDistortion7,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion9 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD9, allContinuousHarmonicDistortion.HarmonicDistortion9,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion11 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD11, allContinuousHarmonicDistortion.HarmonicDistortion11,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion13 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD13, allContinuousHarmonicDistortion.HarmonicDistortion13,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion15 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD15, allContinuousHarmonicDistortion.HarmonicDistortion15,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion17 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD17, allContinuousHarmonicDistortion.HarmonicDistortion17,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked),
                    HarmonicDistortion19 = CalculateLargestContinousWithLoadFactor(largestContinuousTHIDLoad.HD19, allContinuousHarmonicDistortion.HarmonicDistortion19,
                                                          largestContinuousTHIDLoad.RunningKVA, loadFactor, allContinuousKVABase, kvaBaseErrorChecked)
                };
            }
            else
                return new LargestContinousWithLoadFactor();
        }

        private decimal CalculateLargestContinousWithLoadFactor(decimal largestContinuousHD, decimal allContinuousHD, decimal largestContinuousRunningKVA, decimal loadFactor,
                                                                decimal allContinuousKVABase, decimal kvaBaseErrorChecked)
        {
            return (largestContinuousHD * largestContinuousRunningKVA +
                                           (allContinuousHD * (allContinuousKVABase == 0 ? 1 : allContinuousKVABase) -
                                           largestContinuousHD * largestContinuousRunningKVA) * loadFactor) / kvaBaseErrorChecked;
        }

        private decimal GetLargestContinousWithLoadFactorTHID(LoadSummaryLoadListDto loadSummaryLoadListDto, LargestContinousWithLoadFactor largestContinousWithLoadFactor)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;
            else
            {
                //var largestContinousWithLoadFactor = GetLargestContinousWithLoadFactor(loadFactor);

                return Convert.ToDecimal(Math.Sqrt(Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion3, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion5, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion7, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion9, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion11, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion13, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion15, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion17, 2.0) +
                     Math.Pow((double)largestContinousWithLoadFactor.HarmonicDistortion19, 2.0)));
            }
        }

        #endregion


        #region LargestMomentaryTHID        

        private static decimal GetLargestMomentaryTHID_KVABase(LoadSummaryLoadListDto loadSummaryLoadListDto)
        {
            if (loadSummaryLoadListDto.LoadSequenceSummary.Shed)
                return 0;

            decimal kva = 0;
            BaseLoadListDto loadData = null;

            if (loadSummaryLoadListDto.SequenceTypeID == (int)SequenceTypeEnum.NonConcurrent)
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    kva = (load.THIDMomentary * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                    if (kva > 0)
                    {
                        if (loadData == null || (loadData != null && (loadData.THIDMomentary * loadData.RunningKVA * (loadData.THIDContinuous > 0 ? 0 : 1)) < kva))
                        {
                            loadData = load;
                        }
                    }
                }

                return loadData != null ? loadData.RunningKVA : 0;
            }
            else
            {
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    kva += load.RunningKVA * (load.THIDMomentary > 0 ? 0 : 1) * (load.THIDContinuous == 0 ? 1 : 0);
                }
            }

            return kva;
        }

        private LargestMomentaryHarmonics GetLargestMomentaryTHID(LoadSummaryLoadListDto loadSummaryLoadListDto)
        {
            decimal hd3 = 0, hd5 = 0, hd7 = 0, hd9 = 0, hd11 = 0, hd13 = 0, hd15 = 0, hd17 = 0, hd19 = 0;
            decimal largestMomentaryTHIDKVABase = GetLargestMomentaryTHID_KVABase(loadSummaryLoadListDto);
            if (loadSummaryLoadListDto.SequenceTypeID != (int)SequenceTypeEnum.Concurrent)
            {
                LoadSequenceDto ld = null;
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    if (ld == null)
                        ld = load;
                    else
                    {
                        if ((ld.THIDMomentary * ld.RunningKVA * (ld.THIDContinuous > 0 ? 0 : 1)) < (load.THIDMomentary * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1)))
                            ld = load;
                    }
                }

                if (ld.THIDContinuous > 0)
                {
                    return new LargestMomentaryHarmonics
                    {
                        HarmonicDistortion3 = 0,
                        HarmonicDistortion5 = 0,
                        HarmonicDistortion7 = 0,
                        HarmonicDistortion9 = 0,
                        HarmonicDistortion11 = 0,
                        HarmonicDistortion13 = 0,
                        HarmonicDistortion15 = 0,
                        HarmonicDistortion17 = 0,
                        HarmonicDistortion19 = 0,
                    };
                }
                else
                {
                    return new LargestMomentaryHarmonics
                    {
                        HarmonicDistortion3 = ld.HD3,
                        HarmonicDistortion5 = ld.HD5,
                        HarmonicDistortion7 = ld.HD7,
                        HarmonicDistortion9 = ld.HD9,
                        HarmonicDistortion11 = ld.HD11,
                        HarmonicDistortion13 = ld.HD13,
                        HarmonicDistortion15 = ld.HD15,
                        HarmonicDistortion17 = ld.HD17,
                        HarmonicDistortion19 = ld.HD19,
                    };
                }
            }

            else
            {
                if (largestMomentaryTHIDKVABase > 0)
                {
                    foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                    {
                        hd3 += (load.HD3 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd5 += (load.HD5 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd7 += (load.HD7 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd9 += (load.HD9 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd11 += (load.HD11 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd13 += (load.HD13 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd15 += (load.HD15 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd17 += (load.HD17 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                        hd19 += (load.HD19 * load.RunningKVA * (load.THIDContinuous > 0 ? 0 : 1));
                    }

                    hd3 = hd3 / largestMomentaryTHIDKVABase;
                    hd5 = hd5 / largestMomentaryTHIDKVABase;
                    hd7 = hd7 / largestMomentaryTHIDKVABase;
                    hd9 = hd9 / largestMomentaryTHIDKVABase;
                    hd11 = hd11 / largestMomentaryTHIDKVABase;
                    hd13 = hd13 / largestMomentaryTHIDKVABase;
                    hd15 = hd15 / largestMomentaryTHIDKVABase;
                    hd17 = hd17 / largestMomentaryTHIDKVABase;
                    hd19 = hd19 / largestMomentaryTHIDKVABase;
                }
            }
            return new LargestMomentaryHarmonics
            {
                HarmonicDistortion3 = hd3,
                HarmonicDistortion5 = hd5,
                HarmonicDistortion7 = hd7,
                HarmonicDistortion9 = hd9,
                HarmonicDistortion11 = hd11,
                HarmonicDistortion13 = hd13,
                HarmonicDistortion15 = hd15,
                HarmonicDistortion17 = hd17,
                HarmonicDistortion19 = hd19,
            };
        }

        #endregion


        #region AllContinousTHID

        private AllContinuousHarmonicDistortion GetAllContinuousHarmonicDistortion()
        {
            decimal hd3 = 0, hd5 = 0, hd7 = 0, hd9 = 0, hd11 = 0, hd13 = 0, hd15 = 0, hd17 = 0, hd19 = 0;
            decimal allContinuousKVABase = AllContinuousKVABase(_loadSummaryLoadListDto);
            foreach (var load in _loadSummaryLoadListDto.LoadSequenceList)
            {
                if (load.THIDContinuous > 0)
                {
                    hd3 += (load.RunningKVA * load.HD3 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd5 += (load.RunningKVA * load.HD5 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd7 += (load.RunningKVA * load.HD7 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd9 += (load.RunningKVA * load.HD9 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd11 += (load.RunningKVA * load.HD11 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd13 += (load.RunningKVA * load.HD13 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd15 += (load.RunningKVA * load.HD15 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd17 += (load.RunningKVA * load.HD17 * (load.THIDContinuous > 0 ? 1 : 0));
                    hd19 += (load.RunningKVA * load.HD19 * (load.THIDContinuous > 0 ? 1 : 0));
                }
            }

            if (allContinuousKVABase == 0)
            {
                return new AllContinuousHarmonicDistortion
                {
                    HarmonicDistortion3 = hd3,
                    HarmonicDistortion5 = hd5,
                    HarmonicDistortion7 = hd7,
                    HarmonicDistortion9 = hd9,
                    HarmonicDistortion11 = hd11,
                    HarmonicDistortion13 = hd13,
                    HarmonicDistortion15 = hd15,
                    HarmonicDistortion17 = hd17,
                    HarmonicDistortion19 = hd19
                };
            }
            else
            {
                return new AllContinuousHarmonicDistortion
                {
                    HarmonicDistortion3 = hd3 / allContinuousKVABase,
                    HarmonicDistortion5 = hd5 / allContinuousKVABase,
                    HarmonicDistortion7 = hd7 / allContinuousKVABase,
                    HarmonicDistortion9 = hd9 / allContinuousKVABase,
                    HarmonicDistortion11 = hd11 / allContinuousKVABase,
                    HarmonicDistortion13 = hd13 / allContinuousKVABase,
                    HarmonicDistortion15 = hd15 / allContinuousKVABase,
                    HarmonicDistortion17 = hd17 / allContinuousKVABase,
                    HarmonicDistortion19 = hd19 / allContinuousKVABase
                };
            }
        }

        private decimal AllContinuousKVABase(LoadSummaryLoadListDto loadSummaryLoadListDto)
        {
            decimal kva = 0;
            foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                kva += load.RunningKVA * (load.THIDContinuous > 0 ? 1 : 0);

            return kva;
        }

        private static decimal GetAllContinuousAndMomentary_KVABase(LoadSummaryLoadListDto loadSummaryLoadListDto)
        {
            if (!loadSummaryLoadListDto.LoadSequenceSummary.Shed)
            {
                decimal kva = 0;
                foreach (var load in loadSummaryLoadListDto.LoadSequenceList)
                {
                    kva += load.THIDKva;
                }

                return kva == 0 ? (decimal)0.1 : kva;
            }

            return 0;
        }

        #endregion        

        private decimal GetKVABaseErrorChecked(decimal kvaRunning)
        {
            decimal kva = 0;
            var kVABaseNonErrorChecked = GetKVABaseNonErrorChecked();
            if (kvaRunning < kVABaseNonErrorChecked)
            {
                kva = kvaRunning == 0 ? (decimal)0.000000001 : kvaRunning;
            }
            else
            {
                kva = kVABaseNonErrorChecked == 0 ? (decimal)0.000000001 : kVABaseNonErrorChecked;
            }

            return kva < (decimal)0.2 ? (decimal)0.000000001 : kva;
        }

        private decimal GetKVABaseNonErrorChecked()
        {
            var largestContinuousTHIDLoad = GetLargestContinuousTHIDLoad();
            var largestMomentaryTHID_KVABase = GetLargestMomentaryTHID_KVABase(_loadSummaryLoadListDto);
            var allContinuousAndMomentary_KVABase = GetAllContinuousAndMomentary_KVABase(_loadSummaryLoadListDto);
            decimal kva = largestContinuousTHIDLoad != null ? largestContinuousTHIDLoad.RunningKVA : 0;
            return kva + largestMomentaryTHID_KVABase + (allContinuousAndMomentary_KVABase - kva - largestMomentaryTHID_KVABase) * _loadSummaryLoadListDto.LoadFactor;
        }

        private LoadSequenceDto GetLargestContinuousTHIDLoad()
        {
            LoadSequenceDto thid = null;

            foreach (var load in _loadSummaryLoadListDto.LoadSequenceList)
            {
                if (thid == null)
                {
                    thid = load;
                }
                else
                {
                    if ((thid.THIDContinuous * thid.RunningKVA) < (load.THIDContinuous * load.RunningKVA))
                        thid = load;
                }
            }
            if (thid != null && thid.THIDContinuous == 0)
                return null;
            else
                return thid;
        }

        #endregion

        #region Analysis Properties

        #region Transient Analysis


        private LoadSummaryLoadsDto FillAnalysisProperties(LoadSummaryLoadsDto loadSummaryLoads, SolutionSummaryRecommendedProductDto recommendedProductDetail, SolutionSetup solutionSetup)
        {
            foreach (var loadSequence in loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                loadSequence.LoadSequenceSummary.AlternatorExpectedVoltageDip = GetAlternatorExpectedVoltageDip(loadSequence, recommendedProductDetail);
                loadSequence.LoadSequenceSummary.VoltsHertzMultiplier = _generatorRepository.GetSingle(g => g.ID == recommendedProductDetail.GeneratorID).VoltsHertzMultiplier;
                loadSequence.LoadSequenceSummary.ExpectedFrequencyDip = GetExpectedFrequencyDip(loadSequence, recommendedProductDetail, solutionSetup);
                loadSequence.LoadSequenceSummary.EngineExpectedVoltageDip = GetEngineExpectedVoltageDip(loadSequence, recommendedProductDetail, loadSequence.LoadSequenceSummary.ExpectedFrequencyDip,
                    solutionSetup, loadSequence.LoadSequenceSummary.VoltsHertzMultiplier);
                loadSequence.LoadSequenceSummary.ExpectedVoltageDip = loadSequence.LoadSequenceSummary.AlternatorExpectedVoltageDip >= loadSequence.LoadSequenceSummary.EngineExpectedVoltageDip
                    ? loadSequence.LoadSequenceSummary.AlternatorExpectedVoltageDip : loadSequence.LoadSequenceSummary.EngineExpectedVoltageDip;
                loadSequence.LoadSequenceSummary.ProjectAllowableVoltageDip = GetProjectAllowableVoltageDip(loadSummaryLoads, loadSequence);
                loadSequence.LoadSequenceSummary.PercAllowableVoltageDip = GetPercAllowableVoltageDip(loadSequence.LoadSequenceSummary.EngineExpectedVoltageDip, loadSequence.LoadSequenceSummary.ProjectAllowableVoltageDip);
                loadSequence.LoadSequenceSummary.ProjectAllowableFrequencyDip = GetProjectAllowableFrequencyDip(loadSummaryLoads, loadSequence);
                loadSequence.LoadSequenceSummary.PercAllowableFrequencyDip = GetPercAllowableFrequencyDip(loadSequence.LoadSequenceSummary.ExpectedFrequencyDip, loadSequence.LoadSequenceSummary.ProjectAllowableFrequencyDip);
            }
            return loadSummaryLoads;
        }

        private decimal GetAlternatorExpectedVoltageDip(LoadSummaryLoadListDto loadSequence, SolutionSummaryRecommendedProductDto recommendedProductDetail)
        {
            var alternatorDetail = _generatorAlternator.GetAlternatorDetail(_alternatorRepository.GetSingle(a => a.ID == recommendedProductDetail.AlternatorID));

            return (GetVdipFromKVA(loadSequence.LoadSequenceSummary.StartingKVA, alternatorDetail) /
                Convert.ToInt16(recommendedProductDetail.ParallelQuantityList.FirstOrDefault(p => p.ID == recommendedProductDetail.ParallelQuantityID).Value));
        }

        private decimal GetExpectedFrequencyDip(LoadSummaryLoadListDto loadSequence, SolutionSummaryRecommendedProductDto recommendedProductDetail, SolutionSetup solutionSetup)
        {
            var quantity = Convert.ToInt16(recommendedProductDetail.ParallelQuantityList.FirstOrDefault(p => p.ID == recommendedProductDetail.ParallelQuantityID).Value);
            var generatorDetail = _generatorAlternator.GetGeneratorAlternatorDetailByID((int)recommendedProductDetail.GeneratorID, solutionSetup, quantity);

            return GetFDipFromKW(loadSequence.LoadSequenceSummary.StartingKW, generatorDetail);
        }

        private decimal GetEngineExpectedVoltageDip(LoadSummaryLoadListDto loadSequence, SolutionSummaryRecommendedProductDto recommendedProductDetail, decimal expectedFrequencyDip,
            SolutionSetup solutionSetup, decimal? voltsHertzMultiplier)
        {
            return (expectedFrequencyDip / Convert.ToDecimal(solutionSetup.Frequency.Value)) * (decimal)voltsHertzMultiplier;
        }

        private decimal GetProjectAllowableVoltageDip(LoadSummaryLoadsDto loadSummaryLoads, LoadSummaryLoadListDto loadSequence)
        {
            decimal vdip = -1;
            foreach (var sequence in loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                if (_sequenceRepository.GetSingle(s => s.ID == sequence.SequenceID).Ordinal < _sequenceRepository.GetSingle(s => s.ID == loadSequence.SequenceID).Ordinal)
                {
                    if (vdip == -1 || (sequence.LoadSequenceSummary.VDipPerc / 100) < vdip)
                        vdip = (sequence.LoadSequenceSummary.VDipPerc / 100);
                }
                else
                {
                    break;
                }
            }
            if (vdip == -1 || (loadSequence.LoadSequenceSummary.VDipPerc / 100) < vdip)
                vdip = (loadSequence.LoadSequenceSummary.VDipPerc / 100);

            return vdip;
        }

        private decimal GetPercAllowableVoltageDip(decimal engineExpectedVoltageDip, decimal projectAllowableVoltageDip)
        {
            if (projectAllowableVoltageDip > 0)
                return engineExpectedVoltageDip / projectAllowableVoltageDip;
            else
                return 0;
        }

        private decimal GetProjectAllowableFrequencyDip(LoadSummaryLoadsDto loadSummaryLoads, LoadSummaryLoadListDto loadSequence)
        {
            decimal fdip = -1;
            foreach (var sequence in loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                if (_sequenceRepository.GetSingle(s => s.ID == sequence.SequenceID).Ordinal < _sequenceRepository.GetSingle(s => s.ID == loadSequence.SequenceID).Ordinal)
                {
                    if (fdip == -1 || sequence.LoadSequenceSummary.FDipHertz < fdip)
                        fdip = sequence.LoadSequenceSummary.FDipHertz;
                }
                else
                {
                    break;
                }
            }
            if (fdip == -1 || loadSequence.LoadSequenceSummary.FDipHertz < fdip)
                fdip = loadSequence.LoadSequenceSummary.FDipHertz;

            return fdip;
        }

        private decimal GetPercAllowableFrequencyDip(decimal expectedFrequencyDip, decimal projectAllowableFrequencyDip)
        {
            if (projectAllowableFrequencyDip > 0)
                return expectedFrequencyDip / projectAllowableFrequencyDip;
            else
                return 0;
        }

        private LoadSummaryLoadListDto GetVdipLoadSequence()
        {
            decimal perc = -1;
            LoadSummaryLoadListDto sequence = null;
            foreach (var loadSequence in _loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                if (loadSequence.LoadSequenceSummary.PercAllowableVoltageDip > perc)
                {
                    perc = loadSequence.LoadSequenceSummary.PercAllowableVoltageDip;
                    sequence = loadSequence;
                }
            }
            return sequence;
        }

        private LoadSummaryLoadListDto GetFdipLoadSequence()
        {
            decimal perc = -1;
            LoadSummaryLoadListDto sequence = null;
            foreach (var loadSequence in _loadSummaryLoads.ListOfLoadSummaryLoadList)
            {
                if (loadSequence.LoadSequenceSummary.PercAllowableFrequencyDip > perc)
                {
                    perc = loadSequence.LoadSequenceSummary.PercAllowableFrequencyDip;
                    sequence = loadSequence;
                }
            }
            return sequence;
        }

        #endregion

        #endregion

        #endregion
    }
}
