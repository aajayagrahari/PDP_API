namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class ACLoadDto : BaseSolutionLoadDto
    {
        public ACLoadDto()
        {
            ACLoadPickListDto = new ACLoadPickListDto();
        }

        public decimal? Cooling { get; set; }

        public int? CoolingUnitsID { get; set; }

        public int? CompressorsID { get; set; }

        public int? CoolingLoadID { get; set; }

        public int? ReheatLoadID { get; set; }

        public ACLoadPickListDto ACLoadPickListDto { get; set; }
    }
}
