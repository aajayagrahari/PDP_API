using PowerDesignPro.BusinessProcessors.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDesignPro.BusinessProcessors.Dtos.Admin;
using System.Net;

namespace PowerDesignPro.BusinessProcessors.Interface
{
    public interface IAdmin
    {
        #region Generator
        IEnumerable<GeneratorDto> GetGeneratorDetails();

        IEnumerable<GeneratorDto> GetGenerator(int productFamilyId);

        GeneratorDto GetGenerator(SearchAdminRequestDto searchDto);

        AdminResponseDto SaveGeneratorDetail(GeneratorDto generatorResponseDto, string userID);

        bool DeleteGenerator(int generatorID, string userName);

        bool ImportGensetData(IEnumerable<ImportGensetDataRequestDto> gensetData, string userName);

        IEnumerable<ProductFamilyDto> GetProductFamilies();

        ProductFamilyDto GetProductFamily(SearchAdminRequestDto searchDto);

        AdminResponseDto SaveProductFamily(ProductFamilyDto productFamilyResponseDto);
        
        bool DeleteProductFamily(int productFamilyID, string userName);

        IEnumerable<DocumentationDto> GetDocumentationsByGenerator(int generatorID);

        DocumentationDto GetDocumentation(SearchAdminRequestDto searchDto);

        bool DeleteDocumentation(int documentationID, int generatorID, string userName);

        AdminResponseDto SaveDocumentation(DocumentationDto documentationDto);

        #endregion

        #region Alternator
        IEnumerable<AlternatorDto> GetAlternatorDetails();

        AlternatorDto GetAlternator(SearchAdminRequestDto searchDto);

        AdminResponseDto SaveAlternatorDetail(AlternatorDto alternatorResponseDto);

        bool DeleteAlternator(int alternatorID, string userName);

        IEnumerable<AlternatorFamilyDto> GetAlternatorFamilies();

        AdminResponseDto SaveAlternatorFamily(AlternatorFamilyDto alternatorFamilyResponseDto);

        bool DeleteAlternatorFamily(int alternatorFamilyID,string userName);

        bool ImportAlternatorData(IEnumerable<ImportAlternatorDataRequestDto> alternatorData, string userName);

        #endregion

        #region Maintain Market Vertical
        IEnumerable<SolutionApplicationDto> GetMaintainMarketVertical();

        AdminResponseDto SaveMaintainMarketVertical(SolutionApplicationDto maintainMarketVerticalResponseDto);

        bool DeleteMaintainMarketVerticals(int maintainMarketVerticalID, string userName);
        #endregion

        #region Maintain Harmonic Device
        IEnumerable<HarmonicDeviceTypeDto> GetHarmonicDeviceTypeDetail();

        HarmonicDeviceTypeDto GetHarmonicDeviceType(SearchAdminRequestDto searchDto);

        AdminResponseDto SaveHarmonicDeviceTypeDetail(HarmonicDeviceTypeDto harmonicDeviceRequestDto);

        bool DeleteHarmonicDeviceType(int ID, string userName);
        #endregion
    }
}
