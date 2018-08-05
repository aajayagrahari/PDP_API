using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Models;
using PowerDesignPro.Data.Framework.Interface;
using NSubstitute;
using PowerDesignPro.BusinessProcessors.Processors;
using System.Collections.Generic;
using System.Linq;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.BusinessProcessors.Interface;

namespace PowerDesignPro.Test
{
    [TestClass]
    public class TestSolutionLoadProcessor
    {
        private SolutionLoadProcessor _solutionLoadProcessor;

        private IEntityBaseRepository<LoadDefaults> _loadDefaultsRepository;
        private IEntityBaseRepository<BasicLoad> _basicLoadRepository;
        private IMapper<BasicLoad, BasicLoadDto> _basicLoadEntityToBasicLoadDtoMapper;
        private IMapper<LoadDefaults, BasicLoadDto> _loadDefaultsEntityToBasicLoadDtoMapper;
        private IMapper<BasicLoadDto, BasicLoad> _addBasicLoadDtoToEntityMapper;
        private IEntityBaseRepository<SolutionSetup> _solutionSetupRepository;
        private IEntityBaseRepository<Solution> _solutionRepository;

        private IMapper<BasicLoad, LoadDefaultDto> _basicLoadEntityToLoadDefaultDtoMapper;
        private IMapper<LoadDefaults, LoadDefaultDto> _loadDefaultsEntityToLoadDefaultDtoMapper;

        private IEntityBaseRepository<ACLoad> _acLoadRepository;
        private IMapper<ACLoad, ACLoadDto> _acLoadEntityToacLoadDtoMapper;
        private IMapper<LoadDefaults, ACLoadDto> _loadDefaultsEntityToACLoadDtoMapper;
        private IMapper<ACLoadDto, ACLoad> _addAcLoadDtoToEntityMapper;

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

        private IPickList _pickList;

        [TestInitialize]
        public void Init()
        {
            _loadDefaultsRepository = Substitute.For<IEntityBaseRepository<LoadDefaults>>();
            _basicLoadRepository = Substitute.For<IEntityBaseRepository<BasicLoad>>();
            _solutionSetupRepository = Substitute.For<IEntityBaseRepository<SolutionSetup>>();
            _solutionRepository = Substitute.For<IEntityBaseRepository<Solution>>();
            _basicLoadEntityToBasicLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<BasicLoad, BasicLoadDto>>();
            _basicLoadEntityToLoadDefaultDtoMapper = Substitute.ForPartsOf<AutoMapper<BasicLoad, LoadDefaultDto>>();
            _loadDefaultsEntityToBasicLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, BasicLoadDto>>();
            _loadDefaultsEntityToLoadDefaultDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, LoadDefaultDto>>();
            _addBasicLoadDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<BasicLoadDto, BasicLoad>>();
            _acLoadRepository = Substitute.For<IEntityBaseRepository<ACLoad>>();
            _acLoadEntityToacLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<ACLoad, ACLoadDto>>();
            _loadDefaultsEntityToACLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, ACLoadDto>>();
            _addAcLoadDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<ACLoadDto, ACLoad>>();
            _lightingLoadRepository = Substitute.For<IEntityBaseRepository<LightingLoad>>();
            _lightingLoadEntityToLightingLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<LightingLoad, LightingLoadDto>>();
            _loadDefaultsEntityToLightingLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, LightingLoadDto>>();
            _addLightingLoadDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<LightingLoadDto, LightingLoad>>();
            _upsLoadRepository = Substitute.For<IEntityBaseRepository<UPSLoad>>();
            _upsLoadEntityToUpsLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<UPSLoad, UPSLoadDto>>();
            _loadDefaultsEntityToUpsLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, UPSLoadDto>>();
            _addUpsLoadDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<UPSLoadDto, UPSLoad>>();
            _welderLoadRepository = Substitute.For<IEntityBaseRepository<WelderLoad>>();
            _welderLoadEntityToWelderLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<WelderLoad, WelderLoadDto>>();
            _loadDefaultsEntityToWelderLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, WelderLoadDto>>();
            _addWelderLoadDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<WelderLoadDto, WelderLoad>>();

            _motorLoadRepository = Substitute.For<IEntityBaseRepository<MotorLoad>>();
            _motorLoadEntityToMotorLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<MotorLoad, MotorLoadDto>>();
            _loadDefaultsEntityToMotorLoadDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, MotorLoadDto>>();
            _addMotorLoadDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<MotorLoadDto, MotorLoad>>();

            _solutionLoadProcessor = Substitute.For<SolutionLoadProcessor>
             (_loadDefaultsRepository,
              _basicLoadRepository,
              _solutionSetupRepository,
              _solutionRepository,
              _basicLoadEntityToBasicLoadDtoMapper,
              _loadDefaultsEntityToBasicLoadDtoMapper,
              _loadDefaultsEntityToLoadDefaultDtoMapper,
              _addBasicLoadDtoToEntityMapper,
              _acLoadRepository,
              _acLoadEntityToacLoadDtoMapper,
              _loadDefaultsEntityToACLoadDtoMapper,
              _addAcLoadDtoToEntityMapper,
              _basicLoadEntityToLoadDefaultDtoMapper,
              _lightingLoadRepository,
              _lightingLoadEntityToLightingLoadDtoMapper,
              _loadDefaultsEntityToLightingLoadDtoMapper,
              _addLightingLoadDtoToEntityMapper,
              _upsLoadRepository,
              _upsLoadEntityToUpsLoadDtoMapper,
              _loadDefaultsEntityToUpsLoadDtoMapper,
              _addUpsLoadDtoToEntityMapper,
              _welderLoadRepository,
              _welderLoadEntityToWelderLoadDtoMapper,
              _loadDefaultsEntityToWelderLoadDtoMapper,
              _addWelderLoadDtoToEntityMapper,
              _motorLoadRepository,
              _motorLoadEntityToMotorLoadDtoMapper,
              _loadDefaultsEntityToMotorLoadDtoMapper,
              _addMotorLoadDtoToEntityMapper,
              _pickList);
        }

