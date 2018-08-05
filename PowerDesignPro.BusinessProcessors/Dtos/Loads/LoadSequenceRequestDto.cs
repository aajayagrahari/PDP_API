namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class LoadSequenceRequestDto
    {
        public int ProjectID { get; set; }

        public int SolutionID { get; set; }

        public int SolutionLoadID { get; set; }

        public int LoadFamilyID { get; set; }

        public int LoadSequenceID { get; set; }
    }
}
