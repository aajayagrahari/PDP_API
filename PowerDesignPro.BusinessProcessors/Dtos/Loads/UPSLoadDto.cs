namespace PowerDesignPro.BusinessProcessors.Dtos
{
    public class UPSLoadDto : BaseSolutionLoadDto
    {
        public UPSLoadDto()
        {
            UPSLoadPickListDto = new UPSLoadPickListDto();
        }

        public UPSLoadPickListDto UPSLoadPickListDto { get; set; }

        public int? PhaseID { get; set; }

        public int? EfficiencyID { get; set; }

        public int? ChargeRateID { get; set; }

        public int? PowerFactorID { get; set; }

        public bool UPSRevertToBattery { get; set; }

        public int? LoadLevelID { get; set; }

        public decimal? SizeKVA { get; set; }

        public int? SizeKVAUnitsID { get; set; }

        public int? UPSTypeID { get; set; }

        public int? HarmonicDeviceTypeID { get; set; }

        public int? HarmonicContentID { get; set; }
    }
}
