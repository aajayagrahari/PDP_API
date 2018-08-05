using PowerDesignPro.BusinessProcessors.Dtos;
using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    /// <summary>
    /// contains a signature for all required operations on Project
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// Gets the project detail.
        /// </summary>
        /// <param name="searchDto">The search dto.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ProjectDetailDto GetProjectDetail(SearchProjectRequestDto searchDto, string userName);

        /// <summary>
        /// Gets the solution header details in project.
        /// </summary>
        /// <param name="projectRequestDto">The project request dto.</param>
        /// <returns></returns>
        IEnumerable<SolutionHeaderDetailsInProjectDto> GetSolutionHeaderDetailsInProject(SearchProjectRequestDto projectRequestDto, string userName);

        /// <summary>
        /// Deletes the project.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="solutionId">The solution identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        int DeleteSolution(int projectId, int solutionId,string userName);

       
    }
}