        #region SolutionBasicLoadProcessor
        [TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        [DataRow(6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForBasicLoad_LoadNotFound(int loadID, string userName)
        {
            var searchBasicLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID
            };

            _loadDefaultsRepository.
                AllIncluding(x => x.HarmonicDeviceType).
                Where(l => l.LoadID == searchBasicLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList());

            var result = _solutionLoadProcessor.GetLoadDetailsForBasicLoad(searchBasicLoadRequestDto, userName);
            _loadDefaultsEntityToBasicLoadDtoMapper.AddMap(Arg.Any<LoadDefaults>()).DidNotReceive();
        }

        //[TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        //[DataRow(1, "Test_Username", 0)]
        //public void GetLoadDetailsForBasicLoad_LoadFound(int loadID, string userName, int? basicLoadID = 0)
        //{
        //    var searchBasicLoadRequestDto = new SearchBaseLoadRequestDto
        //    {
        //        ID = basicLoadID,
        //        LoadID = loadID
        //    };

        //    _loadDefaultsRepository.
        //     AllIncluding(x => x.HarmonicDeviceType).
        //     Where(l => l.LoadID == searchBasicLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList());

        //    _solutionSetupRepository.GetAll(x => x.SolutionID == searchBasicLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

        //    var result = _solutionLoadProcessor.GetLoadDetailsForBasicLoad(searchBasicLoadRequestDto, userName);
        //    Assert.AreEqual(result.LoadID, loadID);
        //}

        [TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        [DataRow(1, 6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForBasicLoad_BasicLoadNotFound(int loadID, int basicLoadID, string userName)
        {
            var searchBasicLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = basicLoadID
            };

            _loadDefaultsRepository.
             AllIncluding(x => x.HarmonicDeviceType).
             Where(l => l.LoadID == searchBasicLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList());

            _basicLoadRepository.
                GetSingle(x => x.ID == searchBasicLoadRequestDto.ID).
                ReturnsForAnyArgs(BasicLoadList().FirstOrDefault(x => x.ID == searchBasicLoadRequestDto.ID));

            var result = _solutionLoadProcessor.GetLoadDetailsForBasicLoad(searchBasicLoadRequestDto, userName);
            _basicLoadEntityToBasicLoadDtoMapper.AddMap(Arg.Any<BasicLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        [DataRow(1, 1, "Test_Username")]
        public void GetLoadDetailsForBasicLoad_BasicLoadFound(int loadID, int basicLoadID, string userName)
        {
            var searchBasicLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = basicLoadID
            };

            _loadDefaultsRepository.
             AllIncluding(x => x.HarmonicDeviceType).
             Where(l => l.LoadID == searchBasicLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList());

            _basicLoadRepository.
                GetSingle(x => x.ID == searchBasicLoadRequestDto.ID).
                ReturnsForAnyArgs(BasicLoadList().FirstOrDefault(x => x.ID == searchBasicLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchBasicLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());


            var result = _solutionLoadProcessor.GetLoadDetailsForBasicLoad(searchBasicLoadRequestDto, userName);
            Assert.AreEqual(result.ID, basicLoadID);
        }

        [TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 0, "Test_SolutionLoad_1")]
        public void SaveSolutionLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var basicLoadDto = new BasicLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var addedBasicLoad = new BasicLoad
            {
                ID = 1,
                Description = descriptionAdd
            };

            _basicLoadRepository.Add(Arg.Any<BasicLoad>()).Returns(addedBasicLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionBasicLoad(basicLoadDto, userID, userName);
            Assert.AreEqual(addedBasicLoad.ID, actualResult.ID);
        }

        [TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 1, "Test_SolutionLoad_1")]
        public void UpdateSolutionLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var basicLoadDto = new BasicLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var updateBasicLoad = new BasicLoad
            {
                ID = ID,
                Description = "Test_SolutionLoad_Updated"
            };

            _basicLoadRepository.Find(ID).ReturnsForAnyArgs(BasicLoadList().FirstOrDefault(x => x.ID == basicLoadDto.ID));
            _basicLoadRepository.Update(Arg.Any<BasicLoad>()).Returns(updateBasicLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionBasicLoad(basicLoadDto, userID, userName);
            Assert.AreEqual(updateBasicLoad.Description, actualResult.Description);
        }

        [TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        [DataRow(1, 6, (int)SolutionLoadFamilyEnum.Basic, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolutionLoad_BasicLoadNotFound(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _basicLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(BasicLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            _basicLoadRepository.Delete(Arg.Any<BasicLoad>().DidNotReceive());
        }

        [TestCategory("SolutionBasicLoadProcessor"), TestMethod]
        [DataRow(1, 1, (int)SolutionLoadFamilyEnum.Basic, "UID123")]
        public void DeleteSolutionLoad_BasicLoadSuccessfully(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _basicLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(BasicLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var deleted = false;
            _basicLoadRepository.When(x => x.Delete(Arg.Any<BasicLoad>())).Do(r =>
            {
                deleted = true;
            });

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            Assert.AreEqual(deleted, actualResult);
        }

        #endregion

        #region SolutionLoadProcessor
        [TestCategory("SolutionLoadProcessor"), TestMethod]
        [DataRow(1, 1, 7, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolutionLoad_LoadFamilyNotFound(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            _loadDefaultsRepository.Delete(Arg.Any<LoadDefaults>().DidNotReceive());
        }

        #endregion

        #region SolutionACLoadProcessor
        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow(1, 6, (int)SolutionLoadFamilyEnum.AC, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolutionLoad_ACLoadNotFound(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _acLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(AcLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            _acLoadRepository.Delete(Arg.Any<ACLoad>().DidNotReceive());
        }

        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow(1, 1, (int)SolutionLoadFamilyEnum.AC, "UID123")]
        public void DeleteSolutionLoad_ACLoadSuccessfully(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _acLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(AcLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var deleted = false;
            _acLoadRepository.When(x => x.Delete(Arg.Any<ACLoad>())).Do(r =>
            {
                deleted = true;
            });

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            Assert.AreEqual(deleted, actualResult);
        }

        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow(6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForACLoad_LoadNotFound(int loadID, string userName)
        {
            var searchAcLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchAcLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchAcLoadRequestDto.LoadID));

            var result = _solutionLoadProcessor.GetLoadDetailsForACLoad(searchAcLoadRequestDto, userName);
            _loadDefaultsEntityToACLoadDtoMapper.AddMap(Arg.Any<LoadDefaults>()).DidNotReceive();
        }

        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow(1, "Test_Username", 0)]
        public void GetLoadDetailsForACLoad_LoadFound(int loadID, string userName, int? acLoadID = 0)
        {
            var searchAcLoadRequestDto = new SearchBaseLoadRequestDto
            {
                ID = acLoadID,
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchAcLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchAcLoadRequestDto.LoadID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchAcLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            _acLoadRepository.GetAll(x => x.LoadID == searchAcLoadRequestDto.LoadID && x.SolutionID == searchAcLoadRequestDto.SolutionID).ReturnsForAnyArgs(AcLoadList());

            var result = _solutionLoadProcessor.GetLoadDetailsForACLoad(searchAcLoadRequestDto, userName);
            Assert.AreEqual(result.LoadID, loadID);
        }

        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow(1, 6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForACLoad_ACLoadNotFound(int loadID, int acLoadID, string userName)
        {
            var searchAcLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = acLoadID
            };

            _loadDefaultsRepository.
                GetSingle(l => l.LoadID == searchAcLoadRequestDto.LoadID).
                ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchAcLoadRequestDto.LoadID));

            _acLoadRepository.
                GetSingle(x => x.ID == searchAcLoadRequestDto.ID).
                ReturnsForAnyArgs(AcLoadList().FirstOrDefault(x => x.ID == searchAcLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchAcLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForACLoad(searchAcLoadRequestDto, userName);
            _acLoadEntityToacLoadDtoMapper.AddMap(Arg.Any<ACLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow(1, 1, "Test_Username")]
        public void GetLoadDetailsForACLoad_AcLoadFound(int loadID, int acLoadID, string userName)
        {
            var searchAcLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = acLoadID
            };

            _loadDefaultsRepository.
                 GetSingle(l => l.LoadID == searchAcLoadRequestDto.LoadID).
                 ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchAcLoadRequestDto.LoadID));

            _acLoadRepository.
                GetSingle(x => x.ID == searchAcLoadRequestDto.ID).
                ReturnsForAnyArgs(AcLoadList().FirstOrDefault(x => x.ID == searchAcLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchAcLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForACLoad(searchAcLoadRequestDto, userName);
            Assert.AreEqual(result.ID, acLoadID);
        }

        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 0, "Test_SolutionLoad_1")]
        public void SaveSolutionACLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var acLoadDto = new ACLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var addedAcLoad = new ACLoad
            {
                ID = 1,
                Description = descriptionAdd
            };

            _acLoadRepository.Add(Arg.Any<ACLoad>()).Returns(addedAcLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionACLoad(acLoadDto, userID, userName);
            Assert.AreEqual(addedAcLoad.ID, actualResult.ID);
        }

        [TestCategory("SolutionACLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 1, "Test_SolutionLoad_1")]
        public void UpdateSolutionAcLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var acLoadDto = new ACLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var UpdatedAcLoad = new ACLoad
            {
                ID = ID,
                Description = "Test_SolutionLoad_Updated"
            };

            _acLoadRepository.Find(ID).ReturnsForAnyArgs(AcLoadList().FirstOrDefault(x => x.ID == acLoadDto.ID));
            _acLoadRepository.Update(Arg.Any<ACLoad>()).Returns(UpdatedAcLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionACLoad(acLoadDto, userID, userName);
            Assert.AreEqual(UpdatedAcLoad.Description, actualResult.Description);
        }
        #endregion

        #region SolutionLightingLoadProcessor
        [TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        [DataRow(1, 6, (int)SolutionLoadFamilyEnum.Lighting, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolutionLoad_LightingLoadNotFound(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _lightingLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(LightingLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            _lightingLoadRepository.Delete(Arg.Any<LightingLoad>().DidNotReceive());
        }

        [TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        [DataRow(1, 1, (int)SolutionLoadFamilyEnum.Lighting, "UID123")]
        public void DeleteSolutionLoad_LightingLoadSuccessfully(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _lightingLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(LightingLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var deleted = false;
            _lightingLoadRepository.When(x => x.Delete(Arg.Any<LightingLoad>())).Do(r =>
            {
                deleted = true;
            });

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            Assert.AreEqual(deleted, actualResult);
        }

        [TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        [DataRow(6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForLightingLoad_LoadNotFound(int loadID, string userName)
        {
            var searchLightingLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchLightingLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchLightingLoadRequestDto.LoadID));

            var result = _solutionLoadProcessor.GetLoadDetailsForLightingLoad(searchLightingLoadRequestDto, userName);
            _loadDefaultsEntityToLightingLoadDtoMapper.AddMap(Arg.Any<LoadDefaults>()).DidNotReceive();
        }

        //[TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        //[DataRow(1, "Test_Username", 0)]
        //public void GetLoadDetailsForLightingLoad_LoadFound(int loadID, string userName, int? lightingLoadID = 0)
        //{
        //    var searchLightingLoadRequestDto = new SearchBaseLoadRequestDto
        //    {
        //        ID = lightingLoadID,
        //        LoadID = loadID
        //    };

        //    _loadDefaultsRepository.GetSingle(l => l.LoadID == searchLightingLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchLightingLoadRequestDto.LoadID));

        //    _solutionSetupRepository.GetAll(x => x.SolutionID == searchLightingLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

        //    _lightingLoadRepository.GetAll(x => x.LoadID == searchLightingLoadRequestDto.LoadID && x.SolutionID == searchLightingLoadRequestDto.SolutionID).ReturnsForAnyArgs(LightingLoadList());

        //    var result = _solutionLoadProcessor.GetLoadDetailsForLightingLoad(searchLightingLoadRequestDto, userName);
        //    Assert.AreEqual(result.LoadID, loadID);
        //}

        [TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        [DataRow(1, 6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForLightingLoad_LightingLoadNotFound(int loadID, int lightingLoadID, string userName)
        {
            var searchLightingLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = lightingLoadID
            };

            _loadDefaultsRepository.
                GetSingle(l => l.LoadID == searchLightingLoadRequestDto.LoadID).
                ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchLightingLoadRequestDto.LoadID));

            _lightingLoadRepository.
                GetSingle(x => x.ID == searchLightingLoadRequestDto.ID).
                ReturnsForAnyArgs(LightingLoadList().FirstOrDefault(x => x.ID == searchLightingLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchLightingLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForLightingLoad(searchLightingLoadRequestDto, userName);
            _lightingLoadEntityToLightingLoadDtoMapper.AddMap(Arg.Any<LightingLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        [DataRow(1, 1, "Test_Username")]
        public void GetLoadDetailsForLightingLoad_LightingLoadFound(int loadID, int lightingLoadID, string userName)
        {
            var searchLightingLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = lightingLoadID
            };

            _loadDefaultsRepository.
                 GetSingle(l => l.LoadID == searchLightingLoadRequestDto.LoadID).
                 ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchLightingLoadRequestDto.LoadID));

            _lightingLoadRepository.
                GetSingle(x => x.ID == searchLightingLoadRequestDto.ID).
                ReturnsForAnyArgs(LightingLoadList().FirstOrDefault(x => x.ID == searchLightingLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchLightingLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForLightingLoad(searchLightingLoadRequestDto, userName);
            Assert.AreEqual(result.ID, lightingLoadID);
        }

        [TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 0, "Test_SolutionLoad_1")]
        public void SaveSolutionLightingLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var lightingLoadDto = new LightingLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var addedLightingLoad = new LightingLoad
            {
                ID = 1,
                Description = descriptionAdd
            };

            _lightingLoadRepository.Add(Arg.Any<LightingLoad>()).Returns(addedLightingLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionLightingLoad(lightingLoadDto, userID, userName);
            Assert.AreEqual(addedLightingLoad.ID, actualResult.ID);
        }

        [TestCategory("SolutionLightingLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 1, "Test_SolutionLoad_1")]
        public void UpdateSolutionLightingLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var lightingLoadDto = new LightingLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var UpdatedLightingLoad = new LightingLoad
            {
                ID = ID,
                Description = "Test_SolutionLoad_Updated"
            };

            _lightingLoadRepository.Find(ID).ReturnsForAnyArgs(LightingLoadList().FirstOrDefault(x => x.ID == lightingLoadDto.ID));
            _lightingLoadRepository.Update(Arg.Any<LightingLoad>()).Returns(UpdatedLightingLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionLightingLoad(lightingLoadDto, userID, userName);
            Assert.AreEqual(UpdatedLightingLoad.Description, actualResult.Description);
        }

        #endregion

        #region SolutionUPSLoadProcessor
        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow(1, 6, (int)SolutionLoadFamilyEnum.UPS, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolutionLoad_UpsLoadNotFound(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _upsLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(UpsLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            _upsLoadRepository.Delete(Arg.Any<UPSLoad>().DidNotReceive());
        }

        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow(1, 1, (int)SolutionLoadFamilyEnum.UPS, "UID123")]
        public void DeleteSolutionLoad_UpsLoadSuccessfully(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _upsLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(UpsLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var deleted = false;
            _upsLoadRepository.When(x => x.Delete(Arg.Any<UPSLoad>())).Do(r =>
            {
                deleted = true;
            });

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            Assert.AreEqual(deleted, actualResult);
        }

        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow(6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForUpsLoad_LoadNotFound(int loadID, string userName)
        {
            var searchUpsLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchUpsLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchUpsLoadRequestDto.LoadID));

            var result = _solutionLoadProcessor.GetLoadDetailsForUpsLoad(searchUpsLoadRequestDto, userName);
            _loadDefaultsEntityToUpsLoadDtoMapper.AddMap(Arg.Any<LoadDefaults>()).DidNotReceive();
        }

        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow(1, "Test_Username", 0)]
        public void GetLoadDetailsForUpsLoad_LoadFound(int loadID, string userName, int? upsLoadID = 0)
        {
            var searchUpsLoadRequestDto = new SearchBaseLoadRequestDto
            {
                ID = upsLoadID,
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchUpsLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchUpsLoadRequestDto.LoadID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchUpsLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            _upsLoadRepository.GetAll(x => x.LoadID == searchUpsLoadRequestDto.LoadID && x.SolutionID == searchUpsLoadRequestDto.SolutionID).ReturnsForAnyArgs(UpsLoadList());

            var result = _solutionLoadProcessor.GetLoadDetailsForUpsLoad(searchUpsLoadRequestDto, userName);
            Assert.AreEqual(result.LoadID, loadID);
        }

        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow(1, 6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForUpsLoad_UpsLoadNotFound(int loadID, int upsLoadID, string userName)
        {
            var searchUpsLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = upsLoadID
            };

            _loadDefaultsRepository.
                GetSingle(l => l.LoadID == searchUpsLoadRequestDto.LoadID).
                ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchUpsLoadRequestDto.LoadID));

            _upsLoadRepository.
                GetSingle(x => x.ID == searchUpsLoadRequestDto.ID).
                ReturnsForAnyArgs(UpsLoadList().FirstOrDefault(x => x.ID == searchUpsLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchUpsLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForUpsLoad(searchUpsLoadRequestDto, userName);
            _upsLoadEntityToUpsLoadDtoMapper.AddMap(Arg.Any<UPSLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow(1, 1, "Test_Username")]
        public void GetLoadDetailsForUpsLoad_UpsLoadFound(int loadID, int upsLoadID, string userName)
        {
            var searchUpsLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = upsLoadID
            };

            _loadDefaultsRepository.
                 GetSingle(l => l.LoadID == searchUpsLoadRequestDto.LoadID).
                 ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchUpsLoadRequestDto.LoadID));

            _upsLoadRepository.
                GetSingle(x => x.ID == searchUpsLoadRequestDto.ID).
                ReturnsForAnyArgs(UpsLoadList().FirstOrDefault(x => x.ID == searchUpsLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchUpsLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForUpsLoad(searchUpsLoadRequestDto, userName);
            Assert.AreEqual(result.ID, upsLoadID);
        }

        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 0, "Test_SolutionLoad_1")]
        public void SaveSolutionUpsLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var upsLoadDto = new UPSLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var addedUPSLoad = new UPSLoad
            {
                ID = 1,
                Description = descriptionAdd
            };

            _upsLoadRepository.Add(Arg.Any<UPSLoad>()).Returns(addedUPSLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionUpsLoad(upsLoadDto, userID, userName);
            Assert.AreEqual(addedUPSLoad.ID, actualResult.ID);
        }

        [TestCategory("SolutionUPSLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 1, "Test_SolutionLoad_1")]
        public void UpdateSolutionUpsLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var upsLoadDto = new UPSLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var updatedUPSLoad = new UPSLoad
            {
                ID = ID,
                Description = "Test_SolutionLoad_Updated"
            };

            _upsLoadRepository.Find(ID).ReturnsForAnyArgs(UpsLoadList().FirstOrDefault(x => x.ID == upsLoadDto.ID));
            _upsLoadRepository.Update(Arg.Any<UPSLoad>()).Returns(updatedUPSLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionUpsLoad(upsLoadDto, userID, userName);
            Assert.AreEqual(updatedUPSLoad.Description, actualResult.Description);
        }

        #endregion

        #region SolutionWelderLoadProcessor
        [TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        [DataRow(1, 6, (int)SolutionLoadFamilyEnum.Welder, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolutionLoad_WelderLoadNotFound(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _welderLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(WelderLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            _welderLoadRepository.Delete(Arg.Any<WelderLoad>().DidNotReceive());
        }

        [TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        [DataRow(1, 1, (int)SolutionLoadFamilyEnum.Welder, "UID123")]
        public void DeleteSolutionLoad_WelderLoadSuccessfully(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _welderLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(WelderLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var deleted = false;
            _welderLoadRepository.When(x => x.Delete(Arg.Any<WelderLoad>())).Do(r =>
            {
                deleted = true;
            });

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            Assert.AreEqual(deleted, actualResult);
        }

        [TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        [DataRow(6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForWelderLoad_LoadNotFound(int loadID, string userName)
        {
            var searchWelderLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchWelderLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchWelderLoadRequestDto.LoadID));

            var result = _solutionLoadProcessor.GetLoadDetailsForWelderLoad(searchWelderLoadRequestDto, userName);
            _loadDefaultsEntityToWelderLoadDtoMapper.AddMap(Arg.Any<LoadDefaults>()).DidNotReceive();
        }

        //[TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        //[DataRow(1, "Test_Username", 0)]
        //public void GetLoadDetailsForWelderLoad_LoadFound(int loadID, string userName, int? welderLoadID = 0)
        //{
        //    var searchWelderLoadRequestDto = new SearchBaseLoadRequestDto
        //    {
        //        ID = welderLoadID,
        //        LoadID = loadID
        //    };

        //    _loadDefaultsRepository.GetSingle(l => l.LoadID == searchWelderLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchWelderLoadRequestDto.LoadID));

        //    _solutionSetupRepository.GetAll(x => x.SolutionID == searchWelderLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

        //    _welderLoadRepository.GetAll(x => x.LoadID == searchWelderLoadRequestDto.LoadID && x.SolutionID == searchWelderLoadRequestDto.SolutionID).ReturnsForAnyArgs(WelderLoadList());

        //    var result = _solutionLoadProcessor.GetLoadDetailsForWelderLoad(searchWelderLoadRequestDto, userName);
        //    Assert.AreEqual(result.LoadID, loadID);
        //}

        [TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        [DataRow(1, 6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForWelderLoad_WelderLoadNotFound(int loadID, int welderLoadID, string userName)
        {
            var searchWelderLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = welderLoadID
            };

            _loadDefaultsRepository.
                GetSingle(l => l.LoadID == searchWelderLoadRequestDto.LoadID).
                ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchWelderLoadRequestDto.LoadID));

            _welderLoadRepository.
                GetSingle(x => x.ID == searchWelderLoadRequestDto.ID).
                ReturnsForAnyArgs(WelderLoadList().FirstOrDefault(x => x.ID == searchWelderLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchWelderLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForWelderLoad(searchWelderLoadRequestDto, userName);
            _welderLoadEntityToWelderLoadDtoMapper.AddMap(Arg.Any<WelderLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        [DataRow(1, 1, "Test_Username")]
        public void GetLoadDetailsForWelderLoad_WelderLoadFound(int loadID, int welderLoadID, string userName)
        {
            var searchWelderLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = welderLoadID
            };

            _loadDefaultsRepository.
                 GetSingle(l => l.LoadID == searchWelderLoadRequestDto.LoadID).
                 ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchWelderLoadRequestDto.LoadID));

            _welderLoadRepository.
                GetSingle(x => x.ID == searchWelderLoadRequestDto.ID).
                ReturnsForAnyArgs(WelderLoadList().FirstOrDefault(x => x.ID == searchWelderLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchWelderLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForWelderLoad(searchWelderLoadRequestDto, userName);
            Assert.AreEqual(result.ID, welderLoadID);
        }

        [TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 0, "Test_SolutionLoad_1")]
        public void SaveSolutionWelderLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var welderLoadDto = new WelderLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var addedWelderLoad = new WelderLoad
            {
                ID = 1,
                Description = descriptionAdd
            };

            _welderLoadRepository.Add(Arg.Any<WelderLoad>()).Returns(addedWelderLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionWelderLoad(welderLoadDto, userID, userName);
            Assert.AreEqual(addedWelderLoad.ID, actualResult.ID);
        }

        [TestCategory("SolutionWelderLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 1, "Test_SolutionLoad_1")]
        public void UpdateSolutionWelderLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var welderLoadDto = new WelderLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var updatedWelderLoad = new WelderLoad
            {
                ID = ID,
                Description = "Test_SolutionLoad_Updated"
            };

            _welderLoadRepository.Find(ID).ReturnsForAnyArgs(WelderLoadList().FirstOrDefault(x => x.ID == welderLoadDto.ID));
            _welderLoadRepository.Update(Arg.Any<WelderLoad>()).Returns(updatedWelderLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionWelderLoad(welderLoadDto, userID, userName);
            Assert.AreEqual(updatedWelderLoad.Description, actualResult.Description);
        }

        #endregion

        #region SolutionMotorLoadProcessor
        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow(1, 6, (int)SolutionLoadFamilyEnum.Motor, "UID123")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolutionLoad_MotorLoadNotFound(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _motorLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(MotorLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            _motorLoadRepository.Delete(Arg.Any<MotorLoad>().DidNotReceive());
        }

        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow(1, 1, (int)SolutionLoadFamilyEnum.Motor, "UID123")]
        public void DeleteSolutionLoad_MotorLoadSuccessfully(int solutionID, int solutionLoadID, int loadFamilyID, string userID)
        {
            _motorLoadRepository.
              GetSingle(x => x.ID == solutionLoadID && x.Solution.Project.UserID == userID).
              ReturnsForAnyArgs(MotorLoadList().FirstOrDefault(x => x.ID == solutionLoadID));

            var deleted = false;
            _motorLoadRepository.When(x => x.Delete(Arg.Any<MotorLoad>())).Do(r =>
            {
                deleted = true;
            });

            var actualResult = _solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, userID);
            Assert.AreEqual(deleted, actualResult);
        }

        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow(6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForMotorLoad_LoadNotFound(int loadID, string userName)
        {
            var searchMotorLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchMotorLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchMotorLoadRequestDto.LoadID));

            var result = _solutionLoadProcessor.GetLoadDetailsForMotorLoad(searchMotorLoadRequestDto, userName);
            _loadDefaultsEntityToMotorLoadDtoMapper.AddMap(Arg.Any<LoadDefaults>()).DidNotReceive();
        }

        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow(1, "Test_Username", 0)]
        public void GetLoadDetailsForMotorLoad_LoadFound(int loadID, string userName, int? upsLoadID = 0)
        {
            var searchMotorLoadRequestDto = new SearchBaseLoadRequestDto
            {
                ID = upsLoadID,
                LoadID = loadID
            };

            _loadDefaultsRepository.GetSingle(l => l.LoadID == searchMotorLoadRequestDto.LoadID).ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchMotorLoadRequestDto.LoadID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchMotorLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            _motorLoadRepository.GetAll(x => x.LoadID == searchMotorLoadRequestDto.LoadID && x.SolutionID == searchMotorLoadRequestDto.SolutionID).ReturnsForAnyArgs(MotorLoadList());

            var result = _solutionLoadProcessor.GetLoadDetailsForUpsLoad(searchMotorLoadRequestDto, userName);
            Assert.AreEqual(result.LoadID, loadID);
        }

        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow(1, 6, "Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetLoadDetailsForMotorLoad_MotorLoadNotFound(int loadID, int motorLoadID, string userName)
        {
            var searchMotorLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = motorLoadID
            };

            _loadDefaultsRepository.
                GetSingle(l => l.LoadID == searchMotorLoadRequestDto.LoadID).
                ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchMotorLoadRequestDto.LoadID));

            _motorLoadRepository.
                GetSingle(x => x.ID == searchMotorLoadRequestDto.ID).
                ReturnsForAnyArgs(MotorLoadList().FirstOrDefault(x => x.ID == searchMotorLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchMotorLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForMotorLoad(searchMotorLoadRequestDto, userName);
            _motorLoadEntityToMotorLoadDtoMapper.AddMap(Arg.Any<MotorLoad>()).DidNotReceive();
        }

        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow(1, 1, "Test_Username")]
        public void GetLoadDetailsForMotorLoad_MotorLoadFound(int loadID, int motorLoadID, string userName)
        {
            var searchMotorLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = loadID,
                ID = motorLoadID
            };

            _loadDefaultsRepository.
                 GetSingle(l => l.LoadID == searchMotorLoadRequestDto.LoadID).
                 ReturnsForAnyArgs(LoadDefaultsList().FirstOrDefault(x => x.ID == searchMotorLoadRequestDto.LoadID));

            _motorLoadRepository.
                GetSingle(x => x.ID == searchMotorLoadRequestDto.ID).
                ReturnsForAnyArgs(MotorLoadList().FirstOrDefault(x => x.ID == searchMotorLoadRequestDto.ID));

            _solutionSetupRepository.GetAll(x => x.SolutionID == searchMotorLoadRequestDto.SolutionID).ReturnsForAnyArgs(SolutionSetUpList());

            var result = _solutionLoadProcessor.GetLoadDetailsForMotorLoad(searchMotorLoadRequestDto, userName);
            Assert.AreEqual(result.ID, motorLoadID);
        }

        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 0, "Test_SolutionLoad_1")]
        public void SaveSolutionMotorLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var motorLoadDto = new MotorLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var addedMotorLoad = new MotorLoad
            {
                ID = 1,
                Description = descriptionAdd
            };

            _motorLoadRepository.Add(Arg.Any<MotorLoad>()).Returns(addedMotorLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionMotorLoad(motorLoadDto, userID, userName);
            Assert.AreEqual(addedMotorLoad.ID, actualResult.ID);
        }

        [TestCategory("SolutionMotorLoadProcessor"), TestMethod]
        [DataRow("UID123", "Test_Username", 1, "Test_SolutionLoad_1")]
        public void UpdateSolutionMotorLoad_Successfully(string userID, string userName, int ID, string descriptionAdd)
        {
            var motorLoadDto = new MotorLoadDto
            {
                ID = ID,
                Description = descriptionAdd
            };

            var updatedMotorLoad  = new MotorLoad
            {
                ID = ID,
                Description = "Test_SolutionLoad_Updated"
            };

            _motorLoadRepository.Find(ID).ReturnsForAnyArgs(MotorLoadList().FirstOrDefault(x => x.ID == motorLoadDto.ID));
            _motorLoadRepository.Update(Arg.Any<MotorLoad>()).Returns(updatedMotorLoad);
            var actualResult = _solutionLoadProcessor.SaveSolutionMotorLoad(motorLoadDto, userID, userName);
            Assert.AreEqual(updatedMotorLoad.Description, actualResult.Description);
        }

        #endregion

        #region Load List
        private IQueryable<BasicLoad> BasicLoadList()
        {
            var list = new List<BasicLoad>();
            for (int i = 1; i <= 5; i++)
            {
                var basicLoad = new BasicLoad
                {
                    SolutionID = i,
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

        private IQueryable<LoadDefaults> LoadDefaultsList()
        {
            var list = new List<LoadDefaults>();
            for (int i = 1; i <= 5; i++)
            {
                var loadDefaults = new LoadDefaults
                {
                    ID = i,
                    LoadID = i,
                    ModifiedDateTime = DateTime.UtcNow,
                    RunningPFEditable = true,
                    SizeStartingEditable = true,
                    SizeRunningEditable = true,
                    StartingPFEditable = true,
                    HarmonicTypeEditable = true,
                    Load=new Load
                    {
                        ID = i,
                        Description = "LoadName"
                    },
                    HarmonicDeviceType = new HarmonicDeviceType
                    {
                        ID = i,
                        HarmonicContentID = i
                    }
                };

                list.Add(loadDefaults);
            }

            return list.AsQueryable();
        }

        private IQueryable<SolutionSetup> SolutionSetUpList()
        {
            var list = new List<SolutionSetup>();
            for (int i = 1; i <= 5; i++)
            {
                var solutionSetup = new SolutionSetup
                {
                    SolutionID = i,
                    VoltagePhaseID = i,
                    VoltageNominalID = i,
                    VoltageSpecificID = i,
                    FrequencyID = i,
                    VoltageSpecific = new VoltageSpecific
                    {
                        Value = i.ToString(),
                    },
                    Frequency = new Frequency
                    {
                        Value = i.ToString(),
                    }
                };

                list.Add(solutionSetup);
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
        #endregion
    }
}
