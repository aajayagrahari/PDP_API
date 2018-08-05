using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Models;
using System;


namespace PowerDesignPro.BusinessProcessors.Mapper.ToDto
{
    /// <summary>
    /// This class Override's the default AutoMapper implementation and map data from Project Entity to ProjectDetailDto
    /// </summary>
    /// <seealso cref="PowerDesignPro.Common.Mapper.AutoMapper{PowerDesignPro.Data.Models.Project, PowerDesignPro.BusinessProcessors.Dtos.ProjectDetailDto}" />
    /// <seealso cref="PowerDesignPro.Common.Mapper.IMapper{PowerDesignPro.Data.Models.Project, PowerDesignPro.BusinessProcessors.Dtos.ProjectDetailDto}" />
    public class ProjectEntityToProjectDetailDtoMapper
                : AutoMapper<Project, ProjectDetailDto>, IMapper<Project, ProjectDetailDto>
    {
        /// <summary>
        /// Adds the map.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public override ProjectDetailDto AddMap(Project input, string userId, string userName)
        {
            return new ProjectDetailDto
            {
                ID = input.ID,
                ProjectName = input.ProjectName,
                ContactName = input.ContactName,
                ContactEmail = input.ContactEmail,
                CreatedBy = input.CreatedBy,
                CreatedDateTime = String.Format("{0:g}", input.CreatedDateTime),
                ModifiedDateTime = String.Format("{0:g}", input.ModifiedDateTime),
                Company = input.User.CompanyName,
                Phone = input.User.Phone,
                UserName = userName
            };
        }
    }
}
