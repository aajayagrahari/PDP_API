using PowerDesignPro.BusinessProcessors.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    /// <summary>
    /// Defines the signature for all the UserDashboard required operations.
    /// </summary>
    public interface IUserDashboard
    {
        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <param name="searchDto">The search dto.</param>
        /// <returns></returns>
        IEnumerable<DashboardProjectDetailDto> GetProjects(SearchProjectRequestDto searchDto);

        /// <summary>
        /// Gets the projects by search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        IEnumerable<DashboardProjectDetailDto> GetProjectsBySearchText(string searchText);

        /// <summary>
        /// Adds the project.
        /// </summary>
        /// <param name="addProjectDto">The add project dto.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        AddProjectResponseDto AddProject(AddProjectDto addProjectDto, string userId, string userName);


        /// <summary>
        /// Updates the project.
        /// </summary>
        /// <param name="addProjectDto">The update project dto.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        AddProjectResponseDto UpdateProject(AddProjectDto addProjectDto, string userId, string userName);

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        int DeleteProject(int projectId, string userId);

        int DeleteSharedProject(int projectId, string userId, string userName);

        SharedProjectsDto SaveSharedProject(SharedProjectsDto sharedProjectsDto, string userID,string userName);

        IEnumerable<DashboardSharedProjectDetailDto> GetSharedProjects(SearchShareProjectRequestDto searchDto);

        IEnumerable<ProjectSolutionListDto> GetSolutionListForSharedProject(int projectID);
        
    }
}
