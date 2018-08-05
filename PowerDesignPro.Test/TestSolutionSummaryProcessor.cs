using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDesignPro.BusinessProcessors.Processors;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using NSubstitute;
using PowerDesignPro.Common.CustomException;
using System.Linq;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Data.Models.Loads;
using PowerDesignPro.Data.Models.User;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.BusinessProcessors.Dtos.Project;
using PowerDesignPro.Common;
using System.Configuration;

namespace PowerDesignPro.Test
{
    /// <summary>
    /// Summary description for TestSolutionSummaryProcessor
    /// </summary>
    [TestClass]
    public class TestSolutionSummaryProcessor
    {
        private SolutionSummaryProcessor _solutionSummaryProcessor;

        private IEntityBaseRepository<Project> _projectRepository;

        private IEntityBaseRepository<Solution> _solutionRepository;

        private IEntityBaseRepository<Sequence> _sequenceRepository;

        private IEntityBaseRepository<SolutionSetup> _solutionSetupRepository;

        private IEntityBaseRepository<BasicLoad> _basicLoadRepository;

        private IEntityBaseRepository<ACLoad> _acLoadRepository;

        private IEntityBaseRepository<LightingLoad> _lightingLoadRepository;

        private IEntityBaseRepository<UPSLoad> _upsLoadRepository;

        private IEntityBaseRepository<WelderLoad> _welderLoadRepository;

        private IEntityBaseRepository<MotorLoad> _motorLoadRepository;

        private IEntityBaseRepository<LoadSequence> _loadSequenceRepository;

        private IEntityBaseRepository<FrequencyDip> _frequencyDipRepository;

        private IEntityBaseRepository<VoltageDip> _voltageDipRepository;

        private IEntityBaseRepository<Brand> _brandRepository;

        private IEntityBaseRepository<ProductFamily> _productFamilyRepository;

        private IEntityBaseRepository<Generator> _generatorRepository;

        private IEntityBaseRepository<Alternator> _alternatorRepository;

        private IEntityBaseRepository<RecommendedProduct> _recommendedProductRepository;

        private IEntityBaseRepository<ParallelQuantity> _parallelQuantityRepository;

        private IGeneratorAlternator _generatorAlternator;

        private IPickList _pickListProcessor;

        private ITraceMessage _traceMessageProcessor;

        private IEntityBaseRepository<GasPipingSizingMethod> _gasPipingSizingMethodRepository;

        private IEntityBaseRepository<GasPipingPipeSize> _gasPipingPipeSizeRepository;

        private IEntityBaseRepository<ExhaustPipingPipeSize> _exhaustPipingPipeSizeRepository;

        private IEntityBaseRepository<GasPipingSolution> _gasPipingSolutionRepository;

        private IEntityBaseRepository<ExhaustPipingSolution> _exhaustPipingSolutionRepository;

        private IEntityBaseRepository<HarmonicProfile> _harmonicProfileRepository;

        private IEntityBaseRepository<RequestForQuote> _requestForQuoteRepository;

        private LoadSummaryLoadsDto _loadSummaryLoads;
        private LoadSummaryLoadListDto _loadSummaryLoadListDto;
        private IEnumerable<int> _regulatoryFilters;

        private SolutionSummaryLoadSummaryDto _solutionSummaryLoadSummary;
        private decimal _metricMultiplecationPipeSizeFactor;
        private int _tempRank;
        private decimal _exhaustFlowMetricConversionFactor;
        private decimal _pressureMetricConversionFactor;
        private decimal _meterToFootConversionFactor;
        private IDictionary<string, decimal> _conversionFactorList = new Dictionary<string, decimal>();

