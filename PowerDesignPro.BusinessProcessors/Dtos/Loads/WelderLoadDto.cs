namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class WelderLoadDto : BaseSolutionLoadDto
    {
        public WelderLoadDto()
        {
            WelderLoadPickListDto = new WelderLoadPickListDto();
        }

        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        public decimal? SizeStarting { get; set; }

        public int? SizeStartingUnitsID { get; set; }

        public int? RunningPFID { get; set; }

        public int? StartingPFID { get; set; }

        public bool? SizeRunningEditable { get; set; }

        public bool? RunningPFEditable { get; set; }

        public bool? HarmonicTypeEditable { get; set; }

        public int? WelderTypeID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? HarmonicContentID { get; set; }

        public bool? LockVoltageDip { get; set; }

        public int FrequencyID { get; internal set; }

        public WelderLoadPickListDto WelderLoadPickListDto { get; set; }
    }
}
