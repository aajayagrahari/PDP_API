using PowerDesignPro.BusinessProcessors.Dtos.PickList;
using System.Collections.Generic;

namespace PowerDesignPro.BusinessProcessors.Dtos
{
    /// <summary>
    /// POCO class for Load Summary in Recommended Product
    /// </summary>
    public class SolutionSummaryRecommendedProductDto : BaseSolutionLimitsDto
    {
        public SolutionSummaryRecommendedProductDto()
        {
            ProductFamilyList = new List<PickListDto>();
            FamilySelectionMethodList = new List<PickListDto>();
            SizingMethodList = new List<PickListDto>();
            GeneratorList = new List<GeneratorPickListDto>();
            ParallelQuantityList = new List<PickListDto>();
            AlternatorList = new List<AlternatorPickListDto>();
            GeneratorDocuments = new List<SolutionSummaryGeneratorDocumentationDto>();
            DescriptionPartwo = "";
        }

        public string Description { get; set; }
        public string DescriptionPartwo { get; set; }

        public decimal RunningKW { get; set; }

        public decimal PeakKW { get; set; }

        public IEnumerable<PickListDto> ProductFamilyList { get; set; }

        public int? ProductFamilyID { get; set; }

        public IEnumerable<PickListDto> FamilySelectionMethodList { get; set; }

        public int FamilySelectionMethodID { get; set; }

        public IEnumerable<PickListDto> SizingMethodList { get; set; }

        public int SizingMethodID { get; set; }

        public IEnumerable<PickListDto> GeneratorList { get; set; }

        public int? GeneratorID { get; set; }

        public IEnumerable<PickListDto> ParallelQuantityList { get; set; }

        public int ParallelQuantityID { get; set; }

        public IEnumerable<PickListDto> AlternatorList { get; set; }

        public int? AlternatorID { get; set; }

        public IEnumerable<SolutionSummaryGeneratorDocumentationDto> GeneratorDocuments { get; set; }
    }
}
