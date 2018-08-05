using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Models;
using System;

namespace PowerDesignPro.BusinessProcessors.Mapper.ToDto
{
    public class SolutionEntityToSolutionHeaderDetailsDtoMapper
        : AutoMapper<Solution, SolutionHeaderDetailsInProjectDto>,
        IMapper<Solution, SolutionHeaderDetailsInProjectDto>
    {
        public override SolutionHeaderDetailsInProjectDto AddMap(Solution input, string userId = null, string userName = null)
        {
            var result = base.AddMap(input, userId, userName);
            result.SolutionID = input.ID;
            result.CreatedDateTime = String.Format("{0:g}", input.CreatedDateTime);
            result.ModifiedDateTime = String.Format("{0:g}", input.ModifiedDateTime);
            return result;
        }
    }
}
