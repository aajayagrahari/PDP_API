using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerDesignPro.BusinessProcessors.Processors;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data;
using PowerDesignPro.BusinessProcessors.Dtos;
using NSubstitute;
using System.Linq;
using System.Collections.Generic;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Data.Models;
using PowerDesignPro.BusinessProcessors.Mapper.ToDto;

namespace PowerDesignPro.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class TestProjectProcessor
    {
        /// <summary>
        /// The project processor
        /// </summary>
        private ProjectProcessor _projectProcessor;

        /// <summary>
        /// The project repository
        /// </summary>
        private IEntityBaseRepository<Project> _projectRepository;

        /// <summary>
        /// The solution repository
        /// </summary>
        private IEntityBaseRepository<Solution> _solutionRepository;
        /// <summary>
        /// Shared Project Repository
        /// </summary>
        private IEntityBaseRepository<SharedProject> _sharedProjectRepository;

        /// <summary>
        /// The project entity to project detail dto mapper
        /// </summary>
        private AutoMapper<Project, ProjectDetailDto> _projectEntityToProjectDetailDtoMapper;

        /// <summary>
        /// The solution entity to solution header details dto mapper
        /// </summary>
        private AutoMapper<Solution, SolutionHeaderDetailsInProjectDto> _solutionEntityToSolutionHeaderDetailsDtoMapper;

      
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            _projectRepository = Substitute.For<IEntityBaseRepository<Project>>();
            _solutionRepository = Substitute.For<IEntityBaseRepository<Solution>>();
            _sharedProjectRepository = Substitute.For<IEntityBaseRepository<SharedProject>>();
            _projectEntityToProjectDetailDtoMapper = Substitute.ForPartsOf<ProjectEntityToProjectDetailDtoMapper>();
            _solutionEntityToSolutionHeaderDetailsDtoMapper = Substitute.ForPartsOf<SolutionEntityToSolutionHeaderDetailsDtoMapper>();

          

            _projectProcessor = Substitute.For<ProjectProcessor>
              (_projectRepository, 
              _solutionRepository,
              _sharedProjectRepository,
              _projectEntityToProjectDetailDtoMapper,
              _solutionEntityToSolutionHeaderDetailsDtoMapper);
        }

        /// <summary>
        /// Gets the project detail user identifier not found.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        [TestCategory("ProjectProcessor"),TestMethod]
        [DataRow("Test_User","",0)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetProjectDetail_UserIdNotFound(string userName,string userId,int projectId)
        {
            _projectRepository.GetAll(x => x.Active).ReturnsForAnyArgs(ProjectList());
            var searchDto = new SearchProjectRequestDto
            {
                UserID = userId,
                ID= projectId
            };

            var result = _projectProcessor.GetProjectDetail(searchDto, userName);
            _projectEntityToProjectDetailDtoMapper.AddMap(Arg.Any<Project>()).DidNotReceive();

        }

        /// <summary>
        /// Gets the project detail project not found.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        [TestCategory("ProjectProcessor"), TestMethod]
        [DataRow("Test_User", "UID1234",6)]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetProjectDetail_ProjectNotFound(string userName,string userId, int projectId)
        {
            var searchDto = new SearchProjectRequestDto
            {
                UserID = userId,
                ID = projectId
            };

            _projectRepository.GetSingle(
                p => p.Active 
                && p.UserID == searchDto.UserID 
                && p.ID == searchDto.ID).ReturnsForAnyArgs(ProjectList().FirstOrDefault(x=>x.ID == searchDto.ID));

            var result = _projectProcessor.GetProjectDetail(searchDto, userName);
            _projectEntityToProjectDetailDtoMapper.AddMap(Arg.Any<Project>()).DidNotReceive();

        }

        /// <summary>
        /// Gets the project detail successfully.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        //[TestCategory("ProjectProcessor"), TestMethod]
        //[DataRow("Test_User", "UID123", 1)]
        //public void GetProjectDetail_Successfully(string userName, string userId, int projectId)
        //{
        //    var searchDto = new SearchProjectRequestDto
        //    {
        //        UserID = userId,
        //        ID = projectId
        //    };

        //    var addedProject = new ProjectDetailDto
        //    {
        //        UserName = userName,
        //    };

        //    _projectRepository.GetSingle(
        //        p => p.Active
        //        && p.UserID == searchDto.UserID
        //        && p.ID == searchDto.ID).ReturnsForAnyArgs(ProjectList().FirstOrDefault(x => x.ID == searchDto.ID));

        //    var result = _projectProcessor.GetProjectDetail(searchDto, userName);
        //    Assert.AreEqual(addedProject.UserName, result.UserName);

        //}

        /// <summary>
        /// Gets the solution header details in project user identifier not found.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        [TestCategory("ProjectProcessor"), TestMethod]
        [DataRow("",0,"")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetSolutionHeaderDetailsInProject_UserIdNotFound(string userId, int projectId, string userName)
        {
            var searchDto = new SearchProjectRequestDto
            {
                UserID = userId,
                ID = projectId
            };

            var result = _projectProcessor.GetSolutionHeaderDetailsInProject(searchDto, userName);
        }

        /// <summary>
        /// Gets the solution header details in project project not found.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        [TestCategory("ProjectProcessor"), TestMethod]
        [DataRow("UID1234", 6, "TestUserName")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void GetSolutionHeaderDetailsInProject_ProjectNotFound(string userId, int projectId, string userName)
        {
            var searchDto = new SearchProjectRequestDto
            {
                UserID = userId,
                ID = projectId
            };

            _projectRepository.GetSingle(
                p => p.Active
                && p.UserID == searchDto.UserID
                && p.ID == searchDto.ID).ReturnsForAnyArgs(ProjectList().FirstOrDefault(x => x.ID == searchDto.ID));

            var result = _projectProcessor.GetSolutionHeaderDetailsInProject(searchDto, userName);
        }

        /// <summary>
        /// Gets the solution header details in project solution not found.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        [TestCategory("ProjectProcessor"), TestMethod]
        [DataRow("UID1234", 2, "TestUserName")]
        //[ExpectedException(typeof(PowerDesignProException))]
        public void GetSolutionHeaderDetailsInProject_SolutionNotFound(string userId, int projectId, string userName)
        {
            var searchDto = new SearchProjectRequestDto
            {
                UserID = userId,
                ID = projectId
            };

            _projectRepository.GetSingle(
              p => p.Active
              && p.UserID == searchDto.UserID
              && p.ID == searchDto.ID).ReturnsForAnyArgs(EmptySolutionList().FirstOrDefault(x => x.ID == searchDto.ID));

            var result = _projectProcessor.GetSolutionHeaderDetailsInProject(searchDto, userName);

        }

        /// <summary>
        /// Gets the solution header details in project successfully.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        [TestCategory("ProjectProcessor"), TestMethod]
        [DataRow("UID1234",1,"TestUserName")]
        public void GetSolutionHeaderDetailsInProject_Successfully(string userId,int projectId, string userName)
        {
            _solutionRepository.GetAll(p => p.ProjectID == projectId).ReturnsForAnyArgs(SolutionList());
            var searchDto = new SearchProjectRequestDto
            {
                UserID = userId,
                ID = projectId
            };

            _projectRepository.GetSingle(
              p => p.Active
              && p.UserID == searchDto.UserID
              && p.ID == searchDto.ID).ReturnsForAnyArgs(ProjectList().FirstOrDefault(x => x.ID == searchDto.ID));

            var result = _projectProcessor.GetSolutionHeaderDetailsInProject(searchDto, userName);

        }

        /// <summary>
        /// Deletes the solution solution not found.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="solutionId">The solution identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [TestCategory("ProjectProcessor"), TestMethod]
        [DataRow(2, 7,"Test_Username")]
        [ExpectedException(typeof(PowerDesignProException))]
        public void DeleteSolution_SolutionNotFound(int projectId, int solutionId, string userName)
        {
            _solutionRepository.GetSingle(
               x => x.ID == solutionId &&
               x.ProjectID == projectId && x.Active).ReturnsForAnyArgs(SolutionList().FirstOrDefault(x => x.ID == solutionId && x.ProjectID == projectId && x.Active));

            var result = _projectProcessor.DeleteSolution(projectId, solutionId, userName);
            _solutionRepository.Update(Arg.Any<Solution>().DidNotReceive());

        }

        /// <summary>
        /// Deletes the solution successfully.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="solutionId">The solution identifier.</param>
        /// <param name="userName">Name of the user.</param>
        [TestCategory("ProjectProcessor"), TestMethod]
        [DataRow(1, 1,"Test_Username")]
        public void DeleteSolution_Successfully(int projectId, int solutionId,string userName)
        {
            var deleteSolution = new Solution
            {
                ID = solutionId,
                ProjectID = projectId,
            };

            _solutionRepository.GetSingle(
                x => x.ID == solutionId && 
                x.ProjectID == projectId && x.Active).ReturnsForAnyArgs(SolutionList().FirstOrDefault(x => x.ID == solutionId && x.ProjectID == projectId && x.Active)); 

            _solutionRepository.Update(Arg.Any<Solution>()).Returns(deleteSolution);

            var result = _projectProcessor.DeleteSolution(projectId, solutionId, userName);
            Assert.AreEqual(deleteSolution.ID, result);
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
                    ID = i,
                    ModifiedDateTime = DateTime.UtcNow,
                    OwnedBy = "TestUserName",
                    Solutions = new List<Solution> {
                        new Solution
                        {
                                SolutionName = "Test_Solution_" + i,
                                ProjectID = 1,
                                ID= i,
                                Description="Test",
                                SpecRefNumber="REFTest",
                                ModifiedDateTime = DateTime.UtcNow,
                                CreatedDateTime = DateTime.UtcNow,
                                Active =true,
                                 OwnedBy = "TestUserName",
                        }
                    }
                };

                list.Add(project);
            }

            return list.AsQueryable();
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
                    SolutionName = "Test_Solution_" + i,
                    ProjectID = 1,
                    ID = i,
                    Description = "Test",
                    SpecRefNumber = "REFTest",
                    ModifiedDateTime = DateTime.UtcNow,
                    CreatedDateTime = DateTime.UtcNow,
                    Active = true,
                    OwnedBy = "TestUserName",
                    Project = new Project {
                        ModifiedBy="Test_Username",
                        ModifiedDateTime= DateTime.UtcNow,
                        OwnedBy = "TestUserName",
                    }
                };

                list.Add(solution);
            }

            return list.AsQueryable();
        }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         

        /// <summary>
        /// Empties the solution list.
        /// </summary>
        /// <returns></returns>
        private IQueryable<Project> EmptySolutionList()
        {
            var list = new List<Project>();
            for (int i = 1; i <= 5; i++)
            {
                var project = new Project
                {
                    ProjectName = "Test_Project_" + i,
                    Active = true,
                    UserID = "UID123",
                    ID = i,
                    ModifiedDateTime = DateTime.UtcNow,
                };

                list.Add(project);
            }

            return list.AsQueryable();
        }

       
    }
}
