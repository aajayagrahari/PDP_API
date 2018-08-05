using PowerDesignPro.BusinessProcessors.Dtos;
using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface ISolutionLoad
    {
        BasicLoadDto GetLoadDetailsForBasicLoad(SearchBaseLoadRequestDto searchLoadDto, string userName);

        BasicLoadDto SaveSolutionBasicLoad(BasicLoadDto basicLoadDto, string userId, string userName);

        bool DeleteSolutionLoad(int solutionID, int solutionLoadID, int loadFamilyID, string userID);

        dynamic GetSolutionDetail(int solutionId);
        
        ACLoadDto GetLoadDetailsForACLoad(SearchBaseLoadRequestDto searchACLoadDto, string userName);

        ACLoadDto SaveSolutionACLoad(ACLoadDto acLoadDto, string userID, string userName);

        LightingLoadDto GetLoadDetailsForLightingLoad(SearchBaseLoadRequestDto searchLightingLoadDto, string userName);

        LightingLoadDto SaveSolutionLightingLoad(LightingLoadDto lightingLoadDto, string userID, string userName);

        UPSLoadDto GetLoadDetailsForUpsLoad(SearchBaseLoadRequestDto searchUpsLoadDto, string userName);

        UPSLoadDto SaveSolutionUpsLoad(UPSLoadDto upsLoadDto, string userID, string userName);

        WelderLoadDto GetLoadDetailsForWelderLoad(SearchBaseLoadRequestDto searchWelderLoadDto, string userName);

        WelderLoadDto SaveSolutionWelderLoad(WelderLoadDto welderLoadDto, string userID, string userName);

        MotorLoadDto GetLoadDetailsForMotorLoad(SearchBaseLoadRequestDto searchMotorLoadDto, string userName);

        MotorLoadDto SaveSolutionMotorLoad(MotorLoadDto motorLoadDto, string userID, string userName);

        bool CheckLoadExistForSolution(int solutionId);
    }
}
