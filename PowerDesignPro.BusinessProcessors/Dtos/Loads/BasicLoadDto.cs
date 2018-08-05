namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class BasicLoadDto : BaseSolutionLoadDto
    {
        public BasicLoadDto()
        {
            BasicLoadPickListDto = new BasicLoadPickListDto();
        }

        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        public decimal? SizeStarting { get; set; }

        public int? SizeStartingUnitsID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? HarmonicContentID { get; set; }

        public int? RunningPFID { get; set; }

        public int? StartingPFID { get; set; }

        public bool? SizeRunningEditable { get; set; }

        public bool? SizeStartingEditable { get; set; }

        public bool? StartingPFEditable { get; set; }

        public bool? RunningPFEditable { get; set; }

        public bool? HarmonicTypeEditable { get; set; }

        public bool? LockVoltageDip { get; set; }

        public BasicLoadPickListDto BasicLoadPickListDto { get; set; }

        public int FrequencyID { get; internal set; }

       
    }
}
