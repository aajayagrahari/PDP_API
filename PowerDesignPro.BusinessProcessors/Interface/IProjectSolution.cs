using PowerDesignPro.BusinessProcessors.Dtos;
using System;
using System.Collections.Generic;
using System.Net;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    /// <summary>
    /// ProjectSolution Interface : defines all the required methods for Solution
    /// </summary>
    public interface IProjectSolution
    {
        SolutionHeaderDetailDto GetSolutionHeaderDetails(string userID, int projectID, int solutionID, string userName);
        PowerDesignPro.Data.Models.Solution GetSolution(string userID, int projectID, int solutionID, string userName = "");
        /// <summary>
        /// Get default solution setup when user is creating a new solution
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="projectID">The project identifier.</param>
        /// <returns></returns>
        BaseSolutionSetupDto LoadDefaultSolutionSetupForNewSolution(string userID, int projectID);

        /// <summary>
        /// Get user default solution setup
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        BaseSolutionSetupDto LoadUserDefaultSolutionSetup(string userID);

        /// <summary>
        /// Global settings to override the user default settings
        /// </summary>
        /// <returns></returns>
        GlobalDefaultSolutionSetupDto LoadGlobalDefaultSolutionSetup();

        /// <summary>
        /// Get solution setup for existing solution
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="projectID">The project identifier.</param>
        /// <param name="solutionID">The solution identifier.</param>
        /// <returns></returns>
        ProjectSolutionDto LoadSolutionSetupForExistingSolution(string userID, int projectID, int solutionID, string userName);

        /// <summary>
        /// Save solution details
        /// </summary>
        /// <param name="projectSolutionResponseDto">The project solution response dto.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        ProjectSolutionDto SaveSolutionDetail(ProjectSolutionDto projectSolutionResponseDto, string userID, string userName);

        int CheckUserDefaultSetup(string UserID);

        /// <summary>
        /// Add/Save User Default Solution Setting values
        /// </summary>
        /// <param name="userDefaultSolutionSetupDto"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool SaveUserDefaultSolutionSetup(UserDefaultSolutionSetupDto userDefaultSolutionSetupDto, string userID, string userName);

        Dictionary<string, int> CopySolution(SolutionRequestDto solutionRequestDto, string userID, string userName);

        GrantEditAccessResponseDto GrantEditAccess(SolutionRequestDto solutionRequestDto, string userID, string userName);
    }
}
