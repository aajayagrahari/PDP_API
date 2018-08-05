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
    public class TestAdminProcessor
    {
        private AdminProcessor _adminProcessor;

        private IEntityBaseRepository<Generator> _generatorRepository;

        private IEntityBaseRepository<GeneratorAvailableVoltage> _generatorAvailableVoltageRepository;

        private IEntityBaseRepository<GeneratorRegulatoryFilter> _generatorRegulatoryFilterRepository;

        private IEntityBaseRepository<GeneratorAvailableAlternator> _generatorAvailableAlternatorRepository;

        private IEntityBaseRepository<AlternatorFamily> _alternatorFamilyRepository;

        private IMapper<Generator, GeneratorDto> _generatorEntityToGeneratorDtoMapper;

        private IMapper<GeneratorDto, Generator> _addGeneratorDtoToEntityMapper;

        private IEntityBaseRepository<Alternator> _alternatorRepository;

        private IMapper<Alternator, AlternatorDto> _alternatorEntityToAlternatorDtoMapper;

        private IMapper<AlternatorDto, Alternator> _addAlternatorDtoToEntityMapper;

        private IEntityBaseRepository<ProductFamily> _productFamilyRepository;

        private IMapper<ImportGensetDataRequestDto, Generator> _addImportGensetDataRequestDtoToGeneratorEntityMapper;

        private IEntityBaseRepository<RegulatoryFilter> _regulatoryFilterRepository;

        private IEntityBaseRepository<Frequency> _frequencyRepository;

        private IEntityBaseRepository<FuelType> _fuelTypeRepository;

        private IMapper<ImportAlternatorDataRequestDto, Alternator> _addImportAlternatorDataRequestDtoToAlternatorEntityMapper;

        private IEntityBaseRepository<VoltagePhase> _voltagePhaseRepository;

        private IEntityBaseRepository<VoltageNominal> _voltageNominalRepository;

        private IEntityBaseRepository<Brand> _brandRepository;

        private IEntityBaseRepository<SolutionApplication> _solutionApplicationRepository;

        private IEntityBaseRepository<Documentation> _documentationRepository;

        private IEntityBaseRepository<HarmonicDeviceType> _harmonicDeviceTypeRepository;

        private IMapper<HarmonicDeviceType, HarmonicDeviceTypeDto> _harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper;

        private IMapper<HarmonicDeviceTypeDto, HarmonicDeviceType> _addHarmonicDeviceDtoToEntityMapper;

        [TestInitialize]
        public void Init()
        {
            _generatorRepository = Substitute.For<IEntityBaseRepository<Generator>>();
            _generatorAvailableVoltageRepository = Substitute.For<IEntityBaseRepository<GeneratorAvailableVoltage>>();
            _generatorRegulatoryFilterRepository = Substitute.For<IEntityBaseRepository<GeneratorRegulatoryFilter>>();
            _generatorAvailableAlternatorRepository = Substitute.For<IEntityBaseRepository<GeneratorAvailableAlternator>>();
            _alternatorFamilyRepository = Substitute.For<IEntityBaseRepository<AlternatorFamily>>();
            _generatorEntityToGeneratorDtoMapper = Substitute.ForPartsOf<AutoMapper<Generator, GeneratorDto>>();
            _addGeneratorDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<GeneratorDto, Generator>>();
            _alternatorRepository = Substitute.For<IEntityBaseRepository<Alternator>>();
            _alternatorEntityToAlternatorDtoMapper = Substitute.ForPartsOf<AutoMapper<Alternator, AlternatorDto>>();
            _addAlternatorDtoToEntityMapper = Substitute.ForPartsOf<AutoMapper<AlternatorDto, Alternator>>();
            _productFamilyRepository = Substitute.For<IEntityBaseRepository<ProductFamily>>();
            _addImportGensetDataRequestDtoToGeneratorEntityMapper = Substitute.ForPartsOf<AutoMapper<ImportGensetDataRequestDto, Generator>>();
            _regulatoryFilterRepository = Substitute.For<IEntityBaseRepository<RegulatoryFilter>>();
            _frequencyRepository = Substitute.For<IEntityBaseRepository<Frequency>>();
            _fuelTypeRepository = Substitute.For<IEntityBaseRepository<FuelType>>();
            _addImportAlternatorDataRequestDtoToAlternatorEntityMapper = Substitute.ForPartsOf<AutoMapper<ImportAlternatorDataRequestDto, Alternator>>();
            _voltagePhaseRepository = Substitute.For<IEntityBaseRepository<VoltagePhase>>();
            _voltageNominalRepository = Substitute.For<IEntityBaseRepository<VoltageNominal>>();
            _brandRepository = Substitute.For<IEntityBaseRepository<Brand>>();
            _solutionApplicationRepository = Substitute.For<IEntityBaseRepository<SolutionApplication>>();
            _documentationRepository = Substitute.For<IEntityBaseRepository<Documentation>>();
            _harmonicDeviceTypeRepository = Substitute.For<IEntityBaseRepository<HarmonicDeviceType>>();
            _harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper = Substitute.ForPartsOf<AutoMapper<HarmonicDeviceType, HarmonicDeviceTypeDto>>();
            _addHarmonicDeviceDtoToEntityMapper= Substitute.ForPartsOf<AutoMapper<HarmonicDeviceTypeDto, HarmonicDeviceType>>();

            _adminProcessor = Substitute.For<AdminProcessor>
            (
             _generatorRepository,
            _generatorEntityToGeneratorDtoMapper,
            _alternatorRepository,
            _alternatorFamilyRepository,
            _alternatorEntityToAlternatorDtoMapper,
            _addAlternatorDtoToEntityMapper,
            _addGeneratorDtoToEntityMapper,
            _generatorAvailableVoltageRepository,
            _generatorRegulatoryFilterRepository,
            _productFamilyRepository,
            _addImportGensetDataRequestDtoToGeneratorEntityMapper,
            _generatorAvailableAlternatorRepository,
            _regulatoryFilterRepository,
            _frequencyRepository,
            _fuelTypeRepository,
            _addImportAlternatorDataRequestDtoToAlternatorEntityMapper,
            _voltagePhaseRepository,
            _voltageNominalRepository,
            _brandRepository,
            _solutionApplicationRepository,
            _documentationRepository,
            _harmonicDeviceTypeRepository,
            _harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper,
            _addHarmonicDeviceDtoToEntityMapper
             );
        }

        #region Generator 
        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0)]
        public void GetGenerator_IDNotGreaterThenZero(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            var result = _adminProcessor.GetGenerator(searchAdminRequestDto);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetGenerator_GeneratorNotFound(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };
            _generatorRepository.GetSingle(p => p.ID == searchAdminRequestDto.ID && p.Active).
                ReturnsForAnyArgs(LoadGeneratorList().FirstOrDefault(x => x.ID == searchAdminRequestDto.ID));

            var result = _adminProcessor.GetGenerator(searchAdminRequestDto);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1)]
        public void GetGenerator_Successfully(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };
            _generatorRepository.GetSingle(p => p.ID == searchAdminRequestDto.ID && p.Active).
                ReturnsForAnyArgs(LoadGeneratorList().FirstOrDefault(x => x.ID == searchAdminRequestDto.ID));

            var result = _adminProcessor.GetGenerator(searchAdminRequestDto);
            Assert.AreEqual(result.ID, id);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0, "Test_ModelDesc","TestUID")]
        public void AddGeneratorDetail_Successfully(int ID, string descriptionAdd,string userID)
        {
            var generatorDto = new GeneratorDto
            {
                ID = ID,
                ModelDescription = descriptionAdd,
                SelectedVoltageNominalList = new List<PickListDto> { new PickListDto { ID = 1, } },
                SelectedRegulatoryFilterList = new List<PickListDto> { new PickListDto { ID = 1, } },
                SelectedAlternatorList = new List<PickListDto> { new PickListDto { ID = 1, } }
            };

            var addedGenerator = new Generator
            {
                ID = 1,
                ModelDescription = descriptionAdd
            };

            _generatorRepository.Add(Arg.Any<Generator>()).Returns(addedGenerator);
            var actualResult = _adminProcessor.SaveGeneratorDetail(generatorDto, userID);
            Assert.AreEqual(addedGenerator.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "Test_ModelDesc", "TestUID")]
        public void UpdateGeneratorDetail_Successfully(int ID, string descriptionAdd, string userID)
        {
            var generatorDto = new GeneratorDto
            {
                ID = ID,
                ModelDescription = descriptionAdd,
                SelectedVoltageNominalList = new List<PickListDto> { new PickListDto { ID = 1, } },
                SelectedRegulatoryFilterList = new List<PickListDto> { new PickListDto { ID = 1, } },
                SelectedAlternatorList = new List<PickListDto> { new PickListDto { ID = 1, } }
            };

            var updatedGenerator = new Generator
            {
                ID = ID,
                ModelDescription = descriptionAdd
            };

            _generatorRepository.Find(ID).ReturnsForAnyArgs(LoadGeneratorList().FirstOrDefault(x => x.ID == generatorDto.ID));
            _generatorRepository.Update(Arg.Any<Generator>()).Returns(updatedGenerator);
            var actualResult = _adminProcessor.SaveGeneratorDetail(generatorDto, userID);
            Assert.AreEqual(updatedGenerator.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "Test_ModelDesc", "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateGeneratorDetail_GeneratorNotFound(int ID, string descriptionAdd, string userID)
        {
            var generatorDto = new GeneratorDto
            {
                ID = ID,
                ModelDescription = descriptionAdd,
                SelectedVoltageNominalList = new List<PickListDto> { new PickListDto { ID = 1, } },
                SelectedRegulatoryFilterList = new List<PickListDto> { new PickListDto { ID = 1, } },
                SelectedAlternatorList = new List<PickListDto> { new PickListDto { ID = 1, } }
            };
            
            var actualResult = _adminProcessor.SaveGeneratorDetail(generatorDto, userID);
            _generatorRepository.Update(Arg.Any<Generator>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteGenerator_GeneratorNotFound(int ID, string userID)
        {
            var generatorDto = new GeneratorDto
            {
                ID = ID
            };

            _generatorRepository.
              GetSingle(x => x.ID == generatorDto.ID).
              ReturnsForAnyArgs(LoadGeneratorList().FirstOrDefault(x => x.ID == generatorDto.ID));
          
            var actualResult = _adminProcessor.DeleteGenerator(generatorDto.ID, userID);
            _generatorRepository.Update(Arg.Any<Generator>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "TestUID")]
        public void DeleteGenerator_Successfully(int ID, string userID)
        {
            var generatorDto = new GeneratorDto
            {
                ID = ID
            };

            var updatedGenerator = new Generator
            {
                ID = ID,
                Active = false
            };

            _generatorRepository.
            GetSingle(x => x.ID == generatorDto.ID).
            ReturnsForAnyArgs(LoadGeneratorList().FirstOrDefault(x => x.ID == generatorDto.ID));
            _generatorRepository.Update(Arg.Any<Generator>()).Returns(updatedGenerator);
            var actualResult = _adminProcessor.DeleteGenerator(generatorDto.ID, userID);
            Assert.AreEqual(actualResult, true);
        }
        #endregion

        #region Alternator
        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetAlternator_IDNotGreaterThenZero(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            var result = _adminProcessor.GetAlternator(searchAdminRequestDto);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetAlternator_NotFound(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            var result = _adminProcessor.GetAlternator(searchAdminRequestDto);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1)]
        public void GetAlternator_Successfully(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            _alternatorRepository.GetSingle(p => p.ID == searchAdminRequestDto.ID && p.Active).
             ReturnsForAnyArgs(LoadAlternatorList().FirstOrDefault(x => x.ID == searchAdminRequestDto.ID));

            var result = _adminProcessor.GetAlternator(searchAdminRequestDto);
            Assert.AreEqual(result.ID, id);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0, "Test_ModelDesc")]
        public void AddAlternatorDetail_Successfully(int ID, string descriptionAdd)
        {
            var alternatorDto = new AlternatorDto
            {
                ID = ID,
                ModelDescription = descriptionAdd,
            };

            var addedAlternator = new Alternator
            {
                ID = 1,
                ModelDescription = descriptionAdd
            };

            _alternatorRepository.Add(Arg.Any<Alternator>()).Returns(addedAlternator);
            var actualResult = _adminProcessor.SaveAlternatorDetail(alternatorDto);
            Assert.AreEqual(addedAlternator.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "Test_ModelDesc")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateAlternatorDetail_AlternatorNotFound(int ID, string descriptionAdd)
        {
            var alternatorDto = new AlternatorDto
            {
                ID = ID,
                ModelDescription = descriptionAdd,
            };

            var actualResult = _adminProcessor.SaveAlternatorDetail(alternatorDto);
            _alternatorRepository.Update(Arg.Any<Alternator>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "Test_ModelDesc")]
        public void UpdateAlternatorDetail_Successfully(int ID, string descriptionAdd)
        {
            var alternatorDto = new AlternatorDto
            {
                ID = ID,
                ModelDescription = descriptionAdd,
            };

            var updatedAlternator = new Alternator
            {
                ID = ID,
                ModelDescription = descriptionAdd
            };

            _alternatorRepository.Find(ID).ReturnsForAnyArgs(LoadAlternatorList().FirstOrDefault(x => x.ID == alternatorDto.ID));
            _alternatorRepository.Update(Arg.Any<Alternator>()).Returns(updatedAlternator);
            var actualResult = _adminProcessor.SaveAlternatorDetail(alternatorDto);
            Assert.AreEqual(updatedAlternator.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteAlternator_AlternatorNotFound(int ID, string userID)
        {
            var alternatorDto = new AlternatorDto
            {
                ID = ID
            };

            _alternatorRepository.
              GetSingle(x => x.ID == alternatorDto.ID).
              ReturnsForAnyArgs(LoadAlternatorList().FirstOrDefault(x => x.ID == alternatorDto.ID));

            var actualResult = _adminProcessor.DeleteAlternator(alternatorDto.ID, userID);
            _alternatorRepository.Update(Arg.Any<Alternator>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "TestUID")]
        public void DeleteAlternator_Successfully(int ID, string userID)
        {
            var alternatorDto = new AlternatorDto
            {
                ID = ID
            };


            var updatedAlternator = new Alternator
            {
                ID = ID,
                Active = false
            };

            _alternatorRepository.
            GetSingle(x => x.ID == alternatorDto.ID).
            ReturnsForAnyArgs(LoadAlternatorList().FirstOrDefault(x => x.ID == alternatorDto.ID));
            _alternatorRepository.Update(Arg.Any<Alternator>()).Returns(updatedAlternator);
            var actualResult = _adminProcessor.DeleteAlternator(alternatorDto.ID, userID);
            Assert.AreEqual(actualResult, true);
        }
        #endregion

        #region Alternator Family
        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0, "Test_Desc")]
        public void AddAlternatorFamilyDetail_Successfully(int ID, string descriptionAdd)
        {
            var alternatorFamilyDto = new AlternatorFamilyDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var addedAlternatorFamily = new AlternatorFamily
            {
                ID = 1,
                Description = descriptionAdd
            };

            _alternatorFamilyRepository.GetAll().ReturnsForAnyArgs(LoadAlternatorFamilyList());

            _alternatorFamilyRepository.Add(Arg.Any<AlternatorFamily>()).Returns(addedAlternatorFamily);
            var actualResult = _adminProcessor.SaveAlternatorFamily(alternatorFamilyDto);
            Assert.AreEqual(addedAlternatorFamily.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "Test_Desc")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateAlternatorFamilyDetail_AlternatorFamilyNotFound(int ID, string descriptionAdd)
        {
            var alternatorFamilyDto = new AlternatorFamilyDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var actualResult = _adminProcessor.SaveAlternatorFamily(alternatorFamilyDto);
            _alternatorFamilyRepository.Update(Arg.Any<AlternatorFamily>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "Test_ModelDesc")]
        public void UpdateAlternatorFamilyDetail_Successfully(int ID, string descriptionAdd)
        {
            var alternatorFamilyDto = new AlternatorFamilyDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var updatedAlternatorFamily = new AlternatorFamily
            {
                ID = ID,
                Description = descriptionAdd,
            };

            _alternatorFamilyRepository.Find(ID).ReturnsForAnyArgs(LoadAlternatorFamilyList().FirstOrDefault(x => x.ID == alternatorFamilyDto.ID));
            _alternatorFamilyRepository.Update(Arg.Any<AlternatorFamily>()).Returns(updatedAlternatorFamily);
            var actualResult = _adminProcessor.SaveAlternatorFamily(alternatorFamilyDto);
            Assert.AreEqual(updatedAlternatorFamily.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteAlternatorFamily_AlternatorNotFound(int ID, string userID)
        {
            var alternatorFamilyDto = new AlternatorFamilyDto
            {
                ID = ID
            };

            _alternatorFamilyRepository.
              GetSingle(x => x.ID == alternatorFamilyDto.ID).
              ReturnsForAnyArgs(LoadAlternatorFamilyList().FirstOrDefault(x => x.ID == alternatorFamilyDto.ID));

            var actualResult = _adminProcessor.DeleteAlternatorFamily(alternatorFamilyDto.ID, userID);
            _alternatorFamilyRepository.Update(Arg.Any<AlternatorFamily>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "TestUID")]
        public void DeleteAlternatorFamily_Successfully(int ID, string userID)
        {
            var alternatorFamilyDto = new AlternatorFamilyDto
            {
                ID = ID
            };


            var updatedAlternatorFamily = new AlternatorFamily
            {
                ID = ID,
                Active = false
            };

            _alternatorFamilyRepository.
            GetSingle(x => x.ID == alternatorFamilyDto.ID).
            ReturnsForAnyArgs(LoadAlternatorFamilyList().FirstOrDefault(x => x.ID == alternatorFamilyDto.ID));
            _alternatorFamilyRepository.Update(Arg.Any<AlternatorFamily>()).Returns(updatedAlternatorFamily);
            var actualResult = _adminProcessor.DeleteAlternatorFamily(alternatorFamilyDto.ID, userID);
            Assert.AreEqual(actualResult, true);
        }
        #endregion

        #region Product Family
        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0)]
        public void GetProductFamily_IdEqualsZero(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            var result = _adminProcessor.GetProductFamily(searchAdminRequestDto);
            Assert.AreEqual(result.Id, id);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetProductFamily_NotFound(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            _productFamilyRepository.GetSingle(p => p.ID == searchAdminRequestDto.ID).
                ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == searchAdminRequestDto.ID));

            var result = _adminProcessor.GetProductFamily(searchAdminRequestDto);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1)]
        public void GetProductFamily_Successfully(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            _productFamilyRepository.GetSingle(p => p.ID == searchAdminRequestDto.ID).
                ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == searchAdminRequestDto.ID));

            var result = _adminProcessor.GetProductFamily(searchAdminRequestDto);
            Assert.AreEqual(result.Id, id);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0, "Test_Desc")]
        public void AddProductFamilyDetail_Successfully(int ID, string descriptionAdd)
        {
            var productFamilyDto = new ProductFamilyDto
            {
                Id = ID,
                Description = descriptionAdd,
            };

            var addedProductFamily = new ProductFamily
            {
                ID = 1,
                Description = descriptionAdd
            };

            _productFamilyRepository.GetAll().ReturnsForAnyArgs(LoadProductFamilyList());

            _productFamilyRepository.Add(Arg.Any<ProductFamily>()).Returns(addedProductFamily);
            var actualResult = _adminProcessor.SaveProductFamily(productFamilyDto);
            Assert.AreEqual(addedProductFamily.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "Test_Desc")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateProductFamilyDetail_ProductFamilyNotFound(int ID, string descriptionAdd)
        {
            var productFamilyDto = new ProductFamilyDto
            {
                Id = ID,
                Description = descriptionAdd,
            };

            _productFamilyRepository.Find(productFamilyDto.Id).ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == productFamilyDto.Id));

            var actualResult = _adminProcessor.SaveProductFamily(productFamilyDto);
            _productFamilyRepository.Update(Arg.Any<ProductFamily>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "Test_ModelDesc")]
        public void UpdateProductFamilyDetail_SuccessfullyWhenPriorityEqual(int ID, string descriptionAdd)
        {
            var productFamilyDto = new ProductFamilyDto
            {
                Id = ID,
                Description = descriptionAdd,
                Priority = 1
            };


            var updatedProductFamily = new ProductFamily
            {
                ID = ID,
                Description = descriptionAdd,
            };

            _productFamilyRepository.Find(productFamilyDto.Id).ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == productFamilyDto.Id));
            _productFamilyRepository.Update(Arg.Any<ProductFamily>()).Returns(updatedProductFamily);
            var actualResult = _adminProcessor.SaveProductFamily(productFamilyDto);
            Assert.AreEqual(updatedProductFamily.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "Test_ModelDesc")]
        public void UpdateProductFamilyDetail_SuccessfullyWhenPriorityEqualandDirectionlessthenZero(int ID, string descriptionAdd)
        {
            var productFamilyDto = new ProductFamilyDto
            {
                Id = ID,
                Description = descriptionAdd,
                Priority = 2
            };


            var updatedProductFamily = new ProductFamily
            {
                ID = ID,
                Description = descriptionAdd,
            };

            _productFamilyRepository.Find(productFamilyDto.Id).ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == productFamilyDto.Id));

            _productFamilyRepository.GetAll().ReturnsForAnyArgs(LoadProductFamilyList());

            _productFamilyRepository.Update(Arg.Any<ProductFamily>()).Returns(updatedProductFamily);
            var actualResult = _adminProcessor.SaveProductFamily(productFamilyDto);
            Assert.AreEqual(updatedProductFamily.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(5, "Test_ModelDesc")]
        public void UpdateProductFamilyDetail_SuccessfullyWhenPriorityEqualandDirectionnotlessthenZero(int ID, string descriptionAdd)
        {
            var productFamilyDto = new ProductFamilyDto
            {
                Id = ID,
                Description = descriptionAdd,
                Priority = 2
            };


            var updatedProductFamily = new ProductFamily
            {
                ID = ID,
                Description = descriptionAdd,
            };

            _productFamilyRepository.Find(productFamilyDto.Id).ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == productFamilyDto.Id));

            _productFamilyRepository.GetAll().ReturnsForAnyArgs(LoadProductFamilyList());

            _productFamilyRepository.Update(Arg.Any<ProductFamily>()).Returns(updatedProductFamily);
            var actualResult = _adminProcessor.SaveProductFamily(productFamilyDto);
            Assert.AreEqual(updatedProductFamily.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteProductFamily_ProductFamilyNotFound(int ID, string userID)
        {
            var productFamilyDto = new ProductFamilyDto
            {
                Id = ID
            };

            _productFamilyRepository.
              GetSingle(x => x.ID == productFamilyDto.Id).
              ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == productFamilyDto.Id));

            var actualResult = _adminProcessor.DeleteProductFamily(productFamilyDto.Id, userID);
            _productFamilyRepository.Update(Arg.Any<ProductFamily>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "TestUID")]
        public void DeleteProductFamily_Successfully(int ID, string userID)
        {
            var productFamilyDto = new ProductFamilyDto
            {
                Id = ID
            };


            var updatedProductFamily = new ProductFamily
            {
                ID = ID,
                Active = false
            };

            _productFamilyRepository.
            GetSingle(x => x.ID == productFamilyDto.Id).
            ReturnsForAnyArgs(LoadProductFamilyList().FirstOrDefault(x => x.ID == productFamilyDto.Id));

            _productFamilyRepository.Update(Arg.Any<ProductFamily>()).Returns(updatedProductFamily);
            var actualResult = _adminProcessor.DeleteProductFamily(productFamilyDto.Id, userID);
            Assert.AreEqual(actualResult, true);
        }
        #endregion

        #region Maintain Market Vertical
        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0, "Test_Desc")]
        public void AddMaintainMarketVertical_Successfully(int ID, string descriptionAdd)
        {
            var solutionApplicationDto = new SolutionApplicationDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var addedSolutionApplication = new SolutionApplication
            {
                ID = 1,
                Description = descriptionAdd
            };

            _solutionApplicationRepository.GetAll().ReturnsForAnyArgs(LoadMarketVerticalList());

            _solutionApplicationRepository.Add(Arg.Any<SolutionApplication>()).Returns(addedSolutionApplication);
            var actualResult = _adminProcessor.SaveMaintainMarketVertical(solutionApplicationDto);
            Assert.AreEqual(addedSolutionApplication.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "Test_Desc")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateMaintainMarketVertical_NotFound(int ID, string descriptionAdd)
        {
            var solutionApplicationDto = new SolutionApplicationDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var actualResult = _adminProcessor.SaveMaintainMarketVertical(solutionApplicationDto);
            _solutionApplicationRepository.Update(Arg.Any<SolutionApplication>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "Test_ModelDesc")]
        public void UpdateMaintainMarketVertical_Successfully(int ID, string descriptionAdd)
        {
            var solutionApplicationDto = new SolutionApplicationDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var updatedSolutionApplication = new SolutionApplication
            {
                ID = ID,
                Description = descriptionAdd,
            };

            _solutionApplicationRepository.Find(ID).ReturnsForAnyArgs(LoadMarketVerticalList().FirstOrDefault(x => x.ID == solutionApplicationDto.ID));
            _solutionApplicationRepository.Update(Arg.Any<SolutionApplication>()).Returns(updatedSolutionApplication);
            var actualResult = _adminProcessor.SaveMaintainMarketVertical(solutionApplicationDto);
            Assert.AreEqual(updatedSolutionApplication.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteMaintainMarketVerticals_NotFound(int ID, string userID)
        {
            var solutionApplicationDto = new SolutionApplicationDto
            {
                ID = ID,
            };

            _solutionApplicationRepository.
              GetSingle(x => x.ID == solutionApplicationDto.ID).
              ReturnsForAnyArgs(LoadMarketVerticalList().FirstOrDefault(x => x.ID == solutionApplicationDto.ID));

            var actualResult = _adminProcessor.DeleteMaintainMarketVerticals(solutionApplicationDto.ID, userID);
            _solutionApplicationRepository.Update(Arg.Any<SolutionApplication>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "TestUID")]
        public void DeleteMaintainMarketVerticals_Successfully(int ID, string userID)
        {
            var solutionApplicationDto = new SolutionApplicationDto
            {
                ID = ID,
            };


            var updatedSolutionApplication = new SolutionApplication
            {
                ID = ID,
                Active = false
            };

            _solutionApplicationRepository.
            GetSingle(x => x.ID == solutionApplicationDto.ID).
            ReturnsForAnyArgs(LoadMarketVerticalList().FirstOrDefault(x => x.ID == solutionApplicationDto.ID));
            _solutionApplicationRepository.Update(Arg.Any<SolutionApplication>()).Returns(updatedSolutionApplication);
            var actualResult = _adminProcessor.DeleteMaintainMarketVerticals(solutionApplicationDto.ID, userID);
            Assert.AreEqual(actualResult, true);
        }
        #endregion

        #region Harmonic Device Type
        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetHarmonicDeviceType_IDNotGreaterThenZero(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            var result = _adminProcessor.GetHarmonicDeviceType(searchAdminRequestDto);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetHarmonicDeviceType_NotFound(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            var result = _adminProcessor.GetHarmonicDeviceType(searchAdminRequestDto);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1)]
        public void GetHarmonicDeviceType_Successfully(int id)
        {
            var searchAdminRequestDto = new SearchAdminRequestDto
            {
                ID = id
            };

            _harmonicDeviceTypeRepository.GetSingle(p => p.ID == searchAdminRequestDto.ID && p.Active).
             ReturnsForAnyArgs(LoadHarmonicDeviceTypeList().FirstOrDefault(x => x.ID == searchAdminRequestDto.ID));

            var result = _adminProcessor.GetHarmonicDeviceType(searchAdminRequestDto);
            Assert.AreEqual(result.ID, id);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(0, "Test_HarmonicDevice_1")]
        public void AddHarmonicDeviceDetail_Successfully(int ID, string descriptionAdd)
        {
            var harmonicDeviceTypeDto = new HarmonicDeviceTypeDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var addedHarmonicDeviceType = new HarmonicDeviceType
            {
                ID = 1,
                Description = descriptionAdd
            };

            _harmonicDeviceTypeRepository.GetAll().ReturnsForAnyArgs(LoadHarmonicDeviceTypeList());

            _harmonicDeviceTypeRepository.Add(Arg.Any<HarmonicDeviceType>()).Returns(addedHarmonicDeviceType);
            var actualResult = _adminProcessor.SaveHarmonicDeviceTypeDetail(harmonicDeviceTypeDto);
            Assert.AreEqual(addedHarmonicDeviceType.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "Test_HarmonicDevice_1")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateHarmonicDeviceDetail_HarmonicNotFound(int ID, string descriptionAdd)
        {
            var harmonicDeviceTypeDto = new HarmonicDeviceTypeDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var actualResult = _adminProcessor.SaveHarmonicDeviceTypeDetail(harmonicDeviceTypeDto);
            _harmonicDeviceTypeRepository.Update(Arg.Any<HarmonicDeviceType>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "Test_HarmonicDevice_1")]
        public void UpdateHarmonicDeviceDetail_Successfully(int ID, string descriptionAdd)
        {
            var harmonicDeviceTypeDto = new HarmonicDeviceTypeDto
            {
                ID = ID,
                Description = descriptionAdd,
            };

            var updatedHarmonicDeviceType = new HarmonicDeviceType
            {
                ID = ID,
                Description = descriptionAdd
            };

            _harmonicDeviceTypeRepository.Find(ID).ReturnsForAnyArgs(LoadHarmonicDeviceTypeList().FirstOrDefault(x => x.ID == harmonicDeviceTypeDto.ID));
            _harmonicDeviceTypeRepository.Update(Arg.Any<HarmonicDeviceType>()).Returns(updatedHarmonicDeviceType);
            var actualResult = _adminProcessor.SaveHarmonicDeviceTypeDetail(harmonicDeviceTypeDto);
            Assert.AreEqual(updatedHarmonicDeviceType.ID, actualResult.ID);
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(6, "TestUID")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteHarmonicDeviceType_HarmonicDeviceNotFound(int ID, string userID)
        {
            var harmonicDeviceTypeDto = new HarmonicDeviceTypeDto
            {
                ID = ID
            };

            _harmonicDeviceTypeRepository.
              GetSingle(x => x.ID == harmonicDeviceTypeDto.ID).
              ReturnsForAnyArgs(LoadHarmonicDeviceTypeList().FirstOrDefault(x => x.ID == harmonicDeviceTypeDto.ID));

            var actualResult = _adminProcessor.DeleteHarmonicDeviceType(harmonicDeviceTypeDto.ID, userID);
            _harmonicDeviceTypeRepository.Update(Arg.Any<HarmonicDeviceType>().DidNotReceive());
        }

        [TestCategory("AdminProcessor"), TestMethod]
        [DataRow(1, "TestUID")]
        public void DeleteHarmonicDevice_Successfully(int ID, string userID)
        {
            var harmonicDeviceTypeDto = new HarmonicDeviceTypeDto
            {
                ID = ID
            };


            var updatedHarmonicDeviceType = new HarmonicDeviceType
            {
                ID = ID,
                Active = false
            };

            _harmonicDeviceTypeRepository.
            GetSingle(x => x.ID == harmonicDeviceTypeDto.ID).
            ReturnsForAnyArgs(LoadHarmonicDeviceTypeList().FirstOrDefault(x => x.ID == harmonicDeviceTypeDto.ID));

            _harmonicDeviceTypeRepository.Update(Arg.Any<HarmonicDeviceType>()).Returns(updatedHarmonicDeviceType);
            var actualResult = _adminProcessor.DeleteHarmonicDeviceType(harmonicDeviceTypeDto.ID, userID);
            Assert.AreEqual(actualResult, true);
        }
        #endregion

        #region MaintainMarketVerticalList
        private IQueryable<SolutionApplication> LoadMarketVerticalList()
        {
            var list = new List<SolutionApplication>();
            for (int i = 1; i <= 5; i++)
            {
                var solutionApplication = new SolutionApplication
                {
                    ID = i,
                    Description = "Desc_" + i,
                };

                list.Add(solutionApplication);
            }

            return list.AsQueryable();
        }
        #endregion

        #region ProductFamilyList
        private IQueryable<ProductFamily> LoadProductFamilyList()
        {
            var list = new List<ProductFamily>();
            for (int i = 1; i <= 5; i++)
            {
                var productFamily = new ProductFamily
                {
                    ID = i,
                    Priority = i
                };

                list.Add(productFamily);
            }

            return list.AsQueryable();
        }
        #endregion

        #region AlternatorFamilyList
        private IQueryable<AlternatorFamily> LoadAlternatorFamilyList()
        {
            var list = new List<AlternatorFamily>();
            for (int i = 1; i <= 5; i++)
            {
                var alternatorFamily = new AlternatorFamily
                {
                    ID = i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Active = true
                };

                list.Add(alternatorFamily);
            }

            return list.AsQueryable();
        }
        #endregion

        #region AlternatorList
        private IQueryable<Alternator> LoadAlternatorList()
        {
            var list = new List<Alternator>();
            for (int i = 1; i <= 5; i++)
            {
                var alternator = new Alternator
                {
                    ID = i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Active = true
                };

                list.Add(alternator);
            }

            return list.AsQueryable();
        }
        #endregion

        #region GeneratorList
        private IQueryable<Generator> LoadGeneratorList()
        {
            var list = new List<Generator>();
            for (int i = 1; i <= 5; i++)
            {
                var generator = new Generator
                {
                    ID = i,
                    ModifiedDateTime = DateTime.UtcNow,
                    Active=true
                };

                list.Add(generator);
            }

            return list.AsQueryable();
        }
        #endregion

        #region HarmonicDeviceTypeList
        private IQueryable<HarmonicDeviceType> LoadHarmonicDeviceTypeList()
        {
            var list = new List<HarmonicDeviceType>();
            for (int i = 1; i <= 5; i++)
            {
                var harmonicDeviceType = new HarmonicDeviceType
                {
                    ID = i,
                    Description = "Test_HarmonicDevice_" + i,
                    Value = "Test_HarmonicDevice_" + i,
                    HarmonicContentID = i,
                    StartingMethodID=i,
                    Active = true
                };

                list.Add(harmonicDeviceType);
            }

            return list.AsQueryable();
        }
        #endregion

        #region documentation
        private IQueryable<Documentation> LoadDocumentationList()
        {
            var list = new List<Documentation>();
            for (int i = 1; i <= 5; i++)
            {
                var documentation = new Documentation
                {
                    ID = i,
                    Description = i.ToString()
                };

                list.Add(documentation);
            }

            return list.AsQueryable();
        }
        #endregion
    }
}
