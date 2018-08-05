using System;
using System.Linq;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Common.Constant;
using System.Collections.Generic;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Data.Models;
using System.Threading.Tasks;
using PowerDesignPro.Common;
using System.Net;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    /// <summary>
    /// Processor class to handle all the required operations for User Dashboard.
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Interface.IUserDashboard" />
    public class UserDashboardProcessor : IUserDashboard
    {
        /// <summary>
        /// The project repository
        /// </summary>
        private readonly IEntityBaseRepository<Project> _projectRepository;        

        /// <summary>
        /// The project entity to dto mapper
        /// </summary>
        private readonly IMapper<Project, DashboardProjectDetailDto> _projectEntityToDtoMapper;
        /// <summary>
        /// The add project dto to entity mapper
        /// </summary>
        private readonly IMapper<AddProjectDto, Project> _addProjectDtoToEntityMapper;

        private readonly IEntityBaseRepository<SharedProject> _sharedProjectsRepository;

        private readonly IEntityBaseRepository<SharedProjectSolution> _sharedProjectSolutionRespository;

        private readonly IMapper<SharedProjectsDto, SharedProject> _sharedProjectsEntityToSharedProjectsDtoMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDashboardProcessor"/> class.
        /// </summary>
        /// <param name="projectRepository">The project repository.</param>
        /// <param name="projectEntityToDtoMapper">The project entity to dto mapper.</param>
        /// <param name="addProjectDtoToEntityMapper">The add project dto to entity mapper.</param>
        public UserDashboardProcessor(
            IEntityBaseRepository<Project> projectRepository,
            IMapper<Project, DashboardProjectDetailDto> projectEntityToDtoMapper,
            IMapper<AddProjectDto, Project> addProjectDtoToEntityMapper,
            IEntityBaseRepository<SharedProject> sharedProjectsRepository,
            IEntityBaseRepository<SharedProjectSolution> sharedProjectSolutionRespository,
            IMapper<SharedProjectsDto, SharedProject> sharedProjectsEntityToSharedProjectsDtoMapper)
        {
            _projectRepository = projectRepository;
            _projectEntityToDtoMapper = projectEntityToDtoMapper;
            _addProjectDtoToEntityMapper = addProjectDtoToEntityMapper;
            _sharedProjectsRepository = sharedProjectsRepository;
            _sharedProjectSolutionRespository = sharedProjectSolutionRespository;
            _sharedProjectsEntityToSharedProjectsDtoMapper = sharedProjectsEntityToSharedProjectsDtoMapper;            
        }

        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <param name="searchDto">The search dto.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">
        /// ProjectNotFound
        /// or
        /// ProjectNotFound
        /// </exception>
        public IEnumerable<DashboardProjectDetailDto> GetProjects(SearchProjectRequestDto searchDto)
        {
            var projects = _projectRepository.GetAll(p => p.Active && p.UserID == searchDto.UserID);

            if (projects.Count() == 0 && string.IsNullOrEmpty(searchDto.ProjectName))
            {
                return new List<DashboardProjectDetailDto>();
            }
            else if (projects.Count() == 0)
            {
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            // search by project name
            if (!string.IsNullOrEmpty(searchDto.ProjectName))
            {
                projects = projects.Where(p => p.ProjectName.ToLower().Contains(searchDto.ProjectName.ToLower()));
            }

            // search by solution name
            if (!string.IsNullOrEmpty(searchDto.SolutionName))
            {
                projects = projects.Where(p => p.Solutions.Where(s => s.SolutionName.ToLower().Contains(searchDto.SolutionName.ToLower())).Count() > 0);
            }

            // search by CreateDate of project
            if (!string.IsNullOrEmpty(searchDto.CreateDate))
            {
                DateTime filterDate = Convert.ToDateTime(searchDto.CreateDate);
                projects = projects.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.CreatedDateTime) == filterDate.Date);
            }

            // search by modified date of project
            if (!string.IsNullOrEmpty(searchDto.ModifyDate))
            {
                DateTime filterDate = Convert.ToDateTime(searchDto.ModifyDate);
                projects = projects.Where(p => System.Data.Entity.DbFunctions.TruncateTime(p.ModifiedDateTime) == filterDate.Date);
            }

            if (!string.IsNullOrEmpty(searchDto.UserID) && searchDto.DaysOld > 0)
            {
                var _daysOld = DateTime.UtcNow.AddDays(-searchDto.DaysOld);
                projects = projects.Where(p => p.ModifiedDateTime > _daysOld);
            }

            var projectList = projects.ToList().OrderByDescending(p => p.ModifiedDateTime)
                .Select(project => new DashboardProjectDetailDto
                {
                    ID = project.ID,
                    CreatedDateTime = String.Format("{0:g}", project.CreatedDateTime),
                    ModifiedDateTime = String.Format("{0:g}", project.ModifiedDateTime),
                    ProjectName = project.ProjectName
                });

            if (projects.Count() == 0 && string.IsNullOrEmpty(searchDto.ProjectName))
            {
                return new List<DashboardProjectDetailDto>();
            }
            else if (projectList.Count() == 0)
            {
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            return projectList;
        }


        public IEnumerable<DashboardSharedProjectDetailDto> GetSharedProjects(SearchShareProjectRequestDto searchDto)
        {
            var shareProjects = _sharedProjectsRepository.GetAll(p => p.Active && p.RecipientEmail.ToLower().Contains(searchDto.RecipientEmail.ToLower()))
                 .Select(x => x.Project).ToList();

            if (shareProjects.Count() == 0)
            {
                throw new PowerDesignProException("ShareProjectNotFound", Message.ProjectDashboard);
            }

            var projectList = shareProjects.ToList().OrderByDescending(p => p.ModifiedDateTime)
               .Select(project => new DashboardSharedProjectDetailDto
               {
                   ID = project.ID,
                   CreatedDateTime = String.Format("{0:g}", project.CreatedDateTime),
                   ModifiedDateTime = String.Format("{0:g}", project.ModifiedDateTime),
                   ProjectName = project.ProjectName,
                   SharedUser = project.User.FirstName + " " + project.User.LastName
               });
            return projectList;
        }

        public SharedProjectsDto SaveSharedProject(SharedProjectsDto sharedProjectsDto, string userID, string userName)
        {
            //if (sharedProjectsDto.ID == 0)
            //{
            //    return AddSharedProjects(sharedProjectsDto, userID, userName);
            //}
            //else
            //{
            //    return new SharedProjectsDto
            //    {
            //        ID = 0,
            //    };
            //}

            return AddSharedProjects(sharedProjectsDto, userID, userName);
        }

        private SharedProjectsDto AddSharedProjects(SharedProjectsDto sharedProjectsDto, string userID, string userName)
        {
            try
            {
                SharedProject sharedProjectDetails;
                SharedProject sharedProject = _sharedProjectsRepository.GetSingle(s => s.ProjectID == sharedProjectsDto.ProjectID && s.RecipientEmail.Equals(sharedProjectsDto.RecipientEmail, StringComparison.InvariantCultureIgnoreCase));
                if (sharedProject != null)
                {
                    foreach (var sharedSolutionID in sharedProjectsDto.SelectedSolutionList)
                    {
                        if (sharedProject.SharedProjectSolution.Where(s => s.SharedProjectID == sharedProject.ID && s.SolutionID == sharedSolutionID).Count() <= 0)
                        {
                            sharedProject.SharedProjectSolution.Add(new SharedProjectSolution
                            {
                                SharedProjectID = sharedProject.ID,
                                SolutionID = sharedSolutionID,
                                CreatedDateTime = DateTime.UtcNow,
                                SharedDateTime = DateTime.UtcNow,
                                ModifiedDateTime = DateTime.UtcNow,
                                CreatedBy = userName,
                                ModifiedBy = userName,
                                Active = true
                            });
                        }
                        //else
                        //{
                        //    sharedProject.SharedProjectSolution.Remove(_sharedProjectSolutionRespository.GetSingle(s => s.SolutionID == sharedSolutionID));
                        //}
                    }
                    _sharedProjectsEntityToSharedProjectsDtoMapper.UpdateMap(sharedProjectsDto, sharedProject, userID);
                    sharedProject.ModifiedDateTime = DateTime.UtcNow;
                    sharedProject.ModifiedBy = userName;
                    sharedProject.Active = true;

                    sharedProjectDetails = _sharedProjectsRepository.Update(sharedProject);
                    _sharedProjectsRepository.Commit();
                }
                else
                {
                    sharedProject = _sharedProjectsEntityToSharedProjectsDtoMapper.AddMap(sharedProjectsDto, userID);
                    sharedProject.CreatedDateTime = DateTime.UtcNow;
                    sharedProject.CreatedBy = userName;
                    sharedProject.SharedDateTime = DateTime.UtcNow;
                    sharedProject.ModifiedDateTime = DateTime.UtcNow;
                    sharedProject.ModifiedBy = userName;
                    sharedProject.Active = true;

                    sharedProject.SharedProjectSolution = sharedProjectsDto.SelectedSolutionList.Select(solutionID => new SharedProjectSolution
                    {
                        SolutionID = solutionID,
                        CreatedDateTime = DateTime.UtcNow,
                        SharedDateTime = DateTime.UtcNow,
                        ModifiedDateTime = DateTime.UtcNow,
                        CreatedBy = userName,
                        ModifiedBy = userName,
                        Active = true
                    }).ToList();

                    sharedProjectDetails = _sharedProjectsRepository.Add(sharedProject);
                    _sharedProjectsRepository.Commit();
                }

                Project sharedProjectRow = _projectRepository.GetSingle(p => p.ID == sharedProjectsDto.ProjectID);
                var sharedProjectName = sharedProjectRow.ProjectName;
                var sharerUserName = sharedProjectRow.User.FirstName + " " + sharedProjectRow.User.LastName;

                return new SharedProjectsDto
                {
                    SharedProjectID = sharedProjectDetails.ID,
                    SharedProjectName = sharedProjectName,
                    SharerUserName = sharerUserName
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the projects by search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<DashboardProjectDetailDto> GetProjectsBySearchText(string searchText)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the project.
        /// </summary>
        /// <param name="addProjectDto">The add project dto.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">ProjectNameExist</exception>
        public AddProjectResponseDto AddProject(AddProjectDto addProjectDto, string userId, string userName)
        {
            var projectCount = _projectRepository.GetAll(p => p.UserID == userId && p.Active)
                                                    .Where(p => p.ProjectName.ToUpper() == addProjectDto.ProjectName.ToUpper())
                                                    .Count();
            if (projectCount > 0)
            {
                throw new PowerDesignProException("ProjectNameExist", Message.ProjectDashboard);
            }

            var project = _addProjectDtoToEntityMapper.AddMap(addProjectDto, userId, userName);

            var projectDetail = _projectRepository.Add(project);
            _projectRepository.Commit();

            return new AddProjectResponseDto
            {
                ProjectID = projectDetail.ID,
                ProjectName = projectDetail.ProjectName
            };
        }

        /// <summary>
        /// Updates the project.
        /// </summary>
        /// <param name="addProjectDto">The update project dto.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">ProjectNameExist</exception>
        public AddProjectResponseDto UpdateProject(AddProjectDto addProjectDto, string userId, string userName)
        {
            var projectCount = _projectRepository.GetAll(p => p.UserID == userId && p.Active)
                                .Where(p => p.ID != addProjectDto.ProjectID
                                        && addProjectDto.ProjectName.Equals(
                                            p.ProjectName,
                                            StringComparison.InvariantCultureIgnoreCase)).Count();

            if (projectCount > 0)
            {
                throw new PowerDesignProException("ProjectNameExist", Message.ProjectDashboard);
            }

            var Project = _projectRepository.Find(addProjectDto.ProjectID);

            _addProjectDtoToEntityMapper.UpdateMap(addProjectDto, Project, userId, userName);

            var projectDetail = _projectRepository.Update(Project);
            _projectRepository.Commit();

            return new AddProjectResponseDto
            {
                ProjectID = projectDetail.ID,
                ProjectName = projectDetail.ProjectName
            };
        }


        public AddProjectResponseDto CopyShareProject(SearchProjectRequestDto searchProjectRequestDto, string userId, string userName)
        {
            var Project = _projectRepository.Find(searchProjectRequestDto.ID);
            Project.ID = 0;
            Project.CreatedBy = userName;
            Project.ModifiedDateTime = DateTime.UtcNow;
            Project.ModifiedBy = userName;
            Project.ModifiedDateTime = DateTime.UtcNow;
            Project.Active = true;
            Project.ProjectName = string.Concat(Project.ProjectName, "– Shared V1");

            var projectCount = _projectRepository.GetAll(p => p.UserID == userId && p.Active)
                                                   .Where(p => p.ProjectName.ToUpper() == Project.ProjectName.ToUpper())
                                                   .Count();
            if (projectCount > 0)
            {
                Project.ProjectName = string.Concat(Project.ProjectName, "– Shared V", projectCount + 1);
            }

            Parallel.ForEach(Project.Solutions, solution =>
            {
                solution.ID = 0;
                solution.CreatedBy = userName;
                solution.ModifiedDateTime = DateTime.UtcNow;
                solution.ModifiedBy = userName;
                solution.ModifiedDateTime = DateTime.UtcNow;
                solution.Active = true;
                solution.SolutionName = string.Concat(solution.SolutionName, "– Shared V1");
                var solutionCount = Project.Solutions.Count(s => s.SolutionName.Equals(solution.SolutionName, StringComparison.InvariantCultureIgnoreCase));
                if (solutionCount > 0)
                {
                    solution.SolutionName = string.Concat(solution.SolutionName, "– Shared V", solutionCount + 1);
                }
            });

            var projectDetail = _projectRepository.Add(Project);
            _projectRepository.Commit();

            return new AddProjectResponseDto
            {
                ProjectID = projectDetail.ID,
                ProjectName = projectDetail.ProjectName
            };
        }

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">ProjectNotFound</exception>
        public int DeleteProject(int projectId, string userId)
        {
            var project = _projectRepository.GetSingle(x => x.ID == projectId && x.UserID == userId && x.Active);
            if (project == null)
            {
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            project.Active = false;
            project.UserID = userId;
            project.ModifiedDateTime = DateTime.UtcNow;
            Parallel.ForEach(project.Solutions, solution =>
            {
                solution.ModifiedDateTime = DateTime.UtcNow;
                solution.Active = false;
            });

            _projectRepository.Update(project);
            _projectRepository.Commit();

            return projectId;
        }

        public IEnumerable<ProjectSolutionListDto> GetSolutionListForSharedProject(int projectID)
        {
            return _projectRepository.GetSingle(x => x.ID == projectID && x.Active).Solutions.Where(s => s.Active).Select(x => new ProjectSolutionListDto
            {
                ID = x.ID,
                Name = x.SolutionName,
                Description = x.Description,
                Selected = false
            });
        }

        public int DeleteSharedProject(int projectId, string userId,string userName)
        {
            var shareProject = _sharedProjectsRepository.GetSingle(x => x.ProjectID == projectId && x.Active);
            if (shareProject == null)
            {
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            shareProject.Active = false;
            shareProject.ModifiedDateTime = DateTime.UtcNow;
            shareProject.ModifiedBy = userName;

            Parallel.ForEach(shareProject.SharedProjectSolution, shareSolution =>
            {
                shareSolution.ModifiedDateTime = DateTime.UtcNow;
                shareSolution.ModifiedBy = userName;
                shareSolution.Active = false;
            });

            Parallel.ForEach(shareProject.Project.Solutions.Where(x=>x.Active && x.ProjectID == projectId && x.OwnedBy.Equals(userName, StringComparison.InvariantCultureIgnoreCase)), Solution =>
            {
                Solution.ModifiedDateTime = DateTime.UtcNow;
                Solution.ModifiedBy = userName;
                Solution.Active = false;
            });
                

            _sharedProjectsRepository.Update(shareProject);
            _sharedProjectsRepository.Commit();

            return projectId;
        }
    }
}