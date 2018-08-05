namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class SearchBaseLoadRequestDto
    {
        public int? ID { get; set; }

        public int LoadID { get; set; }

        public int SolutionID { get; set; }

        public int LoadFamilyID { get; set; }
    }
}
