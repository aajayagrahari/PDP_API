namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class MotorLoadDto : BaseSolutionLoadDto
    {
        public MotorLoadDto()
        {
            MotorLoadPickListDto = new MotorLoadPickListDto();
        }

        public decimal? SizeRunning { get; set; }

        public int? SizeRunningUnitsID { get; set; }

        public int? MotorLoadLevelID { get; set; }

        public int? MotorLoadTypeID { get; set; }

        public int? MotorTypeID { get; set; }

        public int? StartingCodeID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? ConfigurationInputID { get; set; }

        public int? HarmonicContentID { get; set; }

        public bool? StartingMethodEditable { get; set; }

        public bool? ConfigurationInputEditable { get; set; }

        public bool? MotorLoadLevelEditable { get; set; }

        public bool? MotorLoadTypeEditable { get; set; }

        public bool? MotorTypeEditable { get; set; }

        public bool? StartingCodeEditable { get; set; }

        public bool? SizeRunningEditable { get; set; }

        public bool? HarmonicTypeEditable { get; set; }

        public MotorLoadPickListDto MotorLoadPickListDto { get; set; }

        public int FrequencyID { get; internal set; }
    }
}
