using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    /// <summary>
    /// Processor class to Get handle all the Project related operations.
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Interface.IProject" />
    public class ProjectProcessor : IProject
    {
        /// <summary>
        /// The project repository
        /// </summary>
        private readonly IEntityBaseRepository<Project> _projectRepository;
        /// <summary>
        /// The solution repository
        /// </summary>
        private readonly IEntityBaseRepository<Solution> _solutionRepository;
        /// <summary>
        /// Shared Project Repository
        /// </summary>
        private readonly IEntityBaseRepository<SharedProject> _sharedProjectRepository;
        /// <summary>
        /// The project entity to project detail dto mapper
        /// </summary>
        private readonly IMapper<Project, ProjectDetailDto> _projectEntityToProjectDetailDtoMapper;

        private readonly IMapper<Solution, SolutionHeaderDetailsInProjectDto> _solutionEntityToSolutionHeaderDetailsDtoMapper;




        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectProcessor"/> class.
        /// </summary>
        /// <param name="projectRepository">The project repository.</param>
        /// <param name="solutionRepository">The solution repository.</param>
        /// <param name="projectEntityToProjectDetailDtoMapper">The project entity to project detail dto mapper.</param>
        public ProjectProcessor(
            IEntityBaseRepository<Project> projectRepository,
            IEntityBaseRepository<Solution> solutionRepository,
            IEntityBaseRepository<SharedProject> sharedProjectRepository,
            IMapper<Project, ProjectDetailDto> projectEntityToProjectDetailDtoMapper,
            IMapper<Solution, SolutionHeaderDetailsInProjectDto> solutionEntityToSolutionHeaderDetailsDtoMapper)

        {
            _projectRepository = projectRepository;
            _solutionRepository = solutionRepository;
            _sharedProjectRepository = sharedProjectRepository;
            _projectEntityToProjectDetailDtoMapper = projectEntityToProjectDetailDtoMapper;
            _solutionEntityToSolutionHeaderDetailsDtoMapper = solutionEntityToSolutionHeaderDetailsDtoMapper;

        }

        #region Public Methods

        /// <summary>
        /// Gets the project detail.
        /// </summary>
        /// <param name="searchDto">The search dto.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">
        /// ProjectNotFound
        /// or
        /// ProjectNotFound
        /// </exception>
        public ProjectDetailDto GetProjectDetail(SearchProjectRequestDto searchDto, string userName)
        {
            if (!string.IsNullOrEmpty(searchDto.UserID) && searchDto.ID > 0)
            {
                var projectDetail = _projectRepository.GetSingle(p => p.Active && p.UserID == searchDto.UserID && p.ID == searchDto.ID);
                
                if (projectDetail == null)
                {
                    return SharedProjectDetail(searchDto, userName);
                    throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
                }

                var projectDetailDto = _projectEntityToProjectDetailDtoMapper.AddMap(projectDetail, userName: userName);
                projectDetailDto.IsReadOnlyAccess = false;

                return projectDetailDto;
            }

            throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
        }

        /// <summary>
        /// Gets the solution header details in project.
        /// </summary>
        /// <param name="projectRequestDto">The project request dto.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">
        /// ProjectNotFound
        /// or
        /// SolutionNotFound
        /// or
        /// ProjectNotFound
        /// </exception>
        public IEnumerable<SolutionHeaderDetailsInProjectDto> GetSolutionHeaderDetailsInProject(SearchProjectRequestDto projectRequestDto, string userName)
        {
            if (!string.IsNullOrEmpty(projectRequestDto.UserID) && projectRequestDto.ID > 0)
            {
                var projectDetail = _projectRepository.GetSingle(p => p.Active && p.UserID == projectRequestDto.UserID && p.ID == projectRequestDto.ID);

                if (projectDetail == null)
                {
                    return SharedProjectSolutionDetail(projectRequestDto, userName);
                    throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
                }

                var solutionList = projectDetail.Solutions.Where(x => x.Active && userName.Equals(x.OwnedBy, StringComparison.InvariantCultureIgnoreCase)).OrderByDescending(s => s.ModifiedDateTime)
                    .Select(solution =>
                    {
                        var solutionDetail = _solutionEntityToSolutionHeaderDetailsDtoMapper.AddMap(solution);
                        solutionDetail.IsReadOnlyAccess = false;
                        return solutionDetail;
                    });

                if (solutionList == null || solutionList.Count() == 0)
                {
                    return new List<SolutionHeaderDetailsInProjectDto>();
                }

                return solutionList;
            }

            throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
        }


        /// <summary>
        /// Deletes the solution.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="solutionId">The solution identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">SolutionNotFound</exception>
        public int DeleteSolution(int projectId, int solutionId, string userName)
        {

            var solution = _solutionRepository.GetSingle(x => x.ID == solutionId && x.ProjectID == projectId && x.Active && userName.Equals(x.CreatedBy, StringComparison.InvariantCultureIgnoreCase));
            if (solution == null)
            {
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
            }

            solution.Active = false;
            solution.ModifiedDateTime = DateTime.UtcNow;
            solution.ModifiedBy = userName;

            solution.Project.ModifiedBy = solution.ModifiedBy;
            solution.Project.ModifiedDateTime = solution.ModifiedDateTime;

            _solutionRepository.Update(solution);
            _solutionRepository.Commit();

            return solutionId;
        }

        #endregion

        #region Private Methods

        private ProjectDetailDto SharedProjectDetail(SearchProjectRequestDto searchDto, string userName)
        {
            var sharedProject = _sharedProjectRepository.GetSingle(sp => sp.ProjectID == searchDto.ID && sp.RecipientEmail.Equals(userName));
            var project = _projectRepository.GetSingle(p => p.Active && p.ID == searchDto.ID);

            if (sharedProject == null)
            {
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            var sharedProjectDetail = _projectEntityToProjectDetailDtoMapper.AddMap(project);
            sharedProjectDetail.UserName = sharedProjectDetail.CreatedBy;
            sharedProjectDetail.IsReadOnlyAccess = true;
            return sharedProjectDetail;
        }


        private IEnumerable<SolutionHeaderDetailsInProjectDto> SharedProjectSolutionDetail(SearchProjectRequestDto projectRequestDto, string userName)
        {
            var sharedProject = _sharedProjectRepository.GetSingle(sp => sp.ProjectID == projectRequestDto.ID && sp.RecipientEmail.Equals(userName) && sp.Active);
            //var project = _projectRepository.GetSingle(p => p.Active && p.ID == projectRequestDto.ID);

            if (sharedProject == null)
            {
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            var sharedProjectSolutionList = sharedProject.SharedProjectSolution.OrderByDescending(s => s.ModifiedDateTime)
                    .Select(sharedSolution =>
                    {
                        var sharedSolutionDetail = _solutionEntityToSolutionHeaderDetailsDtoMapper.AddMap(sharedSolution.Solution);
                        sharedSolutionDetail.IsReadOnlyAccess = true;
                        return sharedSolutionDetail;
                    }).ToList();

            var copiedSolutionList = sharedProject.Project.Solutions.Where(x => userName.Equals(x.OwnedBy, StringComparison.InvariantCultureIgnoreCase) && x.Active);
            foreach (var item in copiedSolutionList)
            {
                var solution = _solutionEntityToSolutionHeaderDetailsDtoMapper.AddMap(item);
                sharedProjectSolutionList.Add(solution);
            }
            //.Select(copySolution =>
            //{
            //    return _solutionEntityToSolutionHeaderDetailsDtoMapper.AddMap(copySolution);
            //}).ToList();

            if (sharedProjectSolutionList == null || sharedProjectSolutionList.Count() == 0)
            {
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
            }

            //sharedProjectSolutionList.ToList().AddRange(copiedSolutionList);

            return sharedProjectSolutionList.OrderByDescending(x => Convert.ToDateTime(x.ModifiedDateTime)); 
        }

        #endregion
    }
}