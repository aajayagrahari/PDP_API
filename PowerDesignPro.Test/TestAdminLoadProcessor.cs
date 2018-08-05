using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Processors;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerDesignPro.Test
{
    [TestClass]
    public class TestAdminLoadProcessor
    {
        private AdminLoadProcessor _adminLoadProcessor;

        private IEntityBaseRepository<Load> _loadRepository;

        private IEntityBaseRepository<LoadFamily> _loadFamilyRepository;

        private IEntityBaseRepository<LoadDefaults> _loadDefaultsRepository;

        private IEntityBaseRepository<Project> _projectRepository;

        private IEntityBaseRepository<BasicLoad> _basicLoadRepository;

        private IEntityBaseRepository<ACLoad> _acLoadRepository;

        private IEntityBaseRepository<LightingLoad> _lightingLoadRepository;

        private IEntityBaseRepository<MotorLoad> _motorLoadRepository;

        private IEntityBaseRepository<MotorCalculation> _motorCalculationRepository;

        private IEntityBaseRepository<WelderLoad> _welderLoadRepository;

        private IEntityBaseRepository<UPSLoad> _upsLoadRepository;

        private IMapper<MotorCalculation, MotorCalculationDto> _motorCalculationEntityToMotorCalculationDtoMapper;

        private IMapper<LoadDefaults, AdminLoadDefaultDto> _generatorEntityToGeneratorDtoMapper;

        private IMapper<AdminLoadDefaultDto, LoadDefaults> _addAdminLoadDefaultDtoToEntityMapper;

        [TestInitialize]
        public void Init()
        {
            _loadRepository = Substitute.For<IEntityBaseRepository<Load>>(); 
            _loadFamilyRepository = Substitute.For<IEntityBaseRepository<LoadFamily>>(); 
            _loadDefaultsRepository = Substitute.For<IEntityBaseRepository<LoadDefaults>>();
            _projectRepository = Substitute.For<IEntityBaseRepository<Project>>();
            _basicLoadRepository = Substitute.For<IEntityBaseRepository<BasicLoad>>();
            _acLoadRepository = Substitute.For<IEntityBaseRepository<ACLoad>>();
            _lightingLoadRepository = Substitute.For<IEntityBaseRepository<LightingLoad>>();
            _motorLoadRepository = Substitute.For<IEntityBaseRepository<MotorLoad>>();
            _motorCalculationRepository = Substitute.For<IEntityBaseRepository<MotorCalculation>>();
            _welderLoadRepository = Substitute.For<IEntityBaseRepository<WelderLoad>>();
            _upsLoadRepository = Substitute.For<IEntityBaseRepository<UPSLoad>>();
            _motorCalculationEntityToMotorCalculationDtoMapper = Substitute.ForPartsOf<AutoMapper<MotorCalculation, MotorCalculationDto>>();
            _generatorEntityToGeneratorDtoMapper = Substitute.ForPartsOf<AutoMapper<LoadDefaults, AdminLoadDefaultDto>>();
            _addAdminLoadDefaultDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<AdminLoadDefaultDto, LoadDefaults>>();

            _adminLoadProcessor = Substitute.For<AdminLoadProcessor>
           (
                _loadRepository, 
                _loadFamilyRepository, 
                _loadDefaultsRepository,
                _projectRepository,
                _basicLoadRepository,
                _acLoadRepository,
                _lightingLoadRepository,
                _motorLoadRepository,
                _motorCalculationRepository,
                _welderLoadRepository,
                _upsLoadRepository,
                _motorCalculationEntityToMotorCalculationDtoMapper,
                _generatorEntityToGeneratorDtoMapper, 
                _addAdminLoadDefaultDtoToEntityMapper
           );
        }

        #region Load
        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(0)]
        public void GetDefaultLoadDetails_LoadIDNotGreaterThenZero(int id)
        {
            var searchBaseLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = id
            };

            var result = _adminLoadProcessor.GetDefaultLoadDetails(searchBaseLoadRequestDto);
        }

        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(6)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetDefaultLoadDetails_LoadNotFound(int id)
        {
            var searchBaseLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = id
            };

            _loadDefaultsRepository.GetSingle(p => p.ID == searchBaseLoadRequestDto.ID && p.Active).
                ReturnsForAnyArgs(LoadDefaultList().FirstOrDefault(x => x.ID == searchBaseLoadRequestDto.ID));

            var result = _adminLoadProcessor.GetDefaultLoadDetails(searchBaseLoadRequestDto);
        }

        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(1)]
        public void GetDefaultLoadDetails_Successfully(int id)
        {
            var searchBaseLoadRequestDto = new SearchBaseLoadRequestDto
            {
                LoadID = id
            };

            _loadDefaultsRepository.GetSingle(p => p.LoadID == searchBaseLoadRequestDto.LoadID).
                ReturnsForAnyArgs(LoadDefaultList().FirstOrDefault(x => x.LoadID == searchBaseLoadRequestDto.LoadID));

            var result = _adminLoadProcessor.GetDefaultLoadDetails(searchBaseLoadRequestDto);
            Assert.AreEqual(result.ID, id);
        }

        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(0, "Test_Load","Test_UID")]
        public void AddLoadDefault_Successfully(int loadID, string loadName,string userID)
        {
            var adminLoadDefaultDto = new AdminLoadDefaultDto
            {
                LoadID = loadID,
                LoadName = loadName,
            };

            var addedLoadDefaults = new LoadDefaults
            {
                LoadID = 1,
            };

            _loadRepository.GetAll().ReturnsForAnyArgs(LoadList());

            _loadDefaultsRepository.Add(Arg.Any<LoadDefaults>()).Returns(addedLoadDefaults);
            var actualResult = _adminLoadProcessor.SaveLoadDetail(adminLoadDefaultDto, userID);
            Assert.AreEqual(addedLoadDefaults.ID, actualResult.ID);
        }

        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(6,"Test_UID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateLoadDefault_LoadNotFound(int loadID, string userID)
        {
            var adminLoadDefaultDto = new AdminLoadDefaultDto
            {
                LoadID = loadID,
            };

            var actualResult = _adminLoadProcessor.SaveLoadDetail(adminLoadDefaultDto,userID);
            _loadDefaultsRepository.Update(Arg.Any<LoadDefaults>().DidNotReceive());
        }

        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(1, "Test_UID")]
        public void UpdateLoadDefault_Successfully(int loadID, string userID)
        {
            var adminLoadDefaultDto = new AdminLoadDefaultDto
            {
                LoadID = loadID,
            };

            var updatedLoadDefaults = new LoadDefaults
            {
                LoadID = loadID,
            };

            _loadDefaultsRepository.GetSingle(p => p.LoadID == adminLoadDefaultDto.LoadID).
               ReturnsForAnyArgs(LoadDefaultList().FirstOrDefault(x => x.LoadID == adminLoadDefaultDto.LoadID));

            _loadDefaultsRepository.Update(Arg.Any<LoadDefaults>()).Returns(updatedLoadDefaults);
            var actualResult = _adminLoadProcessor.SaveLoadDetail(adminLoadDefaultDto, userID);
            Assert.AreEqual(updatedLoadDefaults.ID, actualResult.ID);
        }

        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(6, "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteLoadDefault_LoadNotFound(int loadID, string userID)
        {
            var adminLoadDefaultDto = new AdminLoadDefaultDto
            {
                LoadID = loadID,
            };

            _loadDefaultsRepository.
              GetSingle(x => x.LoadID == adminLoadDefaultDto.LoadID).
              ReturnsForAnyArgs(LoadDefaultList().FirstOrDefault(x => x.LoadID == adminLoadDefaultDto.LoadID));

            var actualResult = _adminLoadProcessor.DeleteLoadDefault(adminLoadDefaultDto.LoadID, userID);
            _loadDefaultsRepository.Update(Arg.Any<LoadDefaults>().DidNotReceive());
        }

        [TestCategory("AdminLoadProcessor"), TestMethod]
        [DataRow(1, "TestUID")]
        public void DeleteLoadDefault_Successfully(int loadID, string userID)
        {
            var adminLoadDefaultDto = new AdminLoadDefaultDto
            {
                LoadID = loadID,
            };

            var updatedLoadDefaults = new LoadDefaults
            {
                LoadID = loadID,
                Active = false
            };

            _loadDefaultsRepository.
            GetSingle(x => x.LoadID == adminLoadDefaultDto.LoadID).
            ReturnsForAnyArgs(LoadDefaultList().FirstOrDefault(x => x.LoadID == adminLoadDefaultDto.LoadID));

            _loadDefaultsRepository.Update(Arg.Any<LoadDefaults>()).Returns(updatedLoadDefaults);
            var actualResult = _adminLoadProcessor.DeleteLoadDefault(adminLoadDefaultDto.LoadID, userID);
            Assert.AreEqual(actualResult, true);
        }
        #endregion


        #region Load Defaults List
        private IQueryable<LoadDefaults> LoadDefaultList()
        {
            var list = new List<LoadDefaults>();
            for (int i = 1; i <= 5; i++)
            {
                var loadDefaults = new LoadDefaults
                {
                    ID = i,
                    LoadID = i,
                    Load = new Load
                    {
                        ID = i,
                        Description = "Test",
                        Value = "Test"
                    },
                };

                list.Add(loadDefaults);
            }

            return list.AsQueryable();
        }

        private IQueryable<Load> LoadList()
        {
            var list = new List<Load>();
            for (int i = 1; i <= 5; i++)
            {
                var load = new Load
                {
                    ID = i,
                   Description="Load" + i
                };

                list.Add(load);
            }

            return list.AsQueryable();
        }
        #endregion
    }
}
