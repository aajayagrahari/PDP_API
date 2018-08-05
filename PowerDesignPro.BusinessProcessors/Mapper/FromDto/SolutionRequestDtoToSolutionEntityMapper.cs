using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Models;
using System;

namespace PowerDesignPro.BusinessProcessors.Mapper.FromDto
{
    /// <summary>
    /// This class Override's the default AutoMapper implementation and map data from SolutionRequestDto to Solution Entity
    /// </summary>
    /// <seealso cref="PowerDesignPro.Common.Mapper.AutoMapper{PowerDesignPro.BusinessProcessors.Dtos.SolutionRequestDto, PowerDesignPro.Data.Models.Solution}" />
    /// <seealso cref="PowerDesignPro.Common.Mapper.IMapper{PowerDesignPro.BusinessProcessors.Dtos.SolutionRequestDto, PowerDesignPro.Data.Models.Solution}" />
    public class SolutionRequestDtoToSolutionEntityMapper
        : AutoMapper<ProjectSolutionDto, Solution>, IMapper<ProjectSolutionDto, Solution>
    {

        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override Solution AddMap(ProjectSolutionDto input, string userId, string userName)
        {
            var solution = base.AddMap(input);
            solution.CreatedDateTime = DateTime.UtcNow;
            solution.ModifiedDateTime = DateTime.UtcNow;
            solution.ModifiedBy = userName;
            solution.CreatedBy = userName;
            solution.OwnedBy = userName;
            solution.Active = true;
            return solution;
        }

        /// <summary>
        /// Updates the map.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public override void UpdateMap(ProjectSolutionDto input, Solution output, string userId = null, string userName = null)
        {
            base.UpdateMap(input, output, userId, userName);
            output.ModifiedDateTime = DateTime.UtcNow;
            output.ModifiedBy = userName;
        }
    }
}
