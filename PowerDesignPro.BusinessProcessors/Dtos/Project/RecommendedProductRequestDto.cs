namespace PowerDesignPro.BusinessProcessors.Dtos.Project
{
    public class RecommendedProductRequestDto
    {
        public int FamilySelectionMethodID { get; set; }

        public int? ProductFamilyID { get; set; }

        //public int? ModuleSizeID { get; set; }

        public int ParallelQuantityID { get; set; }

        public int SizingMethodID { get; set; }

        public int? GeneratorID { get; set; }

        public int? AlternatorID { get; set; }

        public int SolutionID { get; set; }
        
        public int ProjectID { get; set; }

        public string Brand { get; set; }
    }
}
