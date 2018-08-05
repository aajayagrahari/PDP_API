using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDesignPro.BusinessProcessors.Processors;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data;
using PowerDesignPro.BusinessProcessors.Dtos;
using System.Linq;
using System.Collections.Generic;
using NSubstitute;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.BusinessProcessors.Mapper.FromDto;
using PowerDesignPro.Data.Models;

namespace PowerDesignPro.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class TestUserDashboardProcessor
    {
        /// <summary>
        /// The user dashboard processor
        /// </summary>
        private UserDashboardProcessor _userDashboardProcessor;
        /// <summary>
        /// The project repository
        /// </summary>
        private IEntityBaseRepository<Project> _projectRepository;
        /// <summary>
        /// The project entity to dto mapper
        /// </summary>
        private IMapper<Project, DashboardProjectDetailDto> _projectEntityToDtoMapper;
        /// <summary>
        /// The add project dto to entity mapper
        /// </summary>
        private AutoMapper<AddProjectDto, Project> _addProjectDtoToEntityMapper;

        private IEntityBaseRepository<SharedProject> _sharedProjectsRepository;

        private IEntityBaseRepository<SharedProjectSolution> _sharedProjectSolutionRespository;

        private IMapper<SharedProjectsDto, SharedProject> _sharedProjectsEntityToSharedProjectsDtoMapper;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _projectRepository = Substitute.For<IEntityBaseRepository<Project>>();
            _projectEntityToDtoMapper = Substitute.For<AutoMapper<Project, DashboardProjectDetailDto>>();
            _addProjectDtoToEntityMapper = Substitute.ForPartsOf<AddProjectDtoToProjectEntityMapper>();
            _sharedProjectsRepository = Substitute.For<IEntityBaseRepository<SharedProject>>();
            _sharedProjectsEntityToSharedProjectsDtoMapper = Substitute.ForPartsOf<AutoMapper<SharedProjectsDto, SharedProject>>();
            _sharedProjectSolutionRespository = Substitute.For<IEntityBaseRepository<SharedProjectSolution>>();
            _userDashboardProcessor = Substitute.For<UserDashboardProcessor>
              (_projectRepository, _projectEntityToDtoMapper, _addProjectDtoToEntityMapper,
               _sharedProjectsRepository, _sharedProjectSolutionRespository,
              _sharedProjectsEntityToSharedProjectsDtoMapper);
        }

        /// <summary>
        /// Gets the project not found get all.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="userId">The user identifier.</param>
        [TestCategory("UserDashboardProcessor"),TestMethod]
        [DataRow("Test_Project_Not_Found", "UID1234")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetProject_NotFoundGetAll(string  projectName,string userId)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectEmptyList());
            var searchDto = new SearchProjectRequestDto
            {
                ProjectName = projectName,
                UserID = userId
            };
            var result = _userDashboardProcessor.GetProjects(searchDto);
        }

        /// <summary>
        /// Gets the name of the projects by.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="userId">The user identifier.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test_Project_1", "UID123")]
        public void GetProjects_ByName(string projectName, string userId)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var searchDto = new SearchProjectRequestDto
            {
                ProjectName = projectName,
                UserID = userId
            };

            var result = _userDashboardProcessor.GetProjects(searchDto);

            Assert.AreEqual(result.Count(), 1);
        }

        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Woody", "UID123")]
        public void GetProjects_BySolutionName(string solutionName, string userId)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var searchDto = new SearchProjectRequestDto
            {
                ProjectName = string.Empty,
                UserID = userId,
                SolutionName = solutionName,
                CreateDate = string.Empty,
                ModifyDate = string.Empty
            };

            var result = _userDashboardProcessor.GetProjects(searchDto);

            Assert.AreEqual(result.Count(), 0);
        }

        //[TestCategory("UserDashboardProcessor"), TestMethod]
        //[DataRow("1/1/2018", "UID123")]
        //public void GetProjects_ByCreateDate(string createDate, string userId)
        //{
        //    _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
        //    var searchDto = new SearchProjectRequestDto
        //    {
        //        ProjectName = string.Empty,
        //        UserID = userId,
        //        SolutionName = string.Empty,
        //        CreateDate = createDate,
        //        ModifyDate = string.Empty
        //    };

        //    var result = _userDashboardProcessor.GetProjects(searchDto);

        //    Assert.AreEqual(result.Count(), 0);
        //}

        //[TestCategory("UserDashboardProcessor"), TestMethod]
        //[DataRow("1/1/2018", "UID123")]
        //public void GetProjects_ByModifiedDate(string modifyDate, string userId)
        //{
        //    _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
        //    var searchDto = new SearchProjectRequestDto
        //    {
        //        ProjectName = string.Empty,
        //        UserID = userId,
        //        SolutionName = string.Empty,
        //        CreateDate = string.Empty,
        //        ModifyDate = modifyDate
        //    };

        //    var result = _userDashboardProcessor.GetProjects(searchDto);

        //    Assert.AreEqual(result.Count(), 0);
        //}

        /// <summary>
        /// Gets the projects by user identifier.
        /// </summary>
        /// <param name="daysOld">The days old.</param>
        /// <param name="userId">The user identifier.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow(1, "UID123")]
        public void GetProjects_ByUserId(int daysOld,string userId)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());

            var searchDto = new SearchProjectRequestDto
            {
                DaysOld = daysOld,
                UserID = userId
            };

            var result = _userDashboardProcessor.GetProjects(searchDto);

            Assert.AreEqual(result.Count(), 5);
        }

        /// <summary>
        /// Gets the project not found by name and user identifier.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="userId">The user identifier.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test_Project_Not_Found", "UID123")] 
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetProject_NotFoundByNameAndUserId(string projectName, string userId)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var searchDto = new SearchProjectRequestDto
            {
                ProjectName = projectName,
                UserID = userId
            };

            var result = _userDashboardProcessor.GetProjects(searchDto);
        }

        /// <summary>
        /// Adds the project project exist.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test_Project_1", "UID123", "Test_User")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void AddProject_ProjectExist(string projectName,string userID, string userName)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var addDto = new AddProjectDto
            {
                ProjectName = projectName,
            };

            var result = _userDashboardProcessor.AddProject(addDto, userID, userName);
            _projectRepository.Add(Arg.Any<Project>()).DidNotReceive();
        }

        /// <summary>
        /// Adds the project successfully.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectId">The project identifier.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("UID123", "Test_User", "Test_Project_121",121)]
        public void AddProject_Successfully(string userID, string userName,string projectName,int projectId)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var addDto = new AddProjectDto
            {
                ProjectName = projectName,
            };

            var addedProject = new Project
            {
                ID = projectId,
                ProjectName = projectName,
            };

            _projectRepository.Add(Arg.Any<Project>()).Returns(addedProject);

            var actualResult = _userDashboardProcessor.AddProject(addDto, userID, userName);
            Assert.AreEqual(addedProject.ID, actualResult.ProjectID);
        }

        /// <summary>
        /// Updates the project project name exist.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test_Project_1", 2, "UID123", "Test_User")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void UpdateProject_ProjectNameExist(string projectName,int projectId, string userID, string userName)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var addDto = new AddProjectDto
            {
                ProjectID=projectId,
                ProjectName = projectName,
            };

            var result = _userDashboardProcessor.UpdateProject(addDto, userID, userName);
            _projectRepository.Update(Arg.Any<Project>()).DidNotReceive();
        }

        /// <summary>
        /// Updates the project successfully.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test_Project_1", 1, "UID123", "Test_User")]
        public void UpdateProject_Successfully(string projectName, int projectId, string userID, string userName)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var addDto = new AddProjectDto
            {
                ProjectID=projectId,
                ProjectName = projectName,
            };

            var updateProject = new Project
            {
                ID = projectId,
                ProjectName = projectName,
                ContactEmail="Test@Test.com",
                ContactName="Test"

            };

            _projectRepository.Find(projectId).ReturnsForAnyArgs(ProjectList().FirstOrDefault(x => x.ID == addDto.ProjectID));

            _projectRepository.Update(Arg.Any<Project>()).Returns(updateProject);

            var actualResult = _userDashboardProcessor.UpdateProject(addDto, userID, userName);
            Assert.AreEqual(updateProject.ID, actualResult.ProjectID);
        }


        /// <summary>
        /// Gets the projects by search text.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test_Project_1")]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetProjects_BySearchText(string projectName)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var result = _userDashboardProcessor.GetProjectsBySearchText(projectName);

        }

        //[TestCategory("UserDashboardProcessor"), TestMethod]
        //[DataRow(0, "Test1@generac.com", "TestUID","Test_username")]
        //public void AddSharedProjects_Successfully(int ID, string descriptionAdd, string userID,string userName)
        //{
        //    var sharedProjectsDto = new SharedProjectsDto
        //    {
        //        SharedProjectID = ID,
        //        RecipientEmail = descriptionAdd
        //    };

        //    var addedSharedProjects = new SharedProject
        //    {
        //        ID = 1,
        //        RecipientEmail = descriptionAdd
        //    };

        //    _sharedProjectsRepository.Add(Arg.Any<SharedProject>()).Returns(addedSharedProjects);
        //    var actualResult = _userDashboardProcessor.SaveSharedProject(sharedProjectsDto, userID, userName);
        //    Assert.AreEqual(addedSharedProjects.ID, actualResult.SharedProjectID);
        //}

        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test@generac.com", "UID1234")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetShareProject_NotFoundGetAll(string recipientEmail, string userId)
        {
            _sharedProjectsRepository.GetAll(x => x.Active && x.RecipientEmail.ToLower().Contains(recipientEmail.ToLower())).ReturnsForAnyArgs(SharedProjectEmptyList());
            var searchDto = new SearchShareProjectRequestDto
            {
                RecipientEmail = recipientEmail,
            };
            var result = _userDashboardProcessor.GetSharedProjects(searchDto);
        }

        [TestCategory("UserDashboardProcessor"), TestMethod]
        [DataRow("Test1@generac.com", "UID1234")]
        public void GetShareProject_Successfully(string recipientEmail, string userId)
        {
            _sharedProjectsRepository.GetAll(x => x.Active && x.RecipientEmail.ToLower().Contains(recipientEmail.ToLower())).ReturnsForAnyArgs(SharedProjectList());
            var searchDto = new SearchShareProjectRequestDto
            {
                RecipientEmail = recipientEmail,
            };
            var result = _userDashboardProcessor.GetSharedProjects(searchDto);
        }

        /// <summary>
        /// Projects the list.
        /// </summary>
        /// <returns></returns>
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
                    ID=i
                };

                list.Add(project);
            }

            return list.AsQueryable();
        }

        /// <summary>
        /// Projects the empty list.
        /// </summary>
        /// <returns></returns>
        private IQueryable<Project> ProjectEmptyList()
        {
            var list = new List<Project>();
            return list.AsQueryable();
        }

        private IQueryable<SharedProject> SharedProjectEmptyList()
        {
            var list = new List<SharedProject>();
            return list.AsQueryable();
        }

        private IQueryable<SharedProject> SharedProjectList()
        {
            var list = new List<SharedProject>();
            for (int i = 1; i <= 5; i++)
            {
                var sharedProjects = new SharedProject
                {
                    RecipientEmail = "Test" + i + "@generac.com",
                    ProjectID = i,
                    ID = i,
                    Active=true,
                    Project=new Project
                    {
                        ID=i
                    }
                };

                list.Add(sharedProjects);
            }

            return list.AsQueryable();
        }
    }
}
