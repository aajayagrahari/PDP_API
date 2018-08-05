using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Common;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Models;
using System;

namespace PowerDesignPro.BusinessProcessors.Mapper.FromDto
{
    /// <summary>
    /// This class Override's the default AutoMapper implementation and map data from AddProjectDto to Project Entity
    /// </summary>
    /// <seealso cref="PowerDesignPro.Common.Mapper.AutoMapper{PowerDesignPro.BusinessProcessors.Dtos.AddProjectDto, PowerDesignPro.Data.Models.Project}" />
    /// <seealso cref="PowerDesignPro.Common.Mapper.IMapper{PowerDesignPro.BusinessProcessors.Dtos.AddProjectDto, PowerDesignPro.Data.Models.Project}" />
    public class AddProjectDtoToProjectEntityMapper
        : AutoMapper<AddProjectDto, Project>, IMapper<AddProjectDto, Project>
    {
        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override Project AddMap(AddProjectDto input, string userId, string userName)
        {
            return new Project
            {
                ProjectName = input.ProjectName,
                UserID = userId,
                ContactName = input.ContactName,
                ContactEmail = input.ContactEmail,
                CreatedBy = userName,
                CreatedDateTime = ConvertTimeZone.ConvertFromUtcTimeZone("Central Standard Time"),
                ModifiedBy = userName,
                ModifiedDateTime = ConvertTimeZone.ConvertFromUtcTimeZone("Central Standard Time"),
                Active = true
            };
        }

        /// <summary>
        /// Updates the map. Map data from input objects to output
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        public override void UpdateMap(AddProjectDto input, Project output, string userId = null, string userName = null)
        {
            base.UpdateMap(input, output, userId, userName);
            output.ModifiedDateTime = ConvertTimeZone.ConvertFromUtcTimeZone("Central Standard Time");
            output.ModifiedBy = userName;
        }
    }
}