        [TestInitialize]
        public void Init()
        {
            _projectRepository = Substitute.For<IEntityBaseRepository<Project>>();
            _solutionRepository = Substitute.For<IEntityBaseRepository<Solution>>();
            _sequenceRepository = Substitute.For<IEntityBaseRepository<Sequence>>();
            _solutionSetupRepository = Substitute.For<IEntityBaseRepository<SolutionSetup>>();
            _basicLoadRepository = Substitute.For<IEntityBaseRepository<BasicLoad>>();
            _acLoadRepository = Substitute.For<IEntityBaseRepository<ACLoad>>();
            _loadSequenceRepository = Substitute.For<IEntityBaseRepository<LoadSequence>>();
            _frequencyDipRepository = Substitute.For<IEntityBaseRepository<FrequencyDip>>();
            _voltageDipRepository = Substitute.For<IEntityBaseRepository<VoltageDip>>();
            _lightingLoadRepository = Substitute.For<IEntityBaseRepository<LightingLoad>>();
            _upsLoadRepository = Substitute.For<IEntityBaseRepository<UPSLoad>>();
            _welderLoadRepository = Substitute.For<IEntityBaseRepository<WelderLoad>>();
            _motorLoadRepository = Substitute.For<IEntityBaseRepository<MotorLoad>>();
            _brandRepository = Substitute.For<IEntityBaseRepository<Brand>>();
            _productFamilyRepository = Substitute.For<IEntityBaseRepository<ProductFamily>>();
            _generatorRepository = Substitute.For<IEntityBaseRepository<Generator>>();
            _alternatorRepository = Substitute.For<IEntityBaseRepository<Alternator>>();
            _recommendedProductRepository = Substitute.For<IEntityBaseRepository<RecommendedProduct>>();
            _parallelQuantityRepository = Substitute.For<IEntityBaseRepository<ParallelQuantity>>();
            _gasPipingSizingMethodRepository = Substitute.For<IEntityBaseRepository<GasPipingSizingMethod>>();
            _gasPipingPipeSizeRepository = Substitute.For<IEntityBaseRepository<GasPipingPipeSize>>();
            _exhaustPipingPipeSizeRepository = Substitute.For<IEntityBaseRepository<ExhaustPipingPipeSize>>();
            _gasPipingSolutionRepository = Substitute.For<IEntityBaseRepository<GasPipingSolution>>();
            _exhaustPipingSolutionRepository = Substitute.For<IEntityBaseRepository<ExhaustPipingSolution>>();
            _harmonicProfileRepository = Substitute.For<IEntityBaseRepository<HarmonicProfile>>();
            _requestForQuoteRepository = Substitute.For<IEntityBaseRepository<RequestForQuote>>();

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

            _solutionSummaryProcessor = Substitute.For<SolutionSummaryProcessor>
            (
                _projectRepository,
                _solutionRepository,
                _sequenceRepository,
                _solutionSetupRepository,
                _basicLoadRepository,
                _acLoadRepository,
                _lightingLoadRepository,
                _upsLoadRepository,
                _welderLoadRepository,
                _motorLoadRepository,
                _loadSequenceRepository,
                _frequencyDipRepository,
                _voltageDipRepository,
                _productFamilyRepository,
                _generatorRepository,
                _alternatorRepository,
                _recommendedProductRepository,
                _parallelQuantityRepository,
                _brandRepository,
                _generatorAlternator,
                _pickListProcessor,
                _traceMessageProcessor,
                _gasPipingSizingMethodRepository,
                _gasPipingPipeSizeRepository,
                _exhaustPipingPipeSizeRepository,
                _gasPipingSolutionRepository,
                _exhaustPipingSolutionRepository,
                _harmonicProfileRepository,
                _requestForQuoteRepository
            );
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(2, 7, "UID123", "generac")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetSolutionSummary_SolutionNotFound(int projectID, int solutionID, string userID, string brand)
        {
            _solutionRepository.GetSingle(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID)
                             .ReturnsForAnyArgs(SolutionList(true).FirstOrDefault(x => x.ID == solutionID));

            var actualResult = _solutionSummaryProcessor.GetSolutionSummary(projectID, solutionID, userID, brand);
        }

        //######
        //Comment the test because of work in progress when work will complete then complete this test.
        //######

        //[TestCategory("SolutionSummaryProcessor"), TestMethod]
        //[DataRow(1, 1, "UID123", "Test_Solution_1", "generac")]
        //public void GetSolutionSummary_Successfully(int projectID, int solutionID, string userID, string expectedResult, string brand)
        //{
        //    _solutionRepository.GetSingle(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID)
        //                     .ReturnsForAnyArgs(SolutionList(true).FirstOrDefault(x => x.ID == solutionID));

        //    _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID).ReturnsForAnyArgs(SolutionList(true));

        //    _solutionRepository
        //        .AllIncluding(x => x.BasicLoadList,
        //        x => x.MotorLoadList,
        //        x => x.WelderLoadList,
        //        x => x.LightingLoadList,
        //        x => x.ACLoadList,
        //        x => x.UPSLoadList,
        //        x => x.SolutionSetup).ReturnsForAnyArgs(SolutionList(true));

        //    _sequenceRepository.AllIncluding(s => s.SequenceType).ReturnsForAnyArgs(SequenceList());

        //    _parallelQuantityRepository.GetAll().ReturnsForAnyArgs(ParallelQuantityList());

        //    var actualResult = _solutionSummaryProcessor.GetSolutionSummary(projectID, solutionID, userID, brand);
        //    Assert.AreEqual(expectedResult, actualResult.SolutionName);
        //}

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(2, 7, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetSolutionLimits_SolutionNotFound(int projectID, int solutionID, string userID)
        {
            _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID)
                                         .ReturnsForAnyArgs(SolutionList(false));


            var actualResult = _solutionSummaryProcessor.GetSolutionLimits(projectID, solutionID, userID);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, 1, "UID123", "Test Frequency Dip")]
        public void GetSolutionLimits_Successfully(int projectID, int solutionID, string userID, string expectedResult)
        {
            _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID)
                                         .ReturnsForAnyArgs(SolutionList(true));
            _solutionSetupRepository.GetSingle(ss => ss.SolutionID == solutionID).ReturnsForAnyArgs(SolutionSetupList().FirstOrDefault(ss => ss.SolutionID == solutionID));

