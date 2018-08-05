using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Models;
using PowerDesignPro.BusinessProcessors.Dtos;
using NSubstitute;
using PowerDesignPro.BusinessProcessors.Processors;
using System.Linq;
using System.Collections.Generic;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.BusinessProcessors.Mapper.FromDto;

namespace PowerDesignPro.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class TestProjectSolutionProcessor
    {
        /// <summary>
        /// The project solution processor
        /// </summary>
        private ProjectSolutionProcessor _projectSolutionProcessor;

        private IEntityBaseRepository<Project> _projectRepository;
        /// <summary>
        /// The solution repository
        /// </summary>
        private IEntityBaseRepository<Solution> _solutionRepository;
        /// <summary>
        /// The solution setup repository
        /// </summary>
        private IEntityBaseRepository<SolutionSetup> _solutionSetupRepository;
        /// <summary>
        /// The user default solution setup repository
        /// </summary>
        private IEntityBaseRepository<UserDefaultSolutionSetup> _userDefaultSolutionSetupRepository;
        /// <summary>
        /// Shared Project Repository
        /// </summary>
        private IEntityBaseRepository<SharedProject> _sharedProjectRepository;
        /// <summary>
        /// The base solution entity to project solution dto mapper
        /// </summary>
        private IMapper<BaseSolutionSetupEntity, ProjectSolutionDto> _baseSolutionEntityToProjectSolutionDtoMapper;
        /// <summary>
        /// The user default solution setup to base solution setup mapping values dto mapper
        /// </summary>
        private IMapper<UserDefaultSolutionSetup, BaseSolutionSetupMappingValuesDto> _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper;
        /// <summary>
        /// The user default solution setup to global default solution setup dto mapper
        /// </summary>
        private IMapper<UserDefaultSolutionSetup, GlobalDefaultSolutionSetupDto> _userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper;
        /// <summary>
        /// The solution setup to project solution setup response dto mapper
        /// </summary>
        private IMapper<SolutionSetup, BaseSolutionSetupMappingValuesDto> _solutionSetupToProjectSolutionSetupResponseDtoMapper;
        /// <summary>
        /// User Default Solution Setup Dto To Entity mapper
        /// </summary>
        private AutoMapper<UserDefaultSolutionSetupDto, UserDefaultSolutionSetup> _userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper;

        /// <summary>
        /// The solution setup request dto to solution setup entity mapper
        /// </summary>
        private AutoMapper<BaseSolutionSetupMappingValuesDto, SolutionSetup> _solutionSetupRequestDtoToSolutionSetupEntityMapper;

        /// <summary>
        /// The solution request dto to solution entity mapper
        /// </summary>
        private AutoMapper<ProjectSolutionDto, Solution> _solutionRequestDtoToSolutionEntityMapper;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _solutionRepository = Substitute.For<IEntityBaseRepository<Solution>>();
            _projectRepository = Substitute.For<IEntityBaseRepository<Project>>();
            _solutionSetupRepository = Substitute.For<IEntityBaseRepository<SolutionSetup>>();
            _userDefaultSolutionSetupRepository = Substitute.For<IEntityBaseRepository<UserDefaultSolutionSetup>>();
            _sharedProjectRepository = Substitute.For<IEntityBaseRepository<SharedProject>>();
            _baseSolutionEntityToProjectSolutionDtoMapper = Substitute.For<IMapper<BaseSolutionSetupEntity, ProjectSolutionDto>>();
            _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper = Substitute.For<IMapper<UserDefaultSolutionSetup, BaseSolutionSetupMappingValuesDto>>();
            _userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper = Substitute.For<IMapper<UserDefaultSolutionSetup, GlobalDefaultSolutionSetupDto>>();
            _solutionSetupToProjectSolutionSetupResponseDtoMapper = Substitute.For<IMapper<SolutionSetup, BaseSolutionSetupMappingValuesDto>>();
            _solutionSetupRequestDtoToSolutionSetupEntityMapper = Substitute.ForPartsOf<AutoMapper<BaseSolutionSetupMappingValuesDto, SolutionSetup>>();
            _solutionRequestDtoToSolutionEntityMapper = Substitute.ForPartsOf<SolutionRequestDtoToSolutionEntityMapper>();
            _userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper = Substitute.ForPartsOf<AutoMapper<UserDefaultSolutionSetupDto, UserDefaultSolutionSetup>>();

            _projectSolutionProcessor = Substitute.For<ProjectSolutionProcessor>
             (_solutionRepository, 
             _solutionSetupRepository, 
             _userDefaultSolutionSetupRepository,
             _sharedProjectRepository,
             _baseSolutionEntityToProjectSolutionDtoMapper,
             _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper,
             _userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper,
             _solutionSetupToProjectSolutionSetupResponseDtoMapper,
             _solutionSetupRequestDtoToSolutionSetupEntityMapper,
             _solutionRequestDtoToSolutionEntityMapper,
             _userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper,
             _projectRepository);
        }

        /// <summary>
        /// Saves the solution detail save solution exist.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="solutionName">Name of the solution.</param>
        [TestCategory("ProjectSolutionProcessor"), TestMethod]
        [DataRow(1,"UID123", "Test_Solution_1", "Test_UserName")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void SaveSolutionDetail_SaveSolutionExist(int projectId,string userId,string solutionName,string userName)
        {
            _solutionRepository.GetAll(s => s.ProjectID == projectId).ReturnsForAnyArgs(SolutionList());
            var solutionRequest = new ProjectSolutionDto
            {
                SolutionName = solutionName
            };

            var result = _projectSolutionProcessor.SaveSolutionDetail(solutionRequest, userId, userName);
            _solutionRepository.Add(Arg.Any<Solution>()).DidNotReceive();
        }

        /// <summary>
        /// Saves the solution detail save successfully.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <param name="solutionId">The solution identifier.</param>
        [TestCategory("ProjectSolutionProcessor"), TestMethod]
        [DataRow(1, "UID123", "Test_Solution_6", 6, "Test_UserName")]
        public void SaveSolutionDetail_SaveSuccessfully(int projectId, string userId, string solutionName, int solutionId,string userName)
        {
            _solutionRepository.GetAll(p => p.ProjectID == projectId).ReturnsForAnyArgs(SolutionList());

            var solutionRequest = new ProjectSolutionDto
            {
                ProjectID=1,
                SolutionID=0,
                SolutionName = solutionName,
                Description="Test_Desc",
                SpecRefNumber="Test_SpecRefNumber",
                BaseSolutionSetupDto = new BaseSolutionSetupDto {
                    SolutionSetupMappingValuesDto = new BaseSolutionSetupMappingValuesDto {
                        AmbientTemperatureID = 1,
                        ElevationID = 1,
                        VoltagePhaseID = 1,
                        FrequencyID = 1,
                        VoltageNominalID = 1,
                        VoltageSpecificID = 1,
                        UnitsID = 1,
                        MaxRunningLoadID = 1,
                        VoltageDipID = 1,
                        VoltageDipUnitsID = 1,
                        FrequencyDipID = 1,
                        FrequencyDipUnitsID = 1,
                        ContinuousAllowableVoltageDistortionID = 1,
                        MomentaryAllowableVoltageDistortionID = 1,
                        EngineDutyID = 1,
                        FuelTypeID = 1,
                        SolutionApplicationID = 1,
                        EnclosureTypeID = 1,
                        DesiredSoundID = 1,
                        FuelTankID = 1,
                        DesiredRunTimeID = 1,
                        LoadSequenceCyclic1ID = 1,
                        LoadSequenceCyclic2ID = 1,
                        SelectedRegulatoryFilterList = new List<RegulatoryFilterDto> { new RegulatoryFilterDto { Id = 1, } }
                    }
                }
            };

            var addedSolution = new Solution
            {
                ID = solutionId,
                SolutionName = solutionName,
            };

            var updatedProject = new Project
            {
                ModifiedBy = userName,
                ModifiedDateTime = DateTime.UtcNow,
            };

            _projectRepository.Find(solutionRequest.ProjectID).ReturnsForAnyArgs(ProjectList().FirstOrDefault(x => x.ID == solutionRequest.ProjectID));

            _projectRepository.Update(Arg.Any<Project>()).Returns(updatedProject);
            _solutionRepository.Add(Arg.Any<Solution>()).Returns(addedSolution);

            var actualResult = _projectSolutionProcessor.SaveSolutionDetail(solutionRequest, userId, userName);
            Assert.AreEqual(addedSolution.ID, actualResult.SolutionID);
        }

        /// <summary>
        /// Saves the solution detail update solution exist.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userId">The user identifier.</param>
        [TestCategory("ProjectSolutionProcessor"), TestMethod]
        [DataRow(1, "UID123", "Test_UserName")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void SaveSolutionDetail_UpdateSolutionExist(int projectId, string userId,string userName)
        {
            _solutionRepository.GetAll(p => p.ProjectID == projectId).ReturnsForAnyArgs(SolutionList());
            var solutionRequest = new ProjectSolutionDto
            {
                SolutionID=2,
                SolutionName = "Test_Solution_1"
            };

            var result = _projectSolutionProcessor.SaveSolutionDetail(solutionRequest, userId, userName);
            _solutionRepository.Update(Arg.Any<Solution>()).DidNotReceive();
        }

        /// <summary>
        /// Saves the solution detail update successfully.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <param name="solutionId">The solution identifier.</param>
        [TestCategory("ProjectSolutionProcessor"), TestMethod]
        [DataRow(1, "UID123", "Test_Solution_1", 1, "Test_UserName")]
        public void SaveSolutionDetail_UpdateSuccessfully(int projectId, string userId, string solutionName, int solutionId,string userName)
        {
            _solutionRepository.GetAll(p => p.ProjectID == projectId).ReturnsForAnyArgs(SolutionList());

            var solutionRequest = new ProjectSolutionDto
            {
                ProjectID = projectId,
                SolutionID = solutionId,
                SolutionName = solutionName,
                Description = "Test_Desc",
                SpecRefNumber = "Test_SpecRefNumber",
                BaseSolutionSetupDto = new BaseSolutionSetupDto
                {
                    SolutionSetupMappingValuesDto = new BaseSolutionSetupMappingValuesDto
                    {
                        AmbientTemperatureID = 1,
                        ElevationID = 1,
                        VoltagePhaseID = 1,
                        FrequencyID = 1,
                        VoltageNominalID = 1,
                        VoltageSpecificID = 1,
                        UnitsID = 1,
                        MaxRunningLoadID = 1,
                        VoltageDipID = 1,
                        VoltageDipUnitsID = 1,
                        FrequencyDipID = 1,
                        FrequencyDipUnitsID = 1,
                        ContinuousAllowableVoltageDistortionID = 1,
                        MomentaryAllowableVoltageDistortionID = 1,
                        EngineDutyID = 1,
                        FuelTypeID = 1,
                        SolutionApplicationID = 1,
                        EnclosureTypeID = 1,
                        DesiredSoundID = 1,
                        FuelTankID = 1,
                        DesiredRunTimeID = 1,
                        LoadSequenceCyclic1ID = 1,
                        LoadSequenceCyclic2ID = 1,
                        SelectedRegulatoryFilterList = new List<RegulatoryFilterDto> { new RegulatoryFilterDto { Id = 1, } }

                    }
                }
            };

            var updatedSolution = new Solution
            {
                ID = solutionId,
                SolutionName = solutionName,
            };

            _solutionRepository.Find(solutionId).ReturnsForAnyArgs(SolutionList().FirstOrDefault(x => x.ID == solutionRequest.SolutionID));

            _solutionRepository.Update(Arg.Any<Solution>()).Returns(updatedSolution);

            var actualResult = _projectSolutionProcessor.SaveSolutionDetail(solutionRequest, userId, userName);

            Assert.AreEqual(updatedSolution.ID, actualResult.SolutionID);
        }

        /// <summary>
        /// Saves the user default solution setup add successfully.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="Id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [TestCategory("ProjectSolutionProcessor"), TestMethod]
        [DataRow("UID1",1, "Test_UserName")]
        public void SaveUserDefaultSolutionSetup_AddSuccessfully(string userId,int Id,string userName)
        {
            _userDefaultSolutionSetupRepository.GetAll(u => !u.IsGlobalDefaults && u.UserID == userId).ReturnsForAnyArgs(SolutionSetupList(false));

            var addedUserDefaultSolutionSetup = new UserDefaultSolutionSetup
            {
                ID = Id,
                UserID = userId,
            };

            var userDefaultSolutionSetupDto = new UserDefaultSolutionSetupDto
            {
                AmbientTemperatureID=6,
                ElevationID=2,
                VoltagePhaseID = 2,
                FrequencyID = 2,
                VoltageNominalID = 23,
                VoltageSpecificID= 148,
                UnitsID= 1,
                MaxRunningLoadID= 7,
                VoltageDipID= 16,
                VoltageDipUnitsID= 1,
                FrequencyDipID= 1,
                FrequencyDipUnitsID= 1,
                ContinuousAllowableVoltageDistortionID= 4,
                MomentaryAllowableVoltageDistortionID= 4,
                EngineDutyID= 1,
                FuelTypeID= 6,
                SolutionApplicationID= 1,
                EnclosureTypeID= 3,
                DesiredSoundID= 7,
                FuelTankID= 2,
                DesiredRunTimeID= 4,
                LoadSequenceCyclic1ID= 14,
                LoadSequenceCyclic2ID= 14,
                SelectedRegulatoryFilterList = new List<RegulatoryFilterDto> { new RegulatoryFilterDto { Id = 1, } }
            };

            _userDefaultSolutionSetupRepository.Add(Arg.Any<UserDefaultSolutionSetup>()).Returns(addedUserDefaultSolutionSetup);

            var actualResult = _projectSolutionProcessor.SaveUserDefaultSolutionSetup(userDefaultSolutionSetupDto, userId, userName);

            Assert.AreEqual(true, actualResult);
        }

        /// <summary>
        /// Saves the user default solution setup update successfully.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="Id">The identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [TestCategory("ProjectSolutionProcessor"), TestMethod]
        [DataRow("UID1", 1, "Test_UserName")]
        public void SaveUserDefaultSolutionSetup_UpdateSuccessfully(string userId, int Id, string userName)
        {
            _userDefaultSolutionSetupRepository.GetAll(u => !u.IsGlobalDefaults && u.UserID == userId).ReturnsForAnyArgs(SolutionSetupList(true));

            var UpdatedUserDefaultSolutionSetup = new UserDefaultSolutionSetup
            {
                ID = Id,
                UserID = userId,
            };

            var userDefaultSolutionSetupDto = new UserDefaultSolutionSetupDto
            {
                AmbientTemperatureID = 6,
                ElevationID = 2,
                VoltagePhaseID = 2,
                FrequencyID = 2,
                VoltageNominalID = 23,
                VoltageSpecificID = 148,
                UnitsID = 1,
                MaxRunningLoadID = 7,
                VoltageDipID = 16,
                VoltageDipUnitsID = 1,
                FrequencyDipID = 1,
                FrequencyDipUnitsID = 1,
                ContinuousAllowableVoltageDistortionID = 4,
                MomentaryAllowableVoltageDistortionID = 4,
                EngineDutyID = 1,
                FuelTypeID = 6,
                SolutionApplicationID = 1,
                EnclosureTypeID = 3,
                DesiredSoundID = 7,
                FuelTankID = 2,
                DesiredRunTimeID = 4,
                LoadSequenceCyclic1ID = 14,
                LoadSequenceCyclic2ID = 14,
                SelectedRegulatoryFilterList = new List<RegulatoryFilterDto> { new RegulatoryFilterDto { Id = 1, } }
            };

            _userDefaultSolutionSetupRepository.GetSingle(
                u => !u.IsGlobalDefaults && u.UserID == userId).ReturnsForAnyArgs(SolutionSetupList(true).FirstOrDefault(x => x.ID == UpdatedUserDefaultSolutionSetup.ID));

            _userDefaultSolutionSetupRepository.Update(Arg.Any<UserDefaultSolutionSetup>()).Returns(UpdatedUserDefaultSolutionSetup);

            var actualResult = _projectSolutionProcessor.SaveUserDefaultSolutionSetup(userDefaultSolutionSetupDto, userId, userName);

            Assert.AreEqual(true, actualResult);

        }

        /// <summary>
            /// Solutions the list.
            /// </summary>
            /// <returns></returns>
        private IQueryable<Solution> SolutionList()
        {
            var list = new List<Solution>();
            for (int i = 1; i <= 5; i++)
            {
                var solution = new Solution
                {
                    ProjectID = 1,
                    ID = i,
                    SolutionName = "Test_Solution_" + i,
                    Description = "Test_Desc",
                    SpecRefNumber = "Test_SpecRefNumber" + i,
                    Active=true,
                    Project=new Project {
                        ModifiedDateTime =DateTime.UtcNow,
                        ModifiedBy ="Test_Username"
                    },
                    SolutionSetup = new List<SolutionSetup> {
                        new SolutionSetup{
                             SolutionID = i,
                        }
                    }
                };

                list.Add(solution);
            }

            return list.AsQueryable();
        }

        /// <summary>
        /// Solutions the setup list.
        /// </summary>
        /// <param name="statusList">if set to <c>true</c> [status list].</param>
        /// <returns></returns>
        private IQueryable<UserDefaultSolutionSetup> SolutionSetupList(bool statusList)
        {
            var list = new List<UserDefaultSolutionSetup>();
            if(statusList)
            { 
                for (int i = 1; i <= 5; i++)
                {
                    var userDefaultSolutionSetup = new UserDefaultSolutionSetup
                    {
                        ID = i,
                        UserID = "UID" + i,
                        IsGlobalDefaults = true,
                        CreatedDateTime = DateTime.UtcNow,
                    };

                    list.Add(userDefaultSolutionSetup);
                }
            }

            return list.AsQueryable();
        }

        private IQueryable<Project> ProjectList()
        {
            var list = new List<Project>();
            for (int i = 1; i <= 5; i++)
            {
                var project = new Project
                {
                    ProjectName = "Test_Project_" + i,
                    Active = true,
                    UserID = "UID123",
                    ModifiedDateTime = DateTime.UtcNow,
                    ID = i
                };

                list.Add(project);
            }

            return list.AsQueryable();
        }
    }
}
