using PowerDesignPro.BusinessProcessors.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface IAdminLoad
    {
        IEnumerable<LoadsDto> GetLoads();

        AdminLoadDefaultDto GetDefaultLoadDetails(SearchBaseLoadRequestDto searchBasicLoadDto);

        AdminResponseDto SaveLoadDetail(AdminLoadDefaultDto adminLoadDefaultDto, string userID);

        bool DeleteLoadDefault(int loadID, string userName);

        // Dictionary<"userEmail", Dictionary<"Project : Id, Solution : Id", Dictionary<"Success/Failure ", List<Load : Id>>>>
        Dictionary<string, Dictionary<string, Dictionary<string, List<string>>>> UpdateLoadDetails(List<string> userEmailList);
    }
}