            var actualResult = _solutionSummaryProcessor.GetSolutionLimits(projectID, solutionID, userID);
            //Assert.AreEqual(expectedResult, actualResult.Fdip);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(9, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadSequence_LoadFamilyNotFound(int loadFamilyID, string userID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID
            };

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
        }

        #region UpdateLoadSequence_BasicLoad
        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(6, (int)SolutionLoadFamilyEnum.Basic, "UID123", 1)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadSequence_BasicLoadNotFound(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            _basicLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(BasicLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            _basicLoadRepository.Update(Arg.Any<BasicLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, (int)SolutionLoadFamilyEnum.Basic, "UID123", 1)]
        public void UpdateLoadSequence_BasicLoadSuccessfully(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            var updatedBasicLoad = new BasicLoad
            {
                SequenceID = loadSequenceID
            };

            _basicLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(BasicLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            _basicLoadRepository.Update(Arg.Any<BasicLoad>()).Returns(updatedBasicLoad);
            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            Assert.AreEqual(true, actualResult);
        }
        #endregion

        #region UpdateLoadSequence_ACLoad
        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(6, (int)SolutionLoadFamilyEnum.AC, "UID123", 1)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadSequence_ACLoadNotFound(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            _acLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(AcLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            _acLoadRepository.Update(Arg.Any<ACLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, (int)SolutionLoadFamilyEnum.AC, "UID123", 1)]
        public void UpdateLoadSequence_ACLoadSuccessfully(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            var updatedACLoad = new ACLoad
            {
                SequenceID = loadSequenceID
            };

            _acLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(AcLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            _acLoadRepository.Update(Arg.Any<ACLoad>()).Returns(updatedACLoad);
            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            Assert.AreEqual(true, actualResult);
        }

        #endregion

        #region UpdateLoadSequence_LightingLoad
        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(6, (int)SolutionLoadFamilyEnum.Lighting, "UID123", 1)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadSequence_LightingLoadNotFound(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            _lightingLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(LightingLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            _lightingLoadRepository.Update(Arg.Any<LightingLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, (int)SolutionLoadFamilyEnum.Lighting, "UID123", 1)]
        public void UpdateLoadSequence_LightingLoadSuccessfully(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            var updatedLightingLoad = new LightingLoad
            {
                SequenceID = loadSequenceID
            };

            _lightingLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(LightingLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            _lightingLoadRepository.Update(Arg.Any<LightingLoad>()).Returns(updatedLightingLoad);
            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            Assert.AreEqual(true, actualResult);
        }

        #endregion

        #region UpdateLoadSequence_UPSLoad
        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(6, (int)SolutionLoadFamilyEnum.UPS, "UID123", 1)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadSequence_UPSLoadNotFound(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            _upsLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(UpsLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            _upsLoadRepository.Update(Arg.Any<UPSLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, (int)SolutionLoadFamilyEnum.UPS, "UID123", 1)]
        public void UpdateLoadSequence_UPSLoadSuccessfully(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            var updatedUPSLoad = new UPSLoad
            {
                SequenceID = loadSequenceID
            };

            _upsLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(UpsLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            _upsLoadRepository.Update(Arg.Any<UPSLoad>()).Returns(updatedUPSLoad);
            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            Assert.AreEqual(true, actualResult);
        }

        #endregion

        #region UpdateLoadSequence_WelderLoad
        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(6, (int)SolutionLoadFamilyEnum.Welder, "UID123", 1)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadSequence_WelderLoadNotFound(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            _welderLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(WelderLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            _welderLoadRepository.Update(Arg.Any<WelderLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, (int)SolutionLoadFamilyEnum.Welder, "UID123", 1)]
        public void UpdateLoadSequence_WelderLoadSuccessfully(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            var updatedWelderLoad = new WelderLoad
            {
                SequenceID = loadSequenceID
            };

            _welderLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(WelderLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            _welderLoadRepository.Update(Arg.Any<WelderLoad>()).Returns(updatedWelderLoad);
            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            Assert.AreEqual(true, actualResult);
        }

        #endregion

        #region UpdateLoadSequence_MotorLoad
        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(6, (int)SolutionLoadFamilyEnum.Motor, "UID123", 1)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadSequence_MotorLoadNotFound(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            _motorLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(MotorLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            _motorLoadRepository.Update(Arg.Any<MotorLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, (int)SolutionLoadFamilyEnum.Motor, "UID123", 1)]
        public void UpdateLoadSequence_MotorLoadSuccessfully(int solutionLoadID, int loadFamilyID, string userID, int loadSequenceID)
        {
            var loadSequenceRequestDto = new LoadSequenceRequestDto
            {
                LoadFamilyID = loadFamilyID,
                SolutionLoadID = solutionLoadID,
                LoadSequenceID = loadSequenceID
            };

            var updatedMotorLoad = new MotorLoad
            {
                SequenceID = loadSequenceID
            };

            _motorLoadRepository.
             GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
             ReturnsForAnyArgs(MotorLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            _motorLoadRepository.Update(Arg.Any<MotorLoad>()).Returns(updatedMotorLoad);
            var actualResult = _solutionSummaryProcessor.UpdateLoadSequence(loadSequenceRequestDto, userID);
            Assert.AreEqual(true, actualResult);
        }

        #endregion

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(2, 7, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadSummaryLoads_SolutionNotFound(int projectID, int solutionID, string userID)
        {
            _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID)
                                         .ReturnsForAnyArgs(SolutionList(false));

            _solutionSetupRepository.GetSingle(ss => ss.SolutionID == solutionID).ReturnsForAnyArgs(SolutionSetupList().FirstOrDefault(ss => ss.SolutionID == solutionID));

            var actualResult = _solutionSummaryProcessor.GetLoadSummaryLoads(projectID, solutionID, userID);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, 1, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadSummaryLoads_SolutionDetailNotFound(int projectID, int solutionID, string userID)
        {
            var expectedResult = new LoadSummaryLoadsDto();

            _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID)
                                         .ReturnsForAnyArgs(SolutionList(true));

            _solutionSetupRepository.GetSingle(ss => ss.SolutionID == solutionID).ReturnsForAnyArgs(SolutionSetupList().FirstOrDefault(ss => ss.SolutionID == solutionID));

            _solutionRepository
                .AllIncluding(x => x.BasicLoadList,
                x => x.MotorLoadList,
                x => x.WelderLoadList,
                x => x.LightingLoadList,
                x => x.ACLoadList,
                x => x.UPSLoadList,
                x => x.SolutionSetup).ReturnsForAnyArgs(SolutionList(false));

            _sequenceRepository.AllIncluding(s => s.SequenceType).ReturnsForAnyArgs(SequenceList());
            var actualResult = _solutionSummaryProcessor.GetLoadSummaryLoads(projectID, solutionID, userID);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, 1, "UID123")]
        public void GetLoadSummaryLoads_Successfully(int projectID, int solutionID, string userID)
        {
            _solutionRepository.GetSingle(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID)
                             .ReturnsForAnyArgs(SolutionList(true).FirstOrDefault(x => x.ID == solutionID));

            _solutionRepository.GetAll(x => x.ID == solutionID && x.ProjectID == projectID && x.Project.UserID == userID).ReturnsForAnyArgs(SolutionList(true));

            _solutionSetupRepository.GetSingle(ss => ss.SolutionID == solutionID).ReturnsForAnyArgs(SolutionSetupList().FirstOrDefault(ss => ss.SolutionID == solutionID));

            _solutionRepository
                .AllIncluding(x => x.BasicLoadList,
                x => x.MotorLoadList,
                x => x.WelderLoadList,
                x => x.LightingLoadList,
                x => x.ACLoadList,
                x => x.UPSLoadList,
                x => x.SolutionSetup).ReturnsForAnyArgs(SolutionList(true));

            _sequenceRepository.AllIncluding(s => s.SequenceType).ReturnsForAnyArgs(SequenceList());
            var actualResult = _solutionSummaryProcessor.GetLoadSummaryLoads(projectID, solutionID, userID);
            Assert.AreEqual(actualResult.ProjectID, projectID);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, 1, 1, "Test_UID")]
        public void UpdateLoadSequenceShedDetail_ShedExist(int projectID, int solutionID, int SequenceID, string userID)
        {
            var loadSequenceShedRequestDto = new LoadSequenceShedRequestDto
            {
                SolutionID = solutionID,
                SequenceID = SequenceID,
                Shed = true
            };

            var addedLoadSequence = new LoadSequence
            {
                ID = 1,
            };

            _loadSequenceRepository.GetSingle(x => x.SolutionID == loadSequenceShedRequestDto.SolutionID && x.SequenceID == loadSequenceShedRequestDto.SequenceID).
               ReturnsForAnyArgs(LoadSequenceList().FirstOrDefault(x => x.SolutionID == loadSequenceShedRequestDto.SolutionID));

            _loadSequenceRepository.Add(Arg.Any<LoadSequence>()).Returns(addedLoadSequence);

            var actualResult = _solutionSummaryProcessor.UpdateLoadSequenceShedDetail(loadSequenceShedRequestDto, userID);

            Assert.AreEqual(actualResult, true);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, 1, 1, "Test_UID")]
        public void UpdateLoadSequenceShedDetail_ShedNotExist(int projectID, int solutionID, int SequenceID, string userID)
        {
            var loadSequenceShedRequestDto = new LoadSequenceShedRequestDto
            {
                SolutionID = solutionID,
                SequenceID = SequenceID,
                Shed = false
            };

            _loadSequenceRepository.GetSingle(x => x.SolutionID == loadSequenceShedRequestDto.SolutionID && x.SequenceID == loadSequenceShedRequestDto.SequenceID).
               ReturnsForAnyArgs(LoadSequenceList().FirstOrDefault(x => x.SolutionID == loadSequenceShedRequestDto.SolutionID));

            var deleted = false;
            _loadSequenceRepository.When(x => x.Delete(Arg.Any<LoadSequence>())).Do(r =>
            {
                deleted = true;
            });


            var actualResult = _solutionSummaryProcessor.UpdateLoadSequenceShedDetail(loadSequenceShedRequestDto, userID);

            Assert.AreEqual(actualResult, true);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, 6, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateFuelTypeForSolution_SolutionNotFound(int projectID, int solutionID, string userID)
        {
            var updateFuelTypeForSolutionRequestDto = new UpdateFuelTypeForSolutionRequestDto
            {
                SolutionID = solutionID,
            };

            _solutionSetupRepository.GetSingle(x => x.SolutionID == updateFuelTypeForSolutionRequestDto.SolutionID && x.Solution.Project.UserID == userID).
                ReturnsForAnyArgs(SolutionSetupList().FirstOrDefault(x => x.SolutionID == updateFuelTypeForSolutionRequestDto.SolutionID));

            var actualResult = _solutionSummaryProcessor.UpdateFuelTypeForSolution(updateFuelTypeForSolutionRequestDto, userID);
        }

        [TestCategory("SolutionSummaryProcessor"), TestMethod]
        [DataRow(1, 1, "UID123")]
        public void UpdateFuelTypeForSolution_Successfully(int projectID, int solutionID, string userID)
        {
            var updateFuelTypeForSolutionRequestDto = new UpdateFuelTypeForSolutionRequestDto
            {
                SolutionID = solutionID,
            };

            var updatedsolutionSetup = new SolutionSetup
            {
                SolutionID = solutionID,
            };

            _solutionSetupRepository.GetSingle(x => x.SolutionID == updateFuelTypeForSolutionRequestDto.SolutionID && x.Solution.Project.UserID == userID).
                ReturnsForAnyArgs(SolutionSetupList().FirstOrDefault(x => x.SolutionID == updateFuelTypeForSolutionRequestDto.SolutionID));
            _solutionSetupRepository.Update(Arg.Any<SolutionSetup>()).Returns(updatedsolutionSetup);
            var actualResult = _solutionSummaryProcessor.UpdateFuelTypeForSolution(updateFuelTypeForSolutionRequestDto, userID);
            Assert.AreEqual(actualResult, true);
        }

        #region Load List
        private IQueryable<Solution> SolutionList(bool Status)
        {
            var list = new List<Solution>();
            if (Status)
            {
                for (int i = 1; i <= 5; i++)
                {
                    var solution = new Solution
                    {
                        ProjectID = 1,
                        ID = i,
                        SolutionName = "Test_Solution_" + i,
                        Description = "Test_Desc",
                        SpecRefNumber = "Test_SpecRefNumber_" + i,
                        Project = new Project
                        {
                            UserID = "UID123",
                            ModifiedDateTime = DateTime.UtcNow,
                            ModifiedBy = "Test_Username"
                        },
                        SolutionSetup = new List<SolutionSetup> {
                        new SolutionSetup{
                             SolutionID = i,
                             LoadSequenceCyclic1= new LoadSequenceCyclic
                             {
                                 Value="1",
                                 ID=i,
                                 Description="LoadSequenceCyclic1"
                             },
                             MaxRunningLoad = new MaxRunningLoad
                             {
                                 Description="Test Max Running",
                                 Value="1",
                                 ID=i
                             },
                             FrequencyDip= new FrequencyDip
                             {
                                 Description="Test Frequency Dip",
                                 Value="1",
                                 ID=i
                             },
                             VoltageDip= new VoltageDip
                             {
                                  Description="Test Voltage Dip",
                                  Value="1",
                                 ID=i
                             },
                             ContinuousAllowableVoltageDistortion= new ContinuousAllowableVoltageDistortion
                             {
                                  Description="Test Continuous Allowable Voltage Distortion",
                                  Value="1",
                                 ID=i
                             },
                             MomentaryAllowableVoltageDistortion= new MomentaryAllowableVoltageDistortion
                             {
                                  Description="Test Momentary Allowable Voltage Distortion",
                                  Value="1",
                                 ID=i
                             },
                             Frequency= new Frequency
                             {
                                  Description="Test Frequency",
                                  Value="1",
                                 ID=i
                             }
                        }
                    },
                        BasicLoadList = new List<BasicLoad> {
                        new BasicLoad{
                            ID=i,
                            LoadID = 1,
                            SequenceID = 1,
                            Description="Basic Load",
                            Quantity=1,
                            SizeRunning=1,
                            RunningPF= new PF
                            {
                                Description="Running PF"
                            },
                            HarmonicContent= new HarmonicContent
                            {
                                Description="HarmonicContent"
                            },
                            Load = new Load
                            {
                                LoadFamilyID=(int)SolutionLoadFamilyEnum.Basic,
                                Description="Basic Load"
                            },
                             VoltageDip = new VoltageDip
                            {
                                Value="1",
                                Description="Test VoltageDip"
                            },
                              FrequencyDip = new FrequencyDip
                            {
                                Value="1",
                                Description="Test FrequencyDip"
                            },
                              VoltageDipUnits = new VoltageDipUnits
                              {
                                  Value = "Volts",
                                  Description= "Volts"
                              },
                              FrequencyDipUnits = new FrequencyDipUnits
                              {
                                  Value = "Percent",
                                  Description = "Percent"
                              },
                            StartingLoadKva=1,
                            StartingLoadKw=1,
                            RunningLoadKva=1,
                            RunningLoadKw=1,
                            THIDContinuous=1,
                            THIDMomentary=1,
                            THIDKva=1,
                            VoltageSpecificID=1,
                            VoltageDipUnitsID=1,
                            FrequencyDipUnitsID=1,
                            VoltageSpecific=new VoltageSpecific
                            {
                                Value="1",
                            }
                        }
                    },
                        ACLoadList = new List<ACLoad> {
                        new ACLoad{
                            ID=i,
                            LoadID = 1,
                            SequenceID = 2,
                            Description="AC Load",
                            Quantity=1,
                            Load = new Load
                            {
                                LoadFamilyID=(int)SolutionLoadFamilyEnum.AC,
                                Description="AC Load"
                            },
                             VoltageDip = new VoltageDip
                            {
                                Value="1",
                                Description="Test VoltageDip"
                            },
                              FrequencyDip = new FrequencyDip
                            {
                                Value="1",
                                Description="Test FrequencyDip"
                            },
                             VoltageDipUnits = new VoltageDipUnits
                              {
                                  Value = "Volts",
                                  Description= "Volts"
                              },
                              FrequencyDipUnits = new FrequencyDipUnits
                              {
                                  Value = "Percent",
                                  Description = "Percent"
                              },
                            StartingLoadKva=1,
                            StartingLoadKw=1,
                            RunningLoadKva=1,
                            RunningLoadKw=1,
                            THIDContinuous=1,
                            THIDMomentary=1,
                            THIDKva=1,
                            VoltageSpecificID=1,
                            VoltageDipUnitsID=1,
                            FrequencyDipUnitsID=1,
                            Cooling=1,
                            Compressors= new Compressors
                            {
                                Value="1",
                                Description="Test AC Compressor"
                            },
                            CoolingUnits= new SizeUnits
                            {
                                Value="1",
                                Description="BTU"
                            },
                            CoolingLoad = new CoolingLoad
                            {
                                Value = "0.5",
                                Description = "0.5 kw/ton"
                            },
                             ReheatLoad= new ReheatLoad
                            {
                                Value="1",
                                Description="AC Reheat"
                            },
                            VoltageSpecific=new VoltageSpecific
                            {
                                Value="1",
                            }
                        }
                    },
                        LightingLoadList = new List<LightingLoad> {
                        new LightingLoad{
                             ID=i,
                            LoadID = 1,
                            SequenceID = 3,
                             Quantity=1,
                            SizeRunning=1,
                            RunningPF= new PF
                            {
                                Description="Running PF"
                            },
                             HarmonicContent= new HarmonicContent
                            {
                                Description="HarmonicContent"
                            },
                            Load = new Load
                            {
                                LoadFamilyID=(int)SolutionLoadFamilyEnum.Lighting,
                                Description="Lighting Load"
                            },
                             VoltageDip = new VoltageDip
                            {
                                Value="1",
                                Description="Test VoltageDip"
                            },
                              FrequencyDip = new FrequencyDip
                            {
                                Value="1",
                                Description="Test FrequencyDip"
                            },
                             VoltageDipUnits = new VoltageDipUnits
                              {
                                  Value = "Volts",
                                  Description= "Volts"
                              },
                              FrequencyDipUnits = new FrequencyDipUnits
                              {
                                  Value = "Percent",
                                  Description = "Percent"
                              },
                            StartingLoadKva=1,
                            StartingLoadKw=1,
                            RunningLoadKva=1,
                            RunningLoadKw=1,
                            THIDContinuous=1,
                            THIDMomentary=1,
                            THIDKva=1,
                            VoltageSpecificID=1,
                            VoltageDipUnitsID=1,
                            FrequencyDipUnitsID=1,
                            VoltageSpecific=new VoltageSpecific
                            {
                                Value="1",
                            }
                        }
                    },
                        WelderLoadList = new List<WelderLoad> {
                        new WelderLoad{
                           ID=i,
                            LoadID = 1,
                            SequenceID = 4,
                              Quantity=1,
                            SizeRunning=1,
                            RunningPF= new PF
                            {
                                Description="Running PF"
                            },
                             HarmonicContent= new HarmonicContent
                            {
                                Description="HarmonicContent"
                            },
                            Load = new Load
                            {
                                LoadFamilyID=(int)SolutionLoadFamilyEnum.Welder,
                                Description="Welder Load"
                            },
                             VoltageDip = new VoltageDip
                            {
                                Value="1",
                                Description="Test VoltageDip"
                            },
                              FrequencyDip = new FrequencyDip
                            {
                                Value="1",
                                Description="Test FrequencyDip"
                            },
                             VoltageDipUnits = new VoltageDipUnits
                              {
                                  Value = "Volts",
                                  Description= "Volts"
                              },
                              FrequencyDipUnits = new FrequencyDipUnits
                              {
                                  Value = "Percent",
                                  Description = "Percent"
                              },
                            StartingLoadKva=1,
                            StartingLoadKw=1,
                            RunningLoadKva=1,
                            RunningLoadKw=1,
                            THIDContinuous=1,
                            THIDMomentary=1,
                            THIDKva=1,
                            VoltageSpecificID=1,
                            VoltageDipUnitsID=1,
                            FrequencyDipUnitsID=1,
                            VoltageSpecific=new VoltageSpecific
                            {
                                Value="1",
                            }
                        }
                    },
                        UPSLoadList = new List<UPSLoad> {
                        new UPSLoad{
                           ID=i,
                            LoadID = 1,
                            SequenceID = 5,
                              Description="Basic Load",
                            Quantity=1,
                            Load = new Load
                            {
                                LoadFamilyID=(int)SolutionLoadFamilyEnum.UPS,
                                Description="UPS Load"
                            },
                             VoltageDip = new VoltageDip
                            {
                                Value="1",
                                Description="Test VoltageDip"
                            },
                              FrequencyDip = new FrequencyDip
                            {
                                Value="1",
                                Description="Test FrequencyDip"
                            },
                              Efficiency = new Efficiency
                            {
                                Value="1",
                                Description="Test Efficiency"
                            },
                              SizeKVAUnits= new SizeUnits
                            {
                                   Value="1",
                                Description="Kva Units"
                            },
                                LoadLevel= new LoadLevel
                            {
                                   Value="1",
                                Description="Load Level"
                            },
                                 ChargeRate= new ChargeRate
                            {
                                   Value="1",
                                Description="Charge rate"
                            },
                                  HarmonicContent= new HarmonicContent
                            {
                                   Value="1",
                                Description="Harmonic Content"
                            },
                             VoltageDipUnits = new VoltageDipUnits
                              {
                                  Value = "Volts",
                                  Description= "Volts"
                              },
                              FrequencyDipUnits = new FrequencyDipUnits
                              {
                                  Value = "Percent",
                                  Description = "Percent"
                              },
                            StartingLoadKva=1,
                            StartingLoadKw=1,
                            SizeKVA=1,
                            RunningLoadKva=1,
                            RunningLoadKw=1,
                            THIDContinuous=1,
                            THIDMomentary=1,
                            THIDKva=1,
                            VoltageSpecificID=1,
                            VoltageDipUnitsID=1,
                            FrequencyDipUnitsID=1,
                            VoltageSpecific=new VoltageSpecific
                            {
                                Value="1",
                            }
                        }
                    },
                        MotorLoadList = new List<MotorLoad> {
                        new MotorLoad{
                           ID=i,
                            LoadID = 1,
                            SequenceID = 5,
                            Quantity=1,
                            SizeRunning=1,
                            Load = new Load
                            {
                                LoadFamilyID=(int)SolutionLoadFamilyEnum.Motor,
                                Description="Motor Load"
                            },
                             VoltageDip = new VoltageDip
                            {
                                Value="1",
                                Description="Test VoltageDip"
                            },
                              FrequencyDip = new FrequencyDip
                            {
                                Value="1",
                                Description="Test FrequencyDip"
                            },
                               SizeRunningUnits = new SizeUnits
                            {
                                Value="1",
                                Description="Test Size Running"
                            },
                                StartingCode = new StartingCode
                            {
                                Value="1",
                                Description="Test Starting Code"
                            },
                                StartingMethod = new StartingMethod
                            {
                                Value="1",
                                Description="Test Starting Method"
                            },
                                 MotorLoadType = new MotorLoadType
                            {
                                Value="1",
                                Description="Test Motor Load Type"
                            },
                                  MotorLoadLevel = new MotorLoadLevel
                            {
                                Value="1",
                                Description="Test Motor Load Level"
                            },
                             VoltageDipUnits = new VoltageDipUnits
                              {
                                  Value = "Volts",
                                  Description= "Volts"
                              },
                              FrequencyDipUnits = new FrequencyDipUnits
                              {
                                  Value = "Percent",
                                  Description = "Percent"
                              },
                            StartingLoadKva=1,
                            StartingLoadKw=1,
                            RunningLoadKva=1,
                            RunningLoadKw=1,
                            THIDContinuous=1,
                            THIDMomentary=1,
                            THIDKva=1,
                            VoltageSpecificID=1,
                            VoltageDipUnitsID=1,
                            FrequencyDipUnitsID=1,
                            VoltageSpecific=new VoltageSpecific
                            {
                                Value="1",
                            }
                        }
                    }
                    };

                    list.Add(solution);
                }
            }

            return list.AsQueryable();
        }

        private IQueryable<SolutionSetup> SolutionSetupList()
        {
            var list = new List<SolutionSetup>();
            for (int i = 1; i <= 5; i++)
            {
                var solutionSetup = new SolutionSetup
                {
                    SolutionID = i,

                    MaxRunningLoad = new MaxRunningLoad
                    {
                        Value = "1",
                        Description = "Test Max Running"
                    },
                    FrequencyDip = new FrequencyDip
                    {
                        Value = "1",
                        Description = "Test Frequency Dip"
                    },
                    VoltageDip = new VoltageDip
                    {
                        Value = "1",
                        Description = "Test Voltage Dip"
                    },
                    ContinuousAllowableVoltageDistortion = new ContinuousAllowableVoltageDistortion
                    {
                        Description = "Test Continuous Allowable Voltage Distortion"
                    },
                    MomentaryAllowableVoltageDistortion = new MomentaryAllowableVoltageDistortion
                    {
                        Description = "Test Momentary Allowable Voltage Distortion"
                    },
                    Frequency = new Frequency
                    {
                        Value = "50",
                        Description = "50 Hz"
                    }
                };

                list.Add(solutionSetup);
            }
            return list.AsQueryable();
        }

        private IQueryable<BasicLoad> BasicLoadList()
        {
            var list = new List<BasicLoad>();
            for (int i = 1; i <= 5; i++)
            {
                var basicLoad = new BasicLoad
                {
                    SolutionID = i,
                    SequenceID = i,
                    ID = i,
                    Description = "Test_SolutionLoad_" + i,
                    ModifiedDateTime = DateTime.UtcNow,
                    HarmonicDeviceType = new HarmonicDeviceType
                    {
                        ID = i,
                        HarmonicContentID = i
                    },
                    Solution = new Solution
                    {
                        Project = new Project
                        {
                            UserID = "UID123"
                        }
                    }

                };

                list.Add(basicLoad);
            }

            return list.AsQueryable();
        }

        private IQueryable<ACLoad> AcLoadList()
        {
            var list = new List<ACLoad>();
            for (int i = 1; i <= 5; i++)
            {
                var acLoad = new ACLoad
                {
                    SolutionID = i,
                    SequenceID = i,
                    ID = i,
                    Description = "Test_SolutionACLoad_" + i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Solution = new Solution
                    {
                        Project = new Project
                        {
                            UserID = "UID123"
                        }
                    }

                };

                list.Add(acLoad);
            }

            return list.AsQueryable();
        }

        private IQueryable<LightingLoad> LightingLoadList()
        {
            var list = new List<LightingLoad>();
            for (int i = 1; i <= 5; i++)
            {
                var lightingLoad = new LightingLoad
                {
                    SolutionID = i,
                    SequenceID = i,
                    ID = i,
                    Description = "Test_SolutionLightingLoad_" + i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Solution = new Solution
                    {
                        Project = new Project
                        {
                            UserID = "UID123"
                        }
                    }

                };

                list.Add(lightingLoad);
            }

            return list.AsQueryable();
        }

        private IQueryable<UPSLoad> UpsLoadList()
        {
            var list = new List<UPSLoad>();
            for (int i = 1; i <= 5; i++)
            {
                var lightingLoad = new UPSLoad
                {
                    SolutionID = i,
                    ID = i,
                    Description = "Test_SolutionUpsLoad_" + i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Solution = new Solution
                    {
                        Project = new Project
                        {
                            UserID = "UID123"
                        }
                    }
                };
                list.Add(lightingLoad);
            }

            return list.AsQueryable();
        }

        private IQueryable<WelderLoad> WelderLoadList()
        {
            var list = new List<WelderLoad>();
            for (int i = 1; i <= 5; i++)
            {
                var welderLoad = new WelderLoad
                {
                    SolutionID = i,
                    ID = i,
                    Description = "Test_SolutionWelderLoad_" + i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Solution = new Solution
                    {
                        Project = new Project
                        {
                            UserID = "UID123"
                        }
                    }
                };
                list.Add(welderLoad);
            }

            return list.AsQueryable();
        }

        private IQueryable<MotorLoad> MotorLoadList()
        {
            var list = new List<MotorLoad>();
            for (int i = 1; i <= 5; i++)
            {
                var motorLoad = new MotorLoad
                {
                    SolutionID = i,
                    ID = i,
                    Description = "Test_SolutionMotorLoad_" + i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Solution = new Solution
                    {
                        Project = new Project
                        {
                            UserID = "UID123"
                        }
                    }
                };
                list.Add(motorLoad);
            }

            return list.AsQueryable();
        }

        private IQueryable<Sequence> SequenceList()
        {
            var list = new List<Sequence>();
            for (int i = 1; i <= 5; i++)
            {
                var sequence = new Sequence
                {
                    ID = i,
                    Description = "Test_Sequence_" + i,
                    Value = "cyclic 1",
                    SequenceType = new SequenceType
                    {
                        Description = "Test_SequenceType_" + i,
                        ID = i,
                        Value = "1",
                    }
                };

                list.Add(sequence);
            }
            return list.AsQueryable();
        }

        private IQueryable<ParallelQuantity> ParallelQuantityList()
        {
            var list = new List<ParallelQuantity>();
            for (int i = 1; i <= 5; i++)
            {
                var parallelQuantity = new ParallelQuantity
                {
                    ID = i,
                    Ordinal = i,
                    Description = "Test_parallelQuantity_" + i,
                    Value = "Test_parallelQuantity_1",
                };

                list.Add(parallelQuantity);
            }
            return list.AsQueryable();
        }

        private IQueryable<LoadSequence> LoadSequenceList()
        {
            var list = new List<LoadSequence>();
            for (int i = 1; i <= 5; i++)
            {
                var loadSequence = new LoadSequence
                {
                    SolutionID = i,
                    SequenceID = i,
                    ID = i
                };

                list.Add(loadSequence);
            }
            return list.AsQueryable();
        }
        #endregion
    }

}
